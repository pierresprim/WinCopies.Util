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

using WinCopies.Collections.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;
using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public abstract class LinkedCollection<TItems, TNodes, TList> : LinkedListBase<TItems, TNodes>, WinCopies.DotNetFix.IDisposable, IEnumerable, System.Collections.Generic.IReadOnlyCollection<TItems>, ICollection
#if CS8
        , IEnumerable<TItems>
#endif
        where TList : class, IReadOnlyLinkedList<TItems, TNodes> where TNodes : IReadOnlyLinkedListNode<TItems>
    {
        private TList _list;

        protected internal TList InnerList => _list ?? throw GetExceptionForDispose(false);
        protected System.Collections.Generic.ICollection<TItems> InnerCollection => InnerList;
        protected Extensions.IEnumerable<IUIntCountableEnumeratorInfo<TItems>> InnerEnumerable => InnerList;

        public bool IsDisposed => _list == null;

        public override TItems FirstValue => InnerList.FirstValue;
        public TItems LastValue => InnerList.LastValue;

        public override uint Count => InnerList.AsFromType<IUIntCountable>().Count;
        int ICollection.Count => (int)Count;
        int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => (int)Count;

        public override bool HasItems => InnerList.HasItems;

        public override bool IsReadOnly => InnerCollection.IsReadOnly;

        public bool SupportsReversedEnumeration => InnerList.SupportsReversedEnumeration;

        protected override bool IsSynchronized => InnerList.IsSynchronized;
        protected override object SyncRoot => InnerList.SyncRoot;

        bool ICollection.IsSynchronized => InnerList.IsSynchronized;
        object ICollection.SyncRoot => InnerList.SyncRoot;

        protected LinkedCollection(in TList list) => _list = list ?? throw new ArgumentNullException(nameof(list));

        public bool Contains(TItems item) => InnerCollection.Contains(item);

        public void CopyTo(Array array, int index) => InnerList.AsFromType<ICollection>().CopyTo(array, index);
        public void CopyTo(TItems[] array, int arrayIndex) => InnerCollection.CopyTo(array, arrayIndex);

        public IUIntCountableEnumeratorInfo<TItems> GetEnumerator() => InnerEnumerable.GetEnumerator();
        public IUIntCountableEnumeratorInfo<TItems> GetReversedEnumerator() => InnerEnumerable.GetReversedEnumerator();

        protected virtual void DisposeManaged() => _list = null;

        protected virtual void DisposeUnmanaged() { /* Left empty. */ }

        public void Dispose()
        {
            if (IsDisposed)

                return;

            DisposeUnmanaged();
            DisposeManaged();
            GC.SuppressFinalize(this);
        }

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

        ~LinkedCollection() => DisposeUnmanaged();
#if !CS8
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endif
    }

    public class ReadOnlyLinkedCollection<TItems, TNodes, TList> : LinkedCollection<TItems, TNodes, TList>, System.Collections.Generic.ICollection<TItems>, IReadOnlyLinkedList<TItems> where TNodes : IReadOnlyLinkedListNode<TItems> where TList : class, IReadOnlyLinkedList<TItems, TNodes>
    {
        public IReadOnlyLinkedListNode<TItems> First => InnerList.First?.ToReadOnly();
        public IReadOnlyLinkedListNode<TItems> Last => InnerList.Last?.ToReadOnly();

        int System.Collections.Generic.ICollection<TItems>.Count => (int)Count;

        public override bool IsReadOnly => true;

        public ReadOnlyLinkedCollection(in TList list) : base(list) { /* Left empty. */ }

        public IReadOnlyLinkedListNode<TItems> Find(TItems value) => InnerList.Find(value)?.ToReadOnly();
        public IReadOnlyLinkedListNode<TItems> FindLast(TItems value) => InnerList.FindLast(value)?.ToReadOnly();

        protected override TNodes AddFirstItem(TItems value) => throw GetReadOnlyListOrCollectionException();
        protected override TNodes AddLastItem(TItems value) => throw GetReadOnlyListOrCollectionException();

        protected override TItems RemoveFirstItem() => throw GetReadOnlyListOrCollectionException();

        IUIntCountableEnumerator<TItems> Enumeration.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();

        bool System.Collections.Generic.ICollection<TItems>.Contains(TItems item) => throw GetReadOnlyListOrCollectionException();
        void System.Collections.Generic.ICollection<TItems>.CopyTo(TItems[] array, int arrayIndex) => throw GetReadOnlyListOrCollectionException();

        void System.Collections.Generic.ICollection<TItems>.Add(TItems item) => throw GetReadOnlyListOrCollectionException();
        bool System.Collections.Generic.ICollection<TItems>.Remove(TItems item) => throw GetReadOnlyListOrCollectionException();
        void System.Collections.Generic.ICollection<TItems>.Clear() => throw GetReadOnlyListOrCollectionException();

        protected override void ClearItems() => throw GetReadOnlyListOrCollectionException();
#if !CS8
        IEnumerator<TItems> Extensions.Generic.IEnumerable<TItems>.GetReversedEnumerator() => GetReversedEnumerator();
#endif
    }

    public class ReadOnlyLinkedCollection<T> : ReadOnlyLinkedCollection<T, IReadOnlyLinkedListNode<T>, IReadOnlyLinkedList<T>>, System.Collections.Generic.ICollection<T>, IEnumerable<T>, System.Collections.Generic.IReadOnlyCollection<T>, ICollection, IReadOnlyLinkedList<T>
    {
        public ReadOnlyLinkedCollection(in IReadOnlyLinkedList<T> list) : base(list) { /* Left empty. */ }
    }

    public class LinkedCollection<T> : LinkedCollection<T, ILinkedListNode<T>, ILinkedList3<T>>, ILinkedList3<T>, System.Collections.Generic.ICollection<T>, ICollectionBase<T>
    {
        public enum MoveDirection : sbyte
        {
            Before = -1,
            After = 1
        }

        public ILinkedListNode<T> First => InnerList.First;
        public ILinkedListNode<T> Last => InnerList.Last;

        public LinkedCollection(in ILinkedList3<T> list) : base(list) { /* Left empty. */ }
        public LinkedCollection() : base(new LinkedList<T>()) { /* Left empty. */ }

        protected override void ClearItems() => InnerCollection.Clear();
        public void Clear() => ClearItems();

        int System.Collections.Generic.ICollection<T>.Count => (int)Count;
        void System.Collections.Generic.ICollection<T>.Add(T item) => AddItem(item);

        public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator() => InnerList.GetNodeEnumerator();
        public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetReversedNodeEnumerator() => InnerList.GetReversedNodeEnumerator();

        public ILinkedListNode<T> Find(T value) => InnerList.Find(value);
        public ILinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        protected virtual void OnNodeRemoved(ILinkedListNode<T> node) { /* Left empty. */ }

        #region Add Methods
        protected virtual bool OnAddItem(T item) => true;

        protected virtual void OnNodeAdded(ILinkedListNode<T> node) { /* Left empty. */ }

        protected virtual void AddItem(T item)
        {
            if (OnAddItem(item))
            {
                InnerList.AsFromType<IUIntCollection<T>>().Add(item);

                OnNodeAdded(InnerList.First<ILinkedListNode<T>>(node => node.Value.Equals(item)));
            }
        }

        private ILinkedListNode<T> AddItem(T item, in Func<T, ILinkedListNode<T>> func)
        {
            if (OnAddItem(item))
            {
                ILinkedListNode<T> node = func(item);

                OnNodeAdded(node);

                return node;
            }

            return null;
        }

        private ILinkedListNode<T> AddItem(ILinkedListNode<T> node, T item, Func<ILinkedListNode<T>, T, ILinkedListNode<T>> func) => AddItem(item, _item => func(node, _item));

        #region AddAfter Methods
        protected virtual ILinkedListNode<T> AddItemAfter(ILinkedListNode<T> node, T value) => AddItem(node, value, InnerList.AddAfter);

        /// <summary>
        /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(ILinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="ILinkedListNode{T}"/> after which to insert a new <see cref="ILinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="ILinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        public ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value) => AddItemAfter(node, value);
        #endregion
        #region AddBefore Methods
        protected virtual ILinkedListNode<T> AddItemBefore(ILinkedListNode<T> node, T value) => AddItem(node, value, InnerList.AddBefore);

        /// <summary>
        /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(ILinkedListNode{T}, T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="ILinkedListNode{T}"/> before which to insert a new <see cref="ILinkedListNode{T}"/> containing value.</param>
        /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="ILinkedListNode{T}"/> containing value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
        public ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value) => AddItemBefore(node, value);
        #endregion

        #region AddFirst Methods
        protected override ILinkedListNode<T> AddFirstItem(T value) => AddItem(value, InnerList.AddFirst);

        /// <summary>
        /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="ILinkedListNode{T}"/> containing value.</returns>
        public ILinkedListNode<T> AddFirst(T value) => AddFirstItem(value);
        #endregion
        #region AddLast Methods
        protected override ILinkedListNode<T> AddLastItem(T value) => AddItem(value, InnerList.AddLast);

        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <returns>The new <see cref="ILinkedListNode{T}"/> containing value.</returns>
        public ILinkedListNode<T> AddLast(T value) => AddLastItem(value);
        #endregion

        void ICollectionBase<T>.Add(T item) => AddItem(item);
        #endregion Add Methods

        #region Remove Methods
        public bool? Remove(T item) => RemoveItem(item);

        protected virtual bool OnRemoveItem(T item) => true;

        protected virtual bool OnRemoveNode(ILinkedListNode<T> node) => true;

        protected virtual bool? RemoveItem(T item) => RemoveItem(item, out ILinkedListNode<T> _);

        protected virtual ILinkedListNode<T> RemoveItem(T item, out bool cancelled)
        {
            cancelled = RemoveItem(item, out ILinkedListNode<T> node) == null;

            return node;
        }

        protected virtual ILinkedListNode<T> RemoveItem2(T item) => RemoveItem(item, out bool _);

        public ILinkedListNode<T> Remove2(T item) => RemoveItem2(item);

        ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove2(item);

        private void RemoveNode(ILinkedListNode<T> node, in Action action)
        {
            if (OnRemoveNode(node))
            {
                action();

                OnNodeRemoved(node);
            }
        }

        protected virtual void RemoveNode(ILinkedListNode<T> node)
        {
            if (OnRemoveNode(node))
            {
                InnerList.Remove(node);

                OnNodeRemoved(node);
            }
        }

        protected virtual bool? RemoveItem(T item, out ILinkedListNode<T> node)
        {
            if (OnRemoveItem(item))
            {
                node = Find(item);

                if (node == null)

                    return false;

                if (OnRemoveNode(node))
                {
                    _ = InnerList.Remove(item);

                    OnNodeRemoved(node);

                    return true;
                }
            }

            node = null;

            return null;
        }

        public void Remove(ILinkedListNode<T> node) => RemoveNode(node);

        #region RemoveFirst Methods
        protected override T RemoveFirstItem()
        {
            ILinkedListNode<T>
#if CS8
                ?
#endif
                node = InnerList.First;

            RemoveNode(node, InnerList.RemoveFirst);

            return node.Value;
        }

        public void RemoveFirst() => RemoveFirstItem();
        #endregion

        #region RemoveLast Methods
        protected virtual void RemoveLastItem() => RemoveNode(InnerList.First, InnerList.RemoveFirst);

        public void RemoveLast() => RemoveLastItem();
        #endregion

        bool System.Collections.Generic.ICollection<T>.Remove(T item) => Remove(item) == true;

        bool ICollectionBase<T>.Remove(T item) => Remove(item) == true;
        #endregion Remove Methods

        protected virtual void SwapItem(ILinkedListNode<T> x, ILinkedListNode<T> y) => InnerList.Swap(x, y);

        public void Swap(ILinkedListNode<T> x, ILinkedListNode<T> y) => SwapItem(x, y);

        protected virtual void OnNodeMoved(ILinkedListNode<T> node, ILinkedListNode<T> other, MoveDirection moveDirection) { /* Left empty. */ }

        protected virtual bool MoveNodeAfter(ILinkedListNode<T> node, ILinkedListNode<T> after)
        {
            if (InnerList.MoveAfter(node, after))
            {
                OnNodeMoved(node, after, MoveDirection.After);

                return true;
            }

            return false;
        }

        public bool MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => MoveNodeAfter(node, after);

        protected virtual bool MoveNodeBefore(ILinkedListNode<T> node, ILinkedListNode<T> before)
        {
            if (InnerList.MoveBefore(node, before))
            {
                OnNodeMoved(node, before, MoveDirection.Before);

                return true;
            }

            return false;
        }

        public bool MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => MoveNodeBefore(node, before);

        public IUIntCountableEnumerable<ILinkedListNode<T>> AsNodeEnumerable() => InnerList.AsNodeEnumerable();

        public IQueue<T> AsQueue() => new Abstraction.Generic.Queue<T, ILinkedList<T>>(this);
        public IStack<T> AsStack() => new Abstraction.Generic.Stack<T, ILinkedList<T>>(this);

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetEnumerator() => GetNodeEnumerator();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> Extensions.IEnumerable<IUIntCountableEnumeratorInfo<ILinkedListNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();
#if !CS8
        IUIntCountableEnumerator<T> Enumeration.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();
        System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => GetEnumerator();

        IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

        IEnumerator<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>>.GetEnumerator() => GetNodeEnumerator();

        IEnumerator<ILinkedListNode<T>> Extensions.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => GetReversedNodeEnumerator();
#endif
    }
}
#endif
