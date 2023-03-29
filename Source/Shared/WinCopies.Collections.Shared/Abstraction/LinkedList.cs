/* Copyright © Pierre Sprimont, 2021
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.ThrowHelper;
using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.Abstraction.Generic
{
    public class LinkedListNode<T> :
#if !CS8
        LinkedListNodeBase<T>,
#endif
        ILinkedListNode<T>, IEquatable<ILinkedListNode<T>>
    {
        private DotNetFix.Generic.IReadOnlyLinkedListNode<T>
#if CS8
            ?
#endif
            _asReadOnly;

        protected internal System.Collections.Generic.LinkedListNode<T> InnerNode { get; }

        public T Value { get => InnerNode.Value; set => InnerNode.Value = value; }
        bool ILinkedListNodeBase<T>.IsAvailableByRef =>
#if CS9
            true
#else
            false
#endif
            ;
        ref T ILinkedListNodeBase<T>.ValueRef =>
#if CS9
            ref InnerNode.ValueRef;
#else
            throw new InvalidOperationException("This feature is not supported by this implementation.");
#if !CS8
        object IReadOnlyLinkedListNode.Value => Value;
#endif
#endif
        bool IReadOnlyLinkedListNode.IsReadOnly => false;

        public
#if !CS8
            override
#endif
            ILinkedListNode<T>
#if CS8
            ?
#endif
            Previous => GetNode(InnerNode.Previous);
        public
#if !CS8
            override
#endif
            ILinkedListNode<T>
#if CS8
            ?
#endif
            Next => GetNode(InnerNode.Next);
        public ILinkedList3<T> List => GetList();

        public LinkedListNode(in System.Collections.Generic.LinkedListNode<T> node) => InnerNode = node ?? throw GetArgumentNullException(nameof(node));

        public LinkedListNode<T>
#if CS8
            ?
#endif
            GetNode(in System.Collections.Generic.LinkedListNode<T>
#if CS8
            ?
#endif
            node) => node == null ? null : new
#if !CS9
            LinkedListNode<T>
#endif
            (node);

        private LinkedList<T> GetList() => new
#if !CS9
            LinkedList<T>
#endif
            (InnerNode.List);

        public bool Equals(ILinkedListNode<T> other) => InnerNode == LinkedList<T>.TryGetNode(other);

        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => obj is ILinkedListNode<T> other && Equals(other);

        DotNetFix.Generic.IReadOnlyLinkedListNode<T> IReadOnlyLinkedListNodeBase2<DotNetFix.Generic.IReadOnlyLinkedListNode<T>>.ToReadOnly() => _asReadOnly
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

        public static bool operator ==(LinkedListNode<T>
#if CS8
            ?
#endif
            x, ILinkedListNode<T>
#if CS8
            ?
#endif
            y) => x.AsObject() == null ? y == null : x.Equals(y);
        public static bool operator !=(LinkedListNode<T>
#if CS8
            ?
#endif
            x, ILinkedListNode<T>
#if CS8
            ?
#endif
            y) => !(x == y);
    }

    public class LinkedList<T> : LinkedListBase<T, ILinkedListNode<T>>, ILinkedList3<T>, IEquatable<LinkedList<T>>
    {
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        protected System.Collections.ICollection InnerCollection => InnerList;
        protected System.Collections.Generic.ICollection<T> InnerCollection2 => InnerList;

        ILinkedListNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase2<ILinkedListNode<T>>.First => GetNode(InnerList.First);
        ILinkedListNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase2<ILinkedListNode<T>>.Last => GetNode(InnerList.Last);

        public override T FirstValue => (InnerList.First ?? throw GetEmptyListOrCollectionException()).Value;
        T IReadOnlyLinkedListCore<T>.LastValue => (InnerList.Last ?? throw GetEmptyListOrCollectionException()).Value;

        public bool SupportsReversedEnumeration => true;

        int System.Collections.Generic.IReadOnlyCollection<T>.Count => InnerList.Count;
        public override uint Count => (uint)InnerList.Count;

        public override bool HasItems => InnerList.Count > 0;

        ILinkedListNode<T> IReadOnlyLinkedListBase<T, ILinkedListNode<T>>.Find(T value) => GetNode(InnerList.Find(value));
        ILinkedListNode<T> IReadOnlyLinkedListBase<T, ILinkedListNode<T>>.FindLast(T value) => GetNode(InnerList.FindLast(value));

        ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => GetNode(InnerList.AddAfter(GetNode(node), value));
        ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value) => GetNode(InnerList.AddBefore(GetNode(node), value));

        protected override ILinkedListNode<T> AddFirstItem(T value) => GetNode(InnerList.AddFirst(value));
        protected override ILinkedListNode<T> AddLastItem(T value) => GetNode(InnerList.AddLast(value));

        ILinkedListNode<T> ILinkedList<T>.AddFirst(T value) => AddFirstItem(value);
        ILinkedListNode<T> ILinkedList<T>.AddLast(T value) => AddLastItem(value);

        void ILinkedList<T>.Remove(ILinkedListNode<T> node) => InnerList.Remove(GetNode(node));

        public IEnumeratorInfo2<ILinkedListNode<T>> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new LinkedListEnumerator<T>(this, enumerationDirection, null, null);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator(in EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

        public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator2(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<ILinkedListNode<T>>(GetNodeEnumerator(enumerationDirection), () => (uint)InnerList.Count);

        public IEnumeratorInfo2<T> GetEnumerator2(in EnumerationDirection enumerationDirection) => new EnumeratorInfo<T>(GetEnumerator(enumerationDirection));

        public IUIntCountableEnumeratorInfo<T> GetEnumerator3(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(GetEnumerator2(enumerationDirection), () => (uint)InnerList.Count);

        //IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.Extensions.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        int System.Collections.Generic.ICollection<T>.Count => InnerList.Count;
        int ICollection.Count => InnerList.Count;

        public override bool IsReadOnly => InnerCollection2.IsReadOnly;

        protected override bool IsSynchronized => InnerCollection.IsSynchronized;
        protected override object SyncRoot => InnerCollection.SyncRoot;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        public LinkedList(in System.Collections.Generic.LinkedList<T> list) => InnerList = list ?? throw GetArgumentNullException(nameof(list));

        public LinkedListNode<T>
#if CS8
            ?
#endif
            GetNode(in System.Collections.Generic.LinkedListNode<T>
#if CS8
            ?
#endif
            node) => node == null ? null : new
#if !CS9
            LinkedListNode<T>
#endif
            (node);

        public static System.Collections.Generic.LinkedListNode<T>
#if CS8
            ?
#endif
            TryGetNode(in ILinkedListNode<T> node) => node is LinkedListNode<T> _node ? _node.InnerNode : null;

        public static System.Collections.Generic.LinkedListNode<T> GetNode(in ILinkedListNode<T> node) => TryGetNode(node) ?? throw new ArgumentException("The given node is not contained in the current list.", nameof(node));

        protected override T RemoveFirstItem()
        {
            System.Collections.Generic.LinkedListNode<T>
#if CS8
                ?
#endif
            node = InnerList.First;

            InnerList.Remove(node);

            return node.Value;
        }

        void ILinkedList<T>.RemoveFirst() => RemoveFirstItem();
        void ILinkedList<T>.RemoveLast() => InnerList.RemoveLast();

        void System.Collections.Generic.ICollection<T>.Add(T item) => InnerList.AsFromType<System.Collections.Generic.ICollection<T>>().Add(item);
        bool System.Collections.Generic.ICollection<T>.Remove(T item) => InnerList.Remove(item);
        void System.Collections.Generic.ICollection<T>.Clear() => InnerList.Clear();

        bool System.Collections.Generic.ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(System.Array array, int index) => InnerList.AsFromType<ICollection>().CopyTo(array, index);

        public bool Equals(LinkedList<T>
#if CS8
            ?
#endif
            other) => InnerList == other?.InnerList;
        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => obj is LinkedList<T> list && Equals(list);
        public override int GetHashCode() => InnerList.GetHashCode();

        public static bool operator ==(LinkedList<T> x, ILinkedList<T> y) => x.AsObject() == null ? y == null : x.Equals(y);
        public static bool operator !=(LinkedList<T> x, ILinkedList<T> y) => !(x == y);

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        protected static NotSupportedException GetOperationNotSupportedException() => new NotSupportedException("This operation is not supported by this implementation.");

        DotNetFix.Generic.IUIntCountableEnumerable<ILinkedListNode<T>> ILinkedList<T>.AsNodeEnumerable() => throw GetOperationNotSupportedException();

        public void Add(T item) => InnerCollection2.Add(item);
        public bool Remove(T item) => InnerList.Remove(item);
        protected override void ClearItems() => InnerList.Clear();
        public void Clear() => ClearItems();

        public bool Contains(T item) => InnerList.Contains(item);

        public IQueue<T> AsQueue() => new Abstraction.Generic.Queue<T, ILinkedList<T>>(this);
        public IStack<T> AsStack() => new Abstraction.Generic.Stack<T, ILinkedList<T>>(this);

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => throw GetOperationNotSupportedException();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetEnumerator() => throw GetOperationNotSupportedException();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => throw GetOperationNotSupportedException();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Extensions.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetReversedEnumerator() => throw GetOperationNotSupportedException();

        public IUIntCountableEnumeratorInfo<T> GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);
        public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator3(EnumerationDirection.LIFO);

        void IReadOnlyCollectionBase<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        protected System.Collections.Generic.LinkedListNode<T>
#if CS8
            ?
#endif
            UpdateNodeState(in T item, in Action<System.Collections.Generic.LinkedListNode<T>> action)
        {
            System.Collections.Generic.LinkedListNode<T>
#if CS8
                ?
#endif
                node = InnerList.Find(item);

            if (node != null)

                action(node);

            return node;
        }

        protected bool Move(in ILinkedListNode<T> node, ILinkedListNode<T> other, Action<System.Collections.Generic.LinkedListNode<T>, System.Collections.Generic.LinkedListNode<T>> action)
        {
            bool result = false;

            UpdateNodeState(node.Value, _node => UpdateNodeState(other.Value, __node =>
            {
                InnerList.Remove(_node);

                action(__node, _node);

                result = true;
            }));

            return result;
        }

        public bool MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => Move(node, after, InnerList.AddAfter);
        public bool MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => Move(node, before, InnerList.AddBefore);

        ILinkedListNode<T>
#if CS8
            ?
#endif
            ILinkedList3<T>.Remove(T item) => GetNode(UpdateNodeState(item, InnerList.Remove));
        public void Swap(ILinkedListNode<T> x, ILinkedListNode<T> y)
        {
            if (!Move(x, y, (_x, _y) =>
            {
                System.Collections.Generic.LinkedListNode<T>
#if CS8
                    ?
#endif
                node = null;
                System.Collections.Generic.LinkedListNode<T>
#if CS8
                    ?
#endif
                _node = null;

                void doWork(System.Collections.Generic.LinkedListNode<T> toProcess, ref System.Collections.Generic.LinkedListNode<T>
#if CS8
                    ?
#endif
                    __node, in Action<Action<System.Collections.Generic.LinkedListNode<T>, System.Collections.Generic.LinkedListNode<T>>> action)
                {
                    Action<System.Collections.Generic.LinkedListNode<T>, System.Collections.Generic.LinkedListNode<T>> getAction(ref System.Collections.Generic.LinkedListNode<T>
#if CS8
                        ?
#endif
                        ___node)
                    {
                        if ((___node = toProcess.Previous) == null)
                        {
                            ___node = toProcess.Next;

                            return InnerList.AddBefore;
                        }

                        return InnerList.AddAfter;
                    }

                    InnerList.Remove(toProcess);

                    action(getAction(ref __node));
                }

                doWork(_x, ref node, first => doWork(_y, ref _node, second =>
                {
                    first(node, _y);
                    second(_node, _x);
                }));
            }))

                throw new ArgumentException("At least one of the given nodes was not found.");
        }
#if !CS8
        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);
        System.Collections.Generic.IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

        IUIntCountableEnumerator<T> Enumeration.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();
        IEnumerator<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>>.GetEnumerator() => throw GetOperationNotSupportedException();

        System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
    }
}
#endif
