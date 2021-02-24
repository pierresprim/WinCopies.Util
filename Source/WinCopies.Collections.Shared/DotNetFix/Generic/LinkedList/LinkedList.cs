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
using System.Diagnostics;

#if !WinCopies3
using System.Runtime.Serialization;
#else
using System.Collections.Generic;
using System.Linq;

using WinCopies.Collections.Generic;
using WinCopies.Linq;

using static WinCopies.Collections.ThrowHelper;
using static WinCopies.ThrowHelper;
#endif
#endif

namespace WinCopies.Collections.DotNetFix
{
    public enum EnumerationDirection
    {
        FIFO = 1,

        LIFO = 2
    }

#if WinCopies3
#endif
#if CS7
#if WinCopies3
    namespace Generic
    {
#endif
        public interface ILinkedListNode<T>
        {
            bool IsReadOnly { get; }

#if WinCopies3
            IReadOnlyLinkedList
#else
ILinkedList
#endif
                <T> List
            { get; }

            ILinkedListNode<T> Previous { get; }

            ILinkedListNode<T> Next { get; }

            T Value { get; }
        }

        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public class LinkedList<T> :
#if WinCopies3
            ILinkedList3<T>, ISortable<T>, IEnumerableInfoLinkedList<T>
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
            {
                // Left empty.
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that contains elements copied from the specified <see cref="IEnumerable"/> and has sufficient capacity to accommodate the number of elements copied.
            /// </summary>
            /// <param name="collection">The <see cref="IEnumerable"/> whose elements are copied to the new <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
            public LinkedList(System.Collections.Generic.IEnumerable<T> collection)
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
            public class LinkedListNode : ILinkedListNode<T>
            {
                public bool IsReadOnly => false;

                public LinkedList<T> List { get; internal set; }

                IReadOnlyLinkedList<T> ILinkedListNode<T>.List => List;

                public LinkedListNode Previous { get; internal set; }

                ILinkedListNode<T> ILinkedListNode<T>.Previous => Previous;

                public LinkedListNode Next { get; internal set; }

                ILinkedListNode<T> ILinkedListNode<T>.Next => Next;

                public T Value { get; set; }

                public LinkedListNode(T value) => Value = value;

                internal void OnRemove()
                {
                    List = null;
                    Previous = null;
                    Next = null;
                }
            }

            public class Enumerator : WinCopies.Collections.Generic.Enumerator<LinkedListNode>, IUIntCountableEnumeratorInfo<LinkedListNode>
            {
                private LinkedList<T> _list;
                private Action _action;
                private LinkedListNode _currentNode;
                private Action _reset;
                private bool _first = true;
                private readonly uint _version;

                public EnumerationDirection EnumerationDirection { get; }

                public uint Count => _list.Count;

                public override bool? IsResetSupported => true;

                /// <summary>
                /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
                /// </summary>
                protected override LinkedListNode CurrentOverride => _currentNode;

                public Enumerator(LinkedList<T> list, in EnumerationDirection enumerationDirection)
                {
                    list._enumeratorsCount++;

                    _list = list ?? throw GetArgumentNullException(nameof(list));

                    _version = list.EnumerableVersion;

                    EnumerationDirection = enumerationDirection;

                    switch (enumerationDirection)
                    {
                        case EnumerationDirection.FIFO:

                            _action = () => _currentNode = _currentNode.Next;

                            _reset = () => _currentNode = list.First;

                            break;

                        case EnumerationDirection.LIFO:

                            _action = () => _currentNode = _currentNode.Previous;

                            _reset = () => _currentNode = list.Last;

                            break;
                    }
                }

                protected override void ResetOverride()
                {
                    base.ResetOverride();

                    ThrowIfVersionHasChanged(_list.EnumerableVersion, _version);

                    _first = true;

                    _reset();
                }

                protected override bool MoveNextOverride()
                {
                    ThrowIfVersionHasChanged(_list.EnumerableVersion, _version);

                    if (_first)
                    {
                        _first = false;

                        if (_list.First == null)

                            return false;

                        _reset();

                        return true;
                    }

                    _action();

                    if (_currentNode == null)
                    {
                        ResetCurrent();

                        return false;
                    }

                    // The new node has already been updated in the _action delegate.

                    return true;
                }

                protected override void ResetCurrent() => _currentNode = null;

                protected override void DisposeManaged()
                {
                    base.DisposeManaged();

                    _action = null;

                    ResetCurrent();

                    _reset = null;
                }

                protected override void Dispose(bool disposing)
                {
                    _list.DecrementEnumeratorsCount();

                    _list = null;

                    base.Dispose(disposing);
                }

                ~Enumerator() => Dispose(false);
            }

            #region Fields

            private uint _enumeratorsCount = 0;
            private object _syncRoot;

            #endregion

            private uint EnumerableVersion { get; set; } = 0;

            #region Properties
            public LinkedListNode First { get; private set; }

            ILinkedListNode<T>
#if WinCopies3
                IReadOnlyLinkedList
#else
                ILinkedList
#endif
                <T>.First => First;

            public T FirstValue => First.Value;

            public LinkedListNode Last { get; private set; }

            ILinkedListNode<T>
#if WinCopies3
                IReadOnlyLinkedList
#else
                ILinkedList
#endif
                <T>.Last => Last;

            public T LastValue => Last.Value;

            public uint Count { get; private set; }

            int ICollection.Count => (int)Count;

            int ICollection<T>.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;

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

                    EnumerableVersion = 0;
            }

            private static void ThrowIfNodeAlreadyHasList(in LinkedListNode node, in string argumentName)
            {
                if ((node ?? throw GetArgumentNullException(argumentName)).List != null)

                    throw new ArgumentException("The given node is already contained in another list.");
            }

            private void ThrowIfNotContainedNode(in LinkedListNode node, in string argumentName) => _ThrowIfNotContainedNode(node ?? throw GetArgumentNullException(argumentName), argumentName);

            private void _ThrowIfNotContainedNode(in LinkedListNode node, in string argumentName) => ThrowHelper.ThrowIfNotContainedNode(node, argumentName, this);

            private void IncrementEnumerableVersion()
            {
                if (_enumeratorsCount > 0)

                    EnumerableVersion++;
            }

            private void OnNodeAdded(in LinkedListNode node)
            {
                node.List = this;

                Count++;
            }

            private void OnNewItemAdded(in LinkedListNode node)
            {
                OnNodeAdded(node);

                IncrementEnumerableVersion();
            }

            private void ReInitNodes()
            {
                First = null;

                Last = null;
            }

            private void OnItemRemoved(in LinkedListNode node)
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

            ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => AddAfter(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {nameof(LinkedListNode)}."), value);

            public LinkedListNode AddAfter(LinkedListNode node, T value)
            {
                ThrowIfNotContainedNode(node, nameof(node));

                var newNode = new LinkedListNode(value);

                _AddAfter(node, newNode);

                return newNode;
            }

            public void AddAfter(LinkedListNode addAfter, LinkedListNode node)
            {
                ThrowIfNotContainedNode(addAfter, nameof(addAfter));

                _AddAfter(addAfter, node);
            }

            private void _AddAfter(LinkedListNode addAfter, LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));
                ThrowIfNodesAreEqual(node, addAfter);

                Weld(addAfter, node, addAfter.Next);
            }

            ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value) => AddBefore(node is LinkedListNode _node ? _node : throw new ArgumentException($"{nameof(node)} must be an instance of {nameof(LinkedListNode)}."), value);

            public LinkedListNode AddBefore(LinkedListNode node, T value)
            {
                ThrowIfNotContainedNode(node, nameof(node));

                var newNode = new LinkedListNode(value);

                _AddBefore(node, newNode);

                return newNode;
            }

            public void AddBefore(LinkedListNode addBefore, LinkedListNode node)
            {
                ThrowIfNotContainedNode(addBefore, nameof(addBefore));

                _AddBefore(addBefore, node);
            }

            private void _AddBefore(in LinkedListNode addBefore, in LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));
                ThrowIfNodesAreEqual(node, addBefore);

                Weld(addBefore.Previous, node, addBefore);
            }

            public ILinkedListNode<T> AddFirst(T value)
            {
                var node = new LinkedListNode(value);

                _AddFirst(node);

                return node;
            }

            public void AddFirst(LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddFirst(node);
            }

            private void _AddFirst(LinkedListNode node)
            {
                if (First != null)

                    Weld(node, First);

#if CS8
                Last ??= node;
#else
                if (Last == null)

                    Last = node;
#endif

                First = node;

                OnNewItemAdded(node);
            }

            public ILinkedListNode<T> AddLast(T value)
            {
                var node = new LinkedListNode(value);

                _AddLast(node);

                return node;
            }

            public void AddLast(LinkedListNode node)
            {
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddLast(node);
            }

            private void _AddLast(LinkedListNode node)
            {
                if (Last != null)
                {
                    node.Previous = Last;

                    Last.Next = node;
                }

#if CS8
                First ??= node;
#else
                if (First == null)

                    First = node;
#endif

                Last = node;

                OnNewItemAdded(node);
            }

            void ICollection<T>.Add(T item) => AddLast(item);



            public LinkedListNode Find(T value) => Find(value, EnumerationDirection.FIFO);

            ILinkedListNode<T>
#if WinCopies3
                IReadOnlyLinkedList
#else
                ILinkedList
#endif
                <T>.Find(T value) => Find(value);

            public LinkedListNode FindLast(T value) => Find(value, EnumerationDirection.LIFO);

            ILinkedListNode<T>
#if WinCopies3
                IReadOnlyLinkedList
#else
                ILinkedList
#endif
                <T>.FindLast(T value) => FindLast(value);

            private LinkedListNode Find(T value, EnumerationDirection enumerationDirection)
            {
                Func<LinkedListNode, bool> predicate;

                if (value == null)

                    predicate = node => node.Value == null;

                else

                    predicate = node => value.Equals(node.Value);

                return new Enumerable<LinkedListNode>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefault(predicate);
            }



            public IUIntCountableEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

            public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

            public IUIntCountableEnumeratorInfo<T> GetEnumerator(EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value), () => Count);

            public IUIntCountableEnumeratorInfo<LinkedListNode> GetNodeEnumerator(in EnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

            IUIntCountableEnumeratorInfo<ILinkedListNode<T>> IEnumerableInfoLinkedList<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection);

            IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

#if !WinCopies3
            System.Collections.Generic.IEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator();
#endif

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

            IUIntCountableEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

            IEnumeratorInfo2<T> IEnumerable<T, IEnumeratorInfo2<T>>.GetEnumerator() => GetEnumerator();

            IEnumeratorInfo2<T> Collections.Generic.IEnumerable<T, IEnumeratorInfo2<T>>.GetReversedEnumerator() => GetReversedEnumerator();

            IUIntCountableEnumerator<T> IUIntCountableEnumerable<T>.GetEnumerator() => GetEnumerator();

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList3<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection);

            System.Collections.Generic.IEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator(EnumerationDirection enumerationDirection) => GetEnumerator(enumerationDirection);

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

            void ILinkedList<T>.Remove(ILinkedListNode<T> node) => Remove(node as LinkedListNode ?? throw new ArgumentException($"{nameof(node)} must be an instance of {nameof(LinkedListNode)}."));

            bool ICollection<T>.Remove(T item) => Remove2(item) != null;

            public LinkedListNode Remove2(T item)
            {
                LinkedListNode node = Find(item);

                if (node == null)

                    return null;

                Remove(node);

                return node;
            }

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

                    EnumerableVersion++;

                while (Count != 0)

                    RemoveFirst();

                ReInitNodes();
            }

            public bool Contains(T item)
            {
                Predicate<T> predicate;

                if (item == null)

                    predicate = _item => _item == null;

                else

                    predicate = _item => item.Equals(_item);

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
            #endregion
#endif

#if WinCopies3
            protected bool OnNodeCoupleAction(in ILinkedListNode<T> x, in string xArgumentName, in ILinkedListNode<T> y, in string yArgumentName, in Func<LinkedListNode, LinkedListNode, bool> func)
            {
                if (x is LinkedListNode _x)

                    if (y is LinkedListNode _y)

                        return func(_x, _y);

                    else

                        throw y == null ? GetArgumentNullException(xArgumentName) : GetNotContainedLinkedListNodeException(nameof(y));

                else

                    throw x == null ? GetArgumentNullException(yArgumentName) : GetNotContainedLinkedListNodeException(nameof(x));
            }

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
#endif
        }
#if WinCopies3
    }
#endif
#endif
}
