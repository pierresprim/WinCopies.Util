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
using System.Diagnostics;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

#if CS8
using System.Diagnostics.CodeAnalysis;
#endif

namespace WinCopies.Collections.DotNetFix
{
    namespace Generic
    {
        public class QueueCollection<TQueue, TItems> : SimpleLinkedCollection<TQueue, TItems>, IQueue<TItems> where TQueue : IQueue<TItems>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom queue.
            /// </summary>
            /// <param name="queue">The inner queue for this <see cref="QueueCollection{T}"/>.</param>
            public QueueCollection(in TQueue queue) : base(queue) { /* Left empty. */ }

            /// <summary>
            /// Adds an object to the end of the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="QueueCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            public virtual void Enqueue(TItems
#if CS9
                ?
#endif
                item) => InnerList.Enqueue(item);

            /// <summary>
            /// Removes and returns the object at the beginning of the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection{T}"/>.</returns>
            /// <exception cref="InvalidOperationException">The <see cref="QueueCollection{T}"/> is empty.</exception>
            /// <seealso cref="TryDequeue(out TItems)"/>
            public virtual TItems Dequeue() => InnerList.Dequeue();

            /// <summary>
            /// Tries to remove and return the object at the beginning of the <see cref="QueueCollection{T}"/>.
            /// </summary>
            /// <param name="result">The object at the beginning of the <see cref="QueueCollection{T}"/>. This value can be <see langword="null"/> when the return value is <see langword="false"/>.</param>
            /// <returns>A <see cref="bool"/> value that indicates whether a value has actually been removed and retrieved.</returns>
            /// <seealso cref="Dequeue"/>
            public virtual bool TryDequeue(
#if CS8
                [MaybeNullWhen(false)]
#endif
                out TItems result) => InnerList.TryDequeue(out result);
#if !CS8
            void IQueueCore.Enqueue(object item) => Enqueue((TItems)item);
            object IQueueCore.Dequeue() => Dequeue();
            public bool TryDequeue(out object result) => UtilHelpers.TryGetValue<TItems>(TryDequeue, out result);
#endif
        }

        public class QueueCollection<T> : QueueCollection<IQueue<T>, T>
        {
            public QueueCollection() : this(new Queue<T>()) { /* Left empty. */ }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueCollection{T}"/> class with a custom queue.
            /// </summary>
            /// <param name="queue">The inner queue for this <see cref="QueueCollection{T}"/>.</param>
            public QueueCollection(in IQueue<T> queue) : base(queue) { /* Left empty. */ }
        }
    }
}
#endif
