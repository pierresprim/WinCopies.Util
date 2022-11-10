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

namespace WinCopies.Collections.DotNetFix
{
    public interface IReadOnlyLinkedListNode
    {
        bool IsReadOnly { get; }

        object
#if CS8
            ?
#endif
            Value
        { get; }
    }

    public interface IReadOnlyLinkedListNode<out T> where T : IReadOnlyLinkedListNode
    {
        T
#if CS9
            ?
#endif
            Previous
        { get; }
        T
#if CS9
            ?
#endif
            Next
        { get; }
    }

    namespace Generic
    {
        public interface IReadOnlyLinkedListNodeBase<out T> : IReadOnlyLinkedListNode
        {
            new T Value { get; }
#if CS8
            object
#if CS8
                ?
#endif
                IReadOnlyLinkedListNode.Value => Value;
#endif
        }

        public interface IReadOnlyLinkedListNodeBase2<out T> where T : IReadOnlyLinkedListNodeBase2<T>, IReadOnlyLinkedListNode
        {
            T ToReadOnly();
        }

        public interface IReadOnlyLinkedListNodeBase2<out TValue, out TNode> : IReadOnlyLinkedListNodeBase<TValue>, IReadOnlyLinkedListNodeBase2<TNode> where TNode : IReadOnlyLinkedListNodeBase2<TValue, TNode>, IReadOnlyLinkedListNode
        {
            // Left empty.
        }

        public interface IReadOnlyLinkedListNodeBase<out TNode, out TReadOnly> : DotNetFix.IReadOnlyLinkedListNode<TNode>, IReadOnlyLinkedListNodeBase2<TReadOnly> where TNode : IReadOnlyLinkedListNodeBase<TNode, TReadOnly>, IReadOnlyLinkedListNode where TReadOnly : IReadOnlyLinkedListNodeBase2<TReadOnly>, IReadOnlyLinkedListNode
        {
            // Left empty.
        }

        public interface IReadOnlyLinkedListNode<out TValue, out TNode, out TReadOnly> : IReadOnlyLinkedListNodeBase<TValue>, IReadOnlyLinkedListNodeBase<TNode, TReadOnly> where TNode : IReadOnlyLinkedListNodeBase<TNode, TReadOnly>, IReadOnlyLinkedListNode where TReadOnly : IReadOnlyLinkedListNodeBase2<TValue, TReadOnly>, IReadOnlyLinkedListNode
        {
            // Left empty.
        }

        public abstract class ReadOnlyLinkedListNode<TValue, TNode, TReadOnly> : IReadOnlyLinkedListNode<TValue, TNode, TReadOnly> where TNode : IReadOnlyLinkedListNodeBase<TNode, TReadOnly>, IReadOnlyLinkedListNode where TReadOnly : IReadOnlyLinkedListNodeBase2<TValue, TReadOnly>, IReadOnlyLinkedListNode
        {
            protected IReadOnlyLinkedListNode<TValue, TNode, TReadOnly> InnerNode { get; }

            public bool IsReadOnly => true;

            public TValue Value => InnerNode.Value;
#if !CS8
            object IReadOnlyLinkedListNode.Value => InnerNode.Value;
#endif
            public abstract TNode Previous { get; }
            public abstract TNode Next { get; }

            public ReadOnlyLinkedListNode(in IReadOnlyLinkedListNode<TValue, TNode, TReadOnly> node) => InnerNode = node;

            public abstract TReadOnly ToReadOnly();
        }

        public interface IReadOnlyLinkedListNode<T> : IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T>, IReadOnlyLinkedListNode<T>>, IReadOnlyLinkedListNodeBase2<T, IReadOnlyLinkedListNode<T>>
        {
            // Left empty.
        }

        public class ReadOnlyLinkedListNode<T> : ReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T>, IReadOnlyLinkedListNode<T>>, IReadOnlyLinkedListNode<T>
        {
            public override IReadOnlyLinkedListNode<T> Previous => InnerNode.Previous?.ToReadOnly();
            public override IReadOnlyLinkedListNode<T> Next => InnerNode.Next?.ToReadOnly();

            public ReadOnlyLinkedListNode(in IReadOnlyLinkedListNode<T> node) : base(node) { /* Left empty. */ }

            public override IReadOnlyLinkedListNode<T> ToReadOnly() => this;
        }

        public interface IReadOnlyLinkedListNodeBase3<out T>
        {
            T List { get; }
        }

        public interface IReadOnlyLinkedListNode<out TValue, out TNode, out TReadOnly, out TList> : IReadOnlyLinkedListNode<TValue, TNode, TReadOnly>, IReadOnlyLinkedListNodeBase3<TList> where TList : IReadOnlyLinkedListBase<TValue> where TNode : IReadOnlyLinkedListNode<TValue, TNode, TReadOnly> where TReadOnly : IReadOnlyLinkedListNodeBase2<TValue, TReadOnly>
        {
            // Left empty.
        }

        public interface ILinkedListNodeBase<T> : IReadOnlyLinkedListNodeBase<T>
        {
            new T Value { get; set; }

            bool IsAvailableByRef { get; }
            ref T ValueRef { get; }
        }

        public interface ILinkedListNode<TValue, out TNode, out TReadOnly> : ILinkedListNodeBase<TValue>, IReadOnlyLinkedListNode<TValue, TNode, TReadOnly> where TNode : ILinkedListNode<TValue, TNode, TReadOnly> where TReadOnly : IReadOnlyLinkedListNodeBase2<TValue, TReadOnly>
        {
            // Left empty.
        }

        public interface ILinkedListNode<TValue, out TNode, out TReadOnly, out TList> : ILinkedListNode<TValue, TNode, TReadOnly>, IReadOnlyLinkedListNode<TValue, TNode, TReadOnly, TList> where TList : ILinkedList<TValue> where TNode : ILinkedListNode<TValue, TNode, TReadOnly> where TReadOnly : IReadOnlyLinkedListNodeBase2<TValue, TReadOnly>
        {
            // Left empty.
        }

        public interface ILinkedListNode<T> : ILinkedListNode<T, ILinkedListNode<T>, IReadOnlyLinkedListNode<T>, ILinkedList3<T>>, IReadOnlyLinkedListNode<T>
        {
#if CS8
            DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>> AsReadWrite => this;

            IReadOnlyLinkedListNode<T>
#if CS8
                ?
#endif
            DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>.Previous => AsReadWrite.Previous;
            IReadOnlyLinkedListNode<T>
#if CS8
                ?
#endif
            DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>.Next => AsReadWrite.Next;
#endif
        }
#if !CS8
        public abstract class LinkedListNodeBase<T> : DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>
        {
            public abstract ILinkedListNode<T> Previous { get; }
            public abstract ILinkedListNode<T> Next { get; }

            IReadOnlyLinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>.Previous => Previous;
            IReadOnlyLinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<IReadOnlyLinkedListNode<T>>.Next => Next;
        }
#endif
        public class LinkedListNode<T> :
#if !CS8
            LinkedListNodeBase<T>,
#endif
            ILinkedListNode<T>
        {
            protected ILinkedListNode<T> InnerNode { get; }
#if !CS8
            protected DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>> InnerReadWriteNode => InnerNode;
#endif
            public bool IsReadOnly => false;

            public
#if !CS8
                override
#endif
                ILinkedListNode<T> Previous =>
#if CS8
                InnerNode.AsReadWrite
#else
                InnerReadWriteNode
#endif
                .Previous;
            public
#if !CS8
                override
#endif
                ILinkedListNode<T> Next =>
#if CS8
                InnerNode.AsReadWrite
#else
                InnerReadWriteNode
#endif
                .Next;

            public ILinkedList3<T> List => InnerNode.List;

            public T Value { get => InnerNode.Value; set => InnerNode.Value = value; }
            public bool IsAvailableByRef => InnerNode.IsAvailableByRef;
            public ref T ValueRef => ref InnerNode.ValueRef;
#if !CS8
            object IReadOnlyLinkedListNode.Value => Value;
#endif
            public LinkedListNode(in ILinkedListNode<T> node)
            {
                if ((InnerNode = node ?? throw new ArgumentNullException(nameof(node))).IsReadOnly)

                    throw new ArgumentException("The given node is read-only.", nameof(node));
            }

            public IReadOnlyLinkedListNode<T> ToReadOnly() => InnerNode.ToReadOnly();
        }
    }
}
#endif
