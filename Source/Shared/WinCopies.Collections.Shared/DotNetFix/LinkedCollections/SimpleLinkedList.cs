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
    public abstract class SimpleLinkedList<T> : SimpleLinkedListBase<T>, ISimpleLinkedListCommon where T : ISimpleLinkedListNode<T>
    {
        private object OnRemove()
        {
            T firstItem = FirstItem;

            object result = firstItem.Value;

            firstItem.Clear();

            Decrement();

            return result;
        }

        protected object PeekItem() => FirstItem.Value;

        /// <summary>
        /// When overridden in a derived class, adds a given item to the current list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        protected abstract T AddItem(object
#if CS8
            ?
#endif
            value, out bool actionAfter);

        public object Peek() => HasItems ? PeekItem() : throw GetEmptyListOrCollectionException();

        public bool TryPeek(out object result)
        {
            if (HasItems)
            {
                result = PeekItem();

                return true;
            }

            result = null;

            return false;
        }

        /// <summary>
        /// Adds a given item to the current list.
        /// </summary>
        /// <param name="value">The item to add.</param>
        public void Add(object
#if CS8
            ?
#endif
            value)
        {
            FirstItem = IsReadOnly ? throw GetReadOnlyListOrCollectionException() : AddItem(value, out bool actionAfter);

            Increment(actionAfter);
        }

        /// <summary>
        /// Removes the first or last item from the current list, depending on the linked list type (FIFO/LIFO).
        /// </summary>
        public object
#if CS8
            ?
#endif
            Remove()
        {
            ThrowIfEmptyListOrCollection(IsReadOnly ? throw GetReadOnlyListOrCollectionException() : this);

            return OnRemove();
        }

        public bool TryRemove(out object
#if CS8
            ?
#endif
            result)
        {
            if (IsReadOnly || Count == 0)
            {
                result = null;

                return false;
            }

            result = OnRemove();

            return true;
        }
    }

    public abstract class SimpleLinkedList : SimpleLinkedList<SimpleLinkedListNode>, ISimpleLinkedList
    {
        // Left empty.
    }
}
