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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface
#if WinCopies3
        IQueueBase<T> : ISimpleLinkedListBase
#else
        IQueue<T> : ISimpleLinkedList<T>
#endif
    {
        void Enqueue(T item);

        T Dequeue();

#if WinCopies3
        bool TryDequeue(out T result);

        // These methods are defined here because the Peek operation of the Queue is not the same as the Stack one.

        T Peek();

        bool TryPeek(out T result);

        void Clear();
    }

    public interface IQueue<T> : ISimpleLinkedList<T>, IQueueBase<T>
    {
        // These methods are re-defined to avoid amibguous calls.

        new T Peek();

        new bool TryPeek(out T result);

        new void Clear();
#endif
    }

    public class Queue<T> : SimpleLinkedList<T>, IQueue<T>
    {
        private SimpleLinkedListNode<T> _lastItem;

#if !WinCopies3
        public new uint Count => base.Count;

        public new T Peek() => base.Peek();
#endif

        public void Enqueue(T item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            if (FirstItem == null)
            {
                actionAfter = true;

                return new SimpleLinkedListNode<T>(item);
            }

            else
            {
                actionAfter = false;

                var newNode = new SimpleLinkedListNode<T>(item);

                _lastItem.Next = newNode;

                _lastItem = newNode;

                return FirstItem;
            }
        }

        protected sealed override void OnItemAdded() => _lastItem = FirstItem;

        public T Dequeue() => Remove();

        public bool TryDequeue(out T result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;
    }
}
