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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public abstract class EnumerableSimpleLinkedList<T> : EnumerableSimpleLinkedListBase, IEnumerableSimpleLinkedList<T>
    {
        public abstract T Peek();

        public abstract bool TryPeek(out T result);

        public void CopyTo(T[] array, int arrayIndex) => WinCopies.
#if !WinCopies3
                Util.Extensions
#else
                Collections.EnumerableExtensions
#endif
                .CopyTo(this, array, arrayIndex, Count);

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract System.Collections.Generic.IEnumerator<T> GetEnumerator();

        public T[] ToArray()
        {
            if (Count > int.MaxValue)

                throw new ArgumentOutOfRangeException("Too many items in list or collection.");

            T[] result = new T[Count];

            int i = -1;

            foreach (T value in this)

                result[++i] = value;

            return result;
        }
    }
}