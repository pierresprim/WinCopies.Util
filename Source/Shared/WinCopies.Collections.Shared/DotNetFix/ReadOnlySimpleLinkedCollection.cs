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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    [Serializable]
    public class ReadOnlySimpleLinkedCollection<T> : ReadOnlySimpleLinkedList<T>, IEnumerable, ICollection, IPeekable where T : IPeekable, IEnumerable, ICollection, ISimpleLinkedListBase2
    {
        protected ICollection InnerCollection => InnerList;

        int ICollection.Count => (int)Count;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        protected ReadOnlySimpleLinkedCollection(in T list) : base(list) { /* Left empty. */ }

        public bool Contains(object item) => InnerList.Contains(item);

        public void CopyTo(System.Array array, int index) => InnerList.CopyTo(array, index);

        public object[] ToArray() => InnerList.ToArray(
#if !CS5
            0, (int)Count
#endif
            );

        public System.Collections.IEnumerator GetEnumerator() => InnerList.GetEnumerator();
    }

    public class ReadOnlyQueueCollection : ReadOnlySimpleLinkedCollection<IEnumerableQueue>, IEnumerableQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="QueueCollection"/>.</param>
        public ReadOnlyQueueCollection(in IEnumerableQueue queue) : base(queue) { /* Left empty. */ }
        public ReadOnlyQueueCollection(in QueueCollection queueCollection) : this(queueCollection.InnerList) { /* Left empty. */ }

        void IQueueCore.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();
        object IQueueCore.Dequeue() => throw GetReadOnlyListOrCollectionException();
        bool IQueueCore.TryDequeue(out object result) => throw GetReadOnlyListOrCollectionException();
    }

    public class ReadOnlyStackCollection : ReadOnlySimpleLinkedCollection<IEnumerableStack>, IEnumerableStack
    {
        public ReadOnlyStackCollection(in IEnumerableStack stack) : base(stack) { /* Left empty. */ }
        public ReadOnlyStackCollection(in StackCollection stackCollection) : this(stackCollection.InnerList) { /* Left empty. */ }

        void IStackCore.Push(object item) => throw GetReadOnlyListOrCollectionException();
        object IStackCore.Pop() => throw GetReadOnlyListOrCollectionException();
        bool IStackCore.TryPop(out object result) => throw GetReadOnlyListOrCollectionException();
    }
}
