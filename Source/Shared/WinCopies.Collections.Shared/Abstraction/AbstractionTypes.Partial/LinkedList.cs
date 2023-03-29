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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.Collections.ThrowHelper;
using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination>
    {
        public static class LinkedListTypes<TList, TNode> where TNode : ILinkedListNode<TSource>, IReadOnlyLinkedListNodeBase3<ILinkedList3<TSource>> where TList : ILinkedList3<TSource>
        {
            public class LinkedListNode : ILinkedListNode<TDestination>, IEquatable<ILinkedListNode<TDestination>>
            {
                private DotNetFix.Generic.IReadOnlyLinkedListNode<TDestination>

#if CS8
                    ?
#endif
                    _asReadOnly;

                protected internal TNode InnerNode { get; }
                protected DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<TSource>> InnerReadWriteNode => InnerNode;

                ILinkedList3<TDestination> IReadOnlyLinkedListNodeBase3<ILinkedList3<TDestination>>.List => GetList();

                TDestination ILinkedListNodeBase<TDestination>.Value { get => InnerNode.Value; set => InnerNode.Value = InnerNode.IsReadOnly ? throw GetReadOnlyListOrCollectionException() : value is TSource _value ? _value : throw GetArgumentException(); }

                TDestination IReadOnlyLinkedListNodeBase<TDestination>.Value => InnerNode.Value;

                object
#if CS8
                    ?
#endif
                    IReadOnlyLinkedListNode.Value => InnerNode.Value;
                bool ILinkedListNodeBase<TDestination>.IsAvailableByRef => false;
                public ref TDestination ValueRef => throw new InvalidOperationException("This implementation does not support this feature.");

                public bool IsReadOnly => InnerNode.IsReadOnly;

                public ILinkedListNode<TDestination>
#if CS8
                    ?
#endif
                    Previous => GetNode(InnerReadWriteNode.Previous);
                public ILinkedListNode<TDestination>
#if CS8
                    ?
#endif
                    Next => GetNode(InnerReadWriteNode.Next);

                DotNetFix.Generic.IReadOnlyLinkedListNode<TDestination> DotNetFix.IReadOnlyLinkedListNode<DotNetFix.Generic.IReadOnlyLinkedListNode<TDestination>>.Previous => Previous;

                DotNetFix.Generic.IReadOnlyLinkedListNode<TDestination> DotNetFix.IReadOnlyLinkedListNode<DotNetFix.Generic.IReadOnlyLinkedListNode<TDestination>>.Next => Next;

                public LinkedListNode(in TNode node) => InnerNode = node
#if CS8
                    ??
#else
                    == null ?
#endif
                    throw GetArgumentNullException(nameof(node))
#if !CS8
                    : node
#endif
                    ;

                public static LinkedListTypes<TList, T>.LinkedListNode
#if CS8
                    ?
#endif
                    GetNode<T>(in T
#if CS9
                    ?
#endif
                    node) where T : ILinkedListNode<TSource>, IReadOnlyLinkedListNodeBase3<ILinkedList3<TSource>> => node == null ? null : new
#if !CS9
                    LinkedListTypes<TList, T>.LinkedListNode
#endif
                    (node);

                private LinkedListTypes<ILinkedList3<TSource>, TNode>.LinkedList GetList() => new
#if !CS9
                    LinkedListTypes<ILinkedList3<TSource>, TNode>.LinkedList
#endif
                    (InnerNode.List);

                public bool Equals(ILinkedListNode<TDestination>
#if CS8
                    ?
#endif
                    other) => Equals(InnerNode, LinkedList.TryGetNode(other));
                public override bool Equals(object
#if CS8
                    ?
#endif
                    obj) => obj is ILinkedListNode<TDestination> other && Equals(other);

                public DotNetFix.Generic.IReadOnlyLinkedListNode<TDestination> ToReadOnly() => _asReadOnly
#if CS8
                    ??=
#else
                    ?? (_asReadOnly =
#endif
                    new ReadOnlyLinkedListNode<TDestination>(this)
#if !CS8
                    )
#endif
                    ;

                public static bool operator ==(LinkedListNode x, ILinkedListNode<TDestination> y) => x.AsObject() == null ? y == null : x.Equals(y);
                public static bool operator !=(LinkedListNode x, ILinkedListNode<TDestination> y) => !(x == y);
            }

            public class LinkedList : LinkedListBase<TDestination, ILinkedListNode<TDestination>>, ILinkedList3<TDestination>, IEquatable<ILinkedList<TDestination>>
            {
                #region Properties
                #region InnerList
                protected TList InnerList { get; }

                protected ICollection InnerCollection => InnerList;
                protected System.Collections.Generic.ICollection<TSource> InnerCollectionGeneric => InnerList;
                protected IUIntCountable InnerCountable => InnerList;
                protected ISimpleLinkedListCore InnerSimpleLinkedList => InnerList;
                #endregion InnerList

                ILinkedListNode<TDestination> IReadOnlyLinkedListBase2<ILinkedListNode<TDestination>>.First => GetNode(InnerList.First);
                ILinkedListNode<TDestination> IReadOnlyLinkedListBase2<ILinkedListNode<TDestination>>.Last => GetNode(InnerList.Last);

                public override TDestination FirstValue => InnerList.FirstValue;
                TDestination IReadOnlyLinkedListCore<TDestination>.LastValue => InnerList.LastValue;

                public bool SupportsReversedEnumeration => true;

                public override uint Count => InnerCountable.Count;
                int System.Collections.Generic.IReadOnlyCollection<TDestination>.Count => InnerCollection.Count;
                int System.Collections.Generic.ICollection<TDestination>.Count => InnerList.AsFromType<TList, System.Collections.Generic.ICollection<TSource>>().Count;
                int ICollection.Count => InnerCollection.Count;

                public override bool HasItems => InnerSimpleLinkedList.HasItems;

                public override bool IsReadOnly => InnerSimpleLinkedList.IsReadOnly;

                protected override object SyncRoot => InnerCollection.SyncRoot;
                protected override bool IsSynchronized => InnerCollection.IsSynchronized;

                object ICollection.SyncRoot => InnerCollection.SyncRoot;
                bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
                #endregion Properties

                public LinkedList(in TList list) => InnerList = list == null ? throw GetArgumentNullException(nameof(list)) : list;

                #region Methods
                #region Add
                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddAfter(ILinkedListNode<TDestination> node, TDestination value) => value is TSource _value ? GetNode(InnerList.AddAfter(GetNode(node), _value)) : throw GetArgumentException();

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddBefore(ILinkedListNode<TDestination> node, TDestination value) => value is TSource _value ? GetNode(InnerList.AddBefore(GetNode(node), _value)) : throw GetArgumentException();

                protected override ILinkedListNode<TDestination> AddFirstItem(TDestination value) => value is TSource _value ? GetNode(InnerList.AddFirst(_value)) : throw GetArgumentException();

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddFirst(TDestination value) => AddFirstItem(value);

                protected override ILinkedListNode<TDestination> AddLastItem(TDestination value) => value is TSource _value ? GetNode(InnerList.AddFirst(_value)) : throw GetArgumentException();

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddLast(TDestination value) => AddLastItem(value);

                protected void Add(TDestination item) => InnerCollectionGeneric.Add(item is TSource _item ? _item : throw GetArgumentException());

                void System.Collections.Generic.ICollection<TDestination>.Add(TDestination item) => Add(item);

                void ICollectionBase<TDestination>.Add(TDestination item) => Add(item);
                #endregion Add

                #region Remove
                void ILinkedList<TDestination>.Remove(ILinkedListNode<TDestination> node) => InnerList.Remove(GetNode(node));

                protected override TDestination RemoveFirstItem()
                {
                    ILinkedListNode<TSource> node = InnerList.First;

                    InnerList.Remove(node);

                    return node.Value;
                }

                void ILinkedList<TDestination>.RemoveFirst() => InnerList.RemoveFirst();
                void ILinkedList<TDestination>.RemoveLast() => InnerList.RemoveLast();

                bool System.Collections.Generic.ICollection<TDestination>.Remove(TDestination item) => item is TSource _item && ((System.Collections.Generic.ICollection<TSource>)InnerList).Remove(_item);

                public ILinkedListNode<TDestination> Remove(TDestination item) => item is TSource _item ? GetNode(InnerList.Remove(_item)) : throw GetArgumentException();

                bool ICollectionBase<TDestination>.Remove(TDestination item) => Remove(item) != null;
                #endregion Remove

                #region Enumeration
                public IEnumeratorInfo2<ILinkedListNode<TDestination>> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new LinkedListEnumerator<TDestination>(this, enumerationDirection, null, null);

                public System.Collections.Generic.IEnumerator<TDestination> GetEnumerator(in EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

                public IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>> GetNodeEnumerator2(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<ILinkedListNode<TDestination>>(GetNodeEnumerator(enumerationDirection), () => InnerCountable.Count);

                public IEnumeratorInfo2<TDestination> GetEnumerator2(in EnumerationDirection enumerationDirection) => new EnumeratorInfo<TDestination>(GetEnumerator(enumerationDirection));

                public IUIntCountableEnumeratorInfo<TDestination> GetEnumerator3(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<TDestination>(GetEnumerator2(enumerationDirection), () => InnerCountable.Count);

                public IUIntCountableEnumeratorInfo<TDestination> GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);
                public IUIntCountableEnumeratorInfo<TDestination> GetReversedEnumerator() => GetEnumerator3(EnumerationDirection.LIFO);

                public IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>> GetNodeEnumerator() => GetNodeEnumerator2(EnumerationDirection.FIFO);
                IUIntCountableEnumerator<TDestination> Enumeration.IEnumerable<IUIntCountableEnumerator<TDestination>>.GetEnumerator() => GetEnumerator();
                public IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>> GetReversedNodeEnumerator() => GetNodeEnumerator2(EnumerationDirection.LIFO);

                IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>> Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>>>.GetEnumerator() => GetNodeEnumerator();
                IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>> Extensions.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

                System.Collections.Generic.IEnumerator<ILinkedListNode<TDestination>> Collections.Extensions.Generic.IEnumerable<ILinkedListNode<TDestination>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

                System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerCollection.GetEnumerator();
                System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#if !CS8
                System.Collections.Generic.IEnumerator<TDestination> IEnumerable<TDestination>.GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);
                System.Collections.Generic.IEnumerator<TDestination> Extensions.Generic.IEnumerable<TDestination>.GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);
                IEnumerator<ILinkedListNode<TDestination>> IEnumerable<ILinkedListNode<TDestination>>.GetEnumerator() => GetNodeEnumerator();
#endif
                #endregion Enumeration

                #region Misc
                ILinkedListNode<TDestination>
#if CS8
                    ?
#endif
                    IReadOnlyLinkedListBase<TDestination, ILinkedListNode<TDestination>>.Find(TDestination value) => value is TSource _value ? GetNode(InnerList.Find(_value)) : null;
                ILinkedListNode<TDestination>
#if CS8
                    ?
#endif
                    IReadOnlyLinkedListBase<TDestination, ILinkedListNode<TDestination>>.FindLast(TDestination value) => value is TSource _value ? GetNode(InnerList.FindLast(_value)) : null;

                protected bool Contains(TDestination item) => item is TSource _item && InnerCollectionGeneric.Contains(_item);
                bool System.Collections.Generic.ICollection<TDestination>.Contains(TDestination item) => Contains(item);
                bool IReadOnlyCollectionBase<TDestination>.Contains(TDestination item) => Contains(item);

                void System.Collections.Generic.ICollection<TDestination>.CopyTo(TDestination[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);
                void ICollection.CopyTo(System.Array array, int index) => InnerList.CopyTo(array, index);

                public bool Equals(ILinkedList<TDestination>
#if CS8
                    ?
#endif
                    other) => other == null ? false : other is LinkedList _other ? object.Equals(InnerList, _other.InnerList) : object.Equals(InnerList, other);

                public bool MoveAfter(ILinkedListNode<TDestination> node, ILinkedListNode<TDestination> after) => InnerList.MoveAfter(GetNode(node), GetNode(after));
                public bool MoveBefore(ILinkedListNode<TDestination> node, ILinkedListNode<TDestination> before) => InnerList.MoveBefore(GetNode(node), GetNode(before));

                public void Swap(ILinkedListNode<TDestination> x, ILinkedListNode<TDestination> y) => InnerList.Swap(GetNode(x), GetNode(y));

                DotNetFix.Generic.IUIntCountableEnumerable<ILinkedListNode<TDestination>> ILinkedList<TDestination>.AsNodeEnumerable() => new UIntCountableEnumerable<ILinkedListNode<TDestination>, IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>>>(GetNodeEnumerator2(EnumerationDirection.FIFO));

                void IReadOnlyCollectionBase<TDestination>.CopyTo(TDestination[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

                public static ILinkedListNode<TDestination> GetNode<T>(in T node) where T : ILinkedListNode<TSource> => new LinkedListTypes<TList, T>.LinkedListNode(node);

                public static TNode
#if CS9
                    ?
#endif
                    TryGetNode(in ILinkedListNode<TDestination>
#if CS8
                    ?
#endif
                    node) => node is LinkedListNode _node ? _node.InnerNode : default;

                public static ILinkedListNode<TSource> GetNode(in ILinkedListNode<TDestination> node)
                {
                    TNode
#if CS9
                        ?
#endif
                    _node = TryGetNode(node);

                    return _node == null ? throw new ArgumentException("The given node is not contained in the current list.", nameof(node)) : _node;
                }

                protected override void ClearItems() => InnerList.AsFromType<IClearable>().Clear();

                public void Clear() => ClearItems();

                public IQueue<TDestination> AsQueue() => new Queue(InnerList.AsQueue());
                public IStack<TDestination> AsStack() => new Stack(InnerList.AsStack());
                #endregion Misc
                #endregion Methods

                public static bool operator ==(LinkedList x, ILinkedList<TDestination> y) => x.AsObject() == null ? y == null : x.Equals(y);
                public static bool operator !=(LinkedList x, ILinkedList<TDestination> y) => !(x == y);
            }
        }
    }
}
#endif
