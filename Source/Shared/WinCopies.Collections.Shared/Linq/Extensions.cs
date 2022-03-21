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

using WinCopies.Collections;
using WinCopies.Collections.Generic;

using static WinCopies.Collections.ThrowHelper;

#if !WinCopies3
using WinCopies.Util;

using static WinCopies.Util.Util;
#else
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Linq
{
    public static class Extensions
    {
        public static System.Collections.Generic.IEnumerable<TOut> SelectConverterIfNotNull<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Converter<TIn, TOut> converter) where TOut : class
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value;
        }

        public static System.Collections.Generic.IEnumerable<TOut> SelectIfNotNull<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Func<TIn, TOut> converter) where TOut : class
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value;
        }

        public static System.Collections.Generic.IEnumerable<TOut> SelectConverterIfNotNullStruct<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Converter<TIn, TOut?> converter) where TOut : struct
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut? value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value.Value;
        }

        public static System.Collections.Generic.IEnumerable<TOut> SelectIfNotNullStruct<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Func<TIn, TOut?> converter) where TOut : struct
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut? value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value.Value;
        }

        public static T FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T value)
        {
#if CS7
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))) is System.Collections.Generic.IReadOnlyList<T> list)

                    return list.Count > 0 ? list[0] : value;
#endif

            if (enumerable is IList<T> _list)

                return _list.Count > 0 ? _list[0] : value;

            foreach (T _value in enumerable)

                return _value;

            return value;
        }

        public static T FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

#if CS7
            if (enumerable is System.Collections.Generic.IReadOnlyList<T> list)

                return list.Count > 0 ? list[0] : func();
#endif

            if (enumerable is IList<T> _list)

                return _list.Count > 0 ? _list[0] : func();

            foreach (T _value in enumerable)

                return _value;

            return func();
        }

        public static bool FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T value, out T result)
        {
#if CS7
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))) is System.Collections.Generic.IReadOnlyList<T> list)
            {
                if (list.Count > 0)
                {
                    result = list[0];

                    return true;
                }

                result = value;

                return false;
            }
#endif

            if (enumerable is IList<T> _list)
            {
                if (_list.Count > 0)
                {
                    result = _list[0];

                    return true;
                }

                result = value;

                return false;
            }

            foreach (T _value in enumerable)
            {
                result = _value;

                return true;
            }

            result = value;

            return false;
        }

        public static bool FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T> func, out T result)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

#if CS7
            if (enumerable is System.Collections.Generic.IReadOnlyList<T> list)
            {
                if (list.Count > 0)
                {
                    result = list[0];

                    return true;
                }

                result = func();

                return false;
            }
#endif

            if (enumerable is IList<T> _list)
            {
                if (_list.Count > 0)
                {
                    result = _list[0];

                    return true;
                }

                result = func();

                return false;
            }

            foreach (T _value in enumerable)
            {
                result = _value;

                return true;
            }

            result = func();

            return false;
        }

        public static T FirstOrDefaultValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable) => enumerable.FirstOrValue(default(T));

        public static bool FirstOrDefaultValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, out T result) => enumerable.FirstOrValue(default(T), out result);

        public static T FirstValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable) => enumerable.FirstOrValue(() => throw GetNoItemException());

        public static bool FirstValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, out T result) => enumerable.FirstOrValue(() => throw GetNoItemException(), out result);

        public static T FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T value, in Func<T, bool> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return value;
        }

        public static T FirstOrValuePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T value, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return value;
        }

        public static T FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T> func, in Func<T, bool> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return func();
        }

        public static T FirstOrValuePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T> func, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return func();
        }

        public static bool FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T value, in Func<T, bool> predicate, out T result)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))
                {
                    result = _value;

                    return true;
                }

            result = value;

            return false;
        }

        public static bool FirstOrValuePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in T value, in Predicate<T> predicate, out T result)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))
                {
                    result = _value;

                    return true;
                }

            result = value;

            return false;
        }

        public static bool FirstOrValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T> func, in Func<T, bool> predicate, out T result)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))
                {
                    result = _value;

                    return true;
                }

            result = func();

            return false;
        }

        public static bool FirstOrValuePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T> func, in Predicate<T> predicate, out T result)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))
                {
                    result = _value;

                    return true;
                }

            result = func();

            return false;
        }

        public static T FirstOrDefaultValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T, bool> func) => enumerable.FirstOrValue(default(T), func);

        public static T FirstOrDefaultValuePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> func) => enumerable.FirstOrValuePredicate(default(T), func);

        public static bool FirstOrDefaultValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T, bool> func, out T result) => enumerable.FirstOrValue(default(T), func, out result);

        public static bool FirstOrDefaultValuePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> func, out T result) => enumerable.FirstOrValuePredicate(default(T), func, out result);

        public static T FirstValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T, bool> func) => enumerable.FirstOrValue(() => throw GetNoItemException(), func);

        public static T FirstPredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> func) => enumerable.FirstOrValuePredicate(() => throw GetNoItemException(), func);

        public static bool FirstValue<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Func<T, bool> func, out T result) => enumerable.FirstOrValue(() => throw GetNoItemException(), func, out result);

        public static bool FirstPredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> func, out T result) => enumerable.FirstOrValuePredicate(() => throw GetNoItemException(), func, out result);

        public static System.Collections.Generic.IEnumerable<T> Select<T>(this System.Collections.IEnumerable enumerable, Func<object, T> func)
        {
            foreach (object value in enumerable)

                yield return func(value);
        }

        public static System.Collections.Generic.IEnumerable<TValue> OrderBy<TValue, TKey>(this System.Collections.Generic.IEnumerable<TValue> enumerable, Func<TValue, TKey> func, Comparison<TKey> comparison) => enumerable.OrderBy(func, new Comparer2<TKey>(comparison));

        public static T FirstOrDefault<T>(this System.Collections.IEnumerable enumerable, in Func<object, bool> predicate)
        {
            foreach (object item in enumerable)

                if (item is T _item && predicate(item))

                    return _item;

            return default;
        }

        public static TOut FirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Func<TIn, bool> predicate)
        {
            foreach (TIn item in enumerable)

                if (item is TOut _item && predicate(item))

                    return _item;

            return default;
        }

        public static TOut PredicateFirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Predicate<TIn> predicate)
        {
            foreach (TIn item in enumerable)

                if (item is TOut _item && predicate(item))

                    return _item;

            return default;
        }

        public static TOut FirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Func<TOut, bool> predicate)
        {
            foreach (TIn item in enumerable)

                if (item is TOut _item && predicate(_item))

                    return _item;

            return default;
        }

        public static TOut PredicateFirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Predicate<TOut> predicate)
        {
            foreach (TIn item in enumerable)

                if (item is TOut _item && predicate(_item))

                    return _item;

            return default;
        }

        public static TOut FirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Func<TIn, bool> inPredicate, in Func<TOut, bool> outPredicate)
        {
            foreach (TIn item in enumerable)

                if (item is TOut _item && inPredicate(item) && outPredicate(_item))

                    return _item;

            return default;
        }

        public static TOut PredicateFirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Predicate<TIn> inPredicate, in Predicate<TOut> outPredicate)
        {
            foreach (TIn item in enumerable)

                if (item is TOut _item && inPredicate(item) && outPredicate(_item))

                    return _item;

            return default;
        }

        public static System.Collections.Generic.IEnumerable<TOut> WhereSelect<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Func<TIn, bool> where, Func<TIn, TOut> select)
        {
            foreach (TIn item in enumerable)

                if (where(item))

                    yield return select(item);
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

        /// <summary>
        /// Iterates through a given <see cref="System.Collections.IEnumerable"/> and tries to convert the items to a given generic type parameter. If an item cannot be converted, it is ignored in the resulting enumerable.
        /// </summary>
        /// <typeparam name="T">The generic type parameter for the resulting enumerable. Only the items that can be converted to this type will be present in the resulting enumerable.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>An enumerable containing all the items from <paramref name="enumerable"/> that could be converted to <typeparamref name="T"/>.</returns>
        /// <seealso cref="To{T}(System.Collections.IEnumerable)"/>
        public static System.Collections.Generic.IEnumerable<T> As<T>(this System.Collections.IEnumerable enumerable) => new Enumerable<T>(() => new TypeConverterEnumerator<T>(enumerable));

        /// <summary>
        /// Iterates through a given <see cref="System.Collections.IEnumerable"/> and directly converts the items to a given generic type parameter. An <see cref="InvalidCastException"/> is thrown when an item cannot be converted.
        /// </summary>
        /// <typeparam name="T">The generic type parameter for the resulting enumerable. All items in <paramref name="enumerable"/> will be converted to this type.</typeparam>
        /// <param name="enumerable">The source enumerable.</param>
        /// <returns>An enumerable containing the same items as they from <paramref name="enumerable"/>, with these items converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">An item could not be converted.</exception>
        /// <seealso cref="As{T}(System.Collections.IEnumerable)"/>
        public static System.Collections.Generic.IEnumerable<T> To<T>(this System.Collections.IEnumerable enumerable) => enumerable.SelectConverter(value => (T)value);
        
        public static
#if !WinCopies3
System.Collections.Generic.IEnumerable
#else
            Collections.Generic.IEnumerableInfo
#endif
            <T> Join<T>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> enumerable, bool keepEmptyEnumerables, params T[] join) => new
#if WinCopies3
            EnumerableInfo
#else
            Enumerable
#endif
            <T>(() => new JoinEnumerator<T>(enumerable, keepEmptyEnumerables, join)
#if WinCopies3
            , null
#endif
            );

        public static System.Collections.Generic.IEnumerable<T> WherePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

            foreach (T value in enumerable)

                if (func(value))

                    yield return value;
        }

        public static IEnumerable WherePredicate(this IEnumerable enumerable, Predicate func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

            foreach (object value in enumerable)

                if (func(value))

                    yield return value;
        }

#if !WinCopies3
        [Obsolete("This method has been replaced by Select<TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, System.Converter<TSource, TDestination> func).")]
        public static System.Collections.Generic.IEnumerator<TDestination> Select<TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, Func<TSource, TDestination> func) => new SelectEnumerator<TSource, TDestination>(enumerator, func);
#endif

        public static
#if !WinCopies3
System.Collections.Generic.IEnumerator
#else
            IEnumeratorInfo2
#endif
            <TDestination>
#if !WinCopies3
Select
#else
            SelectConverter
#endif
            <TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, Converter<TSource, TDestination> func) => new SelectEnumerator<TSource, TDestination>(enumerator, func);

        public static
#if !WinCopies3
System.Collections.Generic.IEnumerable
#else
            Collections.Generic.IEnumerableInfo
#endif
            <TOut>
#if !WinCopies3
Select
#else
            SelectConverter
#endif
            <TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Converter<TIn, TOut> selector)
#if !WinCopies3
        {
            foreach (TIn item in enumerable)

                yield return selector(item);
        }
#else
            => new EnumerableInfo<TOut>(() => new SelectEnumerator<TIn, TOut>(enumerable, selector), null);

        public static Collections.Generic.IEnumerableInfo<TOut> SelectConverter<TIn, TOut>(this Collections.Generic.IEnumerableInfo<TIn> enumerable, Converter<TIn, TOut> selector) => new EnumerableInfo<TOut>(() => new SelectEnumerator<TIn, TOut>(enumerable, selector), () => new SelectEnumerator<TIn, TOut>(enumerable.GetReversedEnumerator(), selector));

        public static T Last<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable)
        {
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))).SupportsReversedEnumeration)
            {
                System.Collections.Generic.IEnumerator<T> enumerator = enumerable.GetReversedEnumerator();

                return enumerator.MoveNext() ? enumerator.Current : throw new InvalidOperationException("The enumerable is empty.");
            }

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).Last();
        }

        public static T PredicateLast<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            if (enumerable.SupportsReversedEnumeration)
            {
                foreach (T item in new Enumerable<T>(() => enumerable.GetReversedEnumerator()))

                    if (predicate(item))

                        return item;

                throw new InvalidOperationException("No item found that matches the given predicate.");
            }

            // todo:

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).Last(item => predicate(item));
        }

        public static T LastOrDefault<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable)
        {
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))).SupportsReversedEnumeration)
            {
                System.Collections.Generic.IEnumerator<T> enumerator = enumerable.GetReversedEnumerator();

                return enumerator.MoveNext() ? enumerator.Current : default;
            }

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).LastOrDefault();
        }

        public static T PredicateLastOrDefault<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            if (enumerable.SupportsReversedEnumeration)
            {
                foreach (T item in new Enumerable<T>(() => enumerable.GetReversedEnumerator()))

                    if (predicate(item))

                        return item;

                return default;
            }

            // todo:

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).LastOrDefault(item => predicate(item));
        }
#endif

        /// <summary>
        /// Returns the first item, if any, from <typeparamref name="T"/> in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the first item of the given type.</param>
        /// <returns>The first item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="PredicateFirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        public static T FirstOrDefault<T>(this IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (item is T _item) return _item;

            return default;
        }

        /// <summary>
        /// Returns the first item, if any, from <typeparamref name="T"/> and that validates a given predicate in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the first item of the given type.</param>
        /// <param name="predicate">The predicate to validate.</param>
        /// <returns>The first item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        public static T PredicateFirstOrDefault<T>(this IEnumerable enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (item is T _item && predicate(_item)) return _item;

            return default;
        }

        /// <summary>
        /// Returns the last item, if any, from <typeparamref name="T"/> in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the last item of the given type.</param>
        /// <returns>The last item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="PredicateFirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        public static T LastOrDefault<T>(this IEnumerable enumerable)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (item is T _item)

                    value = _item;

            return value;
        }

        /// <summary>
        /// Returns the last item, if any, from <typeparamref name="T"/> and that validates a given predicate in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the last item of the given type.</param>
        /// <param name="predicate">The predicate to validate.</param>
        /// <returns>The last item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="PredicateFirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="FirstOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        public static T LastOrDefault<T>(this IEnumerable enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (item is T _item && predicate(_item))

                    value = _item;

            return value;
        }

        public static T FirstOrDefault<T>(this IEnumerable enumerable, in Func<object, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (func(item) is T _item) return _item;

            return default;
        }

        public static T LastOrDefault<T>(this IEnumerable enumerable, in Func<object, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (func(item) is T _item)

                    value = _item;

            return value;
        }

#if CS9
#nullable enable
        public static T FirstOrDefault<T>(this IEnumerable enumerable, in Func<object, T?> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object item in enumerable)

                if (func(item) is T _item) return _item!;

            return default!;
        }

        public static T LastOrDefault<T>(this IEnumerable enumerable, in Func<object, T?> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            T value = default;

            foreach (object item in enumerable)

                if (func(item) is T _item)

                    value = _item;

            return value!;
        }
#nullable restore
#endif

        public static TOut FirstOrDefault<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, in Func<TIn, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (TIn item in enumerable)

                if (func(item) is TOut _item) return _item;

            return default;
        }

        public static TOut LastOrDefault<TIn, TOut>(this IEnumerable enumerable, in Func<TIn, object> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut value = default;

            foreach (TIn item in enumerable)

                if (func(item) is TOut _item)

                    value = _item;

            return value;
        }

        public static IEnumerable AppendValues(this IEnumerable enumerable, System.Collections.Generic.IEnumerable<IEnumerable> newValues)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object obj in enumerable)

                yield return obj;

            foreach (IEnumerable _enumerable in newValues)

                foreach (object _obj in _enumerable)

                    yield return _obj;
        }

        public static IEnumerable AppendValues(this IEnumerable enumerable, params IEnumerable[] newValues) => AppendValues(enumerable, newValues.ToEnumerable());

        public static System.Collections.Generic.IEnumerable<T> AppendValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, params System.Collections.Generic.IEnumerable<T>[] newValues)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (T obj in enumerable)

                yield return obj;

            foreach (System.Collections.Generic.IEnumerable<T> _enumerable in newValues)

                foreach (T _obj in _enumerable)

                    yield return _obj;
        }
        public static System.Collections.Generic.IEnumerable<T> AppendValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, System.Collections.Generic.IEnumerable<T> values)
        {
            foreach (T value in enumerable)

                yield return value;

            foreach (T _value in values)

                yield return _value;
        }

        public static System.Collections.Generic.IEnumerable<T> AppendValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, params T[] values) => enumerable.AppendValues(values.ToEnumerable());

        public static System.Collections.Generic.IEnumerable<T> Merge<T>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> enumerable)
        {
            foreach (System.Collections.Generic.IEnumerable<T> _enumerable in enumerable)

                foreach (T item in _enumerable)

                    yield return item;
        }

        public static System.Collections.IEnumerable Merge(this System.Collections.Generic.IEnumerable<System.Collections.IEnumerable> enumerable)
        {
            foreach (System.Collections.IEnumerable _enumerable in enumerable)

                foreach (object item in _enumerable)

                    yield return item;
        }
        public static System.Collections.IEnumerable TryMerge(this System.Collections.Generic.IEnumerable<System.Collections.IEnumerable> enumerable)
        {
            foreach (System.Collections.IEnumerable _enumerable in enumerable)

                if (_enumerable != null)

                    foreach (object item in _enumerable)

                        yield return item;
        }

        public static System.Collections.Generic.IEnumerable<T> TryMerge<T>(this System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> enumerable)
        {
            foreach (System.Collections.Generic.IEnumerable<T> _enumerable in enumerable)

                if (_enumerable != null)

                    foreach (T item in _enumerable)

                        yield return item;
        }

        public static System.Collections.Generic.IEnumerable<TOut> WherePredicateSelect<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Predicate<TIn> where, Converter<TIn, TOut> select)
        {
            foreach (TIn item in enumerable)

                if (where(item))

                    yield return select(item);
        }

        public static System.Collections.Generic.IEnumerable<TOut> SelectWhere<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Func<TIn, TOut> select, Func<TOut, bool> where)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (where((_item = select(item))))

                    yield return _item;
        }

        public static System.Collections.Generic.IEnumerable<TOut> SelectWherePredicate<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Converter<TIn, TOut> select, Predicate<TOut> where)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (where((_item = select(item))))

                    yield return _item;
        }

        public static System.Collections.Generic.IEnumerable<TOut> WhereSelectWhere<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Func<TIn, bool> inPredicate, Func<TIn, TOut> converter, Func<TOut, bool> outPredicate)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (inPredicate(item))

                    if (outPredicate((_item = converter(item))))

                        yield return _item;
        }

        public static System.Collections.Generic.IEnumerable<TOut> WhereSelectWherePredicate<TIn, TOut>(this System.Collections.Generic.IEnumerable<TIn> enumerable, Predicate<TIn> inPredicate, Converter<TIn, TOut> converter, Predicate<TOut> outPredicate)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (inPredicate(item))

                    if (outPredicate((_item = converter(item))))

                        yield return _item;
        }

        public static System.Collections.Generic.IEnumerable<T> PrependValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, System.Collections.Generic.IEnumerable<T> values)
        {
            foreach (T item in values)

                yield return item;

            foreach (T item in enumerable)

                yield return item;
        }

        public static System.Collections.Generic.IEnumerable<T> PrependValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, params T[] values) => enumerable.PrependValues(values.ToEnumerable());

        public static System.Collections.Generic.IEnumerable<T> PrependValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> values)
        {
            foreach (System.Collections.Generic.IEnumerable<T> _values in values)

                foreach (T item in _values)

                    yield return item;

            foreach (T item in enumerable)

                yield return item;
        }

        public static System.Collections.Generic.IEnumerable<T> PrependValues<T>(this System.Collections.Generic.IEnumerable<T> enumerable, params System.Collections.Generic.IEnumerable<T>[] values) => enumerable.PrependValues(values.ToEnumerable());

        public static System.Collections.Generic.IEnumerable<T> Surround<T>(this System.Collections.Generic.IEnumerable<T> enumerable, System.Collections.Generic.IEnumerable<T> firstItems, System.Collections.Generic.IEnumerable<T> lastItems)
        {
            foreach (T item in firstItems)

                yield return item;

            foreach (T item in enumerable)

                yield return item;

            foreach (T item in lastItems)

                yield return item;
        }

        public static System.Collections.Generic.IEnumerable<T> Surround<T>(this System.Collections.Generic.IEnumerable<T> enumerable, System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> firstItems, System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<T>> lastItems)
        {
            foreach (System.Collections.Generic.IEnumerable<T> _firstItems in firstItems)

                foreach (T item in _firstItems)

                    yield return item;

            foreach (T item in enumerable)

                yield return item;

            foreach (System.Collections.Generic.IEnumerable<T> _lastItems in lastItems)

                foreach (T item in _lastItems)

                    yield return item;
        }

        public static System.Collections.Generic.IEnumerable<T> SelectConverter<T>(this System.Collections.IEnumerable enumerable, Converter<T> converter)
        {
            foreach (object value in enumerable)

                yield return converter(value);
        }
    }
}
