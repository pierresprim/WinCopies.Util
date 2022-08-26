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
        IStackBase
#else
        IStack
#endif
        <T> :
#if WinCopies3
        ISimpleLinkedListBase, IPeekable<T>
#else
        ISimpleLinkedList<T>
#endif
    {
        void Push(T item);

        T Pop();

#if WinCopies3
        bool TryPop(out T
#if CS9
            ?
#endif
            result);
    }

    public interface IStack<T> : ISimpleLinkedList<T>, IStackBase<T>
    {
        // Left empty.
#endif
    }

    public class Stack<T> : SimpleLinkedList<T>, IStack<T>
    {
#if !WinCopies3
        public new uint Count => base.Count;

        public new T Peek() => base.Peek();
#endif

        public void Push(T item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            actionAfter = false;

            return new SimpleLinkedListNode<T>(item) { Next = FirstItem };
        }

        protected sealed override void OnItemAdded()
        {
            // Left empty.
        }

        public T Pop() => Remove();

        public bool TryPop(out T result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;
    }
}
