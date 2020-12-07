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

        public static
#if WinCopies2
System.Collections.Generic.IEnumerator
#else
            IEnumeratorInfo2
#endif
            <TDestination> Select<TSource, TDestination>(this System.Collections.Generic.IEnumerator<TSource> enumerator, Func<TSource, TDestination> func) => new SelectEnumerator<TSource, TDestination>(enumerator, func);
    }
}
