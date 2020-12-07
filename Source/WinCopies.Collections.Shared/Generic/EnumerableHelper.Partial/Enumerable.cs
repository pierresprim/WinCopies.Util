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

#if !WinCopies2

using WinCopies.Collections.DotNetFix;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        public interface IEnumerableLinkedList : ILinkedList, IEnumerableInfo<T>
        {
            // Left empty.
        }

        internal class Enumerable : LinkedList, IEnumerableLinkedList, IEnumerableQueue, IEnumerableStack
        {
            #region IEnumerableInfo implementation

            public IEnumeratorInfo2<T> GetEnumerator(EnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

            public IEnumeratorInfo2<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumeratorInfo2<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

            System.Collections.Generic.IEnumerator<T> WinCopies.Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

            #endregion
        }
    }
}

#endif
