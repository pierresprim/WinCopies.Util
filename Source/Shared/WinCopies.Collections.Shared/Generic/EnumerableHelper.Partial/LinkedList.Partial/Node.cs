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

#if WinCopies3
using System;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        public interface ILinkedListNode
        {
            ILinkedList List { get; }

            T Value { get; }

            ILinkedListNode Previous { get; }

            ILinkedListNode Next { get; }

            void Remove();
        }

        internal partial class LinkedList
        {
            internal class Node : ILinkedListNode
            {
                private  LinkedList _list;

                public T Value { get; set; }

                public Node Previous { get; set; }

                public Node Next { get; set; }

                ILinkedList ILinkedListNode.List => _list;

                ILinkedListNode ILinkedListNode.Previous => Previous;

                ILinkedListNode ILinkedListNode.Next => Next;

                public Node(in LinkedList list, in T value)
                {
                    _list = list;
                    Value = value;
                }

                public void Remove()
                {
                    (_list ?? throw new InvalidOperationException("The current node is not registered by any list.")).Remove(this);

                    _list = null;
                }
            }
        }
    }
}
#endif
