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
    public interface IStackCore<T> : IStackCore
    {
        void Push(T item);

        new T Pop();
        bool TryPop(out T
#if CS9
            ?
#endif
            result);
#if CS8
        void IStackCore.Push(object? value) => Push((T
#if CS9
            ?
#endif
            )value);
        object? IStackCore.Pop() => Pop();
        bool IStackCore.TryPop(out object? value)
        {
            if (TryPop(out T
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

    public interface IStackCommon<T> : ISimpleLinkedListCommon<T>, IStackCore<T>
    {
        // Left empty.
    }

    public interface IStackBase<T> : IStackBase, ISimpleLinkedListBase, ISimpleLinkedListCommon<T>, IStackCommon<T>
    {
#if CS8
        void IListCommon<T>.Add(T
#if CS9
            ?
#endif
            item) => Push(item);
        T
#if CS9
        ?
#endif
        IListCommon<T>.Remove() => Pop();
        bool IListCommon<T>.TryRemove(out T
#if CS9
            ?
#endif
            item) => TryPop(out item);
#endif
    }

    public interface IStack<T> : ISimpleLinkedList<T>, IStackBase<T>
    {
        // Left empty.
    }

    public class Stack<T> : SimpleLinkedList<T>, IStack<T>
    {
#if !WinCopies3
        public new uint Count => base.Count;

        public new T Peek() => base.Peek();
#endif
        public void Push(T
#if CS9
            ?
#endif
            item) => Add(item);

        protected sealed override SimpleLinkedListNode<T> AddItem(T item, out bool actionAfter)
        {
            actionAfter = false;

            return new SimpleLinkedListNode<T>(item) { Next = FirstItem };
        }

        protected sealed override void OnItemAdded() { /* Left empty. */ }

        public T
#if CS9
            ?
#endif
            Pop() => Remove();

        public bool TryPop(out T
#if CS9
            ?
#endif
            result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode<T> RemoveItem() => FirstItem.Next;
#if !CS8
        void IStackCore.Push(object value) => Push((T)value);
        object IStackCore.Pop() => Pop();
        bool IStackCore.TryPop(out object value)
        {
            if (TryPop(out T _value))
            {
                value = _value;

                return true;
            }

            value = null;

            return false;
        }
#endif
    }
}
