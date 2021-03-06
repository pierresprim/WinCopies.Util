﻿/* Copyright © Pierre Sprimont, 2021
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

#if WinCopies3 && CS7

using System;
using System.Collections;
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

using static WinCopies.Collections.ThrowHelper;
using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination> where TSource : TDestination
    {
        public static class LinkedListTypes<TList, TNode> where TNode : ILinkedListNode<TSource> where TList : ILinkedList3<TSource>
        {
            public class LinkedListNode : ILinkedListNode<TDestination>, IEquatable<ILinkedListNode<TDestination>>
            {
                protected internal TNode InnerNode { get; }

                ILinkedList<TDestination> ILinkedListNode<TDestination>.List => GetList();

                bool ILinkedListNodeBase<TDestination>.IsReadOnly => InnerNode.IsReadOnly;

                ILinkedList<TDestination> IReadOnlyLinkedListNode<TDestination, ILinkedList<TDestination>>.List => GetList();

                IReadOnlyLinkedList<TDestination> IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedList<TDestination>>.List => GetList();

                ILinkedListNode<TDestination> ILinkedListNode<TDestination>.Previous => GetNode(InnerNode.Previous);

                ILinkedListNode<TDestination, ILinkedList<TDestination>> IReadOnlyLinkedListNode<TDestination, ILinkedListNode<TDestination, ILinkedList<TDestination>>, ILinkedList<TDestination>>.Previous => GetNode(InnerNode.Previous);

                IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedList<TDestination>> IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedList<TDestination>>, IReadOnlyLinkedList<TDestination>>.Previous => GetNode(InnerNode.Previous);

                IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => GetNode(InnerNode.Previous);

                ILinkedListNode<TDestination> ILinkedListNode<TDestination>.Next => GetNode(InnerNode.Next);

                ILinkedListNode<TDestination, ILinkedList<TDestination>> IReadOnlyLinkedListNode<TDestination, ILinkedListNode<TDestination, ILinkedList<TDestination>>, ILinkedList<TDestination>>.Next => GetNode(InnerNode.Next);

                IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedList<TDestination>> IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedListNode<TDestination, IReadOnlyLinkedList<TDestination>>, IReadOnlyLinkedList<TDestination>>.Next => GetNode(InnerNode.Next);

                IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => GetNode(InnerNode.Next);

                TDestination ILinkedListNodeBase<TDestination>.Value { get => InnerNode.Value; set => InnerNode.Value = InnerNode.IsReadOnly ? throw GetReadOnlyListOrCollectionException() : value is TSource _value ? _value : throw GetArgumentException(); }

                TDestination IReadOnlyLinkedListNodeBase<TDestination>.Value => InnerNode.Value;

                object IReadOnlyLinkedListNode.Value => InnerNode.Value;

                public LinkedListNode(in TNode node) => InnerNode = node == null ? throw GetArgumentNullException(nameof(node)) : node;

                public static LinkedListTypes<TList, T>.LinkedListNode GetNode<T>(in T node) where T : ILinkedListNode<TSource> => new
#if !CS9
                    LinkedListTypes<TList, T>.LinkedListNode
#endif
                    (node);

                private LinkedListTypes<ILinkedList3<TSource>, TNode>.LinkedList GetList() => new
#if !CS9
                    LinkedListTypes<ILinkedList3<TSource>, TNode>.LinkedList
#endif
                    ((ILinkedList3<TSource>)InnerNode.List);

                public bool Equals(ILinkedListNode<TDestination> other) => object.Equals(InnerNode, LinkedList.TryGetNode(other));

                public static bool operator ==(LinkedListNode x, ILinkedListNode<TDestination> y) => x == null ? y == null : x.Equals(y);

                public static bool operator !=(LinkedListNode x, ILinkedListNode<TDestination> y) => !(x == y);
            }

            public class LinkedList : ILinkedList3<TDestination>, IEquatable<ILinkedList<TDestination>>
            {
                protected TList InnerList { get; }

                bool ILinkedList2<TDestination>.IsReadOnly => false;

                ILinkedListNode<TDestination> ILinkedList<TDestination>.First => GetNode(InnerList.First);

                ILinkedListNode<TDestination> ILinkedList<TDestination>.Last => GetNode(InnerList.Last);

                TDestination IReadOnlyLinkedList2<TDestination>.FirstValue => InnerList.FirstValue;

                TDestination IReadOnlyLinkedList2<TDestination>.LastValue => InnerList.LastValue;

                IReadOnlyLinkedListNode<TDestination> IReadOnlyLinkedList<TDestination>.First => GetNode(InnerList.First);

                IReadOnlyLinkedListNode<TDestination> IReadOnlyLinkedList<TDestination>.Last => GetNode(InnerList.Last);

                bool IReadOnlyLinkedList<TDestination>.SupportsReversedEnumeration => true;

                bool Enumeration.IEnumerable.SupportsReversedEnumeration => true;

                uint IReadOnlyLinkedList<TDestination>.Count => InnerList.Count;

                int IReadOnlyCollection<TDestination>.Count => ((IReadOnlyCollection<TSource>)InnerList).Count;

                uint IUIntCountable.Count => InnerList.Count;

                ILinkedListNode<TDestination> ILinkedList<TDestination>.Find(TDestination value) => value is TSource _value ? GetNode(InnerList.Find(_value)) : null;

                ILinkedListNode<TDestination> ILinkedList<TDestination>.FindLast(TDestination value) => value is TSource _value ? GetNode(InnerList.FindLast(_value)) : null;

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddAfter(ILinkedListNode<TDestination> node, TDestination value) => value is TSource _value ? GetNode(InnerList.AddAfter(GetNode(node), _value)) : throw GetArgumentException();

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddBefore(ILinkedListNode<TDestination> node, TDestination value) => value is TSource _value ? GetNode(InnerList.AddBefore(GetNode(node), _value)) : throw GetArgumentException();

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddFirst(TDestination value) => value is TSource _value ? GetNode(InnerList.AddFirst(_value)) : throw GetArgumentException();

                ILinkedListNode<TDestination> ILinkedList<TDestination>.AddLast(TDestination value) => GetNode(InnerList.AddLast(value is TSource _value ? _value : throw GetArgumentException()));

                void ILinkedList<TDestination>.Remove(ILinkedListNode<TDestination> node) => InnerList.Remove(GetNode(node));

                IReadOnlyLinkedListNode<TDestination> IReadOnlyLinkedList<TDestination>.Find(TDestination value) => value is TSource _value ? GetNode(InnerList.Find(_value)) : null;

                IReadOnlyLinkedListNode<TDestination> IReadOnlyLinkedList<TDestination>.FindLast(TDestination value) => value is TSource _value ? GetNode(InnerList.FindLast(_value)) : null;

                public IEnumeratorInfo2<ILinkedListNode<TDestination>> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new LinkedListEnumerator<TDestination>(this, enumerationDirection, null, null);

                public System.Collections.Generic.IEnumerator<TDestination> GetEnumerator(in EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

                public IUIntCountableEnumeratorInfo<ILinkedListNode<TDestination>> GetNodeEnumerator2(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<ILinkedListNode<TDestination>>(GetNodeEnumerator(enumerationDirection), () => (uint)InnerList.Count);

                public IEnumeratorInfo2<TDestination> GetEnumerator2(in EnumerationDirection enumerationDirection) => new EnumeratorInfo<TDestination>(GetEnumerator(enumerationDirection));

                public IUIntCountableEnumeratorInfo<TDestination> GetEnumerator3(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<TDestination>(GetEnumerator2(enumerationDirection), () => (uint)InnerList.Count);

                IUIntCountableEnumerator<TDestination> ILinkedList<TDestination>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

                IUIntCountableEnumerator<TDestination> ILinkedList<TDestination>.GetReversedEnumerator() => GetEnumerator3(EnumerationDirection.LIFO);

                IUIntCountableEnumerator<ILinkedListNode<TDestination>> ILinkedList<TDestination>.GetNodeEnumerator() => GetNodeEnumerator2(EnumerationDirection.FIFO);

                IUIntCountableEnumerator<ILinkedListNode<TDestination>> ILinkedList<TDestination>.GetReversedNodeEnumerator() => GetNodeEnumerator2(EnumerationDirection.LIFO);

                System.Collections.Generic.IEnumerator<ILinkedListNode<TDestination>> System.Collections.Generic.IEnumerable<ILinkedListNode<TDestination>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

                System.Collections.Generic.IEnumerator<ILinkedListNode<TDestination>> Collections.Generic.IEnumerable<ILinkedListNode<TDestination>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

                System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

                int ICollection<TDestination>.Count => ((ICollection<TDestination>)InnerList).Count;

                int ICollection.Count => ((ICollection)InnerList).Count;

                bool ICollection<TDestination>.IsReadOnly => false;

                object ICollection.SyncRoot => InnerList.SyncRoot;

                bool ICollection.IsSynchronized => InnerList.IsSynchronized;

                public LinkedList(in TList list) => InnerList = list == null ? throw GetArgumentNullException(nameof(list)) : list;

                public static ILinkedListNode<TDestination> GetNode<T>(in T node) where T : ILinkedListNode<TSource> => new LinkedListTypes<TList, T>.LinkedListNode(node);

                public static TNode TryGetNode(in ILinkedListNode<TDestination> node) => node is LinkedListNode _node ? _node.InnerNode : default;

                public static ILinkedListNode<TSource> GetNode(in ILinkedListNode<TDestination> node)
                {
                    TNode _node = TryGetNode(node);

                    return _node == null ? throw new ArgumentException("The given node is not contained in the current list.", nameof(node)) : _node;
                }

                void ILinkedList<TDestination>.RemoveFirst() => InnerList.RemoveFirst();

                void ILinkedList<TDestination>.RemoveLast() => InnerList.RemoveLast();

                void ICollection<TDestination>.Add(TDestination item) => InnerList.Add(item is TSource _item ? _item : throw GetArgumentException());

                void ICollection<TDestination>.Clear() => InnerList.Clear();

                bool ICollection<TDestination>.Contains(TDestination item) => item is TSource _item && InnerList.Contains(_item);

                void ICollection<TDestination>.CopyTo(TDestination[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

                bool ICollection<TDestination>.Remove(TDestination item) => item is TSource _item && ((ICollection<TSource>)InnerList).Remove(_item);

                void ICollection.CopyTo(Array array, int index) => InnerList.CopyTo(array, index);

                public bool Equals(ILinkedList<TDestination> other) => other == null ? false : other is LinkedList _other ? object.Equals(InnerList, _other.InnerList) : object.Equals(InnerList, other);

                public static bool operator ==(LinkedList x, ILinkedList<TDestination> y) => x == null ? y == null : x.Equals(y);

                public static bool operator !=(LinkedList x, ILinkedList<TDestination> y) => !(x == y);

                System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

                public ILinkedListNode<TDestination> Remove(TDestination item) => item is TSource _item ? GetNode(InnerList.Remove(_item)) : throw GetArgumentException();

                public bool MoveAfter(ILinkedListNode<TDestination> node, ILinkedListNode<TDestination> after) => InnerList.MoveAfter(GetNode(node), GetNode(after));

                public bool MoveBefore(ILinkedListNode<TDestination> node, ILinkedListNode<TDestination> before) => InnerList.MoveBefore(GetNode(node), GetNode(before));

                public void Swap(ILinkedListNode<TDestination> x, ILinkedListNode<TDestination> y) => InnerList.Swap(GetNode(x), GetNode(y));

#if !CS8
                System.Collections.Generic.IEnumerator<TDestination> System.Collections.Generic.IEnumerable<TDestination>.GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

                System.Collections.Generic.IEnumerator<TDestination> Collections.Generic.IEnumerable<TDestination>.GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

                IUIntCountableEnumerator<TDestination> IReadOnlyLinkedList2<TDestination>.GetEnumerator() => GetEnumerator3(EnumerationDirection.FIFO);

                IUIntCountableEnumerator<TDestination> IReadOnlyLinkedList2<TDestination>.GetReversedEnumerator() => GetEnumerator3(EnumerationDirection.LIFO);
#endif
            }
        }
    }
}

#endif
