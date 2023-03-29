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

#if CS7
using System;
using System.Collections;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    [Serializable]
    public abstract class ReadOnlyEnumerableSimpleLinkedList<T> : ReadOnlySimpleLinkedList<T>, ICollection where T : IUIntCountable, IPeekable, ISimpleLinkedListBase2, ICollection
    {
        protected ICollection InnerCollection => InnerList;

        int ICollection.Count => (int)Count;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        protected ReadOnlyEnumerableSimpleLinkedList(in T list) : base(list) { /* Left empty. */ }

        public void CopyTo(System.Array array, int arrayIndex) => EnumerableExtensions.CopyTo(this, array, arrayIndex, Count);

        public object[] ToArray() => InnerList.ToArray();

        public System.Collections.IEnumerator GetEnumerator() => InnerList.GetEnumerator();
    }

    public class ReadOnlyEnumerableQueue : ReadOnlyEnumerableSimpleLinkedList<IEnumerableQueue>, IEnumerableQueue
    {
        public ReadOnlyEnumerableQueue(in IEnumerableQueue queue) : base(queue) { /* Left empty. */ }

        void IQueueCore.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();
        object IQueueCore.Dequeue() => throw GetReadOnlyListOrCollectionException();
        bool IQueueCore.TryDequeue(out object result)
        {
            result = null;

            return false;
        }
    }

    public class ReadOnlyEnumerableStack : ReadOnlyEnumerableSimpleLinkedList<IEnumerableStack>, IEnumerableStack
    {
        public ReadOnlyEnumerableStack(in IEnumerableStack stack) : base(stack) { /* Left empty. */ }

        void IStackCore.Push(object item) => throw GetReadOnlyListOrCollectionException();
        object IStackCore.Pop() => throw GetReadOnlyListOrCollectionException();
        bool IStackCore.TryPop(out object result)
        {
            result = null;

            return false;
        }
    }
}
#endif
