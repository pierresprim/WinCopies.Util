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

namespace WinCopies.Collections
{
    public static partial class EnumerableHelper
    {
        public interface ILinkedListNodeBase
        {
            void Remove();
        }

        public interface ILinkedListNodeBase<T> : ILinkedListNodeBase
        {
            T Previous { get; }
            T Next { get; }
        }

        internal interface ILinkedListNode<T> : ILinkedListNodeBase<T>
        {
            new T Previous { get; set; }
            new T Next { get; set; }
        }

        public interface ILinkedListNode<TList, TNode> : ILinkedListNodeBase<TNode> where TList : ILinkedListBase where TNode : ILinkedListNode<TList, TNode>
        {
            TList List { get; }
        }

        public interface ILinkedListNode : ILinkedListNode<ILinkedList, ILinkedListNode>
        {
            object Value { get; }
        }

        internal partial class LinkedList
        {
            internal class Node<TList, TNode> : ILinkedListNode<TList, TNode>, ILinkedListNode<TNode> where TList : class, _ILinkedListBase<TNode>, ILinkedListBase where TNode : ILinkedListNode<TList, TNode>
            {
                private TList
#if CS9
                    ?
#endif
                    _list;

                public TNode Previous { get; set; }
                public TNode Next { get; set; }

                public TList List => _list;

                public Node(in TList list) => _list = list;

                public void Remove()
                {
                    (_list ?? throw new InvalidOperationException("The current node is not registered by any list.")).Remove(this);

                    _list = default;
                }
            }

            internal class Node : Node<LinkedList, Node>, ILinkedListNode, ILinkedListNode<Node>
            {
                public object Value { get; set; }

                ILinkedList ILinkedListNode<ILinkedList, ILinkedListNode>.List => List;

                ILinkedListNode ILinkedListNodeBase<ILinkedListNode>.Previous => Previous;
                ILinkedListNode ILinkedListNodeBase<ILinkedListNode>.Next => Next;

                public Node(in LinkedList list, in object value) : base(list) => Value = value;
            }
        }
    }

    namespace Generic
    {
        public static partial class EnumerableHelper<T>
        {
            public interface ILinkedListNode : EnumerableHelper.ILinkedListNode<ILinkedList, ILinkedListNode>
            {
                T Value { get; }
            }

            internal partial class LinkedList
            {
                internal class Node : EnumerableHelper.LinkedList.Node<LinkedList, Node>, ILinkedListNode, EnumerableHelper.ILinkedListNode<Node>
                {
                    public T Value { get; set; }

                    EnumerableHelper<T>.ILinkedList EnumerableHelper.ILinkedListNode<EnumerableHelper<T>.ILinkedList, EnumerableHelper<T>.ILinkedListNode>.List => List;

                    EnumerableHelper<T>.ILinkedListNode EnumerableHelper.ILinkedListNodeBase<EnumerableHelper<T>.ILinkedListNode>.Previous => Previous;
                    EnumerableHelper<T>.ILinkedListNode EnumerableHelper.ILinkedListNodeBase<EnumerableHelper<T>.ILinkedListNode>.Next => Next;

                    public Node(in LinkedList list, in T value) : base(list) => Value = value;
                }
            }
        }
    }
}
