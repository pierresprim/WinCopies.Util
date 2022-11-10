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
    public interface IQueueCore
    {
        void Enqueue(object
#if CS8
            ?
#endif
            item);

        object
#if CS8
            ?
#endif
            Dequeue();

        bool TryDequeue(out object
#if CS9
            ?
#endif
            result);
    }

    public interface IQueueBase : ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCommon, IQueueCore
    {
#if CS8
        void IListCommon.Add(object? item) => Enqueue(item);
        object? IListCommon.Remove() => Dequeue();
        bool IListCommon.TryRemove(out object? item) => TryDequeue(out item);
#endif
    }

    public interface IQueue : ISimpleLinkedList, IQueueBase
    {
        // Left empty.
    }

    public class Queue : SimpleLinkedList, IQueue
    {
        private SimpleLinkedListNode
#if CS8
            ?
#endif
            _lastItem;
#if !WinCopies3
        public new bool IsReadOnly => base.IsReadOnly;
        public new uint Count => base.Count;
        public new object Peek() => base.Peek();
#endif
        public void Enqueue(object
#if CS8
            ?
#endif
            item) => Add(item);

        protected sealed override SimpleLinkedListNode AddItem(object
#if CS8
            ?
#endif
            value, out bool actionAfter)
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

        protected sealed override SimpleLinkedListNode RemoveItem() => FirstItem.Next;
    }
}
