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

using WinCopies.Collections.DotNetFix.Generic;

#if CS8
using System.Diagnostics.CodeAnalysis;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        [Serializable]
        public class QueueCollection
            <
#if WinCopies3
        TQueue, TItems
#else
        T
#endif
       > :
#if WinCopies3
        IQueue<TItems>, IUIntCountable
#else
        IEnumerableQueue<T>, System.Collections.Generic.IReadOnlyCollection<
#if WinCopies3
TItems
#else
            T
#endif
            >, ICollection
#endif
#if WinCopies3
            where TQueue : IQueue<TItems>
#endif
        {
            protected internal
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

            public bool IsReadOnly => false;

#if WinCopies3
            bool ISimpleLinkedListBase2.IsSynchronized => InnerQueue.IsSynchronized;

            object ISimpleLinkedListBase2.SyncRoot => InnerQueue.SyncRoot;

            bool ISimpleLinkedListBase.HasItems => InnerQueue.HasItems;
#else
        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class.
        /// </summary>
        public QueueCollection() : this(new System.Collections.Generic.Queue<T>())
        {
            // Left empty.
        }
#endif

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom queue.
            /// </summary>
            /// <param name="queue">The inner queue for this <see cref="QueueCollection{T}"/>.</param>
            public QueueCollection(in
#if !WinCopies3
            System.Collections.Generic.Queue<T>
#else
            TQueue
#endif
             queue) => InnerQueue = queue;

            /// <summary>
            /// Removes all objects from the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            protected virtual void ClearItems() => InnerQueue.Clear();

            /// <summary>
            /// Removes all objects from the <see cref="QueueCollection{T}"/>. Override the <see cref="ClearItems"/> method to provide a custom implementation.
            /// </summary>
            public void Clear() => ClearItems();

            /// <summary>
            /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            protected virtual
#if WinCopies3
TItems
#else
            T
#endif
            DequeueItem() => InnerQueue.Dequeue();

            /// <summary>
            /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>. Override the <see cref="DequeueItem"/> to provide a custom implementation.
            /// </summary>
            /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryDequeue(out T)"/>
            public
#if WinCopies3
TItems
#else
            T
#endif
             Dequeue() => DequeueItem();

            /// <summary>
            /// Adds an object to the end of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            protected virtual void EnqueueItem(
#if WinCopies3
TItems
#else
            T
#endif
             item) => InnerQueue.Enqueue(item);

            /// <summary>
            /// Adds an object to the end of the <see cref="QueueCollection{T}"/>. Override the <see cref="EnqueueItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            public void Enqueue(
#if WinCopies3
TItems
#else
            T
#endif
             item) => EnqueueItem(item);

            /// <summary>
            /// Returns the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <returns>The object at the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryPeek(out TItems)"/>
            public
#if WinCopies3
TItems
#else
            T
#endif
             Peek() => InnerQueue.Peek();

#if !WinCopies3
        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the queue; otherwise, <see langword="false"/>.</returns>
        public bool Contains(T item) => InnerQueue.Contains(item);

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="QueueCollection{T}"/>, if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess() => InnerQueue.TrimExcess();

        /// <summary>
        /// Copies the <see cref="QueueCollection{T}"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="QueueCollection{T}"/>.</returns>
        public T[] ToArray() => InnerQueue.ToArray();
        
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/> for the <see cref="QueueCollection{T}"/>.</returns>
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerQueue.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerQueue.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerQueue).CopyTo(array, index);

        /// <summary>
        /// Copies the <see cref="QueueCollection{T}"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="QueueCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex) => InnerQueue.CopyTo(array, arrayIndex);
#endif

            /// <summary>
            /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>. Override this method to provide a custom implementation.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
            protected virtual bool TryDequeueItem(
#if CS8
                [MaybeNullWhen(false)]
#endif
                out 
#if WinCopies3
TItems
#else
            T
#endif
             result)
#if CS8
            => InnerQueue.TryDequeue(out result);
#else
        {
            if (IsReadOnly || Count == 0)
            {
                result = default;

                return false;
            }

            result = InnerQueue.Dequeue();

            return true;
        }
#endif

            /// <summary>
            /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>. Override the <see cref="TryDequeueItem(out T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
            /// <seealso cref="Dequeue"/>
            public bool TryDequeue(
#if CS8
                [MaybeNullWhen(false)]
#endif
                out 
#if WinCopies3
TItems
#else
            T
#endif
             result) => TryDequeueItem(out result);

            /// <summary>
            /// Tries to peek the object at the beginning of the <see cref="QueueCollection{T}"/> without removing it.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been retrieved.</returns>
            /// <seealso cref="Peek"/>
            public bool TryPeek(
#if CS8
                [MaybeNullWhen(false)]
#endif
                out 
#if WinCopies3
TItems
#else
            T
#endif
             result)
#if CS8
            => InnerQueue.TryPeek(out result);
#else
        {
            if (Count == 0)
            {
                result = default;

                return false;
            }

            result = InnerQueue.Peek();

            return true;
        }

#if WinCopies3 && !CS8
            bool ISimpleLinkedList.TryPeek(out object result) => ((ISimpleLinkedList)InnerQueue).TryPeek(out result);

            object ISimpleLinkedList.Peek() => ((ISimpleLinkedList)InnerQueue).Peek();
#endif
#endif
        }

#if WinCopies3
        public class QueueCollection<T> : QueueCollection<IQueue<T>, T>
        {
            public QueueCollection() : this(new Queue<T>())
            {
                // Left empty.
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom queue.
            /// </summary>
            /// <param name="queue">The inner queue for this <see cref="QueueCollection{T}"/>.</param>
            public QueueCollection(in IQueue<T> queue) : base(queue)
            {
                // Left empty.
            }
        }
    }
#endif
}
#endif
