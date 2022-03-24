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

using static WinCopies.
#if WinCopies3
    ThrowHelper;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
#else
    Util.Util;
#endif
using WinCopies.Util;

namespace WinCopies.Collections.Abstraction.Generic
{
    public class LinkedListNode<T> :
#if !WinCopies3
        DotNetFix.
#endif
        ILinkedListNode<T>, IEquatable<ILinkedListNode<T>>
    {
#if WinCopies3
        private IReadOnlyLinkedListNodeBase2<T> _asReadOnly;
        private ILinkedListNodeBase2<T> _asReadOnly2;
#endif

        protected internal System.Collections.Generic.LinkedListNode<T> InnerNode { get; }

        ILinkedList<T>
#if !WinCopies3
        DotNetFix.
#endif
        ILinkedListNode<T>.List => GetList();

#if WinCopies3
        ILinkedList<T> IReadOnlyLinkedListNode<T, ILinkedList<T>>.List => GetList();

        IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>.List => GetList();

        ILinkedListNode<T> ILinkedListNode<T>.Previous => GetNode(InnerNode.Previous);

        ILinkedListNode<T, ILinkedList<T>> IReadOnlyLinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Previous => GetNode(InnerNode.Previous);

        IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Previous => GetNode(InnerNode.Previous);

        IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => GetNode(InnerNode.Previous);

        ILinkedListNode<T> ILinkedListNode<T>.Next => GetNode(InnerNode.Next);

        ILinkedListNode<T, ILinkedList<T>> IReadOnlyLinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Next => GetNode(InnerNode.Next);

        IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Next => GetNode(InnerNode.Next);

        IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => GetNode(InnerNode.Next);

        T ILinkedListNodeBase<T>.Value { get => InnerNode.Value; set => InnerNode.Value = value; }

        T IReadOnlyLinkedListNodeBase<T>.Value => InnerNode.Value;

        object IReadOnlyLinkedListNode.Value => InnerNode.Value;

        ILinkedListNode<T, ILinkedList<T>> ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Previous => GetNode(InnerNode.Previous);

        ILinkedListNode<T, ILinkedList<T>> ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Next => GetNode(InnerNode.Next);

        ILinkedListNodeBase2<T> ILinkedListNodeBase2<T>.Previous => GetNode(InnerNode.Previous);

        ILinkedListNodeBase2<T> ILinkedListNodeBase2<T>.Next => GetNode(InnerNode.Next);

        IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Previous => GetNode(InnerNode.Previous);

        IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Next => GetNode(InnerNode.Next);
#else
        DotNetFix.ILinkedListNode<T> DotNetFix.ILinkedListNode<T>.Next => GetNode(InnerNode.Next);

        DotNetFix.ILinkedListNode<T> DotNetFix.ILinkedListNode<T>.Previous => GetNode(InnerNode.Previous);

        T DotNetFix.ILinkedListNode<T>.Value => InnerNode.Value;
#endif

        bool
#if WinCopies3
            IReadOnlyLinkedListNode
#else
            DotNetFix.ILinkedListNode<T>
#endif
       .IsReadOnly => false;

        public LinkedListNode(in System.Collections.Generic.LinkedListNode<T> node) => InnerNode = node ?? throw GetArgumentNullException(nameof(node));

        public LinkedListNode<T> GetNode(in System.Collections.Generic.LinkedListNode<T> node) => new
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

#if WinCopies3
        public IReadOnlyLinkedListNodeBase2<T> ToReadOnly() => _asReadOnly
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

        public ILinkedListNodeBase2<T> ToReadOnly2() => _asReadOnly2
#if CS8
            ??=
#else
            ?? (_asReadOnly2 =
#endif
             new DotNetFix.Generic.LinkedListNode<T>(this)
#if !CS8
            )
#endif
            ;
#endif

        public static bool operator ==(LinkedListNode<T> x, ILinkedListNode<T> y) => x == null ? y == null : x.Equals(y);

        public static bool operator !=(LinkedListNode<T> x, ILinkedListNode<T> y) => !(x == y);
    }

    public class LinkedList<T> : ILinkedList2<T>, IEquatable<LinkedList<T>>
    {
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        protected System.Collections.Generic.ICollection<T> InnerCollection => InnerList.AsOfType<System.Collections.Generic.ICollection<T>>();

        bool ILinkedList2<T>.IsReadOnly => false;

#if WinCopies3
        ILinkedListNode<T> ILinkedList<T>.First => GetNode(InnerList.First);

        ILinkedListNode<T> ILinkedList<T>.Last => GetNode(InnerList.Last);

        T IReadOnlyLinkedList2<T>.FirstValue => InnerList.First.Value;

        T IReadOnlyLinkedList2<T>.LastValue => InnerList.Last.Value;

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => GetNode(InnerList.First);

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => GetNode(InnerList.Last);

        bool IReadOnlyLinkedList<T>.SupportsReversedEnumeration => true;

        bool Enumeration.IEnumerable.SupportsReversedEnumeration => true;

        uint IReadOnlyLinkedList<T>.Count => (uint)InnerList.Count;

        int System.Collections.Generic.IReadOnlyCollection<T>.Count => InnerList.Count;

        uint IUIntCountable.Count => (uint)InnerList.Count;

        ILinkedListNode<T> ILinkedList<T>.Find(T value) => GetNode(InnerList.Find(value));

        ILinkedListNode<T> ILinkedList<T>.FindLast(T value) => GetNode(InnerList.FindLast(value));

        ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => GetNode(InnerList.AddAfter(GetNode(node), value));

        ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value) => GetNode(InnerList.AddBefore(GetNode(node), value));

        ILinkedListNode<T> ILinkedList<T>.AddFirst(T value) => GetNode(InnerList.AddFirst(value));

        ILinkedListNode<T> ILinkedList<T>.AddLast(T value) => GetNode(InnerList.AddLast(value));

        void ILinkedList<T>.Remove(ILinkedListNode<T> node) => InnerList.Remove(GetNode(node));

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => GetNode(InnerList.Find(value));

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => GetNode(InnerList.FindLast(value));

        public IEnumeratorInfo2<ILinkedListNode<T>> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new LinkedListEnumerator<T>(this, enumerationDirection, null, null);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator(in EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

        public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator2(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<ILinkedListNode<T>>(GetNodeEnumerator(enumerationDirection), () => (uint)InnerList.Count);

        public IEnumeratorInfo2<T> GetEnumerator2(in EnumerationDirection enumerationDirection) => new EnumeratorInfo<T>(GetEnumerator(enumerationDirection));

        public IUIntCountableEnumeratorInfo<T> GetEnumerator3(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(GetEnumerator2(enumerationDirection), () => (uint)InnerList.Count);

        IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

        IUIntCountableEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => GetEnumerator3(EnumerationDirection.LIFO);

        IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator2(EnumerationDirection.FIFO);

        IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => GetNodeEnumerator2(EnumerationDirection.LIFO);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

        bool ILinkedList<T>.IsReadOnly => false;

        bool ICollectionBase<T>.IsReadOnly => false;
#else
        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.Last => InnerList.Last;

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.First => InnerList.First;

        int ILinkedList<T>.Count => InnerList.Count;

        void ILinkedList<T>.AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        void ILinkedList<T>.AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        void ILinkedList<T>.AddFirst(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

        void ILinkedList<T>.AddLast(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.Find(T value) => InnerList.Find(value);

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.FindLast(T value) => InnerList.FindLast(value);

        System.Collections.Generic.LinkedList<T>.Enumerator ILinkedList<T>.GetEnumerator() => InnerList.GetEnumerator();

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.AddBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.AddFirst(T value) => InnerList.AddFirst(value);

        System.Collections.Generic.LinkedListNode<T> ILinkedList<T>.AddLast(T value) => InnerList.AddLast(value);

        void ILinkedList<T>.Remove(System.Collections.Generic.LinkedListNode<T> node) => InnerList.Remove(node);

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        void IDeserializationCallback.OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => InnerList.GetEnumerator();
#endif

        int System.Collections.Generic.ICollection<T>.Count => InnerList.Count;

        int ICollection.Count => InnerList.Count;

        bool System.Collections.Generic.ICollection<T>.IsReadOnly => false;

        object ICollection.SyncRoot => InnerList.AsOfType<ICollection>().SyncRoot;

        bool ICollection.IsSynchronized => InnerList.AsOfType<ICollection>().IsSynchronized;

        public LinkedList(in System.Collections.Generic.LinkedList<T> list) => InnerList = list ?? throw GetArgumentNullException(nameof(list));

        public LinkedListNode<T> GetNode(in System.Collections.Generic.LinkedListNode<T> node) => new
#if !CS9
            LinkedListNode<T>
#endif
            (node);

        public static System.Collections.Generic.LinkedListNode<T> TryGetNode(in ILinkedListNode<T> node) => node is LinkedListNode<T> _node ? _node.InnerNode : null;

        public static System.Collections.Generic.LinkedListNode<T> GetNode(in ILinkedListNode<T> node) => TryGetNode(node) ?? throw new ArgumentException("The given node is not contained in the current list.", nameof(node));

        void ILinkedList<T>.RemoveFirst() => InnerList.RemoveFirst();

        void ILinkedList<T>.RemoveLast() => InnerList.RemoveLast();

        void System.Collections.Generic.ICollection<T>.Add(T item) => ((System.Collections.Generic.ICollection<T>)InnerList).Add(item);

        void System.Collections.Generic.ICollection<T>.Clear() => InnerList.Clear();

        bool System.Collections.Generic.ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool System.Collections.Generic.ICollection<T>.Remove(T item) => InnerList.Remove(item);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        public bool Equals(LinkedList<T> other) => other == null ? false : InnerList == other.InnerList;

        public static bool operator ==(LinkedList<T> x, ILinkedList<T> y) => x == null ? y == null : x.Equals(y);

        public static bool operator !=(LinkedList<T> x, ILinkedList<T> y) => !(x == y);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

#if WinCopies3
        DotNetFix.Generic.IUIntCountableEnumerable<ILinkedListNode<T>> ILinkedList<T>.AsNodeEnumerable() => throw new NotSupportedException("This operation is not supported by this implementation.");

        void ILinkedList<T>.Add(T item) => InnerCollection.Add(item);

        bool ILinkedList<T>.Remove(T item) => InnerList.Remove(item);

        void ILinkedList<T>.Clear() => InnerList.Clear();

        bool ILinkedList<T>.Contains(T item) => InnerList.Contains(item);

        void ILinkedList<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool IReadOnlyLinkedList2<T>.Contains(T value) => InnerList.Contains(value);

        void IReadOnlyLinkedList2<T>.CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        IUIntCountableEnumerator<T> IUIntCountableEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

#if !CS8
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

        System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

        IUIntCountableEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

        IUIntCountableEnumerator<T> IReadOnlyLinkedList2<T>.GetReversedEnumerator() => GetEnumerator3(EnumerationDirection.LIFO);

        void ICollectionBase<T>.Add(T item) => InnerCollection.Add(item);

        void ICollectionBase<T>.Clear() => InnerList.Clear();

        bool ICollectionBase<T>.Remove(T item) => InnerList.Remove(item);

        bool IReadOnlyCollectionBase<T>.Contains(T item) => InnerList.Contains(item);

        void IReadOnlyCollectionBase<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        IUIntCountableEnumerator<T> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

        IUIntCountableEnumerator<T> DotNetFix.Generic.IEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);
#endif
#endif
    }
}
#endif
