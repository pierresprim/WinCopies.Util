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
using System.Linq;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;
using WinCopies.Util;

using static WinCopies.ThrowHelper;
using static WinCopies.Collections.ThrowHelper;
using static WinCopies.Collections.Resources.ExceptionMessages;

namespace WinCopies.Collections.Generic
{
    public interface ILinkedTreeNode<T> : ILinkedListNode<T>, ILinkedList3<T>, Extensions.Generic.IEnumerable<ILinkedTreeNode<T>>, IUIntCountableEnumerableInfo<ILinkedTreeNode<T>>, IReadOnlyLinkedList<T, ILinkedTreeNode<T>>
    {
        new ILinkedTreeNode<T>
#if CS8
            ?
#endif
            Previous
        { get; }
        new ILinkedTreeNode<T>
#if CS8
            ?
#endif
            Next
        { get; }

        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            Parent
        { get; }



        ILinkedTreeNode<T> AddAfter(ILinkedTreeNode<T> node, T value);
        ILinkedTreeNode<T> AddBefore(ILinkedTreeNode<T> node, T value);

        new ILinkedTreeNode<T> AddFirst(T value);
        new ILinkedTreeNode<T> AddLast(T value);



        void Remove(ILinkedTreeNode<T> node);
        new ILinkedTreeNode<T> Remove(T item);

        bool MoveAfter(ILinkedTreeNode<T> node, ILinkedTreeNode<T> after);
        bool MoveBefore(ILinkedTreeNode<T> node, ILinkedTreeNode<T> before);

        void Swap(ILinkedTreeNode<T> x, ILinkedTreeNode<T> y);
#if CS8
        ILinkedListNode<T>? DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>>.Previous => Previous;
        ILinkedListNode<T>? DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>>.Next => Next;
#endif
    }

    public interface IEnumerableInfoLinkedTreeNode<T> : ILinkedTreeNode<T>, IEnumerableInfoLinkedList<T>, IUIntCountableEnumerableInfo<ILinkedTreeNode<T>>
    {
        // Left empty.
    }

    public class LinkedTreeNode<T> : LinkedListBase<T, ILinkedTreeNode<T>>, IEnumerableInfoLinkedTreeNode<T>
    {
        public class Enumerator : Enumerator<ILinkedListNode<LinkedTreeNode<T>>, IEnumeratorInfo<ILinkedListNode<LinkedTreeNode<T>>>, LinkedTreeNode<T>>, IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> // LinkedTreeNode<T> does not make checks during enumeration because these checks are performed in the inner LinkedList<LinkedTreeNode<T>>'s enumerator.
        {
            private readonly ILinkedTreeNode<T> _treeNode;

            /// <summary>
            /// When overridden in a derived class, gets the element in the collection at the current position of the enumerator.
            /// </summary>
            protected override LinkedTreeNode<T> CurrentOverride => InnerEnumerator.Current.Value;

            public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

            public uint Count => _treeNode.AsFromType<IUIntCountable>().Count;

            public Enumerator(in LinkedTreeNode<T> treeNode, in EnumerationDirection enumerationDirection) : base(treeNode._list.GetNodeEnumerator(enumerationDirection)) => _treeNode = treeNode;

            protected override bool MoveNextOverride() => InnerEnumerator.AsFromType<System.Collections.IEnumerator>().MoveNext();
        }

        private T _value;
        private DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode _node;
        private DotNetFix.Generic.IReadOnlyLinkedListNode<T> _asReadOnly;
        private readonly DotNetFix.Generic.LinkedList<LinkedTreeNode<T>> _list = new
#if !CS9
            DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>
#endif
            ();

        protected ICollection InnerCollection => _list;

        public override uint Count => _list.Count;

        public override bool HasItems => _list.HasItems;

        public override bool IsReadOnly => false;

        public LinkedTreeNode<T>
#if CS8
            ?
#endif
            Previous => _node?.Previous?.Value;
        public LinkedTreeNode<T>
#if CS8
            ?
#endif
            Next => _node?.Next?.Value;

        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            ILinkedTreeNode<T>.Previous => Previous;
        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            ILinkedTreeNode<T>.Next => Next;

        public T Value { get => _value; set => _value = value; }

        public LinkedTreeNode<T> Parent { get; private set; }

        ILinkedTreeNode<T> ILinkedTreeNode<T>.Parent => Parent;

        protected override bool IsSynchronized => _list.AsFromType<ISimpleLinkedListBase2>().IsSynchronized;
        protected override object SyncRoot => _list.AsFromType<ISimpleLinkedListBase2>().SyncRoot;

        public LinkedTreeNode() { /* Left empty. */ }
        public LinkedTreeNode(in T value) => Value = value;

        public LinkedTreeNode<T>
#if CS8
            ?
#endif
            First => _list.First?.Value;
        public LinkedTreeNode<T>
#if CS8
            ?
#endif
            Last => _list.Last?.Value;

        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase2<ILinkedTreeNode<T>>.First => First;
        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase2<ILinkedTreeNode<T>>.Last => Last;

        ILinkedListNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase2<ILinkedListNode<T>>.First => First;
        ILinkedListNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase2<ILinkedListNode<T>>.Last => Last;

        public override T FirstValue => (First ?? throw GetEmptyListOrCollectionException()).Value;
        public T LastValue => (Last ?? throw GetEmptyListOrCollectionException()).Value;

        int ICollection.Count => InnerCollection.Count;
        int System.Collections.Generic.IReadOnlyCollection<T>.Count => InnerCollection.Count;
        int System.Collections.Generic.ICollection<T>.Count => InnerCollection.Count;

        bool ICollection.IsSynchronized => InnerCollection.IsSynchronized;
        object ICollection.SyncRoot => InnerCollection.SyncRoot;

        public bool SupportsReversedEnumeration => _list.SupportsReversedEnumeration;

        bool ILinkedListNodeBase<T>.IsAvailableByRef => true;
        public ref T ValueRef => ref _value;

        public ILinkedList3<T> List => Parent;

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

            action(node._node = new DotNetFix.Generic.LinkedList<LinkedTreeNode<T>>.LinkedListNode(node));
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
        void ILinkedTreeNode<T>.Swap(ILinkedTreeNode<T> x, ILinkedTreeNode<T> y) => _Swap(ToLinkedTreeNode(x, nameof(x)), ToLinkedTreeNode(y, nameof(y)));

        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddBefore(ILinkedTreeNode<T> node, T value) => AddBefore(ToLinkedTreeNode(node, nameof(node)), value);
        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddAfter(ILinkedTreeNode<T> node, T value) => AddAfter(ToLinkedTreeNode(node, nameof(node)), value);

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

        protected override ILinkedTreeNode<T> AddFirstItem(T value) => AddFirst(value);

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

        protected override ILinkedTreeNode<T> AddLastItem(T value) => AddLast(value);

        ILinkedListNode<T> ILinkedList<T>.AddLast(T value) => AddLast(value);
        ILinkedTreeNode<T> ILinkedTreeNode<T>.AddLast(T value) => AddLast(value);

        public void AddLast(LinkedTreeNode<T> node)
        {
            ThrowIfNodeAlreadyHasList(node, nameof(node));

            _AddLast(node);
        }

        private void _AddLast(LinkedTreeNode<T> node) => Add(node, _node => _list.AddLast(_node), nameof(node));

        void System.Collections.Generic.ICollection<T>.Add(T item) => _list.AsFromType<System.Collections.Generic.ICollection<LinkedTreeNode<T>>>().Add(new LinkedTreeNode<T>(item));



        public LinkedTreeNode<T>
#if CS8
            ?
#endif
            Find(T value) => Find(value, EnumerationDirection.FIFO);

        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase<T, ILinkedTreeNode<T>>.Find(T value) => Find(value);
        ILinkedListNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase<T, ILinkedListNode<T>>.Find(T value) => Find(value);

        public LinkedTreeNode<T>
#if CS8
            ?
#endif
            FindLast(T value) => Find(value, EnumerationDirection.LIFO);

        ILinkedTreeNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase<T, ILinkedTreeNode<T>>.FindLast(T value) => FindLast(value);
        ILinkedListNode<T>
#if CS8
            ?
#endif
            IReadOnlyLinkedListBase<T, ILinkedListNode<T>>.FindLast(T value) => FindLast(value);

        private LinkedTreeNode<T>
#if CS8
            ?
#endif
            Find(T value, EnumerationDirection enumerationDirection)
        {
            Predicate<LinkedTreeNode<T>> predicate;

            predicate = value == null ?
#if !CS9
                (Predicate<LinkedTreeNode<T>>)(
#endif
                node => node.Value == null
#if !CS9
                )
#endif
                : node => value.Equals(node.Value);

            return new Enumerable<LinkedTreeNode<T>>(() => GetNodeEnumerator(enumerationDirection)).FirstOrDefaultPredicate(predicate);
        }



        public void Remove(in LinkedTreeNode<T> node) => Remove(node, _list.Remove, nameof(node));

        private void _Remove(in ILinkedListNode<T> node) => Remove(node is LinkedTreeNode<T> _node ? _node : throw GetNodeIsNotLinkedTreeNodeException(nameof(node)));

        void ILinkedList<T>.Remove(ILinkedListNode<T> node) => _Remove(node);

        void ILinkedTreeNode<T>.Remove(ILinkedTreeNode<T> node) => _Remove(node);

        public LinkedTreeNode<T> Remove(in T item)
        {
            LinkedTreeNode<T>
#if CS8
                ?
#endif
            node = Find(item);

            if (node == null)

                return null;

            Remove(node);

            return node;
        }

        ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove(item);
        ILinkedTreeNode<T> ILinkedTreeNode<T>.Remove(T item) => Remove(item);
        bool System.Collections.Generic.ICollection<T>.Remove(T item) => Remove(item) != null;

        protected override T RemoveFirstItem()
        {
            ThrowIfEmptyListOrCollection(this);

            LinkedTreeNode<T> node = First;

            Remove(node, _node => _list.Remove(_node), null);

            return node.Value;
        }

        public void RemoveFirst() => RemoveFirstItem();

        public void RemoveLast()
        {
            ThrowIfEmptyListOrCollection(this);

            Remove(Last, node => _list.Remove(node), null);
        }


        protected override void ClearItems()
        {
            while (Count > 0)

                RemoveFirst();
        }

        public void Clear() => ClearItems();

        public bool Contains(T item)
        {
            Predicate<T> predicate;

            predicate = item == null ? _item => _item == null : predicate = _item => item.Equals(_item);

            foreach (LinkedTreeNode<T> node in _list.AsFromType<System.Collections.Generic.IEnumerable<LinkedTreeNode<T>>>())

                if (predicate(node.Value))

                    return true;

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) => this.CopyTo(array, arrayIndex, Count);
        public void CopyTo(Array array, int index) => this.CopyTo(array, index, Count);



        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection) => new Enumerator(this, enumerationDirection);

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> Collections.Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Extensions.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

        public IUIntCountableEnumeratorInfo<T> GetEnumerator(in EnumerationDirection enumerationDirection) => new UIntCountableEnumeratorInfo<T>(GetNodeEnumerator(enumerationDirection).SelectConverter(node => node.Value), () => Count);

        public IUIntCountableEnumeratorInfo<T> GetEnumerator() => GetEnumerator(EnumerationDirection.FIFO);
        public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => GetEnumerator(EnumerationDirection.LIFO);

        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.FIFO);

        public IUIntCountableEnumeratorInfo<LinkedTreeNode<T>> GetReversedNodeEnumerator() => GetNodeEnumerator(EnumerationDirection.LIFO);

        public DotNetFix.Generic.IUIntCountableEnumerable<ILinkedListNode<T>> AsNodeEnumerable() => new Enumeration.Generic.UIntCountableEnumerable<LinkedTreeNode<T>, ILinkedListNode<T>>(this);

        public DotNetFix.Generic.IReadOnlyLinkedListNode<T> ToReadOnly() => _asReadOnly
#if CS8
            ??=
#else
            ?? (_asReadOnly =
#endif
            new ReadOnlyLinkedListNode<T>(_node.Value)
#if !CS8
            )
#endif
            ;

        public IQueue<T> AsQueue() => new Abstraction.Generic.Queue<T, ILinkedList<T>>(this);
        public IStack<T> AsStack() => new Abstraction.Generic.Stack<T, ILinkedList<T>>(this);

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

        IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>> Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>>>.GetEnumerator() => GetNodeEnumerator();
        IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>> Extensions.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedTreeNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetEnumerator() => GetNodeEnumerator();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Extensions.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

        System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#if !CS8
        object IReadOnlyLinkedListNode.Value => Value;

        ILinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>>.Previous => Previous;
        ILinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<ILinkedListNode<T>>.Next => Next;

        DotNetFix.Generic.IReadOnlyLinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<DotNetFix.Generic.IReadOnlyLinkedListNode<T>>.Previous => Previous;

        DotNetFix.Generic.IReadOnlyLinkedListNode<T> DotNetFix.IReadOnlyLinkedListNode<DotNetFix.Generic.IReadOnlyLinkedListNode<T>>.Next => Next;

        IUIntCountableEnumerator<T> Enumeration.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedTreeNode<T>> IEnumerable<ILinkedTreeNode<T>>.GetEnumerator() => GetNodeEnumerator();
        System.Collections.Generic.IEnumerator<ILinkedTreeNode<T>> Extensions.Generic.IEnumerable<ILinkedTreeNode<T>>.GetReversedEnumerator() => GetReversedNodeEnumerator();

        void ICollectionBase<T>.Add(T item) => AddLast(item);
        bool ICollectionBase<T>.Remove(T item) => Remove(item) != null;
#endif
    }
}
#endif
