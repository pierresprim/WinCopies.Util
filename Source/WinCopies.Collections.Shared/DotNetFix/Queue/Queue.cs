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

namespace WinCopies.Collections.DotNetFix
{
    public interface IQueue : ISimpleLinkedList
    {
        void Enqueue(object item);

        object Dequeue();

#if WinCopies3
        bool TryDequeue(out object result);
#endif
    }

    public class Queue : SimpleLinkedList, IQueue
    {
        private SimpleLinkedListNode _lastItem;

#if WinCopies2
        public new bool IsReadOnly => base.IsReadOnly;

        public new uint Count => base.Count;

        public new object Peek() => base.Peek();
#endif

        public void Enqueue(object item) => Add(item);

        protected sealed override SimpleLinkedListNode AddItem(object value, out bool actionAfter)
        {
            if (FirstItem == null)
            {
                actionAfter = true;

                return new SimpleLinkedListNode(value);
            }

            else
            {
                actionAfter = false;

                var newNode = new SimpleLinkedListNode(value);

                _lastItem.Next = newNode;

                _lastItem = newNode;

                return FirstItem;
            }
        }

        protected sealed override void OnItemAdded() => _lastItem = FirstItem;

        // public object Peek() => Count > 0 ? FirstItem.Value : throw GetExceptionForEmptyObject();

        public object Dequeue() => Remove();

        public bool TryDequeue(out object result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode RemoveItem() => FirstItem.Next;
    }
}
