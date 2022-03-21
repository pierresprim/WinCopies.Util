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

#if WinCopies3 && CS7
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;

using static WinCopies.ThrowHelper;
using static WinCopies.Collections.ThrowHelper;
using static WinCopies.Collections.Resources.ExceptionMessages;

namespace WinCopies.Collections.Generic
{
    public interface ILinkedTreeNode<T> : ILinkedListNode<T>, ILinkedList3<T>, Collections.Generic.IEnumerable<ILinkedTreeNode<T>>, Collections.Generic.IEnumerableInfo<ILinkedTreeNode<T>>
    {
        new ILinkedTreeNode<T> Previous { get; }

        new ILinkedTreeNode<T> Next { get; }

        ILinkedTreeNode<T> Parent { get; }

        new ILinkedTreeNode<T> First { get; }

        new ILinkedTreeNode<T> Last { get; }

        new ILinkedTreeNode<T> Find(T value);

        new ILinkedTreeNode<T> FindLast(T value);

        new IUIntCountableEnumerator<ILinkedTreeNode<T>> GetNodeEnumerator();

        new IUIntCountableEnumerator<ILinkedTreeNode<T>> GetReversedNodeEnumerator();



        new ILinkedTreeNode<T> AddAfter(ILinkedTreeNode<T> node, T value);

        new ILinkedTreeNode<T> AddBefore(ILinkedTreeNode<T> node, T value);

        new ILinkedTreeNode<T> AddFirst(T value);

        new ILinkedTreeNode<T> AddLast(T value);



        new void Remove(ILinkedTreeNode<T> node);

        new ILinkedTreeNode<T> Remove(T item);

        bool MoveAfter(ILinkedTreeNode<T> node, ILinkedTreeNode<T> after);

        bool MoveBefore(ILinkedTreeNode<T> node, ILinkedTreeNode<T> before);

        void Swap(ILinkedTreeNode<T> x, ILinkedTreeNode<T> y);

#if CS8
        ILinkedList<T> ILinkedListNode<T>.List => Parent;
#endif
    }

    public interface IEnumerableInfoLinkedTreeNode<T> : ILinkedTreeNode<T>, IEnumerableInfoLinkedList<T>, Collections.Generic.IEnumerableInfo<ILinkedTreeNode<T>>
    {
        new IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>> GetNodeEnumerator();

        new IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>> GetReversedNodeEnumerator();

#if CS8
        IUIntCountableEnumerator<ILinkedTreeNode<T>> ILinkedTreeNode<T>.GetNodeEnumerator() => GetNodeEnumerator();

        IUIntCountableEnumerator<ILinkedTreeNode<T>> ILinkedTreeNode<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();
#endif
    }

    public class LinkedTreeNode<T> : IEnumerableInfoLinkedTreeNode<T>
    {
        public class Enumerator : Enumerator<ILinkedListNode<LinkedTreeNode<T>>, IEnumeratorInfo<ILinkedListNode<LinkedTreeNode<T>>>, LinkedTreeNode<T>>, IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> // LinkedTreeNode<T> does not make checks during enumeration because these checks are performed in the inner LinkedList<LinkedTreeNode<T>>'s enumerator.
        {
            private readonly ILinkedTreeNode<T> _treeNode;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override LinkedTreeNode<T> CurrentOverride => InnerEnumerator.Current.Value;

            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public uint Count => _treeNode.Count;

            public Enumerator(in LinkedTreeNode<T> treeNode, in EnumerationDirection enumerationDirection) : base(treeNode._list.GetNodeEnumerator(enumerationDirection)) => _treeNode = treeNode;

            protected override bool MoveNextOverride() => InnerEnumerator.MoveNext();
        }

        private DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode _node;
        private readonly DotNetFix.Generic.LinkedList<LinkedTreeNode<T>> _list = new
#if !CS9
            DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>
#endif
            ();

        public bool IsReadOnly => false;

        public LinkedTreeNode<T> Previous => _node?.Previous?.Value;

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Previous => Previous;

        public LinkedTreeNode<T> Next => _node?.Next?.Value;

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Next => Next;

        public T Value { get; set; }

        public LinkedTreeNode<T> Parent { get; private set; }

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Parent => Parent;

        public LinkedTreeNode() { /* Left empty. */ }

        public LinkedTreeNode(T value) => Value = value;

        public LinkedTreeNode<T> Last => _list.Last?.Value;

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => Last;

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Last => Last;

        public T LastValue => (Last ?? throw GetEmptyListOrCollectionException()).Value;

        public LinkedTreeNode<T> First => _list.First?.Value;

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => First;

        ILinkedTreeNode<T> ILinkedTreeNode<T>.First => First;

        public T FirstValue => (First ?? throw GetEmptyListOrCollectionException()).Value;

        public uint Count => _list.Count;

        public bool SupportsReversedEnumeration => _list.SupportsReversedEnumeration;

        int System.Collections.Generic.ICollection<T>.Count => _list.AsOfType<System.Collections.Generic.ICollection<LinkedTreeNode<T>>>().Count;

        int ICollection.Count => ((ICollection)_list).Count;

        bool ICollection.IsSynchronized => ((ICollection)_list).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_list).SyncRoot;

        int System.Collections.Generic.IReadOnlyCollection<T>.Count => ((IReadOnlyCollection<T>)_list).Count;

        ILinkedListNode<T> ILinkedListNode<T>.Previous => Previous;

        ILinkedListNode<T> ILinkedListNode<T>.Next => Next;

        IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

        IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;

        ILinkedListNode<T> ILinkedList<T>.First => First;

        ILinkedListNode<T> ILinkedList<T>.Last => Last;

#if !CS8
        ILinkedList<T> ILinkedListNode<T>.List => Parent;

        ILinkedListNode<T, ILinkedList<T>> IReadOnlyLinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Previous => Previous;

        ILinkedListNode<T, ILinkedList<T>> IReadOnlyLinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Next => Next;

        ILinkedList<T> IReadOnlyLinkedListNode<T, ILinkedList<T>>.List => Parent;

        IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Previous => Previous;

        IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Next => Next;

        IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>.List => Parent;

        object IReadOnlyLinkedListNode.Value => Value;
#endif

        private static void ThrowIfNodeAlreadyHasList(in LinkedTreeNode<T> node, in string argumentName)
        {
            if ((node ?? throw GetArgumentNullException(argumentName)).Parent != null)

                throw new ArgumentException(NodeIsAlreadyContainedInAnotherListOrTree, argumentName);
        }

        private void ThrowIfNotContainedNode(in LinkedTreeNode<T> node, in string argumentName) => _ThrowIfNotContainedNode(node ?? throw GetArgumentNullException(argumentName), argumentName);

        private void _ThrowIfNotContainedNode(in LinkedTreeNode<T> node, in string argumentName) => ThrowHelper.ThrowIfNotContainedNode(node, argumentName, this);

        private void Add(in LinkedTreeNode<T> node, in Action<DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode> action, in string argumentName)
        {
            ThrowIfNodeAlreadyHasList(node, argumentName);

            node.Parent = this;

            action((node._node = new DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode(node)));
        }

        private void Remove(in LinkedTreeNode<T> node, in Action<DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode> action, in string argumentName)
        {
            ThrowIfNotContainedNode(node, argumentName);

            node.Parent = null;

            action(node._node);

            node._node = null;
        }



        private static ArgumentException GetNodeIsNotLinkedTreeNodeException(in string argumentName) => GetArgumentMustBeAnInstanceOfTypeException(argumentName, nameof(LinkedTreeNode<T>));

        private bool _MoveAfter(LinkedTreeNode<T> node, LinkedTreeNode<T> after)
        {
            _ThrowIfNotContainedNode(node, nameof(node));
            ThrowIfNodesAreEqual(node, after);
            _ThrowIfNotContainedNode(after, nameof(after));

            return _list.MoveAfter(node._node, after._node);
        }

        private bool __MoveAfter(in ILinkedListNode<T> node, ILinkedListNode<T> after) => _MoveAfter(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)), after is LinkedTreeNode<T> _after ? _after : throw GetNodeIsNotLinkedTreeNodeException(nameof(after)));

        bool ILinkedList3<T>.MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => __MoveAfter(node, after);

        bool ILinkedTreeNode<T>.MoveAfter(ILinkedTreeNode<T> node, ILinkedTreeNode<T> after) => __MoveAfter(node, after);

        public bool MoveAfter(in LinkedTreeNode<T> node, in LinkedTreeNode<T> after) => _MoveAfter(node ?? throw GetArgumentNullException(nameof(node)), after ?? throw GetArgumentNullException(nameof(after)));

        private bool _MoveBefore(in LinkedTreeNode<T> node, in LinkedTreeNode<T> before)
        {
            _ThrowIfNotContainedNode(node, nameof(node));
            ThrowIfNodesAreEqual(node, before);
            _ThrowIfNotContainedNode(node, nameof(before));

            return _list.MoveBefore(node._node, before._node);
        }

        private bool __MoveBefore(in ILinkedListNode<T> node, in ILinkedListNode<T> before) => _MoveBefore(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)), before is LinkedTreeNode<T> _before ? _before : throw GetNodeIsNotLinkedTreeNodeException(nameof(before)));

        bool ILinkedList3<T>.MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => __MoveBefore(node, before);

        bool ILinkedTreeNode<T>.MoveBefore(ILinkedTreeNode<T> node, ILinkedTreeNode<T> before) => __MoveBefore(node, before);

        public bool MoveBefore(in LinkedTreeNode<T> node, in LinkedTreeNode<T> before) => _MoveBefore(node ?? throw GetArgumentNullException(nameof(node)), before ?? throw GetArgumentNullException(nameof(before)));

        private LinkedTreeNode<T> ToLinkedTreeNode(in ILinkedListNode<T> node, in string argumentName) => node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(argumentName);

        void ILinkedList3<T>.Swap(ILinkedListNode<T> x, ILinkedListNode<T> y) => _Swap(ToLinkedTreeNode(x, nameof(x)), ToLinkedTreeNode(y, nameof(y)));

#if WinCopies3
        void ILinkedTreeNode<T>.Swap(ILinkedTreeNode<T> x, ILinkedTreeNode<T> y) => _Swap(ToLinkedTreeNode(x, nameof(x)), ToLinkedTreeNode(y, nameof(y)));

        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddBefore(ILinkedTreeNode<T> node, T value) => AddBefore(ToLinkedTreeNode(node, nameof(node)), value);

        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddAfter(ILinkedTreeNode<T> node, T value) => AddAfter(ToLinkedTreeNode(node, nameof(node)), value);
#endif

        public void Swap(LinkedTreeNode<T> x, LinkedTreeNode<T> y) => _Swap(x ?? throw GetArgumentNullException(nameof(x)), y ?? throw GetArgumentNullException(nameof(y)));

        private void _Swap(LinkedTreeNode<T> x, LinkedTreeNode<T> y)
        {
            _ThrowIfNotContainedNode(x, nameof(x));
            ThrowIfNodesAreEqual(x, y);
            _ThrowIfNotContainedNode(y, nameof(y));

            _list.Swap(x._node, y._node);
        }



        ILinkedListNode<T> ILinkedList<T>.AddAfter(ILinkedListNode<T> node, T value) => AddAfter(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)), value);

        public LinkedTreeNode<T> AddAfter(LinkedTreeNode<T> node, T value)
        {
            ThrowIfNotContainedNode(node, nameof(node));

            var _node = new LinkedTreeNode<T>(value);

            _AddAfter(node, _node);

            return _node;
        }

        public void AddAfter(LinkedTreeNode<T> addAfter, LinkedTreeNode<T> node)
        {
            ThrowIfNotContainedNode(addAfter, nameof(addAfter));

            _AddAfter(addAfter, node);
        }

        private void _AddAfter(LinkedTreeNode<T> addAfter, LinkedTreeNode<T> node)
        {
            ThrowIfNodeAlreadyHasList(node, nameof(node));
            ThrowIfNodesAreEqual(node, addAfter);

            Add(node, _node => _list.AddAfter(addAfter._node, _node), nameof(node));
        }

        public void AddBefore(LinkedTreeNode<T> addBefore, LinkedTreeNode<T> node)
        {
            ThrowIfNotContainedNode(addBefore, nameof(addBefore));

            _AddBefore(addBefore, node);
        }

        private void _AddBefore(LinkedTreeNode<T> addBefore, LinkedTreeNode<T> node)
        {
            ThrowIfNodeAlreadyHasList(node, nameof(node));
            ThrowIfNodesAreEqual(node, addBefore);

            Add(node, _node => _list.AddBefore(addBefore._node, _node), nameof(node));
        }

        public LinkedTreeNode<T> AddBefore(LinkedTreeNode<T> node, T value)
        {
            ThrowIfNotContainedNode(node, nameof(node));

            var _node = new LinkedTreeNode<T>(value);

            _AddBefore(node, _node);

            return _node;
        }

        ILinkedListNode<T> ILinkedList<T>.AddBefore(ILinkedListNode<T> node, T value) => AddBefore(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)), value);

        public LinkedTreeNode<T> AddFirst(T value)
        {
            var node = new LinkedTreeNode<T>(value);

            _AddFirst(node);

            return node;
        }

        ILinkedListNode<T> ILinkedList<T>.AddFirst(T value) => AddFirst(value);

        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddFirst(T value) => AddFirst(value);

        public void AddFirst(LinkedTreeNode<T> node)
        {
            ThrowIfNodeAlreadyHasList(node, nameof(node));

            _AddFirst(node);
        }

        private void _AddFirst(LinkedTreeNode<T> node) => Add(node, _node => _list.AddFirst(_node), nameof(node));

        public LinkedTreeNode<T> AddLast(T value)
        {
            var node = new LinkedTreeNode<T>(value);

            _AddLast(node);

            return node;
        }

        ILinkedListNode<T> ILinkedList<T>.AddLast(T value) => AddLast(value);

        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddLast(T value) => AddLast(value);

        public void AddLast(LinkedTreeNode<T> node)
        {
            ThrowIfNodeAlreadyHasList(node, nameof(node));

            _AddLast(node);
        }

        private void _AddLast(LinkedTreeNode<T> node) => Add(node, _node => _list.AddLast(_node), nameof(node));

        void System.Collections.Generic.ICollection<T>.Add(T item) => _list.AsOfType<System.Collections.Generic.ICollection<LinkedTreeNode<T>>>().Add(new LinkedTreeNode<T>(item));



        public LinkedTreeNode<T> Find(in T value) => Find(value, EnumerationDirection.FIFO);

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => Find(value);

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Find(T value) => Find(value);

        ILinkedListNode<T> ILinkedList<T>.Find(T value) => Find(value);

        public LinkedTreeNode<T> FindLast(in T value) => Find(value, EnumerationDirection.LIFO);

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => FindLast(value);

        ILinkedTreeNode<T> ILinkedTreeNode<T>.FindLast(T value) => FindLast(value);

        ILinkedListNode<T> ILinkedList<T>.FindLast(T value) => FindLast(value);

        private LinkedTreeNode<T> Find(T value, EnumerationDirection enumerationDirection)
        {
            Func<LinkedTreeNode<T>, bool> predicate;

            if (value == null)

                predicate = node => node.Value == null;

            else

                predicate = node => value.Equals(node.Value);

            return new Enumerable<LinkedTreeNode<T>>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefault(predicate);
        }



        public void Remove(in LinkedTreeNode<T> node) => Remove(node, _node => _list.Remove(_node), nameof(node));

        private void _Remove(in ILinkedListNode<T> node) => Remove(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)));

        void ILinkedList<T>.Remove(ILinkedListNode<T> node) => _Remove(node);

        void ILinkedTreeNode<T>.Remove(ILinkedTreeNode<T> node) => _Remove(node);

        public LinkedTreeNode<T> Remove(in T item)
        {
            LinkedTreeNode<T> node = Find(item);

            if (node == null)

                return null;

            Remove(node);

            return node;
        }

        ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove(item);

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Remove(T item) => Remove(item);

        bool System.Collections.Generic.ICollection<T>.Remove(T item) => Remove(item) != null;

        public void RemoveFirst()
        {
            ThrowIfEmptyListOrCollection(this);

            Remove(First, node => _list.Remove(node), null);
        }

        public void RemoveLast()
        {
            ThrowIfEmptyListOrCollection(this);

            Remove(Last, node => _list.Remove(node), null);
        }


        public void Clear()
        {
            while (Count > 0)

                RemoveFirst();
        }

        public bool Contains(T item)
        {
            Predicate<T> predicate;

            if (item == null)

                predicate = _item => _item == null;

            else

                predicate = _item => item.Equals(_item);

            foreach (LinkedTreeNode<T> node in _list)

                if (predicate(node.Value))

                    return true;

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, Count);

        public void CopyTo(Array array, int index) => this.CopyTo(array, index, Count);



        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> WinCopies.Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

        public IUIntCountableEnumeratorInfo<T> GetEnumerator(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value), () => Count);

        public IUIntCountableEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

        public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

        IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>> IEnumerableInfoLinkedTreeNode<T>.GetNodeEnumerator() => GetNodeEnumerator();

        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetReversedNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>> IEnumerableInfoLinkedTreeNode<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> IEnumerableInfoLinkedList<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

        IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator();

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> IEnumerableInfoLinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator();

        IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

        IEnumeratorInfo<ILinkedTreeNode<T>> DotNetFix.Generic.IEnumerable<ILinkedTreeNode<T>, IEnumeratorInfo<ILinkedTreeNode<T>>>.GetEnumerator() => GetNodeEnumerator();

        IEnumeratorInfo<ILinkedTreeNode<T>> IEnumerable<ILinkedTreeNode<T>, IEnumeratorInfo<ILinkedTreeNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

        System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();

        public DotNetFix.Generic.IUIntCountableEnumerable<ILinkedListNode<T>> AsNodeEnumerable() => new Enumeration.Generic.UIntCountableEnumerable<LinkedTreeNode<T>, ILinkedListNode<T>>(this);

        IUIntCountableEnumerator<T> IUIntCountableEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

        void ILinkedList<T>.Add(T item) => AddLast(item);

        bool ILinkedList<T>.Remove(T item) => Remove(item) != null;

#if !CS8
        IUIntCountableEnumerator<ILinkedTreeNode<T>> ILinkedTreeNode<T>.GetNodeEnumerator() => GetNodeEnumerator();

        IUIntCountableEnumerator<ILinkedTreeNode<T>> ILinkedTreeNode<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

        IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator();

        IEnumeratorInfo<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>, IEnumeratorInfo<ILinkedListNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

        IEnumeratorInfo<ILinkedListNode<T>> DotNetFix.Generic.IEnumerable<ILinkedListNode<T>, IEnumeratorInfo<ILinkedListNode<T>>>.GetEnumerator() => GetNodeEnumerator();

        IUIntCountableEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => GetReversedEnumerator();

        IUIntCountableEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator() => GetEnumerator();

        IUIntCountableEnumerator<T> IReadOnlyLinkedList2<T>.GetReversedEnumerator() => GetReversedEnumerator();

        IEnumeratorInfo<T> IEnumerable<T, IEnumeratorInfo<T>>.GetReversedEnumerator() => GetReversedEnumerator();

        IEnumeratorInfo<T> DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo<T>>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedTreeNode<T>> System.Collections.Generic.IEnumerable<ILinkedTreeNode<T>>.GetEnumerator() => GetNodeEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedTreeNode<T>> IEnumerable<ILinkedTreeNode<T>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

        void ICollectionBase<T>.Add(T item) => AddLast(item);

        bool ICollectionBase<T>.Remove(T item) => Remove(item) != null;

        IUIntCountableEnumerator<T> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

        IUIntCountableEnumerator<T> DotNetFix.Generic.IEnumerable<T, IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();
#endif
    }
}
#endif
