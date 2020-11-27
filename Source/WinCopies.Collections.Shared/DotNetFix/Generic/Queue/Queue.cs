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
    public interface IQueue<T> : ISimpleLinkedList<T>
    {
        void Enqueue(T item);

        T Dequeue();

#if !WinCopies2
        bool TryDequeue(out T result);
#endif
    }

    public class Queue<T> : SimpleLinkedList<T>, IQueue<T>
    {
        private SimpleLinkedListNode<T> _lastItem;

#if WinCopies2
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
