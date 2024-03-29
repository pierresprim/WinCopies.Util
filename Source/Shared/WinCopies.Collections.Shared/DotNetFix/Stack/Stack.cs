﻿/* Copyright © Pierre Sprimont, 2020
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
    public interface IStackCore
    {
        void Push(object
#if CS8
            ?
#endif
            item);

        object
#if CS8
            ?
#endif
            Pop();

        bool TryPop(out object
#if CS8
            ?
#endif
            result);
    }

    public interface IStackBase : ISimpleLinkedListBase, IPeekable, ISimpleLinkedListCommon, IStackCore
    {
#if CS8
        void IListCommon.Add(object? item) => Push(item);
        object? IListCommon.Remove() => Pop();
        bool IListCommon.TryRemove(out object? item) => TryPop(out item);
#endif
    }

    public interface IStack : ISimpleLinkedList, IStackBase
    {
        // Left empty.
    }

    public class Stack : SimpleLinkedList, IStack
    {
        public void Push(object value) => Add(value);

        protected sealed override SimpleLinkedListNode AddItem(object value, out bool actionAfter)
        {
            actionAfter = false;

            return new SimpleLinkedListNode(value) { Next = FirstItem };
        }

        protected sealed override void OnItemAdded() { /* Left empty. */ }

        public object Pop() => Remove();

        public bool TryPop(out object result) => TryRemove(out result);

        protected sealed override SimpleLinkedListNode RemoveItem() => FirstItem.Next;
    }
}
