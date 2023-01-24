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

using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix
{
    public class QueueCollection<T> : SimpleLinkedCollection<T>, IQueue where T : IQueue
    {
        public QueueCollection(in T queue) : base(queue) { /* Left empty. */ }

        /// <summary>
        /// Adds an object to the end of the <see cref="QueueCollection"/>.
        /// <param name="item">The object to add to the <see cref="QueueCollection"/>. The value can be <see langword="null"/> for reference types.</param>
        public void Enqueue(object
#if CS8
            ?
#endif
            item) => Add(item);

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="QueueCollection"/>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="QueueCollection"/>.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="QueueCollection"/> is empty.</exception>
        public object
#if CS8
            ?
#endif
            Dequeue() => Remove();

        public bool TryDequeue(out object
#if CS8
            ?
#endif
            result) => TryRemove(out result);

        public override void Add(object value) => InnerList.Enqueue(value);
        public override object Remove() => InnerList.Dequeue();
        public override bool TryRemove(out object result) => InnerList.TryDequeue(out result);
    }
    public class QueueCollection : QueueCollection<IQueue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class with a custom queue.
        /// </summary>
        /// <param name="queue">The inner queue for this <see cref="QueueCollection"/>.</param>
        public QueueCollection(in IQueue queue) : base(queue) { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueCollection"/> class.
        /// </summary>
        public QueueCollection() : this(new Queue()) { /* Left empty. */ }
    }
    public class EnumerableQueueCollection : QueueCollection<IEnumerableQueue>, IEnumerableQueue
    {
        int System.Collections.ICollection.Count => InnerList.AsFromType<System.Collections.ICollection>().Count;

        object System.Collections.ICollection.SyncRoot => InnerList.AsFromType<System.Collections.ICollection>().SyncRoot;
        bool System.Collections.ICollection.IsSynchronized => InnerList.AsFromType<System.Collections.ICollection>().IsSynchronized;

        public EnumerableQueueCollection(in IEnumerableQueue queue) : base(queue) { /* Left empty. */ }
        public EnumerableQueueCollection() : this(new EnumerableQueue()) { /* Left empty. */ }

        public void CopyTo(System.Array array, int index) => InnerList.AsFromType<System.Collections.ICollection>().CopyTo(array, index);
        public object[] ToArray() => InnerList.ToArray();
        public System.Collections.IEnumerator GetEnumerator() => InnerList.GetEnumerator();
    }
    public class QueueBaseCollection : SimpleLinkedListBaseCollection<IQueueBase>, IQueueBase
    {
        public QueueBaseCollection(in IQueueBase list) : base(list) { /* Left empty. */ }
        public QueueBaseCollection() : this(EnumerableHelper.GetQueue()) { /* Left empty. */ }

        public void Enqueue(object item) => Add(item);
        public object Dequeue() => Remove();
        public bool TryDequeue(out object result) => TryRemove(out result);
    }

    public class StackCollection<T> : SimpleLinkedCollection<T>, IStack where T : IStack
    {
        public StackCollection(in T stack) : base(stack) { /* Left empty. */ }

        public void Push(object
#if CS8
            ?
#endif
            item) => Add(item);
        public object
#if CS8
            ?
#endif
            Pop() => Remove();
        public bool TryPop(out object
#if CS8
            ?
#endif
            result) => TryRemove(out result);

        public override void Add(object value) => InnerList.Push(value);
        public override object Remove() => InnerList.Pop();
        public override bool TryRemove(out object result) => InnerList.TryPop(out result);
    }
    public class StackCollection : StackCollection<IStack>
    {
        public StackCollection(in IStack stack) : base(stack) { /* Left empty. */ }
        public StackCollection() : this(new Stack()) { /* Left empty. */ }
    }
    public class EnumerableStackCollection : StackCollection<IEnumerableStack>, IEnumerableStack
    {
        int System.Collections.ICollection.Count => InnerList.AsFromType<System.Collections.ICollection>().Count;

        object System.Collections.ICollection.SyncRoot => InnerList.AsFromType<System.Collections.ICollection>().SyncRoot;
        bool System.Collections.ICollection.IsSynchronized => InnerList.AsFromType<System.Collections.ICollection>().IsSynchronized;

        public EnumerableStackCollection(in IEnumerableStack stack) : base(stack) { /* Left empty. */ }
        public EnumerableStackCollection() : this(new EnumerableStack()) { /* Left empty. */ }

        public void CopyTo(System.Array array, int index) => InnerList.AsFromType<System.Collections.ICollection>().CopyTo(array, index);
        public object[] ToArray() => InnerList.ToArray();
        public System.Collections.IEnumerator GetEnumerator() => InnerList.GetEnumerator();
    }
    public class StackBaseCollection : SimpleLinkedListBaseCollection<IStackBase>, IStackBase
    {
        public StackBaseCollection(in IStackBase list) : base(list) { /* Left empty. */ }
        public StackBaseCollection() : this(EnumerableHelper.GetStack()) { /* Left empty. */ }

        public void Push(object item) => Add(item);
        public object Pop() => Remove();
        public bool TryPop(out object result) => TryRemove(out result);
    }
}
