/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public static class EnumerableExtensions
    {
        public static ICountableEnumerable GetCountableEnumerable<TEnumerable>(this TEnumerable enumerable) where TEnumerable : ICountable, IEnumerable => enumerable as ICountableEnumerable ?? new Enumeration.CountableEnumerable<TEnumerable>(enumerable);

        public static IUIntCountableEnumerable GetUIntCountableEnumerable<TEnumerable>(this TEnumerable enumerable) where TEnumerable : IUIntCountable, IEnumerable => enumerable as IUIntCountableEnumerable ?? new Enumeration.UIntCountableEnumerable<TEnumerable>(enumerable);

        public static DotNetFix.Generic.ICountableEnumerable<TItems> GetCountableEnumerable<TEnumerable, TItems>(this TEnumerable enumerable) where TEnumerable : ICountable, System.Collections.Generic.IEnumerable<TItems> => enumerable as DotNetFix.Generic.ICountableEnumerable<TItems> ?? new Enumeration.Generic.CountableEnumerable<TEnumerable, TItems>(enumerable);

        public static DotNetFix.Generic.IUIntCountableEnumerable<TItems> GetUIntCountableEnumerable<TEnumerable, TItems>(this TEnumerable enumerable) where TEnumerable : IUIntCountable, System.Collections.Generic.IEnumerable<TItems> => enumerable as DotNetFix.Generic.IUIntCountableEnumerable<TItems> ?? new Enumeration.Generic.UIntCountableEnumerable<TEnumerable, TItems>(enumerable);

        public static TEnumerator GetEnumerator<TItems, TEnumerator>(this Extensions.Generic.IEnumerable<TItems, TEnumerator> enumerable, in EnumerationDirection enumerationDirection) where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
#if CS8
            => enumerationDirection switch
            {
                EnumerationDirection.FIFO => enumerable.AsFromType<Extensions.IEnumerable<TEnumerator>>().GetEnumerator(),
                EnumerationDirection.LIFO => enumerable.AsFromType<Extensions.IEnumerable<TEnumerator>>().GetReversedEnumerator(),
                _ => throw new InvalidEnumArgumentException($"The given value for the {nameof(enumerationDirection)} parameter is not supported.", nameof(enumerationDirection), enumerationDirection),
            };
#else
        {
            switch (enumerationDirection)
            {
                case EnumerationDirection.FIFO:

                    return enumerable.AsFromType<Extensions.IEnumerable<TEnumerator>>().GetEnumerator();

                case EnumerationDirection.LIFO:

                    return enumerable.AsFromType<Extensions.IEnumerable<TEnumerator>>().GetReversedEnumerator();

                default:

                    throw new InvalidEnumArgumentException($"The given value for the {nameof(enumerationDirection)} parameter is not supported.", nameof(enumerationDirection), enumerationDirection);
            }
        }
#endif

#if CS7
        public static System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator<T>(this ILinkedList<T> enumerable, in EnumerationDirection enumerationDirection)
#if CS8
            => enumerationDirection switch
            {
                EnumerationDirection.FIFO => enumerable.GetNodeEnumerator(),
                EnumerationDirection.LIFO => enumerable.GetReversedNodeEnumerator(),
                _ => throw new InvalidEnumArgumentException($"The given value for the {nameof(enumerationDirection)} parameter is not supported.", nameof(enumerationDirection), enumerationDirection),
            };
#else
        {
            switch (enumerationDirection)
            {
                case EnumerationDirection.FIFO:

                    return enumerable.GetNodeEnumerator();

                case EnumerationDirection.LIFO:

                    return enumerable.GetReversedNodeEnumerator();

                default:

                    throw new InvalidEnumArgumentException($"The given value for the {nameof(enumerationDirection)} parameter is not supported.", nameof(enumerationDirection), enumerationDirection);
            }
        }
#endif
#endif

        public static void SplitReferences<T, U, V, TContainer>(this System.Collections.Generic.IEnumerable<T> enumerable, in bool skipEmptyEnumerables, IRefSplitFactory<T, U, V, TContainer> splitFactory, params T[] separators) where T : class where U : INullableRefEntry<T> where V : System.Collections.Generic.IEnumerable<U>
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(splitFactory, nameof(splitFactory));
            // ThrowIfNull(enumerableNullableValueEntryCallback, nameof(enumerableNullableValueEntryCallback));
            ThrowIfNull(separators, nameof(separators));

            if (separators.Length == 0)

                throw new ArgumentException($"{nameof(separators)} does not contain any value.");

            Predicate<T> predicate = separators.Length == 1
                ? value => value == null ? separators[0] == null : value.Equals(separators[0])
                :
#if !CS9
                (Predicate<T>)(
#endif
                value => separators.Contains(value)
#if !CS9
                )
#endif
                ;
            System.Collections.Generic.IEnumerator<T> enumerator = enumerable.GetEnumerator();

            enumerable = null;

            void subAddAndAdd(in T value)
            {
                splitFactory.SubAdd(splitFactory.GetValueContainer(value));

                splitFactory.Add(splitFactory.GetEnumerable());
            }

            if (skipEmptyEnumerables)
            {
                if (enumerator.MoveNext())
                {
                    T value = enumerator.Current;

                    if (enumerator.MoveNext()) // There are more than one value.
                    {
                        value = null;

                        void tryAdd()
                        {
                            if (predicate(enumerator.Current) && splitFactory.SubCount > 0)
                            {
                                splitFactory.Add(splitFactory.GetEnumerable());

                                splitFactory.SubClear();
                            }

                            else

                                splitFactory.SubAdd(splitFactory.GetValueContainer(enumerator.Current));
                        }

                        tryAdd();

                        while (enumerator.MoveNext())

                            tryAdd();
                    }

                    else // There is one value.
                    {
                        if (predicate(value))

                            return;

                        else

                            subAddAndAdd(enumerator.Current);
                    }
                }

                else // There is no value.

                    return;
            }

            else if (enumerator.MoveNext())
            {
                T value = enumerator.Current;

                if (enumerator.MoveNext()) // There are more than one value.
                {
                    value = null;

                    void tryAdd()
                    {
                        if (predicate(enumerator.Current))
                        {
                            if (splitFactory.SubCount == 0)
                            {
                                splitFactory.SubAdd(splitFactory.GetValueContainer(null));

                                splitFactory.Add(splitFactory.GetEnumerable());
                            }

                            else
                            {
                                splitFactory.Add(splitFactory.GetEnumerable());

                                splitFactory.SubClear();
                            }
                        }

                        else

                            splitFactory.SubAdd(splitFactory.GetValueContainer(enumerator.Current));
                    }

                    tryAdd();

                    while (enumerator.MoveNext())

                        tryAdd();
                }

                else // There is one value.
                {
                    if (predicate(value))
                    {
                        void add() => subAddAndAdd(null);

                        add();
                        add();
                    }

                    else

                        subAddAndAdd(enumerator.Current);
                }
            }

            else // There is no value.
            {
                splitFactory.SubAdd(splitFactory.GetValueContainer(null));

                splitFactory.Add(splitFactory.GetEnumerable());
            }
        }

        private static void ThrowOnInvalidCopyToArrayParameters(in IEnumerable enumerable, in System.Array array)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(array, nameof(array));
        }

        public static void CopyTo(this IEnumerable enumerable, in System.Array array, in int arrayIndex, in int count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = arrayIndex;

            foreach (object value in enumerable)
            {
                if (i >= count)

                    return;

                array.SetValue(value, i++);
            }
        }

        public static void CopyTo(this IEnumerable enumerable, in System.Array array) => CopyTo(enumerable, array, 0, array.Length);

        public static void CopyTo(this IEnumerable enumerable, in System.Array array, in int arrayIndex, in uint count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = arrayIndex;

            foreach (object value in enumerable)

                array.SetValue(value, i++);
        }

        public static void CopyTo<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T[] array, in int arrayIndex, in int count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = arrayIndex;

            foreach (T value in enumerable)

                array[i++] = value;
        }

        public static void CopyTo<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T[] array) => CopyTo(enumerable, array, 0, array.Length);

        public static void CopyTo<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T[] array, in int arrayIndex, in uint count)
        {
            ThrowOnInvalidCopyToArrayParameters(enumerable, array);
            ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, count, nameof(array), nameof(arrayIndex));

            int i = arrayIndex;

            foreach (T value in enumerable)

                array[i++] = value;
        }

#if CS7
        public static ArrayList ToList(this IEnumerable array) => array.ToList(0);

        /// <summary>
        /// Converts an <see cref="IEnumerable"/> to an <see cref="ArrayList"/> from a given index for a given length.
        /// </summary>
        /// <param name="array">The <see cref="IEnumerable"/> to convert</param>
        /// <param name="startIndex">The index from which start the conversion.</param>
        /// <param name="length">The length of items to copy in the out <see cref="ArrayList"/>. Leave this parameter to null if you want to copy all the source <see cref="IEnumerable"/>.</param>
        /// <returns>The result <see cref="ArrayList"/>.</returns>
        public static ArrayList ToList(this IEnumerable array, in int startIndex, in int? length = null)
        {
            ThrowIfNull(array, nameof(array));

            int i = 0;

            if (length == null)
            {
                var arrayBuilder = new ArrayBuilder<object>();

                foreach (object value in array)

                    if (i < startIndex) i++;

                    else // We don't need to increment i anymore when we are here

                        _ = arrayBuilder.AddLast(value);

                return arrayBuilder.ToArrayList();
            }

            else
            {
                var arrayList = new ArrayList(length.Value);

                int count = 0;

                foreach (object value in array)
                {
                    if (i < startIndex)

                        i++;

                    else
                    {
                        _ = arrayList.Add(value);

                        count++;
                    }

                    if (count == length)

                        break;
                }

                return arrayList;
            }
        }
#endif

        //public static List<T> ToList<T>(this System.Collections.Generic.IEnumerable<T> array)
        //{
        //    List<T> arrayList = new List<T>();

        //    foreach (T value in array)

        //        arrayList.Add(value);

        //    return arrayList;
        //}
#if CS7
        /// <summary>
        /// Converts an <see cref="IEnumerable"/> to a <see cref="List{T}"/> from a given index for a given length.
        /// </summary>
        /// <param name="array">The <see cref="IEnumerable"/> to convert</param>
        /// <param name="startIndex">The index from which start the conversion.</param>
        /// <param name="length">The length of items to copy in the out <see cref="List{T}"/>. Leave this parameter to null if you want to copy all the source <see cref="IEnumerable"/>.</param>
        /// <returns>The result <see cref="List{T}"/>.</returns>
        public static List<T> ToList<T>(this System.Collections.Generic.IEnumerable<T> array, in int startIndex, in int? length = null)
        {
            ThrowIfNull(array, nameof(array));

            int i = 0;

            if (length == null)
            {
                var arrayBuilder = new ArrayBuilder<T>();

                foreach (T value in array)

                    if (i < startIndex) i++;

                    else    // We don't need to increment i anymore when we are here

                        _ = arrayBuilder.AddLast(value);

                return arrayBuilder.ToList();
            }

            else
            {
                var arrayList = new List<T>(length.Value);

                int count = 0;

                foreach (T value in array)
                {
                    if (i < startIndex)

                        i++;

                    else
                    {
                        arrayList.Add(value);

                        count++;
                    }

                    if (count == length)

                        break;
                }

                return arrayList;
            }
        }

        public static object[] ToArray(this IEnumerable array)
        {
            ThrowIfNull(array, nameof(array));

            var _array = new ArrayBuilder<object>();

            foreach (object value in array)

                _ = _array.AddLast(value);

            return _array.ToArray();
        }
#endif
        public static object[] ToArray(this IEnumerable array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            object[] _array = new object[length];

            int i = 0;

            int count = 0;

            foreach (object value in array)
            {
                if (i < startIndex)

                    i++;

                else

                    _array[count++] = value;

                if (count == length)

                    break;
            }

            return _array;
        }

        //public static T[] ToArray<T>(this System.Collections.Generic.IEnumerable<T> array)
        //{
        //    T[] _array = new T[length];

        //    int i = 0;

        //    int count = 0;

        //    foreach (T value in array)
        //    {
        //        if (i < startIndex)

        //            i++;

        //        else

        //            _array[count++] = value;

        //        if (count == length)

        //            break;
        //    }

        //    return _array;
        //}

        public static TDestination[] ToArray<TSource, TDestination>(this System.Collections.Generic.IEnumerable<TSource> array, in int startIndex, in int length, in Converter<TSource, TDestination> converter)
        {
            var _array = new TDestination[length];

            array.ToArray(_array, startIndex, length, converter);

            return _array;
        }

        public static void ToArray<TSource, TDestination>(this System.Collections.Generic.IEnumerable<TSource> enumerables, in TDestination[] array, int startIndex, in int length, in Converter<TSource, TDestination> converter)
        {
            foreach (TSource value in enumerables == null ? throw new ArgumentNullException(nameof(enumerables)) : array == null ? throw new ArgumentNullException(nameof(array)) : startIndex < 0 ? throw new ArgumentOutOfRangeException(nameof(startIndex)) : length < 0 || length > array.Length ? throw new ArgumentOutOfRangeException(nameof(length)) : enumerables)
            {
                array[startIndex++] = converter(value);

                if (startIndex == length)

                    break;
            }
        }

        public static T[] ToArray<T>(this System.Collections.Generic.IEnumerable<T> array, in int startIndex, in int length) => array.ToArray(startIndex, length, Delegates.Self);

        public static void ToArray<T>(this System.Collections.Generic.IEnumerable<T> enumerables, in T[] array, in int startIndex, in int length) => enumerables.ToArray(array, startIndex, length, Delegates.Self);

        public static TDestination[] ToArray<TSource, TDestination>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<
#endif
            TSource
#if CS5
                >
#else
            []
#endif
            list, in Converter<TSource, TDestination> selector) => list.ToArray(list.
#if CS5
                Count
#else
                Length
#endif
                , 0, selector);

        public static void ToArray<TSource, TDestination>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<
#endif
            TSource
#if CS5
                >
#else
            []
#endif
            list, in TDestination[] array, in Converter<TSource, TDestination> selector) => list.ToArray(array, 0, list.
#if CS5
                Count
#else
                Length
#endif
                , selector);

        public static TDestination[] ToArray<TSource, TDestination>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<
#endif
            TSource
#if CS5
                >
#else
            []
#endif
            list, in int startIndex, in int length, in Converter<TSource, TDestination> selector)
        {
            var result = new TDestination[length];

            list.ToArray(result, startIndex, length, selector);

            return result;
        }

        public static void ToArray<TSource, TDestination>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<
#endif
            TSource
#if CS5
                >
#else
            []
#endif
            list, in TDestination[] array, in int startIndex, in Converter<TSource, TDestination> selector) => list.ToArray(array, startIndex, list.
#if CS5
                Count
#else
                Length
#endif
                , selector);

        public static void ToArray<TSource, TDestination>(this
#if CS5
            System.Collections.Generic.IReadOnlyList<
#endif
            TSource
#if CS5
                >
#else
            []
#endif
            list, in TDestination[] array, in int startIndex, in int length, in Converter<TSource, TDestination> selector)
        {
            for (int i = list == null ? throw new ArgumentNullException(nameof(list)) : array == null ? throw new ArgumentNullException(nameof(array)) : selector == null ? throw new ArgumentNullException(nameof(selector)) : startIndex < 0 ? throw new ArgumentOutOfRangeException(nameof(startIndex)) : length < 0 || length > list.
#if CS5
                Count
#else
                Length
#endif
                ? throw new ArgumentOutOfRangeException(nameof(length)) : array.Length - startIndex < length ? throw new ArgumentException("Not enough place.") : 0; i < length; i++)

                array[i + startIndex] = selector(list[i]);
        }
#if CS5
        public static TDestination[] ToArray<TSource, TDestination>(this TSource[] list, in Converter<TSource, TDestination> selector) => list.AsReadOnlyList().ToArray(selector);

        public static void ToArray<TSource, TDestination>(this TSource[] list, in TDestination[] array, Converter<TSource, TDestination> selector) => list.AsReadOnlyList().ToArray(array, selector);

        public static TDestination[] ToArray<TSource, TDestination>(this TSource[] list, in int startIndex, int length, Converter<TSource, TDestination> selector) => list.AsReadOnlyList().ToArray(length, startIndex, selector);

        public static void ToArray<TSource, TDestination>(this TSource[] list, in TDestination[] array, in int startIndex, in Converter<TSource, TDestination> selector) => list.AsReadOnlyList().ToArray(array, startIndex, selector);

        public static void ToArray<TSource, TDestination>(this TSource[] list, in TDestination[] array, in int startIndex, in int length, in Converter<TSource, TDestination> selector) => list.AsReadOnlyList().ToArray(array, startIndex, length, selector);
#endif
        public static void Append(object _value, ref StringBuilder stringBuilder, in bool parseStrings, in bool parseSubEnumerables)
        {
            ThrowIfNull(stringBuilder, nameof(stringBuilder));

            if ((_value is string && parseStrings) || (!(_value is string) && _value is IEnumerable && parseSubEnumerables))

                ((IEnumerable)_value).ToString(ref stringBuilder, true);

            else

                _ = stringBuilder.AppendFormat("{0}, ", _value?.ToString());
        }

        private static void ToString(this IEnumerable array, ref StringBuilder stringBuilder, in bool parseSubEnumerables, in bool parseStrings = false)
        {
            _ = stringBuilder.Append('{');

            System.Collections.IEnumerator enumerator = array.GetEnumerator();

            bool atLeastOneLoop = false;

            if (enumerator.MoveNext())
            {
                atLeastOneLoop = true;

                Append(enumerator.Current, ref stringBuilder, parseStrings, parseSubEnumerables);
            }

            while (enumerator.MoveNext())

                Append(enumerator.Current, ref stringBuilder, parseStrings, parseSubEnumerables);

            _ = atLeastOneLoop ? stringBuilder.Insert(stringBuilder.Length - 2, "}") : stringBuilder.Append('}');
        }

        public static string ToString(this IEnumerable array, in bool parseSubEnumerables, in bool parseStrings = false)
        {
            ThrowIfNull(array, nameof(array));

            var stringBuilder = new StringBuilder();

            array.ToString(ref stringBuilder, parseSubEnumerables, parseStrings);

            return stringBuilder.ToString(0, stringBuilder.Length - 2);
        }

        public static StringBuilder ConcatenateString(this System.Collections.IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            var sb = new StringBuilder();

            foreach (object value in enumerable)

                if (value != null)

                    _ = sb.Append(value);

            return sb;
        }

        public static string ConcatenateString2(this System.Collections.IEnumerable enumerable) => enumerable.ConcatenateString().ToString();
    }
}
