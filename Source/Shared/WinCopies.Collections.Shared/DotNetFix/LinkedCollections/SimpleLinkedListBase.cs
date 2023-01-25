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

using System.Threading;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    public abstract class SimpleLinkedListBase : ISimpleLinkedListBase2
    {
        private object _syncRoot;

        /// <summary>
        /// Gets a value indicating whether the current list is read-only.
        /// </summary>
        public abstract bool IsReadOnly { get; }

        public abstract uint Count { get; }

        public bool HasItems => Count != 0;

        object ISimpleLinkedListBase2.SyncRoot => _syncRoot
#if CS8
            ??=
#else
            ?? (_syncRoot =
#endif
            Interlocked.CompareExchange(ref _syncRoot, new object(), null)
#if !CS8
            )
#endif
            ;

        bool ISimpleLinkedListBase2.IsSynchronized => false;

        public void Clear()
        {
            if (IsReadOnly)

                throw GetReadOnlyListOrCollectionException();

            ClearItems();
        }

        protected abstract void ClearItems();
    }

    public abstract class SimpleLinkedListBase<T> : SimpleLinkedListBase where T : ISimpleLinkedListNode<T>
    {
        private uint _count = 0;

        /// <summary>
        /// Gets the number of items in the current list.
        /// </summary>
        public sealed override uint Count => _count;

        public sealed override bool IsReadOnly => false;

        protected internal T FirstItem { get; protected set; }

        protected abstract void OnItemAdded();

        /// <summary>
        /// When overridden in a derived class, removes the first or last item from the current list, depending on the linked list type (FIFO/LIFO).
        /// </summary>
        protected abstract T RemoveItem();

        protected void Increment(in bool actionAfter)
        {
            _count++;

            if (actionAfter)

                OnItemAdded();
        }

        protected void Decrement()
        {
            FirstItem = RemoveItem();

            _count--;
        }

        protected sealed override void ClearItems()
        {
            T node, temp;

            node = FirstItem;

            while (node != null)
            {
                temp = node.Next;

                node.Clear();

                node = temp;
            }

            FirstItem = default;

            _count = 0;
        }
    }
}
