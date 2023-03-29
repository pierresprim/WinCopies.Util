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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    [Serializable]
    public class ReadOnlySimpleLinkedCollection<TList, TItems> : SimpleLinkedCollectionCore<TList>, ISimpleLinkedListBase2, IListCommon<TItems>, IPeekable<TItems> where TList : IUIntCountable, ISimpleLinkedListBase2, IPeekable<TItems>
    {
        public sealed override bool IsReadOnly => true;

        object ISimpleLinkedListBase2.SyncRoot => InnerList.SyncRoot;
        bool ISimpleLinkedListBase2.IsSynchronized => InnerList.IsSynchronized;

        public ReadOnlySimpleLinkedCollection(in TList list) : base(list) { /* Left empty. */ }

        /// <summary>
        /// Returns the object at the beginning of the <see cref="ReadOnlySimpleLinkedCollection{TList, TItems}"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="ReadOnlySimpleLinkedCollection{TList, TItems}"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="ReadOnlySimpleLinkedCollection{TList, TItems}"/> is empty.</exception>
        /// <seealso cref="TryPeek(out TItems)"/>
        public TItems
#if CS9
            ?
#endif
            Peek() => InnerList.Peek();

        /// <summary>
        /// Tries to peek the object at the beginning of the <see cref="ReadOnlySimpleLinkedCollection{TList, TItems}"/> without removing it.
        /// </summary>
        /// <param name="result">The object at the beginning of the <see cref="ReadOnlySimpleLinkedCollection{TList, TItems}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
        /// <seealso cref="Peek"/>
        public bool TryPeek(
#if CS8
            [MaybeNullWhen(false)]
#endif
            out TItems result) => InnerList.TryPeek(out result);

        void IClearable.Clear() => throw GetReadOnlyListOrCollectionException();

        void IListCommon<TItems>.Add(TItems item) => throw GetReadOnlyListOrCollectionException();
        TItems IListCommon<TItems>.Remove() => throw GetReadOnlyListOrCollectionException();
        bool IListCommon<TItems>.TryRemove(out TItems result) => throw GetReadOnlyListOrCollectionException();

        void IListCommon.Add(object item) => throw GetReadOnlyListOrCollectionException();
        object IListCommon.Remove() => throw GetReadOnlyListOrCollectionException();
        bool IListCommon.TryRemove(out object result) => throw GetReadOnlyListOrCollectionException();
#if !CS8
        object IPeekable.Peek() => Peek();
        bool IPeekable.TryPeek(out object result) => UtilHelpers.TryGetValue<TItems>(TryPeek, out result);
#endif
    }

    public class ReadOnlyQueueCollection<TQueue, TItems> : ReadOnlySimpleLinkedCollection<TQueue, TItems>, IQueue<TItems> where TQueue : IQueue<TItems>
    {
        public ReadOnlyQueueCollection(in TQueue queue) : base(queue) { /* Left empty. */ }
        public ReadOnlyQueueCollection(in QueueCollection<TQueue, TItems> queueCollection) : this(queueCollection.InnerList) { /* Left empty. */ }

        void IQueueCore<TItems>.Enqueue(TItems
#if CS9
            ?
#endif
            item) => throw GetReadOnlyListOrCollectionException();
        TItems IQueueCore<TItems>.Dequeue() => throw GetReadOnlyListOrCollectionException();
        bool IQueueCore<TItems>.TryDequeue(out TItems
#if CS9
            ?
#endif
            result)
        {
            result = default;

            return false;
        }
#if !CS8
        void IQueueCore.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();
        object IQueueCore.Dequeue() => throw GetReadOnlyListOrCollectionException();
        bool IQueueCore.TryDequeue(out object result) => throw GetReadOnlyListOrCollectionException();
#endif
    }

    public class ReadOnlyQueueCollection<T> : ReadOnlyQueueCollection<IQueue<T>, T>
    {
        public ReadOnlyQueueCollection(in IQueue<T> queue) : base(queue) { /* Left empty. */ }
        public ReadOnlyQueueCollection(in QueueCollection<IQueue<T>, T> queueCollection) : this(queueCollection.InnerList) { /* Left empty. */ }
    }

    public class ReadOnlyEnumerableQueueCollection<TQueue, TItems> : ReadOnlyQueueCollection<TQueue, TItems>, IEnumerableQueue<TItems>, IReadOnlyCollection<TItems>, ICollection where TQueue : IEnumerableQueue<TItems>
    {
        protected ICollection InnerCollection => InnerList;

        int ICollection.Count => (int)Count;
        int IReadOnlyCollection<TItems>.Count => (int)Count;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        public ReadOnlyEnumerableQueueCollection(in TQueue queue) : base(queue) { /* Left empty. */ }
        public ReadOnlyEnumerableQueueCollection(in QueueCollection<TQueue, TItems> queueCollection) : this(queueCollection.InnerList) { /* Left empty. */ }

        void IEnumerableSimpleLinkedList<TItems>.CopyTo(TItems[] array, int index) => InnerList.CopyTo(array, index);

        TItems[] IEnumerableSimpleLinkedList<TItems>.ToArray() => InnerList.ToArray();

        public System.Collections.Generic.IEnumerator<TItems> GetEnumerator() => InnerList.GetEnumerator();

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the queue; otherwise, <see langword="false"/>.</returns>
        public bool Contains(TItems item) => InnerList.Contains(item);

        void ICollection.CopyTo(System.Array array, int index) => InnerList.CopyTo(array, index);

        /// <summary>
        /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(in TItems[] array, in int arrayIndex) => InnerList.CopyTo(array, arrayIndex);
#if !CS8
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerList.AsFromType<IEnumerable>().GetEnumerator();
#endif
    }

    public class ReadOnlyEnumerableQueueCollection<T> : ReadOnlyEnumerableQueueCollection<IEnumerableQueue<T>, T>
    {
        public ReadOnlyEnumerableQueueCollection(in IEnumerableQueue<T> queue) : base(queue) { /* Left empty. */ }
        public ReadOnlyEnumerableQueueCollection(in QueueCollection<IEnumerableQueue<T>, T> queueCollection) : this(queueCollection.InnerList) { /* Left empty. */ }
    }
}
#endif
