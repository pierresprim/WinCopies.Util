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

using System;

namespace WinCopies.Collections.DotNetFix
{
    internal static class SimpleLinkedListNodeHelper
    {
        public static InvalidOperationException GetIsClearedException() => new
#if !CS9
            InvalidOperationException
#endif
            ("The node is cleared.");
    }

    public abstract class SimpleLinkedListNodeBase<T> : ISimpleLinkedListNodeBase where T : ISimpleLinkedListNode<T>
    {
        private T _next;

        public bool IsCleared { get; private set; }

        public T Next { get => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _next; internal set => _next = IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : value; }

        ISimpleLinkedListNodeBase ISimpleLinkedListNodeBase.Next => Next;

        protected abstract void ResetValue();

        public void Clear()
        {
            ResetValue();

            Next = default;

            IsCleared = true;
        }
    }

    public class SimpleLinkedListNode : SimpleLinkedListNodeBase<SimpleLinkedListNode>, ISimpleLinkedListNode<SimpleLinkedListNode>, ISimpleLinkedListNode2
    {
        private object _value;

        public object Value => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _value;

        ISimpleLinkedListNode2 ISimpleLinkedListNodeBase<ISimpleLinkedListNode2>.Next => Next;

        // public SimpleLinkedListNode Previous { get; private set; }
        //{
        //get => _next; internal set =>
        //{
        //if (value == null)
        //{
        //    if (_next != null)

        //        _next.Previous = null;
        //}

        //else

        //    value.Previous = this;

        //_next = value;
        //}
        //}

        public SimpleLinkedListNode(object value) => _value = value;

        protected override void ResetValue() => _value = null;
    }
}
