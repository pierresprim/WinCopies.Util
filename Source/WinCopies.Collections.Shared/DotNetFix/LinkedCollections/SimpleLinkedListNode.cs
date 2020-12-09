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
        public static InvalidOperationException GetIsClearedException() => new InvalidOperationException("The node is cleared.");
    }

    public class SimpleLinkedListNode : ISimpleLinkedListNode
    {
        private object _value;
        private SimpleLinkedListNode _next;

        public bool IsCleared { get; private set; }

        public object Value => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _value;

        // public SimpleLinkedListNode Previous { get; private set; }

        public SimpleLinkedListNode Next { get => IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : _next; internal set => _next = IsCleared ? throw SimpleLinkedListNodeHelper.GetIsClearedException() : value; }
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

        ISimpleLinkedListNode ISimpleLinkedListNode.
#if WinCopies2
                NextNode
#else
                Next
#endif
                => Next;

        public SimpleLinkedListNode(object value) => _value = value;

        public void Clear()
        {
            _value = null;

            Next = null;

            IsCleared = true;
        }
    }
}
