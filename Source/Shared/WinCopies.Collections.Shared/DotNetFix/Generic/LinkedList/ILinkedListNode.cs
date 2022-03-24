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
namespace WinCopies.Collections.DotNetFix
{
    public interface IReadOnlyLinkedListNode
    {
#if WinCopies3
        bool IsReadOnly { get; }
#endif

        object Value { get; }

        IReadOnlyLinkedListNode Previous { get; }

        IReadOnlyLinkedListNode Next { get; }
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public interface IReadOnlyLinkedListNodeBase<out T> : IReadOnlyLinkedListNode
        {
            T Value { get; }

#if CS8
            object IReadOnlyLinkedListNode.Value => Value;
#endif
#if WinCopies3
        }

        public interface IReadOnlyLinkedListNodeBase2<out T> : IReadOnlyLinkedListNodeBase<T>
        {
            new IReadOnlyLinkedListNodeBase2<T> Previous { get; }

            new IReadOnlyLinkedListNodeBase2<T> Next { get; }

#if CS8
            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;
#endif

            IReadOnlyLinkedListNodeBase2<T> ToReadOnly();
        }

        public class ReadOnlyLinkedListNode<T> : IReadOnlyLinkedListNodeBase2<T>
        {
            protected IReadOnlyLinkedListNodeBase2<T> InnerNode { get; }

            public bool IsReadOnly => true;

            public T Value => InnerNode.Value;

            public IReadOnlyLinkedListNodeBase2<T> Previous => InnerNode.Previous.ToReadOnly();

            public IReadOnlyLinkedListNodeBase2<T> Next => InnerNode.Next.ToReadOnly();

#if !CS8
            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;

            object IReadOnlyLinkedListNode.Value => Value;
#endif

            public ReadOnlyLinkedListNode(in IReadOnlyLinkedListNodeBase2<T> node) => InnerNode = node;

            public IReadOnlyLinkedListNodeBase2<T> ToReadOnly() => this;
#endif
        }

        public interface IReadOnlyLinkedListNode<out TItems, out TList> :
#if WinCopies3
            IReadOnlyLinkedListNodeBase2
#else
            IReadOnlyLinkedListNodeBase
#endif
            <TItems> where TList : IReadOnlyLinkedList<TItems>
        {
            TList List { get; }
        }

        public interface IReadOnlyLinkedListNode<out TItems, out TNodes, out TList> : IReadOnlyLinkedListNode<TItems, TList> where TList : IReadOnlyLinkedList<TItems> where TNodes : IReadOnlyLinkedListNode<TItems, TList>
        {
            TNodes Previous { get; }

            TNodes Next { get; }

#if CS8
            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;
#endif
        }

        public interface IReadOnlyLinkedListNode<T> : IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>
        {
#if CS8
            IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Previous => Previous;

            IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Next => Next;

            IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>.List => List;
#else
            // Left empty.
#endif
        }

        public interface
#if WinCopies3
            ILinkedListNodeBase<T> : IReadOnlyLinkedListNodeBase<T>
        {
            T Value { get; set; }
        }

        public interface ILinkedListNodeBase2<T> : IReadOnlyLinkedListNodeBase2<T>, ILinkedListNodeBase<T>
        {
            new ILinkedListNodeBase2<T> Previous { get; }

            new ILinkedListNodeBase2<T> Next { get; }

            ILinkedListNodeBase2<T> ToReadOnly2();

#if CS8
            IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Previous => Previous;

            IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Next => Next;
#endif
        }

        public class LinkedListNode<T> : ILinkedListNodeBase2<T>
        {
            protected ILinkedListNodeBase2<T> InnerNode { get; }

            public bool IsReadOnly => false;

            public T Value { get => InnerNode.Value; set => InnerNode.Value = value; }

            public ILinkedListNodeBase2<T> Previous => InnerNode.Previous.ToReadOnly2();

            public ILinkedListNodeBase2<T> Next => InnerNode.Next.ToReadOnly2();

#if !CS8
            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;

            object IReadOnlyLinkedListNode.Value => Value;

            IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Previous => Previous;

            IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Next => Next;
#endif

            public LinkedListNode(in ILinkedListNodeBase2<T> node) => InnerNode = node;

            public ILinkedListNodeBase2<T> ToReadOnly2() => this;

            public IReadOnlyLinkedListNodeBase2<T> ToReadOnly() => InnerNode.ToReadOnly();
        }

        public interface ILinkedListNode<TItems, out TList> : ILinkedListNodeBase2<TItems>, IReadOnlyLinkedListNode<TItems, TList> where TList : ILinkedList<TItems>
        {
            // Left empty.
        }

        public interface ILinkedListNode<TItems, out TNodes, out TList> : ILinkedListNode<TItems, TList>, IReadOnlyLinkedListNode<TItems, TNodes, TList> where TList : ILinkedList<TItems> where TNodes : ILinkedListNode<TItems, TList>
        {
            new TNodes Previous { get; }

            new TNodes Next { get; }

#if CS8
            TNodes IReadOnlyLinkedListNode<TItems, TNodes, TList>.Previous => Previous;

            TNodes IReadOnlyLinkedListNode<TItems, TNodes, TList>.Next => Next;
#endif
        }

        public interface ILinkedListNode<T> : ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>, IReadOnlyLinkedListNode<T>
        {
            new ILinkedList<T> List { get; }

            new ILinkedListNode<T> Previous { get; }

            new ILinkedListNode<T> Next { get; }

#if CS8
            ILinkedList<T> IReadOnlyLinkedListNode<T, ILinkedList<T>>.List => List;
#endif
        }
#else
            ILinkedListNode<T>
        {
            bool IsReadOnly { get; }

            ILinkedList<T> List { get; }

            ILinkedListNode<T> Previous { get; }

            ILinkedListNode<T> Next { get; }

            T Value { get; }
#endif
    }
}
#endif
