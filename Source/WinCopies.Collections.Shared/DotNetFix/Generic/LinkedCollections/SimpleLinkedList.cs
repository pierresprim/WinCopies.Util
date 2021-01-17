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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public abstract class SimpleLinkedList<T> : SimpleLinkedListBase, ISimpleLinkedList<T>
    {
        private uint _count = 0;

        public sealed override bool IsReadOnly => false;

        protected internal SimpleLinkedListNode<T> FirstItem { get; private set; }

        public sealed override uint Count => _count;

        protected void Add(in T item)
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

            FirstItem = AddItem(item, out bool actionAfter);

            _count++;

            if (actionAfter)

                OnItemAdded();
        }

        protected abstract SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter);

        protected abstract void OnItemAdded();

        private T OnRemove()
        {
            SimpleLinkedListNode<T> firstItem = FirstItem;

            T result = firstItem.Value;

            FirstItem = RemoveItem();

            firstItem.Clear();

            _count--;

            return result;
        }

        protected T Remove()
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

#if !WinCopies3
            ThrowIfEmpty
#else
            ThrowIfEmptyListOrCollection
#endif
            (this);

            return OnRemove();
        }

        protected bool TryRemove(out T result)
        {
            if (IsReadOnly || Count == 0)
            {
                result = default;

                return false;
            }

            result = OnRemove();

            return true;
        }

        protected abstract SimpleLinkedListNode<T> RemoveItem();

#if !WinCopies3
        public
#else
        protected
#endif
            sealed override void ClearItems()
        {
            SimpleLinkedListNode<T> node, temp;
            node = FirstItem;

            while (node != null)
            {
                temp = node.Next;

                node.Clear();

                node = temp;
            }

            FirstItem = null;

            _count = 0;
        }

        protected T _Peek() => FirstItem.Value;

        public T Peek() => _count > 0 ? _Peek() : throw GetEmptyListOrCollectionException();

        public bool TryPeek(out T result)
        {
            if (_count > 0)
            {
                result = _Peek();

                return true;
            }

            result = default;

            return false;
        }
    }
}