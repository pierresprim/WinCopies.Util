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

#if CS7
using System;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public partial class LinkedList<T>
    {
        public class LinkedListNode : ILinkedListNode<T, LinkedListNode, IReadOnlyLinkedListNode<T>, LinkedList<T>>, ILinkedListNode<T>
        {
            private T _value;
            private IReadOnlyLinkedListNode<T> _asReadOnly;

            public bool IsReadOnly => false;

            public LinkedList<T> List { get; internal set; }

            public LinkedListNode Previous { get; internal set; }
            public LinkedListNode Next { get; internal set; }

            public T Value { get => _value; set => _value = value; }
            public bool IsAvailableByRef => true;
            public ref T ValueRef => ref _value;

            ILinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>>.Previous => Previous;
            ILinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>>.Next => Next;

            ILinkedList3<T> IReadOnlyLinkedListNodeBase3<ILinkedList3<T>>.List => List;
#if !CS8
            object IReadOnlyLinkedListNode.Value => Value;

            IReadOnlyLinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>.Previous => Previous;
            IReadOnlyLinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>.Next => Next;
#endif
            public LinkedListNode(in T value) => Value = value;

            internal void OnRemove()
            {
                List = null;
                Previous = null;
                Next = null;
            }

            public IReadOnlyLinkedListNode<T> ToReadOnly() => _asReadOnly
#if CS8
                ??=
#else
                ?? (_asReadOnly =
#endif
                new ReadOnlyLinkedListNode<T>(this)
#if !CS8
                    )
#endif
                ;
        }
    }
}
#endif
