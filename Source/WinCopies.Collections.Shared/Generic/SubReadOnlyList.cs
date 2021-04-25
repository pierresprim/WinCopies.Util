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

#if WinCopies3

using System;
using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

namespace WinCopies.Collections.Generic
{
    public abstract class SubReadOnlyListBase<TList, TItems, TEnumerator>
#if CS7
        : System.Collections.Generic.IReadOnlyList<TItems>
#endif
        where TList : System.Collections.Generic.IReadOnlyList<TItems>
        where TEnumerator : ICountableEnumeratorInfo<TItems>
    {
        protected TList InnerList { get; }

        public abstract bool SupportsReversedEnumeration { get; }

        protected int StartIndex { get; }

        public TItems this[int index] => index.Between(0, Count) ? InnerList[index += StartIndex] : throw new IndexOutOfRangeException($"{nameof(index)} must be between 0 and {nameof(Count)}.");

        public int Count { get; }

        public SubReadOnlyListBase(in TList list, in int startIndex, in int count)
        {
            if (list == null)

                throw WinCopies.ThrowHelper.GetArgumentNullException(nameof(list));

            if (list.Count == 0)

                throw new ArgumentException("The given list is empty.");

            if (startIndex.Between(0, list.Count - 1))

                if (count.Between(0, list.Count))
                {
                    InnerList = list;

                    StartIndex = startIndex;

                    Count = count;
                }

                else throw new IndexOutOfRangeException($"{nameof(count)} must be between 0 and {nameof(list)}.{nameof(list.Count)}.");

            else throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, $"{nameof(startIndex)} must be between 0 and {nameof(list)}.{nameof(list.Count)} - 1.");
        }

        public abstract TEnumerator GetEnumerator();

        public abstract TEnumerator GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class SubReadOnlyList<T> : SubReadOnlyListBase<System.Collections.Generic.IReadOnlyList<T>, T, ArrayEnumerator<T>>
    {
        public override bool SupportsReversedEnumeration => true;

        public SubReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list, in int startIndex, in int count) : base(list, startIndex, count)
        {
            // Left empty.
        }

        public override ArrayEnumerator<T> GetEnumerator() => new
#if !CS9
            ArrayEnumerator<T>
#endif
            (this);

        public override ArrayEnumerator<T> GetReversedEnumerator() => new
#if !CS9
ArrayEnumerator<T>
#endif
            (this, true);
    }
}

#endif
