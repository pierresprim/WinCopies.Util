﻿/* Copyright © Pierre Sprimont, 2020
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

namespace WinCopies.Collections.DotNetFix
{
    [Serializable]
    public class ReadOnlyQueueCollection : IEnumerable, ICollection
#if !WinCopies3
        , ICloneable
#endif
    {
        protected
#if !WinCopies3
            System.Collections.Queue
#else
IEnumerableQueue
#endif
            InnerQueue

        { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="QueueCollection"/>.</value>
        public
#if !WinCopies3
int
#else
            uint
#endif
            Count => InnerQueue.Count;

#if WinCopies3
        int ICollection.Count => (int)Count;
#endif

        public bool IsReadOnly => true;

        bool ICollection.IsSynchronized => ((ICollection)InnerQueue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerQueue).SyncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="QueueCollection"/>.</param>
        public ReadOnlyQueueCollection(in
#if !WinCopies3
            System.Collections.Queue
#else
            IEnumerableQueue
#endif
            queue) => InnerQueue = queue;

        public ReadOnlyQueueCollection(in QueueCollection queueCollection) : this(queueCollection.InnerQueue) { }

#if !WinCopies3
        /// <summary>
        /// Creates a shallow copy of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>A shallow copy of the <see cref="QueueCollection"/>.</returns>
        public object Clone() => InnerQueue.Clone();
#endif

        /// <summary>
        /// Determines whether an element is in the <see cref="QueueCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is found in the <see cref="System.Collections.Queue"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(object item) => InnerQueue.Contains(item);

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to an existing one-dimensional <see cref="System.Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="QueueCollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is multidimensional. -or- The number of elements in the source <see cref="QueueCollection"/> is greater than the available space from index to the end of the destination array.</exception>
        /// <exception cref="ArrayTypeMismatchException">The type of the source <see cref="QueueCollection"/> cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(Array array, int index) => InnerQueue.CopyTo(array, index);

        /// <summary>
        /// Returns the object at the beginning of the <see cref="QueueCollection"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object Peek() => InnerQueue.Peek();

        /// <summary>
        /// Copies the <see cref="QueueCollection"/> elements to a new array.
        /// </summary>
        /// <returns>A new array containing elements copied from the <see cref="QueueCollection"/>.</returns>
        public object[] ToArray() => InnerQueue.ToArray();

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> for the <see cref="QueueCollection"/>.</returns>
        public System.Collections.IEnumerator GetEnumerator() => InnerQueue.GetEnumerator();
    }
}
