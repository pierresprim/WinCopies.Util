﻿/* Copyright © Pierre Sprimont, 2020
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using WinCopies.Collections.DotNetFix.Generic;

#if WinCopies3
using System.Collections.Generic;
using System.Linq;

using WinCopies.Collections.Generic;
using WinCopies.Linq;

using static WinCopies.Collections.Resources.ExceptionMessages;
using static WinCopies.Collections.ThrowHelper;
using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;
#else
using System.Runtime.Serialization;
#endif
#endif

namespace WinCopies.Collections.DotNetFix
{
#if CS7
    public static class LinkedList
    {
        public static ILinkedList<T> GetLinkedList<T>() => new
#if WinCopies3
            Generic.
#endif
            LinkedList<T>();
        public static ILinkedList<T> GetLinkedList<T>(in System.Collections.Generic.IEnumerable<T> collection) => new
#if WinCopies3
            Generic.
#endif
            LinkedList<T>(collection);
        public static ILinkedList<T> GetLinkedList<T>(params T[] items) => new
#if WinCopies3
            Generic.
#endif
            LinkedList<T>(items);
    }

#if WinCopies3
    namespace Generic
    {
#endif
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class LinkedList<T> :
#if WinCopies3
            IEnumerableInfoLinkedList<T>, ISortable<T>, IExtensibleEnumerable<T>
#else
        System.Collections.Generic.LinkedList<T>, ILinkedList2<T>
#endif
    {
        #region Properties
        public bool IsReadOnly => false;

        public bool SupportsReversedEnumeration => true;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is empty.
        /// </summary>
        public LinkedList()
#if !WinCopies3
        : base()
#endif
        { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that contains elements copied from the specified <see cref="IEnumerable"/> and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable"/> whose elements are copied to the new <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public LinkedList(
#if WinCopies3
                in
#endif
            System.Collections.Generic.IEnumerable<T> collection)
#if !WinCopies3
: base(collection)
#endif
        {
#if !WinCopies3
            // Left empty.
#else
                foreach (T item in collection)

                    AddLast(item);
#endif
        }

#if !WinCopies3
        /// <summary>
        /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is serializable with the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> object containing the information required to serialize the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
        /// <param name="context">A <see cref="StreamingContext"/> object containing the source and destination of the serialized stream associated with the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
        protected LinkedList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            // Left empty.
        }
#endif
        #endregion

#if WinCopies3
            public class LinkedListNode : ILinkedListNode<T, LinkedListNode, LinkedList<T>>, ILinkedListNode<T>
            {
                private IReadOnlyLinkedListNodeBase2<T> _asReadOnly;
                private ILinkedListNodeBase2<T> _asReadOnly2;

                public bool IsReadOnly => false;

                public LinkedList<T> List { get; internal set; }

                public LinkedListNode Previous { get; internal set; }

                public LinkedListNode Next { get; internal set; }

                public T Value { get; set; }

                ILinkedList<T> ILinkedListNode<T>.List => List;

                ILinkedListNode<T> ILinkedListNode<T>.Previous => Previous;

                ILinkedListNode<T> ILinkedListNode<T>.Next => Next;

                IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

                IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;

                IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Previous => Previous;

                IReadOnlyLinkedListNodeBase2<T> IReadOnlyLinkedListNodeBase2<T>.Next => Next;

                ILinkedListNode<T, ILinkedList<T>> ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Previous => Previous;

                ILinkedListNode<T, ILinkedList<T>> ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Next => Next;

                ILinkedListNodeBase2<T> ILinkedListNodeBase2<T>.Previous => Previous;

                ILinkedListNodeBase2<T> ILinkedListNodeBase2<T>.Next => Next;

#if !CS8
                object IReadOnlyLinkedListNode.Value => Value;

                ILinkedList<T> IReadOnlyLinkedListNode<T, ILinkedList<T>>.List => List;

                ILinkedListNode<T, ILinkedList<T>> IReadOnlyLinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Previous => Previous;

                ILinkedListNode<T, ILinkedList<T>> IReadOnlyLinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Next => Next;

                IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Previous => Previous;

                IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Next => Next;

                IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>.List => List;
#endif

                public LinkedListNode(in T value) => Value = value;

                internal void OnRemove()
                {
                    List = null;
                    Previous = null;
                    Next = null;
                }

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
            new LinkedListNode<T>(this)
#if !CS8
                    )
#endif
                    ;
            }

            public class Enumerator : Enumerator<ILinkedListNode<T>>
            {
                private readonly uint _version;

                protected LinkedList<T> List { get; }

                protected IEnumeratorInfo2<ILinkedListNode<T>> InnerEnumerator { get; }

                protected override ILinkedListNode<T> CurrentOverride => InnerEnumerator.Current;

                public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

                public Enumerator(in IEnumeratorInfo2<ILinkedListNode<T>> enumerator, in LinkedList<T> list)
                {
                    InnerEnumerator = enumerator ?? throw GetArgumentNullException(nameof(enumerator));

                    List = list ?? throw GetArgumentNullException(nameof(list));

                    (list._enumeratorsCount == uint.MaxValue ? throw new InvalidOperationException("Too many enumerators are currently active.") : list).IncrementEnumeratorsCount();
                }

                protected override bool MoveNextOverride()
                {
                    ThrowIfVersionHasChanged(List._enumerableVersion, _version);

                    return InnerEnumerator.MoveNext();
                }

                protected override void ResetOverride2()
                {
                    ThrowIfVersionHasChanged(List._enumerableVersion, _version);

                    InnerEnumerator.Reset();
                }

                protected override void DisposeUnmanaged()
                {
                    List.DecrementEnumeratorsCount();

                    base.DisposeUnmanaged();
                }
            }

            public class UIntCountableEnumeratorInfo : Enumerator, IUIntCountableEnumeratorInfo<ILinkedListNode<T>>
            {
                public uint Count => List.Count;

                public UIntCountableEnumeratorInfo(in IEnumeratorInfo2<ILinkedListNode<T>> enumerator, in LinkedList<T> list) : base(enumerator, list) { /* Left empty. */ }
            }

        #region Fields
            private uint _enumeratorsCount = 0;
            private object _syncRoot;
            private uint _enumerableVersion = 0;
        #endregion

        #region Properties
            public LinkedListNode First { get; private set; }

            public T FirstValue => (First ?? throw GetEmptyListOrCollectionException()).Value;

            public LinkedListNode Last { get; private set; }

            public T LastValue => (Last ?? throw GetEmptyListOrCollectionException()).Value;

            ILinkedListNode<T> ILinkedList<T>.First => First;

            ILinkedListNode<T> ILinkedList<T>.Last => Last;

            public uint Count { get; private set; }

            int ICollection.Count => (int)Count;

            int System.Collections.Generic.IReadOnlyCollection<T>.Count => (int)Count;

            int System.Collections.Generic.ICollection<T>.Count => (int)Count;

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot
            {
                get
                {
                    if (_syncRoot == null)

                        _ = System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                    return _syncRoot;
                }
            }
        #endregion

        #region Methods
            private void IncrementEnumeratorsCount() => _enumeratorsCount++;

            private void DecrementEnumeratorsCount()
            {
                _enumeratorsCount--;

                if (_enumeratorsCount == 0)

                    _enumerableVersion = 0;
            }

            private static void ThrowIfNodeAlreadyHasList(in LinkedListNode node, in string argumentName)
            {
                if ((node ?? throw GetArgumentNullException(argumentName)).List != null)

                    throw new ArgumentException(NodeIsAlreadyContainedInAnotherListOrTree, argumentName);
            }

            private void ThrowIfNotContainedNode(in LinkedListNode node, in string argumentName) => _ThrowIfNotContainedNode(node ?? throw GetArgumentNullException(argumentName), argumentName);

            private void _ThrowIfNotContainedNode(in LinkedListNode node, in string argumentName) => ThrowHelper.ThrowIfNotContainedNode(node, argumentName, this);

            private void IncrementEnumerableVersion()
            {
                if (_enumeratorsCount > 0)

                    _enumerableVersion++;
            }

            protected virtual void OnNodeAdded(in LinkedListNode node)
            {
                node.List = this;

                Count++;
            }

            protected virtual void OnNewItemAdded(in LinkedListNode node)
            {
                OnNodeAdded(node);

                IncrementEnumerableVersion();
            }

            private void ReInitNodes()
            {
                First = null;

                Last = null;
            }

            protected virtual void OnItemRemoved(in LinkedListNode node)
            {
                node.OnRemove();

                Count--;

                IncrementEnumerableVersion();

                if (Count == 0)

                    ReInitNodes();
            }

            private void _Weld(in LinkedListNode previous, in LinkedListNode newNode, in LinkedListNode next)
            {
                if (previous == null) // If previous is null, next is First. So, we replace First by newNode.

                    First = newNode;

                else
                {
                    previous.Next = newNode;

                    newNode.Previous = previous;
                }

                if (next == null) // If next is null, previous is Last. So, we replace Last by newNode.

                    Last = newNode;

                else
                {
                    newNode.Next = next;

                    next.Previous = newNode;
                }
            }

            private void Weld(in LinkedListNode previous, in LinkedListNode newNode, in LinkedListNode next)
            {
                _Weld(previous, newNode, next);

                OnNewItemAdded(newNode);
            }

            private void Weld(in LinkedListNode previous, in LinkedListNode next)
            {
                previous.Next = next;

                next.Previous = previous;
            }



            private LinkedListNode AddNewItem(in LinkedListNode node, in Func<LinkedListNode> newNode, in string nodeArgumentName, in string newNodeArgumentName, in ActionIn<LinkedListNode, LinkedListNode> action)
            {
                ThrowIfNotContainedNode(node, nodeArgumentName);

                LinkedListNode _newNode = newNode();

                ThrowIfNodeAlreadyHasList(_newNode, newNodeArgumentName);
                ThrowIfNodesAreEqual(_newNode, node);

                action(node, _newNode);

                return _newNode;
            }

            private void AddNewItem(in LinkedListNode node, LinkedListNode newNode, in string nodeArgumentName, in string newNodeArgumentName, in ActionIn<LinkedListNode, LinkedListNode> action) => AddNewItem(node, () => newNode, nodeArgumentName, newNodeArgumentName, action);

            private LinkedListNode AddNewItem(in LinkedListNode node, in Func<LinkedListNode> newNode, in string nodeArgumentName, in ActionIn<LinkedListNode, LinkedListNode> action) => AddNewItem(node, newNode, nodeArgumentName, null, action);



            private void _AddAfter(in LinkedListNode addAfter, in LinkedListNode node) => Weld(addAfter, node, addAfter.Next);

            public void AddAfter(LinkedListNode addAfter, LinkedListNode node) => AddNewItem(addAfter, node, nameof(addAfter), nameof(node), _AddAfter);

            public LinkedListNode AddAfter(LinkedListNode node, T value) => AddNewItem(node, () => new LinkedListNode(value), nameof(node), _AddAfter);

            ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => AddAfter(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {GetName<LinkedListNode>()}."), value);



            private void _AddBefore(in LinkedListNode addBefore, in LinkedListNode node) => Weld(addBefore.Previous, node, addBefore);

            public void AddBefore(LinkedListNode addBefore, LinkedListNode node) => AddNewItem(addBefore, node, nameof(addBefore), nameof(node), _AddBefore);

            public LinkedListNode AddBefore(LinkedListNode node, T value) => AddNewItem(node, () => new LinkedListNode(value), nameof(node), _AddBefore);

            ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value) => AddBefore(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {GetName<LinkedListNode>()}."), value);



            private void AddFirstOrLast(in LinkedListNode node, in Action action)
            {
                action();

                OnNewItemAdded(node);
            }



            private void _AddFirst(LinkedListNode node) => AddFirstOrLast(node, () =>
            {
                if (First != null)

                    Weld(node, First);

#if !CS8
                if (Last == null)
#endif
                Last
#if CS8
                    ??=
#else
                    =
#endif
                    node;

                First = node;
            });

            public LinkedListNode AddFirst(in T value)
            {
                var node = new LinkedListNode(value);

                _AddFirst(node);

                return node;
            }

            public void AddFirst(in LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddFirst(node);
            }

            ILinkedListNode<T> ILinkedList<T>.AddFirst(T value) => AddFirst(value);



            private void _AddLast(LinkedListNode node) => AddFirstOrLast(node, () =>
            {
                if (Last != null)
                {
                    node.Previous = Last;

                    Last.Next = node;
                }

#if !CS8
                if (First == null)
#endif
                First
#if CS8
                    ??=
#else
                    =
#endif
                    node;

                Last = node;
            });

            public LinkedListNode AddLast(in T value)
            {
                var node = new LinkedListNode(value);

                _AddLast(node);

                return node;
            }

            public void AddLast(in LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddLast(node);
            }

            ILinkedListNode<T> ILinkedList<T>.AddLast(T value) => AddLast(value);



            void System.Collections.Generic.ICollection<T>.Add(T item) => AddLast(item);

            void ICollectionBase<T>.Add(T item) => AddLast(item);



            private LinkedListNode Find(T value, EnumerationDirection enumerationDirection) => new Enumerable<ILinkedListNode<T>>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefault(value == null ?

#if !CS9
                (Func<ILinkedListNode<T>, bool>)
#endif
                (node => node.Value == null) : (node => value.Equals(node.Value))) as LinkedListNode;

            public LinkedListNode Find(T value) => Find(value, EnumerationDirection.FIFO);

            ILinkedListNode<T> ILinkedList<T>.Find(T value) => Find(value);

            public LinkedListNode FindLast(T value) => Find(value, EnumerationDirection.LIFO);

            ILinkedListNode<T> ILinkedList<T>.FindLast(T value) => FindLast(value);

#if !CS8
            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => Find(value);

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => FindLast(value);
#endif



            protected LinkedListEnumerator<T> GetLinkedListEnumerator(in EnumerationDirection enumerationDirection) => new
#if !CS9
                LinkedListEnumerator<T>
#endif
                (this, enumerationDirection, null, null);

            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo(GetLinkedListEnumerator(enumerationDirection), this);

            public System.Collections.Generic.IEnumerator<T> GetEnumerator(in EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value);

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

            public System.Collections.Generic.IEnumerator<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetReversedNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

            IUIntCountableEnumeratorInfo<ILinkedListNode<T>> IEnumerableInfoLinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator();

            IUIntCountableEnumeratorInfo<ILinkedListNode<T>> IEnumerableInfoLinkedList<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

            IEnumeratorInfo<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>, IEnumeratorInfo<ILinkedListNode<T>>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

            IEnumeratorInfo<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>, IEnumeratorInfo<ILinkedListNode<T>>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Extensions.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

            System.Collections.Generic.IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

            public IUIntCountableEnumeratorInfo<T> GetUIntCountableEnumeratorInfoEnumerator() => new UIntCountableEnumeratorInfo<T>(new EnumeratorInfo<T>(GetEnumerator()), () => Count);

            IUIntCountableEnumeratorInfo<T> IEnumerableInfoLinkedList<T>.GetEnumerator() => GetUIntCountableEnumeratorInfoEnumerator();

            private IUIntCountableEnumeratorInfo<T> _GetReversedEnumerator() => new UIntCountableEnumeratorInfo<T>(new EnumeratorInfo<T>(GetReversedEnumerator()), () => Count);

            IUIntCountableEnumeratorInfo<T> IEnumerableInfoLinkedList<T>.GetReversedEnumerator() => _GetReversedEnumerator();

            IUIntCountableEnumerator<T> IUIntCountableEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetUIntCountableEnumeratorInfoEnumerator();

#if !CS8
            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => First;

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => Last;

#if WinCopies3
            IUIntCountableEnumerator<T> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetUIntCountableEnumeratorInfoEnumerator();

            IUIntCountableEnumerator<T> IEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetUIntCountableEnumeratorInfoEnumerator();
#endif

            IEnumeratorInfo<T> IEnumerable<T, IEnumeratorInfo<T>>.GetEnumerator() => new EnumeratorInfo<T>(GetEnumerator());

            IEnumeratorInfo<T> Collections.Generic.IEnumerable<T, IEnumeratorInfo<T>>.GetReversedEnumerator() => new EnumeratorInfo<T>(GetReversedEnumerator());

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();

            IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => GetUIntCountableEnumeratorInfoEnumerator();

            IUIntCountableEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => _GetReversedEnumerator();

            IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

            IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

            IUIntCountableEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator() => GetUIntCountableEnumeratorInfoEnumerator();

            IUIntCountableEnumerator<T> IReadOnlyLinkedList2<T>.GetReversedEnumerator() => _GetReversedEnumerator();
#endif

#if !(WinCopies3 || CS8)
            System.Collections.Generic.IEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator();
#endif

            public void Remove(LinkedListNode node)
            {
                ThrowIfNotContainedNode(node, nameof(node));

                _Remove(node);
            }

            private void _Remove(LinkedListNode node)
            {
                if (node == First)

                    RemoveFirst();

                else if (node == Last)

                    RemoveLast();

                else
                {
                    Weld(node.Previous, node.Next);

                    OnItemRemoved(node);
                }
            }

            void ILinkedList<T>.Remove(ILinkedListNode<T> node) => Remove(node as LinkedListNode ?? throw new ArgumentException($"{nameof(node)} must be an instance of {GetName<LinkedListNode>()}."));

            public LinkedListNode Remove2(T item)
            {
                LinkedListNode node = Find(item);

                if (node == null)

                    return null;

                Remove(node);

                return node;
            }

            public bool Remove(T item) => Remove2(item) != null;

            ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove2(item);

            public void RemoveFirst()
            {
#if !WinCopies3
            ThrowIfEmpty
#else
                ThrowIfEmptyListOrCollection
#endif
(this);

                LinkedListNode node = First;

                First = node.Next;

                OnItemRemoved(node);
            }

            public void RemoveLast()
            {
#if !WinCopies3
            ThrowIfEmpty
#else
                ThrowIfEmptyListOrCollection
#endif
(this);

                LinkedListNode node = Last;

                Last = node.Previous;

                OnItemRemoved(node);
            }

            public void Clear()
            {
                if (_enumeratorsCount > 0)

                    _enumerableVersion++;

                while (Count != 0)

                    RemoveFirst();

                ReInitNodes();
            }

            public bool Contains(T item)
            {
                Predicate<T> predicate = item == null ?
#if !CS9
                    (Predicate<T>)
#endif
                    (_item => _item == null) : (_item => item.Equals(_item));

                LinkedListNode node = First;

                while (node != null)
                {
                    if (predicate(node.Value))

                        return true;

                    node = node.Next;
                }

                return false;
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                ThrowOnInvalidCopyToArrayOperation(array, arrayIndex, Count, nameof(array), nameof(arrayIndex));

                foreach (T item in this)

                    array[arrayIndex++] = item;
            }

            public void CopyTo(Array array, int index) => EnumerableExtensions.CopyTo(this, array, index, Count);

            protected bool OnNodeCoupleAction(in ILinkedListNode<T> x, in string xArgumentName, in ILinkedListNode<T> y, in string yArgumentName, in Func<LinkedListNode, LinkedListNode, bool> func) => x is LinkedListNode _x
                    ? y is LinkedListNode _y ? func(_x, _y) : throw (y == null ? GetArgumentNullException(xArgumentName) : GetNotContainedLinkedListNodeException(nameof(y)))
                    : throw (x == null ? GetArgumentNullException(yArgumentName) : GetNotContainedLinkedListNodeException(nameof(x)));

            /// <summary>
            /// Checks if a node can be moved and, if yes, removes it. This method DOES NOT perform null checks and DOES NOT re-add the item. This is only a method for common move actions, the other actions needing to be performed separately.
            /// </summary>
            /// <param name="node">The node to move.</param>
            /// <param name="nodeArgumentName">The argument name of <paramref name="node"/> for exception throws.</param>
            /// <param name="other">The reference node (the new previous or next node of <paramref name="node"/>) when the move has completed.</param>
            /// <param name="otherArgumentName">The argument name of <paramref name="other"/> for exception throws.</param>
            /// <exception cref="ArgumentException"><paramref name="node"/> or <paramref name="other"/> is not in the current list. OR <paramref name="node"/> and <paramref name="other"/> are equal.</exception>
            protected void OnMove(in LinkedListNode node, in string nodeArgumentName, in LinkedListNode other, in string otherArgumentName)
            {
                _ThrowIfNotContainedNode(node, nodeArgumentName);
                _ThrowIfNotContainedNode(other, otherArgumentName);
                ThrowIfNodesAreEqual(node, other);

                _Remove(node);
            }

            bool ILinkedList3<T>.MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => OnNodeCoupleAction(node, nameof(node), after, nameof(after), _MoveAfter);

            public bool MoveAfter(in LinkedListNode node, in LinkedListNode after) => _MoveAfter(node ?? throw GetArgumentNullException(nameof(node)), after ?? throw GetArgumentNullException(nameof(after)));

            private bool _MoveAfter(LinkedListNode node, LinkedListNode after)
            {
                if (after.Next == node)

                    return false;

                OnMove(node, nameof(node), after, nameof(after));

                // As OnMove calls a method stack that update EnumerableVersion, we just need to call _Weld and OnNodeAdded().

                _Weld(after, node, after.Next);

                OnNodeAdded(node);

                return true;
            }

            bool ILinkedList3<T>.MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => OnNodeCoupleAction(node, nameof(node), before, nameof(before), _MoveBefore);

            public bool MoveBefore(in LinkedListNode node, in LinkedListNode before) => _MoveBefore(node ?? throw GetArgumentNullException(nameof(node)), before ?? throw GetArgumentNullException(nameof(before)));

            private bool _MoveBefore(LinkedListNode node, LinkedListNode before)
            {
                if (before.Previous == node)

                    return false;

                OnMove(node, nameof(node), before, nameof(before));

                // As OnMove calls a method stack that update EnumerableVersion, we just need to call _Weld and OnNodeAdded().

                _Weld(before.Previous, node, before);

                OnNodeAdded(node);

                return true;
            }

            void ILinkedList3<T>.Swap(ILinkedListNode<T> x, ILinkedListNode<T> y) => OnNodeCoupleAction(x, nameof(x), y, nameof(y), _Swap);

            public void Swap(in LinkedListNode x, in LinkedListNode y) => _Swap(x ?? throw GetArgumentNullException(nameof(x)), y ?? throw GetArgumentNullException(nameof(y)));

            private bool _Swap(LinkedListNode x, LinkedListNode y)
            {
                _ThrowIfNotContainedNode(x, nameof(x));
                ThrowIfNodesAreEqual(x, y);
                _ThrowIfNotContainedNode(y, nameof(y));



                void swap(in LinkedListNode _x, in LinkedListNode _y)
                {
                    LinkedListNode[] nodes = { _x.Previous, _y, _x, _y.Next };

                    _y.Previous = null;
                    _x.Next = null;

                    _Weld(nodes[0], nodes[1], nodes[2]);

                    nodes[2].Next = nodes[3];

                    if (nodes[3] != null)

                        nodes[3].Previous = nodes[2];
                }



                IncrementEnumerableVersion();



                //Action updateFirst = null;
                //Action updateLast = null;



                if (x == First)

                    First = y;

                else if (y == First)

                    First = x;



                if (x == Last)

                    Last = y;

                else if (y == Last)

                    Last = x;



                if (x.Next == y)

                    swap(x, y);

                else if (y.Next == x)

                    swap(y, x);

                else
                {
                    LinkedListNode[] nodes = { x.Previous, y, x.Next, y.Previous, x, y.Next };

                    y.Previous = null;
                    y.Next = null;
                    x.Previous = null;
                    x.Next = null;

                    _Weld(nodes[0], nodes[1], nodes[2]);

                    _Weld(nodes[3], nodes[4], nodes[5]);

                    //LinkedListNode tempPrevious = y.Previous;
                    //LinkedListNode tempNext = y.Next;

                    //y.Previous = x.Previous;
                    //y.Next = x.Next;

                    //x.Previous = tempPrevious;
                    //x.Next = tempNext;
                }

                return true;
            }

            public IUIntCountableEnumerable<ILinkedListNode<T>> AsNodeEnumerable() => new UIntCountableEnumerable<ILinkedListNode<T>, IUIntCountableEnumerator<ILinkedListNode<T>>>(GetNodeEnumerator());

            void ILinkedList<T>.Add(T item) => AddLast(item);

        #region ISortable implementation
            public void Sort() => Sort(System.Collections.Generic.Comparer<T>.Default.Compare);

            void ISortable.Sort(Comparison comparison) => Sort((T x, T y) => comparison(x, y));

            public void Sort(Comparison<T> comparison) => Sort((LinkedListNode x, LinkedListNode y) => comparison(x.Value, y.Value));

            public void Sort(Comparison<LinkedListNode> comparison)
            {
                LinkedListNode max;
                LinkedListNode current = Last;
                LinkedListNode _previous;

                void sort()
                {
                    _previous = _previous.Previous;

#if DEBUG
                    if (_previous == null)

                        throw new Exception();
#endif

                    if (comparison(_previous, max) > 0)

                        max = _previous;
                }

                for (uint i = Count - 1; i > 0; i--)
                {
                    max = current;
                    _previous = current;

                    for (uint j = i - 1; j > 0; j--)

                        sort();

                    sort(); // In order to avoid uint j >= 0; j--, because uint j = 0; j-- won't result to -1.

                    if (current == max)

                        current = current.Previous;

                    else
                    {
                        Swap(current, max);

                        current = max.Previous;
                    }
                }
            }
        #endregion



            public System.Collections.Generic.IEnumerable<LinkedListNode> AddRangeBefore(in LinkedListNode node, in System.Collections.Generic.IEnumerable<T> values) => AddRange(node, values, AddBefore);

            public void AddRangeBefore(in LinkedListNode node, in System.Collections.Generic.IEnumerable<LinkedListNode> values) => AddRange(node, values, AddBefore);

            public System.Collections.Generic.IEnumerable<LinkedListNode> AddRangeAfter(in LinkedListNode node, in System.Collections.Generic.IEnumerable<T> values) => AddRange(node, values, AddAfter);

            public void AddRangeAfter(in LinkedListNode node, in System.Collections.Generic.IEnumerable<LinkedListNode> values) => AddRange(node, values, AddAfter);



            protected ArrayBuilder<LinkedListNode> AddRange(System.Collections.Generic.IEnumerable<T> values, FuncIn<T, LinkedListNode> action)
            {
                var builder = new ArrayBuilder<LinkedListNode>();

                foreach (T item in values)

                    _ = builder.AddLast(action(item));

                return builder;
            }

            protected void AddRange(in System.Collections.Generic.IEnumerable<LinkedListNode> values, in ActionIn<LinkedListNode> action)
            {
                foreach (LinkedListNode item in values)

                    action(item);
            }


            protected ArrayBuilder<LinkedListNode> AddRange(LinkedListNode node, in System.Collections.Generic.IEnumerable<T> values, Func<LinkedListNode, T, LinkedListNode> action) => AddRange(values, (in T value) => action(node, value));

            protected void AddRange(LinkedListNode node, in System.Collections.Generic.IEnumerable<LinkedListNode> values, Action<LinkedListNode, LinkedListNode> action) => AddRange(values, (in LinkedListNode newNode) => action(node, newNode));



            public ArrayBuilder<LinkedListNode> AddRangeFirst(in System.Collections.Generic.IEnumerable<T> values) => AddRange(values, AddFirst);

            public void AddRangeFirst(in System.Collections.Generic.IEnumerable<LinkedListNode> values) => AddRange(values, AddFirst);

            public ArrayBuilder<LinkedListNode> AddRangeLast(in System.Collections.Generic.IEnumerable<T> values) => AddRange(values, AddLast);

            public void AddRangeLast(in System.Collections.Generic.IEnumerable<LinkedListNode> values) => AddRange(values, AddLast);



            void IAppendableExtensibleEnumerable<T>.Append(T item) => AddLast(item);

            void IAppendableExtensibleEnumerable<T>.AppendRange(System.Collections.Generic.IEnumerable<T> items) => AddRangeLast(items);



            void IPrependableExtensibleEnumerable<T>.Prepend(T item) => AddFirst(item);

            void IPrependableExtensibleEnumerable<T>.PrependRange(System.Collections.Generic.IEnumerable<T> items) => AddRangeFirst(items);
        #endregion

            ~LinkedList() => Clear();
#endif
    }
#if WinCopies3
    }
#endif
#endif
}