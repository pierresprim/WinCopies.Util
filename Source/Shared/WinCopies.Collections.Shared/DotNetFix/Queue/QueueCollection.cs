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

namespace WinCopies.Collections.DotNetFix
{
    public class QueueCollection : SimpleLinkedCollectionBase<IEnumerableQueue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class.
        /// </summary>
        public QueueCollection() : this(new EnumerableQueue()) { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="QueueCollection"/>.</param>
        public QueueCollection(in IEnumerableQueue queue) : base(queue) { /* Left empty. */ }

        /// <summary>
        /// Adds an object to the end of the <see cref="QueueCollection"/>.
        /// <param name="item">The object to add to the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        public virtual void Enqueue(object
#if CS8
            ?
#endif
            item) => InnerList.Enqueue(item);

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public virtual object
#if CS8
            ?
#endif
            Dequeue() => InnerList.Dequeue();

        public virtual bool TryDequeue(out object
#if CS8
            ?
#endif
            result) => InnerList.TryDequeue(out result);
    }

    public class StackCollection : SimpleLinkedCollectionBase<IEnumerableStack>
    {
        public StackCollection() : this(new EnumerableStack()) { /* Left empty. */ }
        public StackCollection(in IEnumerableStack stack) : base(stack) { /* Left empty. */ }

        public virtual void Push(object
#if CS8
            ?
#endif
            item) => InnerList.Push(item);
        public virtual object
#if CS8
            ?
#endif
            Pop() => InnerList.Pop();
        public virtual bool TryPop(out object
#if CS8
            ?
#endif
            result) => InnerList.TryPop(out result);
    }
}
