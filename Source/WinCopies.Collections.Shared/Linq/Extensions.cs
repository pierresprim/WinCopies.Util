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
using WinCopies.Collections.Generic;

#if WinCopies2
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

#if WinCopies2
        [Obsolete("This method has been replaced by Select<TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, System.Converter<TSource, TDestination> func).")]
        public static System.Collections.Generic.IEnumerator<TDestination> Select<TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, Func<TSource, TDestination> func) => new SelectEnumerator<TSource, TDestination>(enumerator, func);
#endif

        public static
#if WinCopies2
System.Collections.Generic.IEnumerator
#else
            IEnumeratorInfo2
#endif
            <TDestination> Select<TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, Converter<TSource, TDestination> func) => new SelectEnumerator<TSource, TDestination>(enumerator, func);

#if WinCopies3
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

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).Last(item=>predicate(item));
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

            return ((System.Collections.Generic.IEnumerable<T>)enumerable).LastOrDefault(item=>predicate(item));
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
    }
}
