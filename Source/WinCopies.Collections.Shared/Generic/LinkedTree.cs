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

namespace WinCopies.Collections.Generic
{
    public interface ILinkedTreeNode<T> : ILinkedListNode<T>, ILinkedList3<T>, IEnumerableInfoLinkedList<T>
    {
        new ILinkedTreeNode<T> Previous { get; }

        new ILinkedTreeNode<T> Next { get; }

        ILinkedTreeNode<T> Parent { get; }
    }

    public class LinkedTreeNode<T> : ILinkedTreeNode<T>, IEnumerableInfo<T>
    {
        public class Enumerator : Enumerator<DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode, IUIntCountableEnumeratorInfo<DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode>, LinkedTreeNode<T>, IUIntCountableEnumeratorInfo<LinkedTreeNode<T>>>, IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> // LinkedTreeNode<T> does not make checks during enumeration because these checks are performed in the inner LinkedList<LinkedTreeNode<T>>'s enumerator.
        {
            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override LinkedTreeNode<T> CurrentOverride => InnerEnumerator.Current.Value;

            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public uint Count => InnerEnumerator.Count;

            public Enumerator(in LinkedTreeNode<T> treeNode, in EnumerationDirection enumerationDirection) : base(treeNode._list.GetNodeEnumerator(enumerationDirection)) { /* Left empty. */ }

            protected override bool MoveNextOverride() => InnerEnumerator.MoveNext();
        }

        private DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode _node;
        private DotNetFix.Generic.LinkedList<LinkedTreeNode<T>> _list = new DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>();

        public bool IsReadOnly => false;

        IReadOnlyLinkedList<T> ILinkedListNode<T>.List => Parent;

        public LinkedTreeNode<T> Previous => _node?.Previous?.Value;

        ILinkedListNode<T> ILinkedListNode<T>.Previous => Previous;

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Previous => Previous;

        public LinkedTreeNode<T> Next => _node?.Next?.Value;

        ILinkedListNode<T> ILinkedListNode<T>.Next => Next;

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Next => Next;

        public T Value { get; set; }

        public LinkedTreeNode<T> Parent { get; private set; }

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Parent => Parent;

        public LinkedTreeNode() { /* Left empty. */ }

        public LinkedTreeNode(T value) => Value = value;

        public LinkedTreeNode<T> Last => _list.Last?.Value;

        ILinkedListNode<T> IReadOnlyLinkedList<T>.Last => Last;

        public T LastValue => Last.Value;

        public LinkedTreeNode<T> First => _list.First?.Value;

        ILinkedListNode<T> IReadOnlyLinkedList<T>.First => First;

        public T FirstValue => First.Value;

        public uint Count => _list.Count;

        public bool SupportsReversedEnumeration => _list.SupportsReversedEnumeration;

        int ICollection<T>.Count => ((ICollection<LinkedTreeNode<T>>)_list).Count;

        int ICollection.Count => ((ICollection)_list).Count;

        bool ICollection.IsSynchronized => ((ICollection)_list).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_list).SyncRoot;

        int IReadOnlyCollection<T>.Count => ((IReadOnlyCollection<T>)_list).Count;



        private static void ThrowIfNodeAlreadyHasList(in LinkedTreeNode<T> node, in string argumentName)
        {
            if ((node ?? throw GetArgumentNullException(argumentName)).Parent != null)

                throw new ArgumentException("The given node is already contained in another tree.");
        }

        private void ThrowIfNotContainedNode(in LinkedTreeNode<T> node, in string argumentName) => _ThrowIfNotContainedNode(node ?? throw GetArgumentNullException(argumentName), argumentName);

        private void _ThrowIfNotContainedNode(in LinkedTreeNode<T> node, in string argumentName) => ThrowHelper.ThrowIfNotContainedNode(node, argumentName, this);

        private void Add(LinkedTreeNode<T> node, Action<WinCopies.Collections.DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode> action, in string argumentName)
        {
            ThrowIfNodeAlreadyHasList(node, argumentName);

            node.Parent = this;

            action((node._node = new DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode(node)));
        }

        private void Remove(LinkedTreeNode<T> node, Action<DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode> action, in string argumentName)
        {
            ThrowIfNotContainedNode(node, argumentName);

            node.Parent = null;

            action(node._node);

            node._node = null;
        }



        public IUIntCountableEnumeratorInfo<T> GetEnumerator(EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value), () => Count);

        public IUIntCountableEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);

        public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> WinCopies.Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

        IEnumeratorInfo2<T> IEnumerableInfo<T>.GetEnumerator() => GetEnumerator();

        IEnumeratorInfo2<T> IEnumerableInfo<T>.GetReversedEnumerator() => GetReversedEnumerator();

        IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator();

        IUIntCountableEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => GetReversedEnumerator();

        IUIntCountableEnumerator<T> IUIntCountableEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator(EnumerationDirection enumerationDirection) => GetEnumerator(enumerationDirection);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList3<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection);

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> IEnumerableInfoLinkedList<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection);



        private ArgumentException GetNodeIsNotLinkedTreeNodeException(in string argumentName) => new ArgumentException($"{argumentName} must be an instance of {nameof(LinkedTreeNode<T>)}.");

        bool ILinkedList3<T>.MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => _MoveAfter(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)), after is LinkedTreeNode<T> _after ? _after : throw GetNodeIsNotLinkedTreeNodeException(nameof(after)));

        public bool MoveAfter(LinkedTreeNode<T> node, LinkedTreeNode<T> after) => _MoveAfter(node ?? throw GetArgumentNullException(nameof(node)), after ?? throw GetArgumentNullException(nameof(after)));

        private bool _MoveAfter(LinkedTreeNode<T> node, LinkedTreeNode<T> after)
        {
            _ThrowIfNotContainedNode(node, nameof(node));
            ThrowIfNodesAreEqual(node, after);
            _ThrowIfNotContainedNode(after, nameof(after));

            return _list.MoveAfter(node._node, after._node);
        }

        bool ILinkedList3<T>.MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => _MoveBefore(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)), before is LinkedTreeNode<T> _before ? _before : throw GetNodeIsNotLinkedTreeNodeException(nameof(before)));

        public bool MoveBefore(LinkedTreeNode<T> node, LinkedTreeNode<T> before) => _MoveBefore(node ?? throw GetArgumentNullException(nameof(node)), before ?? throw GetArgumentNullException(nameof(before)));

        private bool _MoveBefore(LinkedTreeNode<T> node, LinkedTreeNode<T> before)
        {
            _ThrowIfNotContainedNode(node, nameof(node));
            ThrowIfNodesAreEqual(node, before);
            _ThrowIfNotContainedNode(node, nameof(before));

            return _list.MoveBefore(node._node, before._node);
        }

        void ILinkedList3<T>.Swap(ILinkedListNode<T> x, ILinkedListNode<T> y) => _Swap(x is LinkedTreeNode<T> _x ? _x : throw GetNodeIsNotLinkedTreeNodeException(nameof(x)), y is LinkedTreeNode<T> _y ? _y : throw GetNodeIsNotLinkedTreeNodeException(nameof(y)));

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

        public void AddLast(LinkedTreeNode<T> node)
        {
            ThrowIfNodeAlreadyHasList(node, nameof(node));

            _AddLast(node);
        }

        private void _AddLast(LinkedTreeNode<T> node) => Add(node, _node => _list.AddLast(_node), nameof(node));

        void ICollection<T>.Add(T item) => ((ICollection<LinkedTreeNode<T>>)_list).Add(new LinkedTreeNode<T>(item));



        public LinkedTreeNode<T> Find(in T value) => Find(value, EnumerationDirection.FIFO);

        ILinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => Find(value);

        public LinkedTreeNode<T> FindLast(in T value) => Find(value, EnumerationDirection.LIFO);

        ILinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => FindLast(value);

        private LinkedTreeNode<T> Find(T value, EnumerationDirection enumerationDirection)
        {
            Func<LinkedTreeNode<T>, bool> predicate;

            if (value == null)

                predicate = node => node.Value == null;

            else

                predicate = node => value.Equals(node.Value);

            return new Enumerable<LinkedTreeNode<T>>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefault(predicate);
        }



        public void Remove(LinkedTreeNode<T> node) => Remove(node, _node => _list.Remove(_node), nameof(node));

        void ILinkedList<T>.Remove(ILinkedListNode<T> node) => Remove(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)));

        public LinkedTreeNode<T> Remove(T item)
        {
            LinkedTreeNode<T> node = Find(item);

            Remove(node);

            return node;
        }

        ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove(item);

        bool ICollection<T>.Remove(T item) => Remove(item) != null;

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
    }
}

#endif
