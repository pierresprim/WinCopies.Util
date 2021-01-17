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

#if WinCopies3

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections
{
    public static class EnumerableExtensions
    {
        public static void SplitReferences<T, U, V, TContainer>(this System.Collections.Generic.IEnumerable<T> enumerable, in bool skipEmptyEnumerables, IRefSplitFactory<T, U, V, TContainer> splitFactory, params T[] separators) where T : class where U : INullableRefEntry<T> where V : System.Collections.Generic.IEnumerable<U>
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(splitFactory, nameof(splitFactory));
            // ThrowIfNull(enumerableNullableValueEntryCallback, nameof(enumerableNullableValueEntryCallback));
            ThrowIfNull(separators, nameof(separators));

            if (separators.Length == 0)

                throw new ArgumentException($"{nameof(separators)} does not contain any value.");

            Predicate<T> predicate;

            if (separators.Length == 1)

                predicate = value => value == null ? separators[0] == null : value.Equals(separators[0]);

            else

                predicate = value => separators.Contains(value);

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
                        subAddAndAdd(null);

                        subAddAndAdd(null);
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

        private static void ThrowOnInvalidCopyToArrayParameters(in IEnumerable enumerable, in Array array)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(array, nameof(array));
        }

        public static void CopyTo(this IEnumerable enumerable, in Array array, in int arrayIndex, in int count)
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

        public static void CopyTo(this IEnumerable enumerable, in Array array) => CopyTo(enumerable, array, 0, array.Length);

        public static void CopyTo(this IEnumerable enumerable, in Array array, in int arrayIndex, in uint count)
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

        public static T[] ToArray<T>(this System.Collections.Generic.IEnumerable<T> array, in int startIndex, in int length)
        {
            ThrowIfNull(array, nameof(array));

            var _array = new T[length];

            int i = 0;

            int count = 0;

            foreach (T value in array)
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

        static void Append(object _value, ref StringBuilder stringBuilder, in bool parseStrings, in bool parseSubEnumerables)
        {
            ThrowIfNull(stringBuilder, nameof(stringBuilder));

            if ((_value is string && parseStrings) || (!(_value is string) && _value is IEnumerable && parseSubEnumerables))

                ((IEnumerable)_value).ToString(ref stringBuilder, true);

            else

                _ = stringBuilder.AppendFormat("{0}, ", _value?.ToString());
        }

        private static void ToString(this IEnumerable array, ref StringBuilder stringBuilder, in bool parseSubEnumerables, in bool parseStrings = false)
        {
            _ = stringBuilder.Append("{");

            System.Collections.IEnumerator enumerator = array.GetEnumerator();

            bool atLeastOneLoop = false;

            if (enumerator.MoveNext())

            {

                atLeastOneLoop = true;

                Append(enumerator.Current, ref stringBuilder, parseStrings, parseSubEnumerables);

            }

            while (enumerator.MoveNext())

                Append(enumerator.Current, ref stringBuilder, parseStrings, parseSubEnumerables);

            _ = atLeastOneLoop ? stringBuilder.Insert(stringBuilder.Length - 2, "}") : stringBuilder.Append("}");
        }

        public static string ToString(this IEnumerable array, in bool parseSubEnumerables, in bool parseStrings = false)
        {
            ThrowIfNull(array, nameof(array));

            var stringBuilder = new StringBuilder();

            array.ToString(ref stringBuilder, parseSubEnumerables, parseStrings);

            return stringBuilder.ToString(0, stringBuilder.Length - 2);
        }

        /// <summary>
        /// Yield returns each object of an <see cref="IEnumerable"/>, so the given <see cref="IEnumerable"/> will be considered as an <see cref="IEnumerable{Object}"/>.
        /// </summary>
        /// <param name="enumerable">An <see cref="IEnumerable"/> to consider as a <see cref="IEnumerable{Object}"/>.</param>
        /// <returns>Yield returns the same enumerable as the given <paramref name="enumerable"/>, as an <see cref="IEnumerable{Object}"/>.</returns>
        public static System.Collections.Generic.IEnumerable<object> AsObjectEnumerable(this IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                yield return item;
        }

        public static IEnumerable AppendValues(this IEnumerable enumerable, params IEnumerable[] newValues)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object obj in enumerable)

                yield return obj;

            foreach (IEnumerable _enumerable in newValues)

                foreach (object _obj in _enumerable)

                    yield return _obj;
        }

        public static System.Collections.Generic.IEnumerable<T> AppendValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, params System.Collections.Generic.IEnumerable<T>[] newValues)

        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (T obj in enumerable)

                yield return obj;

            foreach (IEnumerable _enumerable in newValues)

                foreach (T _obj in _enumerable)

                    yield return _obj;
        }
    }
}

#endif
