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
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix;
using WinCopies.Linq;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        public interface IEnumerableLinkedList : ILinkedList, IEnumerableInfo<T>, IAsEnumerable<ILinkedListNode>
        {
            ILinkedListNode FirstNode { get; }
            ILinkedListNode LastNode { get; }

            IEnumeratorInfo<ILinkedListNode> GetNodeEnumerator(EnumerationDirection enumerationDirection);

            IEnumerable<ILinkedListNode> AsEnumerable(EnumerationDirection enumerationDirection);
        }

        internal class Enumerable : LinkedList, IEnumerableLinkedList, IEnumerableQueue, IEnumerableStack
        {
            public bool SupportsReversedEnumeration => true;

            ILinkedListNode IEnumerableLinkedList.FirstNode => FirstNode;
            ILinkedListNode IEnumerableLinkedList.LastNode => LastNode;

            public IEnumeratorInfo<ILinkedListNode> GetNodeEnumerator(EnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

            public IEnumerable<ILinkedListNode> AsEnumerable(EnumerationDirection enumerationDirection) => Collections.Enumerable.FromEnumerator(GetNodeEnumerator(enumerationDirection));

            public IEnumerable<ILinkedListNode> AsEnumerable() => AsEnumerable(EnumerationDirection.FIFO);

            public IEnumeratorInfo<T> GetEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

            public IEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

            IEnumerator<T>
#if WinCopies3
                Extensions.
#endif
                Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

#if !CS8
            System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }
}
#endif
