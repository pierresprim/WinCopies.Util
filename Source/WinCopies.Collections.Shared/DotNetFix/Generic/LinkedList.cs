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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinCopies
#if WinCopies2
    .Util
#else
    .Collections
#endif
    .Resources;
using WinCopies.Linq;

using static WinCopies.Collections.ThrowHelper;

#if WinCopies2
using System.Runtime.Serialization;

using static WinCopies.Util.Util;
#else
using WinCopies.Collections.Generic;
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    public enum LinkedListEnumerationDirection
    {
        FIFO = 1,

        LIFO = 2
    }

    namespace Generic
    {
#endif
        public interface IReadOnlyLinkedList<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>,
#if WinCopies2
            ICountableEnumerable<T>, ISerializable, IDeserializationCallback
#else
            IUIntCountableEnumerable<T>, Collections.Generic.IEnumerable<T>
#endif
        {
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> Last
            { get; }


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> First
            { get; }

            new
#if WinCopies2
int
#else
            uint
#endif
                 Count
            { get; }


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> Find(T value);


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> FindLast(T value);

#if WinCopies2
			new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
#else
            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(LinkedListEnumerationDirection enumerationDirection);
#endif
        }

        public interface ILinkedListNode<T>
        {
            bool IsReadOnly { get; }

            ILinkedList<T> List { get; }

            ILinkedListNode<T> Previous { get; }

            ILinkedListNode<T> Next { get; }

            T Value { get; }
        }

        public interface ILinkedList<T> :
            // TODO:
#if WinCopies2
            ISerializable, IDeserializationCallback, System.Collections.Generic.IEnumerable<T>, 
#else
            IUIntCountableEnumerable<T>, Collections.Generic.IEnumerable<T>,
#endif
            ICollection<T>, ICollection, IReadOnlyCollection<T>
        {
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> Last
            { get; }

#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> First
            { get; }

            new
#if WinCopies2
                int
#else
                uint
#endif
                Count
            { get; }

#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> AddAfter(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node, T value);

#if WinCopies2
            void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
#endif

#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
           <T> AddBefore(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node, T value);

#if WinCopies2
            void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
#endif


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> AddFirst(T value);

#if WinCopies2
            void AddFirst(System.Collections.Generic.LinkedListNode<T> node);
#endif


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> AddLast(T value);

#if WinCopies2
            void AddLast(System.Collections.Generic.LinkedListNode<T> node);
#endif


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> Find(T value);


#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> FindLast(T value);

            //todo: to remove

            new
#if WinCopies2
System.Collections.Generic.LinkedList<T>.Enumerator
#else
              System.Collections.Generic.IEnumerator<T>
#endif
                GetEnumerator();

#if !WinCopies2
            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(LinkedListEnumerationDirection enumerationDirection);
#endif

            void Remove(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node);

            void RemoveFirst();

            void RemoveLast();
        }

        public interface ILinkedList2<T> : ILinkedList<T>
        {
            bool IsReadOnly { get; }
        }

        [DebuggerDisplay("Count = {Count}")]
        public class LinkedList<T> :
#if WinCopies2
            System.Collections.Generic.LinkedList<T>,
#endif
            ILinkedList2<T>
        {
            public bool IsReadOnly => false;

            public bool SupportsReversedEnumeration => true;

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is empty.
            /// </summary>
            public LinkedList()
#if WinCopies2
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
#if WinCopies2
: base(collection)
#endif
            {
#if WinCopies2
                // Left empty.
#else
                foreach (T item in collection)

                    AddLast(item);
#endif
            }

#if WinCopies2
            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is serializable with the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.
            /// </summary>
            /// <param name="info">A <see cref="SerializationInfo"/> object containing the information required to serialize the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <param name="context">A <see cref="StreamingContext"/> object containing the source and destination of the serialized stream associated with the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            protected LinkedList(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                // Left empty.
            }
#else
            public class LinkedListNode : ILinkedListNode<T>
            {
                public bool IsReadOnly => false;

                public LinkedList<T> List { get; internal set; }

                ILinkedList<T> ILinkedListNode<T>.List => List;

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

            public class Enumerator : WinCopies.Collections.Generic.Enumerator<LinkedListNode>
            {
                private LinkedList<T> _list;
                private Action _action;
                private LinkedListNode _currentNode;
                private Action _reset;
                private bool _first = true;
                private readonly uint _version;

                public LinkedListEnumerationDirection EnumerationDirection { get; }

                public override bool? IsResetSupported => true;

                protected override LinkedListNode CurrentOverride => _currentNode;

                public Enumerator(LinkedList<T> list, in LinkedListEnumerationDirection enumerationDirection)
                {
                    list._enumeratorsCount++;

                    _list = list ?? throw GetArgumentNullException(nameof(list));

                    _version = list.EnumerableVersion;

                    EnumerationDirection = enumerationDirection;

                    switch (enumerationDirection)
                    {
                        case LinkedListEnumerationDirection.FIFO:

                            _action = () => _currentNode = _currentNode.Next;

                            _reset = () => _currentNode = list.First;

                            break;

                        case LinkedListEnumerationDirection.LIFO:

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

            private uint _enumeratorsCount = 0;
            private object _syncRoot;

            private uint EnumerableVersion { get; set; } = 0;

            private void IncrementEnumeratorsCount() => _enumeratorsCount++;

            private void DecrementEnumeratorsCount()
            {
                _enumeratorsCount--;

                if (_enumeratorsCount == 0)

                    EnumerableVersion = 0;
            }

            private void ThrowIfNodeAlreadyHasList(in LinkedListNode node, in string argumentName)
            {
                if ((node ?? throw GetArgumentNullException(argumentName)).List != null)

                    throw new ArgumentException("The given node is already contained in another list.");
            }

            private void ThrowIfNotContainedNode(in LinkedListNode node, in string argumentName) => _ThrowIfNotContainedNode(node ?? throw GetArgumentNullException(argumentName));

            private void _ThrowIfNotContainedNode(in LinkedListNode node)
            {
                if (node.List != this)

                    throw new ArgumentException("The given node is not contained in the current list.");
            }

            public LinkedListNode First { get; private set; }

            ILinkedListNode<T> ILinkedList<T>.First => First;

            public LinkedListNode Last { get; private set; }

            ILinkedListNode<T> ILinkedList<T>.Last => Last;

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

            private void OnWeld()
            {
                if (_enumeratorsCount > 0)

                    EnumerableVersion++;
            }

            private void OnNewItemAdded(in LinkedListNode node)
            {
                node.List = this;

                Count++;

                OnWeld();
            }

            private void OnItemRemoved(in LinkedListNode node)
            {
                node.OnRemove();

                Count--;

                OnWeld();
            }

            private void Weld(in LinkedListNode previous, in LinkedListNode newNode, in LinkedListNode next)
            {
                if (previous != null)
                {
                    previous.Next = newNode;

                    newNode.Previous = previous;
                }

                if (next != null)
                {
                    newNode.Next = next;

                    next.Previous = newNode;
                }

                OnNewItemAdded(newNode);
            }

            private void Weld(in LinkedListNode previous, in LinkedListNode next)
            {
                previous.Next = next;

                next.Previous = previous;
            }

            ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value)
            {
                if (node is LinkedListNode _node)
                {
                    _ThrowIfNotContainedNode(_node);

                    var newNode = new LinkedListNode(value);

                    _AddAfter(_node, newNode);

                    return newNode;
                }

                else

                    throw new ArgumentException($"{nameof(node)} must be an instance of {nameof(LinkedListNode)}.");
            }

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
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddAfter(addAfter, node);
            }

            private void _AddAfter(LinkedListNode addAfter, LinkedListNode node) => Weld(addAfter, node, node.Next);

            ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value)
            {
                if (node is LinkedListNode _node)
                {
                    _ThrowIfNotContainedNode(_node);

                    var newNode = new LinkedListNode(value);

                    _AddBefore(_node, newNode);

                    return newNode;
                }

                else

                    throw new ArgumentException($"{nameof(node)} must be an instance of {nameof(LinkedListNode)}.");
            }

            public LinkedListNode AddBefore(LinkedListNode node, T value)
            {
                var newNode = new LinkedListNode(value);

                _AddBefore(node, newNode);

                return newNode;
            }

            public void AddBefore(LinkedListNode addBefore, LinkedListNode node)
            {
                ThrowIfNotContainedNode(addBefore, nameof(addBefore));
                ThrowIfNodeAlreadyHasList(node, nameof(node));

                _AddBefore(addBefore, node);
            }

            private void _AddBefore(LinkedListNode addBefore, LinkedListNode node) => Weld(addBefore.Previous, node, addBefore);

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

#if CS7
                if (Last == null)

                    Last = node;
#else
                Last ??= node;
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

#if CS7
                if (First == null)

                    First = node;
#else
                First ??= node;
#endif

                Last = node;

                OnNewItemAdded(node);
            }

            void ICollection<T>.Add(T item) => AddLast(item);



            public ILinkedListNode<T> Find(T value) => Find(value, LinkedListEnumerationDirection.FIFO);

            public ILinkedListNode<T> FindLast(T value) => Find(value, LinkedListEnumerationDirection.LIFO);

            private ILinkedListNode<T> Find(T value, LinkedListEnumerationDirection enumerationDirection) => new Enumerable<LinkedListNode>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefault(node => (node.Value == null && value == null) || node.Value.Equals(value));



            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => GetEnumerator(LinkedListEnumerationDirection.FIFO);

            public System.Collections.Generic.IEnumerator<T> GetReversedEnumerator() => GetEnumerator(LinkedListEnumerationDirection.LIFO);

            public System.Collections.Generic.IEnumerator<T> GetEnumerator(LinkedListEnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection).Select(node => node.Value);

            public System.Collections.Generic.IEnumerator<LinkedListNode> GetNodeEnumerator(in LinkedListEnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator(LinkedListEnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection);

            public void Remove(LinkedListNode node)
            {
                ThrowIfNotContainedNode(node, nameof(node));

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

            public bool Remove(T item)
            {
                ILinkedListNode<T> node = Find(item);

                if (node is LinkedListNode _node)
                {
                    Remove(_node);

                    return true;
                }

                return false;
            }

            public void RemoveFirst()
            {
#if WinCopies2
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
#if WinCopies2
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
                Count = 0;

                if (_enumeratorsCount > 0)

                    EnumerableVersion++;
            }

            public bool Contains(T item)
            {
                LinkedListNode node = First;

                while (node != null)
                {
                    if ((node.Value == null && item == null) || node.Value.Equals(item))

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

            public void CopyTo(Array array, int index) => Extensions.CopyTo(this, array, index, Count);

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
        }

        [DebuggerDisplay("Count = {Count}")]
        public class ReadOnlyLinkedList<T> : IReadOnlyLinkedList<T>, ILinkedList2<T>
        {
            public bool IsReadOnly => true;

            protected ILinkedList<T> InnerList { get; } // Already was ILinkedList<T> in WinCopies 2.

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Last => InnerList.Last;

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> First => InnerList.First;

            public
#if WinCopies2
                int
#else
                uint
#endif
                Count => InnerList.Count;

#if !WinCopies2
            int ICollection.Count => (int)Count;

            int ICollection<T>.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            bool ICollection<T>.IsReadOnly => true;

            object ICollection.SyncRoot => InnerList.SyncRoot;

            bool ICollection.IsSynchronized => InnerList.IsSynchronized;

            public bool SupportsReversedEnumeration => true;



            public bool Contains(T value) => InnerList.Contains(value);

            public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Find(T value) => InnerList.Find(value);

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> FindLast(T value) => InnerList.FindLast(value);

#if WinCopies2
            public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();
#else
            public
#endif

            System.Collections.Generic.IEnumerator<T>
#if WinCopies2
System.Collections.Generic.IEnumerable<T>.
#endif
                GetEnumerator() => InnerList.GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() =>
#if WinCopies2
InnerList
#else
                ((IEnumerable)InnerList)
#endif
                .GetEnumerator();

#if WinCopies2
            public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

            public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
#else
            public System.Collections.Generic.IEnumerator<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

            public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(LinkedListEnumerationDirection enumerationDirection) => InnerList.GetNodeEnumerator(enumerationDirection);
#endif

            void ICollection<T>.Add(T item) => throw GetReadOnlyListOrCollectionException();

            void ICollection<T>.Clear() => throw GetReadOnlyListOrCollectionException();

            bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => throw GetReadOnlyListOrCollectionException
                ();

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddAfter(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => throw GetReadOnlyListOrCollectionException();

#if WinCopies2
            public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);
#endif

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddBefore(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => throw GetReadOnlyListOrCollectionException();

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirst(T value) => throw GetReadOnlyListOrCollectionException();

            public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLast(T value) => throw GetReadOnlyListOrCollectionException();

            public void Remove(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node) => throw GetReadOnlyListOrCollectionException();

            public void RemoveFirst() => throw GetReadOnlyListOrCollectionException();

            public void RemoveLast() => throw GetReadOnlyListOrCollectionException();
        }
#if !WinCopies2
    }
#endif
}
