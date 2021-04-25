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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        [Serializable]
        public class ReadOnlyQueueCollection<
#if WinCopies3
            TQueue, TItems
#else
            T
#endif
            > :
#if WinCopies3
            IQueue<TItems> where TQueue : IQueue<TItems>
#else
IEnumerableQueue<T>, IReadOnlyCollection<T>, ICollection
#endif
        {
            protected
#if !WinCopies3
            System.Collections.Generic.Queue<T>
#else
TQueue
#endif
            InnerQueue
            { get; }

            /// <summary>
            /// Gets the number of elements contained in the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <value>The number of elements contained in the <see cref="QueueCollection{T}"/>.</value>
            public
#if !WinCopies3
int
#else
                uint
#endif
                Count => InnerQueue.Count;

            public bool IsReadOnly => true;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom <see cref="System.Collections.Generic.Queue{T}"/>.
            /// </summary>
            /// <param name="queue">The inner <see cref="System.Collections.Generic.Queue{T}"/> for this <see cref="QueueCollection{T}"/>.</param>
            public ReadOnlyQueueCollection(in
#if WinCopies3
                TQueue
#else
                System.Collections.Generic.Queue<T>
#endif
                queue) => InnerQueue = queue;

            public ReadOnlyQueueCollection(in QueueCollection<
#if WinCopies3
                TQueue, TItems
#else
                T
#endif
                > queueCollection) : this(queueCollection.InnerQueue) { }

            /// <summary>
            /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryPeek(out T)"/>
            public
#if WinCopies3
                TItems
#else
                T
#endif
                 Peek() => InnerQueue.Peek();

#if CS8
            /// <summary>
            /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
            /// <seealso cref="Peek"/>
            public bool TryPeek([MaybeNullWhen(false)] out 
#if WinCopies3
                TItems
#else
                T
#endif
                 result) => InnerQueue.TryPeek(out result);
#endif

#if WinCopies3
            bool IQueue<TItems>.TryPeek(out TItems result) => InnerQueue.TryPeek(out result);

            void IQueue<TItems>.Clear() => throw GetReadOnlyListOrCollectionException();

            void IQueueBase<TItems>.Enqueue(TItems item) => throw GetReadOnlyListOrCollectionException();

            TItems IQueueBase<TItems>.Dequeue() => throw GetReadOnlyListOrCollectionException();

            bool IQueueBase<TItems>.TryDequeue(out TItems result)
            {
                result = default;

                return false;
            }

            bool IQueueBase<TItems>.TryPeek(out TItems result) => InnerQueue.TryPeek(out result);

            void IQueueBase<TItems>.Clear() => throw GetReadOnlyListOrCollectionException();

            bool ISimpleLinkedListBase<TItems>.TryPeek(out TItems result) => InnerQueue.TryPeek(out result);

            void ISimpleLinkedListBase2.Clear() => throw GetReadOnlyListOrCollectionException();

            object ISimpleLinkedListBase2.SyncRoot => InnerQueue.SyncRoot;

            bool ISimpleLinkedListBase2.IsSynchronized => InnerQueue.IsSynchronized;

            bool ISimpleLinkedListBase.HasItems => InnerQueue.HasItems;

            TItems ISimpleLinkedList<TItems>.Peek() => ((ISimpleLinkedList<TItems>)InnerQueue).Peek();

#if !CS8
            bool ISimpleLinkedList.TryPeek(out object result) => InnerQueue.TryPeek(out result);

            object ISimpleLinkedList.Peek() => ((ISimpleLinkedList)InnerQueue).Peek();
#endif
#else
        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the queue; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item) => InnerQueue.Contains(item);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

        /// <summary>
        /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(in T[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);

        void IQueue<T>.Enqueue(T item) => throw GetReadOnlyListOrCollectionException();

        T IQueue<T>.Dequeue() => throw GetReadOnlyListOrCollectionException();
#endif
        }
#if WinCopies3
        public class ReadOnlyQueueCollection<T> : ReadOnlyQueueCollection<IQueue<T>, T>
        {
            public ReadOnlyQueueCollection(in IQueue<T> queue) : base(queue)
            {
                // Left empty.
            }

            public ReadOnlyQueueCollection(in QueueCollection<IQueue<T>, T> queueCollection) : this(queueCollection.InnerQueue)
            {
                // Left empty.
            }
        }

        public class ReadOnlyEnumerableQueueCollection<TQueue, TItems> : ReadOnlyQueueCollection<TQueue, TItems>, IEnumerableQueue<TItems>, IReadOnlyCollection<TItems>, ICollection where TQueue : IEnumerableQueue<TItems>
        {
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<TItems>.Count => (int)Count;

            bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

            public ReadOnlyEnumerableQueueCollection(in TQueue queue) : base(queue)
            {
                // Left empty.
            }

            public ReadOnlyEnumerableQueueCollection(in QueueCollection<TQueue, TItems> queueCollection) : this(queueCollection.InnerQueue)
            {
                // Left empty.
            }

            void IEnumerableSimpleLinkedList<TItems>.CopyTo(TItems[] array, int index) => InnerQueue.CopyTo(array, index);

            TItems[] IEnumerableSimpleLinkedList<TItems>.ToArray() => InnerQueue.ToArray();

            public
#if WinCopies3
                System.Collections.Generic.IEnumerator
#else
IUIntCountableEnumerator
#endif
                <TItems> GetEnumerator() => InnerQueue.GetEnumerator();

            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerQueue).GetEnumerator();

            /// <summary>
            /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            /// <returns><see langword="true"/> if <paramref name="item"/> is found in the queue; otherwise, <see langword="false"/>.</returns>
            public bool Contains(TItems item) => InnerQueue.Contains(item);

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

            /// <summary>
            /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
            /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
            public void CopyTo(in TItems[] array, in int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);
        }

        public class ReadOnlyEnumerableQueueCollection<T> : ReadOnlyEnumerableQueueCollection<IEnumerableQueue<T>, T>
        {
            public ReadOnlyEnumerableQueueCollection(in IEnumerableQueue<T> queue) : base(queue)
            {
                // Left empty.
            }

            public ReadOnlyEnumerableQueueCollection(in QueueCollection<IEnumerableQueue<T>, T> queueCollection) : this(queueCollection.InnerQueue)
            {
                // Left empty.
            }
        }
    }
#endif
}

#endif
