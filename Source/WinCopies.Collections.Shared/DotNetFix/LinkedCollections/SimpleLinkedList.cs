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

namespace WinCopies.Collections.DotNetFix
{

    public abstract class SimpleLinkedList : SimpleLinkedListBase, ISimpleLinkedList
    {
        private uint _count = 0;

        public sealed override bool IsReadOnly => false;

        protected internal SimpleLinkedListNode FirstItem { get; private set; }

        /// <summary>
        /// Gets the number of items in the current list.
        /// </summary>
        public sealed override uint Count => _count;

        /// <summary>
        /// Adds a given item to the current list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        protected void Add(in object value)
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

            FirstItem = AddItem(value, out bool actionAfter);

            _count++;

            if (actionAfter)

                OnItemAdded();
        }

        /// <summary>
        /// When overridden in a derived class, adds a given item to the current list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        protected abstract SimpleLinkedListNode AddItem(object value, out bool actionAfter);

        protected abstract void OnItemAdded();

        private object OnRemove()
        {
            object result = FirstItem.Value;

            FirstItem.Clear();

            FirstItem = RemoveItem();

            _count--;

            return result;
        }

        /// <summary>
        /// Removes the first or last item from the current list, depending on the linked list type (FIFO/LIFO).
        /// </summary>
        protected object Remove()
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

#if WinCopies2
            ThrowIfEmpty
#else
            ThrowIfEmptyListOrCollection
#endif
                (this);

            return OnRemove();
        }

        protected bool TryRemove(out object result)
        {
            if (IsReadOnly || Count == 0)
            {
                result = null;

                return false;
            }

            result = OnRemove();

            return true;
        }

        /// <summary>
        /// When overridden in a derived class, removes the first or last item from the current list, depending on the linked list type (FIFO/LIFO).
        /// </summary>
        protected abstract SimpleLinkedListNode RemoveItem();

        protected sealed override void ClearItems()
        {
            SimpleLinkedListNode node, temp;

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

        protected object _Peek() => FirstItem.Value;

        public object Peek() => _count > 0 ? _Peek() : throw GetEmptyListOrCollectionException();

        public bool TryPeek(out object result)
        {
            if (_count > 0)
            {
                result = _Peek();

                return true;
            }

            result = null;

            return false;
        }
    }
}
