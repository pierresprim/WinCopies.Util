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

using System.Collections;
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        public interface ILinkedListBase<T>
        {
            T FirstNode { get; }
            T LastNode { get; }
        }

        public interface IEnumerableLinkedListBase<T> : ILinkedListBase<T>, IAsEnumerable<T>
        {
            IEnumeratorInfo<T> GetNodeEnumerator(EnumerationDirection enumerationDirection);

            System.Collections.Generic.IEnumerable<T> AsEnumerable(EnumerationDirection enumerationDirection);
        }

        public interface IEnumerableLinkedList : IEnumerableLinkedListBase<ILinkedListNode>, ILinkedList, IEnumerableInfo
        {
            // Left empty.
        }

        internal class Enumerable : LinkedList, IEnumerableLinkedList, ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCore, IClearable, IQueueCore, IStackCore, IPeekableEnumerableInfo, ILinkedListBase<LinkedList.Node>, IEnumerableQueue, IEnumerableStack
        {
            public bool SupportsReversedEnumeration => true;

            Node ILinkedListBase<Node>.FirstNode => FirstNode;
            Node ILinkedListBase<Node>.LastNode => LastNode;

            ILinkedListNode ILinkedListBase<ILinkedListNode>.FirstNode => FirstNode;
            ILinkedListNode ILinkedListBase<ILinkedListNode>.LastNode => LastNode;

            public IEnumeratorInfo<ILinkedListNode> GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(this, enumerationDirection);

            public System.Collections.Generic.IEnumerable<ILinkedListNode> AsEnumerable(EnumerationDirection enumerationDirection) => Collections.Enumerable.FromEnumerator(GetNodeEnumerator(enumerationDirection));
            public System.Collections.Generic.IEnumerable<ILinkedListNode> AsEnumerable() => AsEnumerable(EnumerationDirection.FIFO);

            public IEnumeratorInfo GetEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

            public IEnumeratorInfo GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);
            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumeratorInfo GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);
            System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();

            void IListCommon.Add(object value) => AddLast(value);
            object IListCommon.Remove() => GetAndRemoveFirst();
            bool IListCommon.TryRemove(out object result) => TryGetAndRemoveFirst(out result);
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            public interface IEnumerableLinkedList : EnumerableHelper.IEnumerableLinkedListBase<ILinkedListNode>, ILinkedList, IEnumerableInfo<T>
            {
                // Left empty.
            }

            internal class Enumerable : LinkedList, IEnumerableLinkedList, ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCore, IClearable, IQueueCore<T>, IStackCore<T>, DotNetFix.Generic.IPeekableEnumerableInfo<T>, EnumerableHelper.ILinkedListBase<LinkedList.Node>, IEnumerableQueue, IEnumerableStack
            {
                public bool SupportsReversedEnumeration => true;

                ILinkedListNode EnumerableHelper.ILinkedListBase<ILinkedListNode>.FirstNode => FirstNode;
                ILinkedListNode EnumerableHelper.ILinkedListBase<ILinkedListNode>.LastNode => LastNode;

                Node EnumerableHelper.ILinkedListBase<Node>.FirstNode => FirstNode;
                Node EnumerableHelper.ILinkedListBase<Node>.LastNode => LastNode;

                void IListCommon<T>.Add(T value) => AddLast(value);
                T IListCommon<T>.Remove() => GetAndRemoveFirst();
                bool IListCommon<T>.TryRemove(out T result) => TryGetAndRemoveFirst(out result);

                void IListCommon.Add(object value) => AddLast((T)value);
                object IListCommon.Remove() => GetAndRemoveFirst();
                bool IListCommon.TryRemove(out object result) => UtilHelpers.TryGetValue<T>(TryGetAndRemoveFirst, out result);

                public IEnumeratorInfo<ILinkedListNode> GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(this, enumerationDirection);

                public System.Collections.Generic.IEnumerable<ILinkedListNode> AsEnumerable(EnumerationDirection enumerationDirection) => Collections.Enumerable.FromEnumerator(GetNodeEnumerator(enumerationDirection));
                public System.Collections.Generic.IEnumerable<ILinkedListNode> AsEnumerable() => AsEnumerable(EnumerationDirection.FIFO);

                public IEnumeratorInfo<T> GetEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);
                public IEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);
                System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

                public IEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);
                System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#if !CS8
                IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();
#endif
            }
        }
    }
}
