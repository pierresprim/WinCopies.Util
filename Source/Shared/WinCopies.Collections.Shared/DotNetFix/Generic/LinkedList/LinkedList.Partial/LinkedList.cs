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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.Collections.Resources.ExceptionMessages;
using static WinCopies.Collections.ThrowHelper;
using static WinCopies.ThrowHelper;
using static WinCopies.UtilHelpers;

#if CS8
using System.Diagnostics.CodeAnalysis;
#endif

namespace WinCopies.Collections.DotNetFix
{
    public static class LinkedList
    {
        public static ILinkedList<T> GetLinkedList<T>() => new Generic.LinkedList<T>();
        public static ILinkedList<T> GetLinkedList<T>(in System.Collections.Generic.IEnumerable<T> collection) => new Generic.LinkedList<T>(collection);
        public static ILinkedList<T> GetLinkedList<T>(params T[] items) => new Generic.LinkedList<T>(items);
    }

    namespace Generic
    {
        [Serializable, DebuggerDisplay("Count = {Count}")]
        public abstract class LinkedListBase<TItems, TNodes> : IQueueCommon<TItems>, IStackCommon<TItems>, ISimpleLinkedListBase2
        {
            public abstract bool IsReadOnly { get; }

            public abstract uint Count { get; }

            public abstract bool HasItems { get; }

            protected abstract bool IsSynchronized { get; }
            protected abstract object SyncRoot { get; }

            bool ISimpleLinkedListBase2.IsSynchronized => IsSynchronized;
            object ISimpleLinkedListBase2.SyncRoot => IsSynchronized;

            public abstract TItems FirstValue { get; }

            TItems
#if CS9
                ?
#endif
                IPeekable<TItems>.Peek() => FirstValue;
            bool IPeekable<TItems>.TryPeek(out TItems
#if CS9
                ?
#endif
                result)
            {
                if (HasItems)
                {
                    result = FirstValue;

                    return true;
                }

                result = default;

                return false;
            }

            protected abstract TNodes AddFirstItem(TItems value);
            protected abstract TNodes AddLastItem(TItems value);

            protected abstract TItems RemoveFirstItem();

            protected bool TryRemove(in Func<TItems
#if CS9
                ?
#endif
                > func, out TItems
#if CS9
                ?
#endif
                result)
            {
                if (Count > 0)
                {
                    result = func();

                    return true;
                }

                result = default;

                return false;
            }

            protected bool TryRemove(in Func<TItems
#if CS9
                ?
#endif
                > func, out object
#if CS9
                ?
#endif
                result)
            {
                if (TryRemove(RemoveFirstItem, out TItems _result))
                {
                    result = _result;

                    return true;
                }

                result = null;

                return false;
            }

            public void Enqueue(TItems
#if CS9
                ?
#endif
                item) => AddLastItem(item);
            public TItems
#if CS9
                ?
#endif
                Dequeue() => RemoveFirstItem();
            public bool TryDequeue(out TItems
#if CS9
                ?
#endif
                result) => TryRemove(RemoveFirstItem, out result);

            public void Push(TItems
#if CS9
                ?
#endif
                item) => AddFirstItem(item);
            public TItems
#if CS9
                ?
#endif
                Pop() => RemoveFirstItem();
            public bool TryPop(out TItems
#if CS9
                ?
#endif
                result) => TryRemove(RemoveFirstItem, out result);

            protected abstract void ClearItems();

            void IListCommon<TItems>.Add(TItems value) => AddLastItem(value);
            TItems IListCommon<TItems>.Remove() => RemoveFirstItem();
            bool IListCommon<TItems>.TryRemove(out TItems result) => TryRemove(RemoveFirstItem, out result);

            void IClearable.Clear() => ClearItems();
#if !CS8
            void IQueueCore.Enqueue(object item) => Enqueue((TItems)item);
            object IQueueCore.Dequeue() => RemoveFirstItem();
            public bool TryDequeue(out object result) => TryRemove(RemoveFirstItem, out result);

            void IStackCore.Push(object item) => Push((TItems)item);
            object IStackCore.Pop() => RemoveFirstItem();
            public bool TryPop(out object result) => TryRemove(RemoveFirstItem, out result);

            void IListCommon.Add(object value) => AddLastItem((TItems)value);
            object IListCommon.Remove() => RemoveFirstItem();
            bool IListCommon.TryRemove(out object result) => TryRemove(RemoveFirstItem, out result);

            object IPeekable.Peek() => FirstValue;
            bool IPeekable.TryPeek(out object result) => UtilHelpers.TryGetValue<TItems>(this.AsFromType<IPeekable<TItems>>().TryPeek, out result);
#endif
        }

        [DebuggerDisplay("Count = {Count}")]
        public partial class LinkedList<T> : LinkedListBase<T, LinkedList<T>.LinkedListNode>, IEnumerableInfoLinkedList<T>, ISortable<T>, IExtensibleEnumerable<T>
        {
            #region Fields
            private uint _count;
            private uint _enumeratorsCount = 0;
            private uint _enumerableVersion = 0;
            #endregion

            #region Properties
            public override bool IsReadOnly => false;

            public bool SupportsReversedEnumeration => true;

            public LinkedListNode First { get; private set; }

            public override T FirstValue => (First ?? throw GetEmptyListOrCollectionException()).Value;

            public LinkedListNode Last { get; private set; }

            public T LastValue => (Last ?? throw GetEmptyListOrCollectionException()).Value;

            ILinkedListNode<T> IReadOnlyLinkedListBase2<ILinkedListNode<T>>.First => First;
            ILinkedListNode<T> IReadOnlyLinkedListBase2<ILinkedListNode<T>>.Last => Last;

            public override uint Count => _count;

            int ICollection.Count => (int)Count;
            int System.Collections.Generic.IReadOnlyCollection<T>.Count => (int)Count;
            int System.Collections.Generic.ICollection<T>.Count => (int)Count;

            public override bool HasItems => Count > 0;

            bool ICollection.IsSynchronized => false;
            object ICollection.SyncRoot => null;

            protected override bool IsSynchronized => false;
            protected override object SyncRoot => null;
            #endregion

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is empty.
            /// </summary>
            public LinkedList() { /* Left empty. */ }

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that contains elements copied from the specified <see cref="IEnumerable"/> and has sufficient capacity to accommodate the number of elements copied.
            /// </summary>
            /// <param name="collection">The <see cref="IEnumerable"/> whose elements are copied to the new <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
            public LinkedList(in System.Collections.Generic.IEnumerable<T> collection)
            {
                foreach (T item in collection)

                    _ = AddLast(item);
            }
            #endregion

            #region Methods
            #region Weld
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

            private static void Weld(in LinkedListNode previous, in LinkedListNode next)
            {
                previous.Next = next;
                next.Previous = previous;
            }
            #endregion Weld

            #region Add
            #region Add Single Item
            protected virtual void OnNodeAdded(in LinkedListNode node)
            {
                node.List = this;

                _count++;
            }

            protected virtual void OnNewItemAdded(in LinkedListNode node)
            {
                OnNodeAdded(node);

                IncrementEnumerableVersion();
            }

            #region AddNewItem
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
            #endregion AddNewItem



            #region AddAfter
            private void _AddAfter(in LinkedListNode addAfter, in LinkedListNode node) => Weld(addAfter, node, addAfter.Next);

            public void AddAfter(LinkedListNode addAfter, LinkedListNode node) => AddNewItem(addAfter, node, nameof(addAfter), nameof(node), _AddAfter);

            public LinkedListNode AddAfter(LinkedListNode node, T value) => AddNewItem(node, () => new LinkedListNode(value), nameof(node), _AddAfter);

            ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => AddAfter(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {GetName<LinkedListNode>()}."), value);
            #endregion AddAfter

            #region AddBefore
            private void _AddBefore(in LinkedListNode addBefore, in LinkedListNode node) => Weld(addBefore.Previous, node, addBefore);

            public void AddBefore(LinkedListNode addBefore, LinkedListNode node) => AddNewItem(addBefore, node, nameof(addBefore), nameof(node), _AddBefore);

            public LinkedListNode AddBefore(LinkedListNode node, T value) => AddNewItem(node, () => new LinkedListNode(value), nameof(node), _AddBefore);

            ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value) => AddBefore(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {GetName<LinkedListNode>()}."), value);
            #endregion AddBefore

            private void AddFirstOrLast(in LinkedListNode node, in Action action)
            {
                action();

                OnNewItemAdded(node);
            }

            #region AddFirst
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

            protected override LinkedListNode AddFirstItem(T value)
            {
                var node = new LinkedListNode(value);

                _AddFirst(node);

                return node;
            }

            public LinkedListNode AddFirst(in T value) => AddFirstItem(value);

            public void AddFirst(in LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddFirst(node);
            }

            ILinkedListNode<T> ILinkedList<T>.AddFirst(T value) => AddFirst(value);
            #endregion AddFirst

            #region AddLast
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

            protected override LinkedListNode AddLastItem(T value)
            {
                var node = new LinkedListNode(value);

                _AddLast(node);

                return node;
            }

            public LinkedListNode AddLast(in T value) => AddLastItem(value);

            public void AddLast(in LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddLast(node);
            }

            ILinkedListNode<T> ILinkedList<T>.AddLast(T value) => AddLast(value);
            #endregion AddLast



            void System.Collections.Generic.ICollection<T>.Add(T item) => AddLast(item);
            void ICollectionBase<T>.Add(T item) => AddLast(item);
            #endregion Add Single Item



            #region AddRange
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
            #endregion AddRange



            void IAppendableExtensibleEnumerable<T>.Append(T item) => AddLast(item);
            void IAppendableExtensibleEnumerable<T>.AppendRange(System.Collections.Generic.IEnumerable<T> items) => AddRangeLast(items);



            void IPrependableExtensibleEnumerable<T>.Prepend(T item) => AddFirst(item);
            void IPrependableExtensibleEnumerable<T>.PrependRange(System.Collections.Generic.IEnumerable<T> items) => AddRangeFirst(items);
            #endregion Add

            #region Find
            private LinkedListNode Find(T value, EnumerationDirection enumerationDirection) => new Enumerable<ILinkedListNode<T>>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefault(value == null ?
#if !CS9
                (Func<ILinkedListNode<T>, bool>)
#endif
                (node => node.Value == null) : (node => value.Equals(node.Value))) as LinkedListNode;

            public LinkedListNode Find(T value) => Find(value, EnumerationDirection.FIFO);
            ILinkedListNode<T> IReadOnlyLinkedListBase<T, ILinkedListNode<T>>.Find(T value) => Find(value);

            public LinkedListNode FindLast(T value) => Find(value, EnumerationDirection.LIFO);
            ILinkedListNode<T> IReadOnlyLinkedListBase<T, ILinkedListNode<T>>.FindLast(T value) => FindLast(value);
            #endregion Find

            #region Remove
            protected virtual void OnItemRemoved(in LinkedListNode node)
            {
                node.OnRemove();

                _count--;

                IncrementEnumerableVersion();

                if (_count == 0)

                    ReInitNodes();
            }

            public void Remove(LinkedListNode node)
            {
                ThrowIfNotContainedNode(node, nameof(node));

                _Remove(node);
            }

            private void _Remove(LinkedListNode node)
            {
                if (node == First)

                    _ = RemoveFirst();

                else if (node == Last)

                    _ = RemoveLast();

                else
                {
                    Weld(node.Previous, node.Next);

                    OnItemRemoved(node);
                }
            }

            void ILinkedList<T>.Remove(ILinkedListNode<T> node) => Remove(node as LinkedListNode ?? throw new ArgumentException($"{nameof(node)} must be an instance of {GetName<LinkedListNode>()}."));

            public LinkedListNode
#if CS8
                ?
#endif
                Remove2(T item)
            {
                LinkedListNode node = Find(item);

                if (node == null)

                    return null;

                Remove(node);

                return node;
            }

            public bool Remove(T item) => Remove2(item) != null;

            ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove2(item);

            protected override T RemoveFirstItem()
            {
                ThrowIfEmptyListOrCollection(this);

                LinkedListNode node = First;

                First = node.Next;

                OnItemRemoved(node);

                return node.Value;
            }

            public T RemoveFirst() => RemoveFirstItem();
            public T RemoveLast()
            {
                ThrowIfEmptyListOrCollection(this);

                LinkedListNode node = Last;

                Last = node.Previous;

                OnItemRemoved(node);

                return node.Value;
            }

            void ILinkedList<T>.RemoveFirst() => RemoveFirstItem();
            void ILinkedList<T>.RemoveLast() => RemoveLast();
            #endregion Remove

            #region Misc
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

            private void ReInitNodes()
            {
                First = null;
                Last = null;
            }

            protected override void ClearItems()
            {
                if (_enumeratorsCount > 0)

                    _enumerableVersion++;

                while (Count != 0)

                    _ = RemoveFirst();

                ReInitNodes();
            }

            public void Clear() => ClearItems();

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

                foreach (T item in this.AsFromType<IEnumerable<T>>())

                    array[arrayIndex++] = item;
            }
            public void CopyTo(Array array, int index) => EnumerableExtensions.CopyTo(this, array, index, Count);

            protected bool OnNodeCoupleAction(in ILinkedListNode<T> x, in string xArgumentName, in ILinkedListNode<T> y, in string yArgumentName, in Func<LinkedListNode, LinkedListNode, bool> func) => x is LinkedListNode _x
                    ? y is LinkedListNode _y ? func(_x, _y) : throw (y == null ? GetArgumentNullException(xArgumentName) : GetNotContainedLinkedListNodeException(nameof(y)))
                    : throw (x == null ? GetArgumentNullException(yArgumentName) : GetNotContainedLinkedListNodeException(nameof(x)));

            public IQueue<T> AsQueue() => new Abstraction.Generic.Queue<T, ILinkedList<T>>(this);
            public IStack<T> AsStack() => new Abstraction.Generic.Stack<T, ILinkedList<T>>(this);
            #endregion Misc

            #region Move
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
            #endregion Move

            #region Swap
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
            #endregion Swap

            #region Enumeration
            private void IncrementEnumeratorsCount() => _enumeratorsCount++;
            private void DecrementEnumeratorsCount()
            {
                _enumeratorsCount--;

                if (_enumeratorsCount == 0)

                    _enumerableVersion = 0;
            }

            public IUIntCountableEnumerable<ILinkedListNode<T>> AsNodeEnumerable() => new UIntCountableEnumerable<ILinkedListNode<T>, IUIntCountableEnumerator<ILinkedListNode<T>>>(GetNodeEnumerator());

            protected LinkedListEnumerator<T> GetLinkedListEnumerator(in EnumerationDirection enumerationDirection) => new
#if !CS9
                LinkedListEnumerator<T>
#endif
                (this, enumerationDirection, null, null);

            public System.Collections.Generic.IEnumerator<T> GetInnerEnumerator(in EnumerationDirection enumerationDirection) => GetLinkedListEnumerator(enumerationDirection).SelectConverter(node => node.Value);

            public IUIntCountableEnumeratorInfo<T> GetEnumerator(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(new EnumeratorInfo<T>(GetInnerEnumerator(enumerationDirection)), () => Count);

            public IUIntCountableEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);
            public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo(GetLinkedListEnumerator(enumerationDirection), this);

            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);
            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetReversedNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

            IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetEnumerator() => GetNodeEnumerator();
            IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Extensions.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();
#if !CS8
            IUIntCountableEnumerator<T> Enumeration.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();
            IEnumerator<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator();
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            IEnumerator<ILinkedListNode<T>> Extensions.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetReversedNodeEnumerator();
            IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();
            System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
            #endregion Enumeration

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
            #endregion Methods

            ~LinkedList() => Clear();
        }
    }
}
#endif
