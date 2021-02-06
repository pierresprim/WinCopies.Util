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
using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

#if !WinCopies3
using WinCopies.Collections;
using WinCopies.Util;

using static WinCopies.Util.Util;
#else
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Linq
{
    public static class Extensions
    {
        public static
#if !WinCopies3
System.Collections.Generic.IEnumerable
#else
            IEnumerableInfo
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
            IEnumerableInfo
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

        public static IEnumerableInfo<TOut> SelectConverter<TIn, TOut>(this IEnumerableInfo<TIn> enumerable, Converter<TIn, TOut> selector) => new EnumerableInfo<TOut>(() => new SelectEnumerator<TIn, TOut>(enumerable, selector), () => new SelectEnumerator<TIn, TOut>(enumerable.GetReversedEnumerator(), selector));

        public static T Last<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable)
        {
            if ((enumerable ?? throw GetArgumentNullException(nameof(enumerable))).SupportsReversedEnumeration)
            {
                System.Collections.Generic.IEnumerator<T> enumerator = enumerable.GetReversedEnumerator();

                return enumerator.MoveNext() ? enumerator.Current : throw new InvalidOperationException("The enumerable is empty.");
            }

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).Last();
        }

        public static T Last<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> predicate)
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

        public static T LastOrDefault<T>(this WinCopies.Collections.Generic.IEnumerable<T> enumerable, Predicate<T> predicate)
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
        /// <seealso cref="FirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
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
        public static T FirstOrDefault<T>(this IEnumerable enumerable, in Predicate<T> predicate)
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
        /// <seealso cref="FirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
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
        /// <seealso cref="FirstOrDefault{T}(IEnumerable, in Predicate{T})"/>
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
