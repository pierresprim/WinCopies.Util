/* Copyright © Pierre Sprimont, 2021
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

#if CS7
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

using WinCopies.Collections;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

using static WinCopies.Consts
#if WinCopies4
    .Common
#endif
    ;

namespace WinCopies
{
    public static class EnumerableExtensions
    {
        public static int TryFindIndexOf<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> predicate)
        {
            ThrowIfNull(enumerable, nameof(enumerable));
            ThrowIfNull(predicate, nameof(predicate));

            int i = NotSetIndex;

            foreach (T item in enumerable)
            {
                i++;

                if (predicate(item))

                    return i;
            }

            return NotSetIndex;
        }

        public static uint FindIndexOf<T>(this System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> predicate)
        {
            int result = enumerable.TryFindIndexOf(predicate);

            return result > NotSetIndex ? (uint)result : throw new ResourceNotFoundException(nameof(enumerable));
        }

        public static int TryFindIndexOf(this IEnumerable enumerable, Predicate predicate) => enumerable.AsObjectEnumerable().TryFindIndexOf(predicate.AsObjectPredicate());

        public static uint FindIndexOfObject(this IEnumerable enumerable, in Predicate predicate) => enumerable.AsObjectEnumerable().FindIndexOf(predicate.AsObjectPredicate());

        public static int TryFindIndexOf<T>(this System.Collections.Generic.IEnumerable<T> enumerable, T obj, EqualityComparison<T> comparison) => enumerable.TryFindIndexOf(comparison.GetPredicate(obj));

        public static uint FindIndexOf<T>(this System.Collections.Generic.IEnumerable<T> enumerable, T obj, EqualityComparison<T> comparison) => enumerable.FindIndexOf(comparison.GetPredicate(obj));

        public static int TryFindIndexOfObject<T>(this System.Collections.Generic.IEnumerable<T> enumerable, object obj) => enumerable.TryFindIndexOf(GetPredicate(obj));

        public static uint FindIndexOf<T>(this System.Collections.Generic.IEnumerable<T> enumerable, object obj) => enumerable.FindIndexOfObject(GetPredicate(obj));

        public static int TryFindIndexOf(this IEnumerable enumerable, object obj, EqualityComparison comparison) => enumerable.TryFindIndexOf(comparison.GetPredicate(obj));

        public static uint FindIndexOf(this IEnumerable enumerable, object obj, EqualityComparison comparison) => enumerable.FindIndexOfObject(comparison.GetPredicate(obj));

        public static int TryFindIndexOf(this IEnumerable enumerable, object obj) => enumerable.TryFindIndexOf(GetPredicate(obj));

        public static uint FindIndexOf(this IEnumerable enumerable, object obj) => enumerable.FindIndexOfObject(GetPredicate(obj));

        public static unsafe bool Contains<T>(this
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
            <T> list,
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
           <T> value, EqualityComparison<T> comparison, int? lowerBound, int? upperBound)
        {
            if (lowerBound < 0 ? throw new ArgumentOutOfRangeException(nameof(lowerBound)) : upperBound < 0 ? throw new ArgumentOutOfRangeException(nameof(upperBound)) : lowerBound > upperBound ? throw new ArgumentException($"{nameof(lowerBound)} must be less than or equal to {nameof(upperBound)}.") : list.Count == 0 || value.Count == 0 || (upperBound.HasValue && upperBound.Value == 0))

                return false;

            if (lowerBound.HasValue && lowerBound.Value == 0)

                return true;

            int* countPtr = (int*)IntPtr.Zero;

            if (lowerBound.HasValue || upperBound.HasValue)
            {
                countPtr = (int*)Marshal.AllocHGlobal(Marshal.SizeOf
#if CS7
                    <
#else
                    (typeof(
#endif
                    int
#if CS7
                    >()
#else
                    ))
#endif
                    );

                *countPtr = 0;
            }

            Func<bool, bool?> inLoopCondition;
            Func<bool> postLoopResultDelegate;

            bool contains(in int i)
            {
                for (int j = 0; j < value.Count; j++)

                    if (!comparison(list[i + j], value[j]))

                        return false;

                return true;
            }

            if (upperBound.HasValue)
            {
                inLoopCondition = _value =>
                {
                    if (_value)

                        if ((*countPtr) == upperBound)

                            return false;

                        else

                            (*countPtr)++;

                    return null;
                };

                if (lowerBound.HasValue)

                    postLoopResultDelegate = () =>
                    {
                        try
                        {
                            return *countPtr >= lowerBound.Value;
                        }

                        finally
                        {
                            Marshal.FreeHGlobal((IntPtr)countPtr);
                        }
                    };

                else

                    postLoopResultDelegate = () =>
                    {
                        Marshal.FreeHGlobal((IntPtr)countPtr);

                        return true;
                    };
            }

            else if (lowerBound.HasValue)
            {
                inLoopCondition = _value =>
                {
                    if (_value && ++*countPtr == lowerBound)

                        return true;

                    else

                        return null;
                };

                postLoopResultDelegate = () =>
                {
                    Marshal.FreeHGlobal((IntPtr)countPtr);

                    return false;
                };
            }

            else
            {
                inLoopCondition = _value =>
                {
                    if (_value)

                        return true;

                    return null;
                };

                postLoopResultDelegate = () => false;
            }

            bool? result;

            for (int i = 0; i < list.Count; i++)
            {
                if (value.Count > list.Count - i)

                    return postLoopResultDelegate();

                result = inLoopCondition(contains(i));

                if (result.HasValue)

                    return result.Value;
            }

            return postLoopResultDelegate();
        }

        public static bool Validate<T>(this
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
            <T> list, in
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
            <T> startsWith, in EqualityComparison<T> comparison, in int skipLength, in int? length, int? lowerBound, int? upperBound, params T[] values) => Validate(list, startsWith, comparison, skipLength, length, lowerBound, upperBound, values);

        public static bool Validate<T>(this
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
            <T> list, in
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
            <T> startsWith, in EqualityComparison<T> comparison, in int skipLength, in int? length, int? lowerBound, int? upperBound, in
#if CS7
            System.Collections.Generic.IReadOnlyList
#else
            IReadOnlyList2
#endif
            <T> value)
        {
            ThrowIfNull(list, nameof(list));
            ThrowIfNull(startsWith, nameof(startsWith));
            ThrowIfNull(value, nameof(value));

            if ((!length.HasValue || length <= value.Count) && startsWith.Count + skipLength + (length ?? 0) <= list.Count)
            {
                if (list.Count == 0)

                    return true;

                if (startsWith.Count > 0)

                    for (int i = 0; i < startsWith.Count; i++)
                    {
                        if (!comparison(list[i], startsWith[i]))

                            return false;
                    }

                int startIndex = startsWith.Count + skipLength;

                int _length = length ?? list.Count - startIndex;

                return _length <= 0 || new SubReadOnlyList<T>(list, startIndex, _length).Contains(value, comparison, lowerBound, upperBound);
            }

            else return false; // throw new InvalidArgumentException($"{nameof(length)} must be less than or equal to {nameof(value)} and the total length of {nameof(startsWith)}, {nameof(skipLength)}, and {nameof(length)} must be less than or equal to the length of {nameof(list)}.");
        }
    }
}
#endif
