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
using WinCopies.Util;

using static WinCopies.Collections.ThrowHelper;
using static WinCopies.ThrowHelper;

namespace WinCopies.Linq
{
    public static class Extensions
    {
        public static int GetIndexOf<T>(this System.Collections.Generic.IEnumerable<T
#if CS9
            ?
#endif
            > enumerable, Predicate<T
#if CS9
            ?
#endif
            > predicate)
        {
            int i = -1;

            foreach (T
#if CS9
                ?
#endif
                item in enumerable)
            {
                i++;

                if (predicate(item))

                    return i;
            }

            return -1;
        }

        public static int GetIndexOf<T>(this System.Collections.Generic.IEnumerable<T> enumerable, T value) => enumerable.GetIndexOf(value == null ?
#if !CS9
            (Predicate<T>)
#endif
            Delegates.
#if WinCopies4
            EqualsNull
#else
            CheckIfEqualsNull
#endif
            <T> : item => value.Equals(item));

        public static bool None<T>(this System.Collections.Generic.IEnumerable<T> enumerable, Func<T, bool> func)
        {
            foreach (T value in enumerable)

                if (func(value))

                    return false;

            return true;
        }
        public static bool NonePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> func)
        {
            foreach (T value in enumerable)

                if (func(value))

                    return false;

            return true;
        }

        public static bool None<T>(this System.Collections.Generic.IEnumerable<T> enumerable, T value)
        {
            foreach (T _value in enumerable)

                if (object.Equals(value, _value))

                    return false;

            return true;
        }

        public static TOut[] Convert<TIn, TOut>(this TIn[] values, in Converter<TIn, TOut> converter)
        {
            var result = new TOut[values.Length];

            for (int i = 0; i < values.Length; i++)

                result[i] = converter(values[i]);

            return result;
        }

        private static TOut[] Select<TIn, TOut>(in int length, in ConverterIn<int, TOut> func)
        {
            var result = new TOut[length];

            for (int i = 0; i < length; i++)

                result[i] = func(i);

            return result;
        }

        private static TOut[] Select<TIn, TOut>(this TIn[] items, in string itemsName, in string funcName, in ConverterIn<int, TOut> func) => Select<TIn, TOut>(
            items == null
            ? throw GetArgumentNullException(itemsName)
            : func == null
            ? throw GetArgumentNullException(funcName)
            : items.Length, func);

        public static TOut[] Select<TIn, TOut>(this TIn[] items, Func<TIn, TOut> func) => Select(items, nameof(items), nameof(func), (in int i) => func(items[i]));

        public static TOut[] SelectConverter<TIn, TOut>(this TIn[] items, Converter<TIn, TOut> func) => Select(items, nameof(items), nameof(func), (in int i) => func(items[i]));

        public static IEnumerable<TOut> WhereSelectPredicateConverter<TIn, TOut>(this IEnumerable<TIn> enumerable, Predicate<TIn> where, Converter<TIn, TOut> select)
        {
            foreach (TIn item in enumerable)

                if (where(item))

                    yield return select(item);
        }
#if !CS8
        public static int LastIndexOf(this string s, in char c)
        {
            for (int i = 0; i < s.Length; i++)

                if (s[i] == c)

                    return i;

            return -1;
        }
#endif
#if !WinCopies4
        public static IEnumerable<TOut> ForEach<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, IEnumerable<TOut>> func)
        {
            foreach (TIn
#if CS9
                ?
#endif
                item in enumerable)

                foreach (TOut
#if CS9
                ?
#endif
                _item in func(item))

                    yield return _item;
        }
#endif
        public static IEnumerable<TOut>
#if WinCopies4
            SelectManyConverter
#else
            ForEachConverter
#endif
            <TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, IEnumerable<TOut>> func)
        {
            foreach (TIn
#if CS9
                ?
#endif
                item in enumerable)

                foreach (TOut
#if CS9
                ?
#endif
                _item in func(item))

                    yield return _item;
        }

        public static IEnumerable<TOut>
#if WinCopies4
            SelectManyConverter
#else
            ForEachConverter
#endif
            <TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, Func<IEnumerable<TOut>>> func) => enumerable.SelectMany(item => func(item)());

        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> values, Func<T, bool> func)
        {
            foreach (T item in values)
            {
                if (func(item))

                    break;

                yield return item;
            }
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> enumerable) where T : struct
        {
            foreach (T item in enumerable)

                return item;

            return null;
        }

        public static T? FirstOrNull<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) where T : struct
        {
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T item in enumerable)

                if (predicate(item))

                    return item;

            return null;
        }

        public static T? FirstOrNullPredicate<T>(this IEnumerable<T> enumerable, Predicate<T> predicate) where T : struct
        {
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T item in enumerable)

                if (predicate(item))

                    return item;

            return null;
        }

        public static TResult? FirstOrNull<TItems, TResult>(this IEnumerable<TItems> enumerable) where TResult : struct
        {
            foreach (TItems item in enumerable)

                if (item is TResult result)

                    return result;

            return null;
        }
#if !WinCopies4
        public static IEnumerable<T> AsReadOnlyEnumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (T
#if CS9
                ?
#endif
                item in enumerable)

                yield return item;
        }

        public static IEnumerable<U> As<T, U>(this IEnumerable<T> enumerable) where T : U
        {
            foreach (T
#if CS9
                ?
#endif
                item in enumerable)

                yield return item;
        }
#endif
        public static bool Any(this IEnumerable enumerable, Func<object, bool> func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

            foreach (object value in enumerable)

                if (func(value))

                    return true;

            return false;
        }

        public static bool AnyPredicate(this IEnumerable enumerable, Predicate func)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));

            foreach (object value in enumerable)

                if (func(value))

                    return true;

            return false;
        }

        public static bool Any<T>(this IEnumerable enumerable) => enumerable.Any(item => item is T);

        public static bool AllPredicate<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            foreach (T item in source)

                if (!predicate(item))

                    return false;

            return true;
        }

        public static bool AnyPredicate<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            foreach (T item in source)

                if (predicate(item))

                    return true;

            return false;
        }

        public static IEnumerable<TOut> SelectConverterIfNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, TOut> converter) where TOut : class
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value;
        }

        public static IEnumerable<TOut> SelectIfNotNull<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> converter) where TOut : class
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value;
        }

        public static IEnumerable<TOut> SelectConverterIfNotNullStruct<TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, TOut?> converter) where TOut : struct
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut? value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value.Value;
        }

        public static IEnumerable<TOut> SelectIfNotNullStruct<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut?> converter) where TOut : struct
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            TOut? value;

            foreach (TIn item in enumerable)

                if ((value = converter(item)) != null)

                    yield return value.Value;
        }

        public static T FirstOrValue<T>(this IEnumerable<T> enumerable, in T value)
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

        public static T
#if CS9
            ?
#endif
            FirstOrValue<T>(this IEnumerable<T> enumerable, in Func<T
#if CS9
            ?
#endif
            > func)
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

        public static bool FirstOrValue<T>(this IEnumerable<T> enumerable, in T value, out T result)
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

        public static bool FirstOrValue<T>(this IEnumerable<T> enumerable, in Func<T> func, out T result)
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

        public static T
#if CS9
            ?
#endif
            FirstOrDefaultValue<T>(this IEnumerable<T> enumerable) => enumerable.FirstOrValue(() => default);

        public static bool FirstOrDefaultValue<T>(this IEnumerable<T> enumerable, out T
#if CS9
            ?
#endif
            result) => enumerable.FirstOrValue(default(T), out result);

        public static T
#if CS9
            ?
#endif
            FirstValue<T>(this IEnumerable<T> enumerable) => enumerable.FirstOrValue(() => throw GetNoItemException());

        public static bool FirstValue<T>(this IEnumerable<T> enumerable, out T result) => enumerable.FirstOrValue(() => throw GetNoItemException(), out result);

        public static T FirstOrValue<T>(this IEnumerable<T> enumerable, in T value, in Func<T, bool> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return value;
        }

        public static T FirstOrValuePredicate<T>(this IEnumerable<T> enumerable, in T value, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return value;
        }

        public static T
#if CS9
            ?
#endif
            FirstOrValue<T>(this IEnumerable<T> enumerable, in Func<T
#if CS9
            ?
#endif
            > func, in Func<T, bool> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return func();
        }

        public static T
#if CS9
            ?
#endif
            FirstOrValuePredicate<T>(this IEnumerable<T> enumerable, in Func<T
#if CS9
            ?
#endif
            > func, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(func, nameof(func));
            ThrowIfNull(predicate, nameof(predicate));

            foreach (T _value in enumerable)

                if (predicate(_value))

                    return _value;

            return func();
        }

        public static bool FirstOrValue<T>(this IEnumerable<T> enumerable, in T value, in Func<T, bool> predicate, out T result)
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

        public static bool FirstOrValuePredicate<T>(this IEnumerable<T> enumerable, in T value, in Predicate<T> predicate, out T result)
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

        public static bool FirstOrValue<T>(this IEnumerable<T> enumerable, in Func<T
#if CS9
            ?
#endif
            > func, in Func<T, bool> predicate, out T
#if CS9
            ?
#endif
            result)
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

        public static bool FirstOrValuePredicate<T>(this IEnumerable<T> enumerable, in Func<T
#if CS9
            ?
#endif
            > func, in Predicate<T> predicate, out T
#if CS9
            ?
#endif
            result)
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

        public static T
#if CS9
            ?
#endif
            FirstOrDefault<T>(this IEnumerable<T> enumerable, in Func<T, bool> func) => enumerable.FirstOrValue(() => default, func);

        public static T
#if CS9
            ?
#endif
            FirstOrDefaultPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> func) => enumerable.FirstOrValuePredicate(() => default, func);

        public static bool FirstOrDefault<T>(this IEnumerable<T> enumerable, in Func<T, bool> func, out T
#if CS9
            ?
#endif
            result) => enumerable.FirstOrValue(() => default, func, out result);

        public static bool FirstOrDefaultPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> func, out T
#if CS9
            ?
#endif
            result) => enumerable.FirstOrValuePredicate(() => default, func, out result);

        public static T
#if CS9
            ?
#endif
            FirstValue<T>(this IEnumerable<T> enumerable, in Func<T, bool> func) => enumerable.FirstOrValue(() => throw GetNoItemException(), func);

        public static T
#if CS9
            ?
#endif
            FirstPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> func) => enumerable.FirstOrValuePredicate(() => throw GetNoItemException(), func);

        public static bool FirstValue<T>(this IEnumerable<T> enumerable, in Func<T, bool> func, out T
#if CS9
            ?
#endif
            result) => enumerable.FirstOrValue(() => throw GetNoItemException(), func, out result);

        public static bool FirstPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> func, out T
#if CS9
            ?
#endif
            result) => enumerable.FirstOrValuePredicate(() => throw GetNoItemException(), func, out result);

        public static IEnumerable<T> Select<T>(this IEnumerable enumerable, Func<object, T> func)
        {
            foreach (object item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                yield return func(item);
        }

        public static IEnumerable<TValue> OrderBy<TValue, TKey>(this IEnumerable<TValue> enumerable, Func<TValue, TKey> func, Comparison<TKey> comparison) => enumerable.OrderBy(func, new Comparer2<TKey>(comparison));

        public static T
#if CS9
            ?
#endif
            FirstOrDefault<T>(this IEnumerable enumerable, in Func<object, bool> predicate)
        {
            foreach (object item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is T _item && predicate(item))

                    return _item;

            return default;
        }

        public static TOut
#if CS9
            ?
#endif
            FirstOrDefault<TIn, TOut>(this IEnumerable<TIn> enumerable, in Func<TIn, bool> predicate)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is TOut _item && predicate(item))

                    return _item;

            return default;
        }

        public static TOut
#if CS9
            ?
#endif
            PredicateFirstOrDefault<TIn, TOut>(this IEnumerable<TIn> enumerable, in Predicate<TIn> predicate)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is TOut _item && predicate(item))

                    return _item;

            return default;
        }

        public static TOut
#if CS9
            ?
#endif
            FirstOrDefault2<TIn, TOut>(this IEnumerable<TIn> enumerable, in Func<TOut, bool> predicate)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is TOut _item && predicate(_item))

                    return _item;

            return default;
        }

        public static TOut
#if CS9
            ?
#endif
            FirstOrDefaultPredicate2<TIn, TOut>(this IEnumerable<TIn> enumerable, in Predicate<TOut> predicate)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is TOut _item && predicate(_item))

                    return _item;

            return default;
        }

        public static TOut
#if CS9
            ?
#endif
            FirstOrDefault<TIn, TOut>(this IEnumerable<TIn> enumerable, in Func<TIn, bool> inPredicate, in Func<TOut, bool> outPredicate)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is TOut _item && inPredicate(item) && outPredicate(_item))

                    return _item;

            return default;
        }

        public static TOut
#if CS9
            ?
#endif
            PredicateFirstOrDefault<TIn, TOut>(this IEnumerable<TIn> enumerable, in Predicate<TIn> inPredicate, in Predicate<TOut> outPredicate)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (item is TOut _item && inPredicate(item) && outPredicate(_item))

                    return _item;

            return default;
        }

        public static IEnumerable<TOut> WhereSelect<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, bool> where, Func<TIn, TOut> select)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (where(item))

                    yield return select(item);
        }

        public static bool AnyPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate, in bool ifValidated)
        {
            foreach (T item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (predicate(item)) return ifValidated;

            return !ifValidated;
        }

        public static bool AnyPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate) => enumerable.AnyPredicate(predicate, true);

        public static bool AllPredicate<T>(this IEnumerable<T> enumerable, in Predicate<T> predicate) => enumerable.AnyPredicate(predicate.Reverse(), false);

        /// <summary>
        /// Yield returns each object of an <see cref="IEnumerable"/>, so the given <see cref="IEnumerable"/> will be considered as an <see cref="IEnumerable{Object}"/>.
        /// </summary>
        /// <param name="enumerable">An <see cref="IEnumerable"/> to consider as a <see cref="IEnumerable{Object}"/>.</param>
        /// <returns>Yield returns the same enumerable as the given <paramref name="enumerable"/>, as an <see cref="IEnumerable{Object}"/>.</returns>
        public static IEnumerable<object> AsObjectEnumerable(this IEnumerable enumerable)
        {
            foreach (object item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                yield return item;
        }

        public static IEnumerable<TOut> SelectMany<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, IEnumerable<TOut>> func, Func<TIn, TOut> _func, RecursiveEnumerationOrder enumerationOrder) => Collections.Enumerable.FromEnumerator(new SelectManyEnumerator<TIn, TOut>(enumerable, func, _func, enumerationOrder));

        public static IEnumerable<TOut> SelectManyConverter<TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, IEnumerable<TOut>> func, Converter<TIn, TOut> _func, RecursiveEnumerationOrder enumerationOrder) => Collections.Enumerable.FromEnumerator(new SelectManyEnumerator<TIn, TOut>(enumerable, new Func<TIn, IEnumerable<TOut>>(func), new Func<TIn, TOut>(_func), enumerationOrder));

        public static IEnumerable<T> Join<T>(this IEnumerable<IEnumerable<T>> enumerable) => enumerable.SelectMany(Delegates.Self);
        public static IEnumerable<T> Join<T>(this IEnumerable<Func<IEnumerable<T>>> enumerable) => enumerable.SelectMany(Delegates.GetResult);

        public static Collections.Generic.IEnumerableInfo<T
#if CS9
                ?
#endif
                > Join<T>(this IEnumerable<IEnumerable<T
#if CS9
                ?
#endif
                >
#if CS8
                ?
#endif
                > enumerable, bool keepEmptyEnumerables, params T[] join) => new EnumerableInfo<T>(() => new JoinEnumerator<T>(enumerable, keepEmptyEnumerables, join), null);
        public static IEnumerable<T
#if CS9
                ?
#endif
                > Join<T>(this IEnumerable<Func<IEnumerable<T
#if CS9
                ?
#endif
                >
#if CS8
                ?
#endif
                >> enumerable, bool keepEmptyEnumerables, params T[] join) => enumerable.Select(Delegates.GetResult).Join(keepEmptyEnumerables, join);

        public static IEnumerable<T> WherePredicate<T>(this System.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> func)
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

        public static IEnumeratorInfo2<TDestination> SelectConverter<TSource, TDestination>(this IEnumerator<TSource> enumerator, Converter<TSource, TDestination> func) => new SelectEnumerator<TSource, TDestination>(enumerator, func);

        public static Collections.Generic.IEnumerableInfo<TOut> SelectConverter<TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, TOut> selector) => new EnumerableInfo<TOut>(() => new SelectEnumerator<TIn, TOut>(enumerable, selector), null);

        public static Collections.Generic.IEnumerableInfo<TOut> SelectConverter<TIn, TOut>(this Collections.Generic.IEnumerableInfo<TIn> enumerable, Converter<TIn, TOut> selector) => new EnumerableInfo<TOut>(() => new SelectEnumerator<TIn, TOut>(enumerable, selector), () => new SelectEnumerator<TIn, TOut>(enumerable.AsFromType<Collections.Extensions.IEnumerable<IEnumeratorInfo<TIn>>>().GetReversedEnumerator(), selector));

        public static T Last<T>(this Collections.Extensions.Generic.IEnumerable<T> enumerable)
        {
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))).SupportsReversedEnumeration)
            {
                IEnumerator<T> enumerator = enumerable.GetReversedEnumerator();

                return enumerator.MoveNext() ? enumerator.Current : throw new InvalidOperationException("The enumerable is empty.");
            }

            return enumerable.Last();
        }

        public static T LastPredicate<T>(this Collections.Extensions.Generic.IEnumerable<T> enumerable, Predicate<T> predicate)
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

            return enumerable.Last(item => predicate(item));
        }

        public static T LastOrDefault<T>(this Collections.Extensions.Generic.IEnumerable<T> enumerable)
        {
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))).SupportsReversedEnumeration)
            {
                IEnumerator<T> enumerator = enumerable.GetReversedEnumerator();

                return enumerator.MoveNext() ? enumerator.Current : default;
            }

            return enumerable.LastOrDefault();
        }

        public static T LastOrDefaultPredicate<T>(this Collections.Extensions.Generic.IEnumerable<T> enumerable, Predicate<T> predicate)
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

            return enumerable.LastOrDefault(item => predicate(item));
        }

        /// <summary>
        /// Returns the first item, if any, from <typeparamref name="T"/> in a given <see cref="IEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to return.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable"/> in which to look for the first item of the given type.</param>
        /// <returns>The first item, if any, from <typeparamref name="T"/> in <paramref name="enumerable"/> or the default value for <typeparamref name="T"/> if none value was found.</returns>
        /// <seealso cref="LastOrDefault{T}(IEnumerable)"/>
        /// <seealso cref="FirstOrDefaultPredicate{T}(IEnumerable, in Predicate{T})"/>
        /// <seealso cref="LastOrDefault{T}(IEnumerable, in Predicate{T})"/>
        public static T FirstOrDefault<T>(this IEnumerable enumerable)
        {
            foreach (object item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

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
        public static T FirstOrDefaultPredicate<T>(this IEnumerable enumerable, in Predicate<T> predicate)
        {
            foreach (object item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

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
        /// <seealso cref="FirstOrDefaultPredicate{T}(IEnumerable, in Predicate{T})"/>
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
        /// <seealso cref="FirstOrDefaultPredicate{T}(IEnumerable, in Predicate{T})"/>
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
            foreach (object item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

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
#endif

        public static TOut FirstOrDefault<TIn, TOut>(this IEnumerable<TIn> enumerable, in Func<TIn, object> func)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

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

        public static IEnumerable AppendValues(this IEnumerable enumerable, IEnumerable<IEnumerable> newValues)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (object obj in enumerable)

                yield return obj;

            foreach (IEnumerable _enumerable in newValues)

                foreach (object _obj in _enumerable)

                    yield return _obj;
        }

        public static IEnumerable AppendValues(this IEnumerable enumerable, params IEnumerable[] newValues) => enumerable.AppendValues(newValues.AsEnumerable());

        public static IEnumerable AppendValues(this IEnumerable enumerable, IEnumerable<object> newValues)
        {
            foreach (object value in enumerable)

                yield return value;

            foreach (object value in newValues)

                yield return value;
        }

        public static IEnumerable AppendValues(this IEnumerable enumerable, params object[] newValues) => enumerable.AppendValues(newValues.AsEnumerable());

        public static IEnumerable<T> AppendValues<T>(this IEnumerable<T> enumerable, IEnumerable<IEnumerable<T>> newValues)
        {
            ThrowIfNull(enumerable, nameof(enumerable));

            foreach (T obj in enumerable)

                yield return obj;

            foreach (IEnumerable<T> _enumerable in newValues)

                foreach (T _obj in _enumerable)

                    yield return _obj;
        }

        public static IEnumerable<T> AppendValues<T>(this IEnumerable<T> enumerable, params IEnumerable<T>[] newValues) => enumerable.AppendValues(newValues.AsEnumerable());

        public static IEnumerable<T> AppendValues<T>(this IEnumerable<T> enumerable, IEnumerable<T> values)
        {
            foreach (T value in enumerable)

                yield return value;

            foreach (T value in values)

                yield return value;
        }

        public static IEnumerable<T> AppendValues<T>(this IEnumerable<T> enumerable, params T[] values) => enumerable.AppendValues(values.AsEnumerable());

        public static IEnumerable<T> Merge<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            foreach (IEnumerable<T> _enumerable in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                foreach (T item in _enumerable)

                    yield return item;
        }

        public static IEnumerable Merge(this IEnumerable<IEnumerable> enumerable)
        {
            foreach (IEnumerable _enumerable in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                foreach (object item in _enumerable)

                    yield return item;
        }

        public static IEnumerable TryMerge(this IEnumerable<IEnumerable> enumerable)
        {
            foreach (IEnumerable _enumerable in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (_enumerable != null)

                    foreach (object item in _enumerable)

                        yield return item;
        }

        public static IEnumerable<T> TryMerge<T>(this IEnumerable<IEnumerable<T>> enumerable)
        {
            foreach (IEnumerable<T> _enumerable in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (_enumerable != null)

                    foreach (T item in _enumerable)

                        yield return item;
        }

        public static IEnumerable<TOut> WherePredicateSelect<TIn, TOut>(this IEnumerable<TIn> enumerable, Predicate<TIn> where, Converter<TIn, TOut> select)
        {
            foreach (TIn item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (where(item))

                    yield return select(item);
        }

        public static IEnumerable<TOut> SelectWhere<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, TOut> select, Func<TOut, bool> where)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (where(_item = select(item)))

                    yield return _item;
        }

        public static IEnumerable<TOut> SelectWherePredicate<TIn, TOut>(this IEnumerable<TIn> enumerable, Converter<TIn, TOut> select, Predicate<TOut> where)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (where((_item = select(item))))

                    yield return _item;
        }

        public static IEnumerable<TOut> WhereSelectWhere<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, bool> inPredicate, Func<TIn, TOut> converter, Func<TOut, bool> outPredicate)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (inPredicate(item))

                    if (outPredicate((_item = converter(item))))

                        yield return _item;
        }

        public static IEnumerable<TOut> WhereSelectWherePredicate<TIn, TOut>(this IEnumerable<TIn> enumerable, Predicate<TIn> inPredicate, Converter<TIn, TOut> converter, Predicate<TOut> outPredicate)
        {
            TOut _item;

            foreach (TIn item in enumerable)

                if (inPredicate(item))

                    if (outPredicate((_item = converter(item))))

                        yield return _item;
        }

        public static IEnumerable PrependValues(this IEnumerable enumerable, IEnumerable values)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(values, nameof(values));

            foreach (object item in values)

                yield return item;

            foreach (object item in enumerable)

                yield return item;
        }

        public static IEnumerable PrependValues(this IEnumerable enumerable, params object[] values) => enumerable.PrependValues(values.AsEnumerable());

        public static IEnumerable PrependValues(this IEnumerable enumerable, IEnumerable<IEnumerable> values)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(values, nameof(values));

            foreach (IEnumerable _values in values)

                foreach (object item in _values)

                    yield return item;

            foreach (object item in enumerable)

                yield return item;
        }

        public static IEnumerable PrependValues(this IEnumerable enumerable, params IEnumerable[] values) => enumerable.PrependValues(values.AsEnumerable());

        public static IEnumerable<T> PrependValues<T>(this IEnumerable<T> enumerable, IEnumerable<T> values)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(values, nameof(values));

            foreach (T item in values)

                yield return item;

            foreach (T item in enumerable)

                yield return item;
        }

        public static IEnumerable<T> PrependValues<T>(this IEnumerable<T> enumerable, params T[] values) => enumerable.PrependValues(values.AsEnumerable());

        public static IEnumerable<T> PrependValues<T>(this IEnumerable<T> enumerable, IEnumerable<IEnumerable<T>> values)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(values, nameof(values));

            foreach (IEnumerable<T> _values in values)

                foreach (T item in _values)

                    yield return item;

            foreach (T item in enumerable)

                yield return item;
        }

        public static IEnumerable<T> PrependValues<T>(this IEnumerable<T> enumerable, params IEnumerable<T>[] values) => enumerable.PrependValues(values.AsEnumerable());

        public static IEnumerable<T> Surround<T>(this IEnumerable<T> enumerable, IEnumerable<T> firstItems, IEnumerable<T> lastItems)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(firstItems, nameof(firstItems));
            ThrowIfNull(lastItems, nameof(lastItems));

            foreach (T item in firstItems)

                yield return item;

            foreach (T item in enumerable)

                yield return item;

            foreach (T item in lastItems)

                yield return item;
        }

        public static IEnumerable<T> Surround<T>(this IEnumerable<T> enumerable, IEnumerable<IEnumerable<T>> firstItems, IEnumerable<IEnumerable<T>> lastItems)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(firstItems, nameof(firstItems));
            ThrowIfNull(lastItems, nameof(lastItems));

            foreach (IEnumerable<T> _firstItems in firstItems)

                foreach (T item in _firstItems)

                    yield return item;

            foreach (T item in enumerable)

                yield return item;

            foreach (IEnumerable<T> _lastItems in lastItems)

                foreach (T item in _lastItems)

                    yield return item;
        }

        public static IEnumerable<T> SelectConverter<T>(this IEnumerable enumerable, Converter<T> converter)
        {
            foreach (object value in enumerable ?? throw GetArgumentNullException(nameof(enumerable)))

                yield return converter(value);
        }

        public static U SelectFirstOrDefault<T, U>(this IEnumerable<T> enumerable, Predicate<T> predicate, Converter<T, U> converter)
        {
            foreach (T item in enumerable ?? throw new ArgumentNullException(nameof(enumerable)))

                if (predicate(item))

                    return converter(item);

            return default;
        }
    }
}
