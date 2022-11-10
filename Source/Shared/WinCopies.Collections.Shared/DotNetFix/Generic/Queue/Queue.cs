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
    public interface IQueueCore<T> : IQueueCore
    {
        void Enqueue(T
#if CS9
            ?
#endif
            item);
        new T
#if CS9
            ?
#endif
        Dequeue();
        bool TryDequeue(out T
#if CS9
            ?
#endif
            result);
#if CS8
        void IQueueCore.Enqueue(object? value) => Enqueue((T
#if CS9
            ?
#endif
            )value);
        object? IQueueCore.Dequeue() => Dequeue();
        bool IQueueCore.TryDequeue(out object? value)
        {
            if (TryDequeue(out T
#if CS9
                ?
#endif
                _value))
            {
                value = _value;

                return true;
            }

            value = null;

            return false;
        }
#endif
    }

    public interface IQueueCommon<T> : ISimpleLinkedListCommon<T>, IQueueCore<T>
    {
        // Left empty.
    }

    public interface IQueueBase<T> : IQueueBase, ISimpleLinkedListBase,  IQueueCommon<T>
    {
#if CS8
        void IListCommon<T>.Add(T
#if CS9
            ?
#endif
            item) => Enqueue(item);
        T
#if CS9
        ?
#endif
        IListCommon<T>.Remove() => Dequeue();
        bool IListCommon<T>.TryRemove(out T
#if CS9
            ?
#endif
            item) => TryDequeue(out item);
#endif
    }

    public interface IQueue<T> : ISimpleLinkedList<T>, IQueueBase<T>, IQueue
    {
        // Left empty.
    }

    public class Queue<T> : SimpleLinkedList<T>, IQueue<T>
    {
        private SimpleLinkedListNode<T>
#if CS8
            ?
#endif
            _lastItem;

        public void Enqueue(T
#if CS9
            ?
#endif
            item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            if (FirstItem == null)
            {
                actionAfter = true;

                return new SimpleLinkedListNode<T>(item);
            }

            actionAfter = false;

            var newNode = new SimpleLinkedListNode<T>(item);

            _lastItem.Next = newNode;
            _lastItem = newNode;

            return FirstItem;
        }

        protected sealed override void OnItemAdded() => _lastItem = FirstItem;

        public T
#if CS9
            ?
#endif
            Dequeue() => Remove();

        public bool TryDequeue(out T
#if CS9
            ?
#endif
            result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;
#if !CS8
        void IQueueCore.Enqueue(object value) => Enqueue((T)value);
        object IQueueCore.Dequeue() => Dequeue();
        bool IQueueCore.TryDequeue(out object value) => UtilHelpers.TryGetValue<T>(TryRemove, out value);
#endif
    }
}
