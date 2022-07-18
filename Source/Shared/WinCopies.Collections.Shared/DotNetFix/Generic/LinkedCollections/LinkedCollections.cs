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

using WinCopies.Util;

#if WinCopies3
using System.Linq;

using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#else
using System.Runtime.Serialization;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        [Serializable]
        public class LinkedCollection<T> : System.Collections.Generic.ICollection<T>, IEnumerable, System.Collections.Generic.IReadOnlyCollection<T>, ICollection,
#if !WinCopies3
              IDeserializationCallback, ISerializable, ILinkedList2<T>, System.Collections.Generic.IEnumerable<T>
#else
            ILinkedList3<T>
#endif
        {
#if WinCopies3
            public enum MoveDirection : sbyte
            {
                Before = -1,

                After = 1
            }
#endif

            protected internal
#if WinCopies3
            ILinkedList3
#else
            System.Collections.Generic.LinkedList
#endif
               <T> InnerList
            { get; }

            public
#if WinCopies3
            ILinkedListNode
#else
            System.Collections.Generic.LinkedListNode
#endif
                <T> Last => InnerList.Last;

            public T LastValue => Last.Value;

            public
#if WinCopies3
            ILinkedListNode
#else
            System.Collections.Generic.LinkedListNode
#endif
                <T> First => InnerList.First;

            public T FirstValue => First.Value;

            public
#if !WinCopies3
int
#else
                uint
#endif
                Count => InnerList.Count;

            public bool IsReadOnly =>
#if !WinCopies3
            false
#else
            InnerList.IsReadOnly
#endif
            ;

#if WinCopies3
            int ICollection.Count => (int)Count;

            int System.Collections.Generic.ICollection<T>.Count => (int)Count;

            int System.Collections.Generic.IReadOnlyCollection<T>.Count => (int)Count;
#endif

            bool ICollection.IsSynchronized => InnerList
#if !WinCopies3
            .AsFromType<ICollection>()
#endif
            .IsSynchronized;

            object ICollection.SyncRoot => InnerList
#if !WinCopies3
            .AsFromType<ICollection>()
#endif
            .SyncRoot;

            bool System.Collections.Generic.ICollection<T>.IsReadOnly => false;

            public LinkedCollection(in
#if WinCopies3
                ILinkedList3
#else
                System.Collections.Generic.LinkedList
#endif
                <T> list) => InnerList = list
#if WinCopies3
                    ?? throw GetArgumentNullException(nameof(list))
#endif
            ;

            public LinkedCollection() : this(new LinkedList<T>()) { /* Left empty. */ }

            #region Add Methods
#if WinCopies3
            protected virtual bool OnAddItem(T item) => true;

            protected virtual void OnNodeAdded(ILinkedListNode<T> node) { /* Left empty. */ }
#endif

            protected virtual void AddItem(T item)
            {
#if WinCopies3
                if (OnAddItem(item))
                {
#endif
                    InnerList.AsFromType<
#if WinCopies3
                IUIntCollection<T>
#else
                ICollection<T>
#endif
                >().Add(item);

#if WinCopies3
                    OnNodeAdded(InnerList.First<ILinkedListNode<T>>(node => node.Value.Equals(item)));
                }
#endif
            }

#if WinCopies3
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
#endif

            #region AddAfter Methods
            #region AddItemAfter
            protected virtual
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemAfter(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) =>
#if WinCopies3
                    AddItem(node, value,
#endif
                    InnerList.AddAfter
#if !WinCopies3
                    (node, value
#endif
                    );
            #endregion

            #region AddAfter
            /// <summary>
            /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddAfter(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => AddItemAfter(node, value);
            #endregion
            #endregion

            #region AddBefore Methods
            #region AddItemBefore
            protected virtual
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemBefore(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) =>
#if WinCopies3
                    AddItem(node, value,
#endif
                    InnerList.AddBefore
#if !WinCopies3
                    (node, value
#endif
                    );
            #endregion

            #region AddBefore
            /// <summary>
            /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddBefore(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => AddItemBefore(node, value);
            #endregion
            #endregion

            #region AddFirst Methods
            #region AddFirstItem
            protected virtual
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirstItem(T value) =>
#if WinCopies3
                    AddItem(value,
#endif
                    InnerList.AddFirst
#if !WinCopies3
                    (value
#endif
                        );
            #endregion

            #region AddFirst
            /// <summary>
            /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirst(T value) => AddFirstItem(value);
            #endregion
            #endregion

            #region AddLast Methods
            #region AddLastItem
            protected virtual
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLastItem(T value) =>
#if WinCopies3
                    AddItem(value,
#endif
                    InnerList.AddLast
#if !WinCopies3
                    (value
#endif
                        );
            #endregion

            #region AddLast
            /// <summary>
            /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLast(T value) => AddLastItem(value);
            #endregion
            #endregion

            void System.Collections.Generic.ICollection<T>.Add(T item) => AddItem(item);

#if WinCopies3
            void ICollectionBase<T>.Add(T item) => AddItem(item);
#endif
            #endregion Add Methods

            #region Find Methods
            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Find(T value) => InnerList.Find(value);

            public
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> FindLast(T value) => InnerList.FindLast(value);
            #endregion Find Methods

#if !CS8 && WinCopies3
            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => Find(value);

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => FindLast(value);
#endif

            protected virtual void ClearItems() => InnerList.Clear();

            public void Clear() => ClearItems();

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => InnerList.AsFromType<ICollection>().CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() =>

#if WinCopies3
                InnerList.AsFromType<System.Collections.Generic.IEnumerable<T>>().GetEnumerator();
#else
                GetEnumerator();

            public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();
#endif

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerList.AsFromType<IEnumerable>().GetEnumerator();

#if WinCopies3
            public bool SupportsReversedEnumeration => InnerList.SupportsReversedEnumeration;

            public IUIntCountableEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

            public IUIntCountableEnumerator<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

            public IUIntCountableEnumerator<ILinkedListNode<T>> GetNodeEnumerator() => InnerList.GetNodeEnumerator();

            public IUIntCountableEnumerator<ILinkedListNode<T>> GetReversedNodeEnumerator() => InnerList.GetReversedNodeEnumerator();

            System.Collections.Generic.IEnumerator<T> Collections.
#if WinCopies3
            Extensions.
#endif
            Generic.IEnumerable<T>.GetReversedEnumerator() => InnerList.AsFromType<Collections.
#if WinCopies3
            Extensions.
#endif
            Generic.IEnumerable<T>>().GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => InnerList.AsFromType<System.Collections.Generic.IEnumerable<ILinkedListNode<T>>>().GetEnumerator();

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.
#if WinCopies3
            Extensions.
#endif
            Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => InnerList.AsFromType<Collections.
#if WinCopies3
            Extensions.
#endif
            Generic.IEnumerable<ILinkedListNode<T>>>().GetReversedEnumerator();

            protected virtual void OnNodeRemoved(ILinkedListNode<T> node) { /* Left empty. */ }

#if !CS8
            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => First;

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => Last;

            System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
#else
        protected virtual void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        /// <summary>
        /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddAfter(System.Collections.Generic.LinkedListNode{T}, T)"/>
        public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

        protected virtual void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        /// <summary>
        /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

        protected virtual void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

        /// <summary>
        /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => AddFirstItem(node);

        protected virtual void AddLastItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

        /// <summary>
        /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => AddLastItem(node);

        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
#endif

            #region Remove Methods
#if WinCopies3
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
#endif

            #region RemoveNode
            protected virtual void
#if WinCopies3
                RemoveNode
#else
                RemoveItem
#endif
                (
#if WinCopies3
                ILinkedListNode<T>
#else
                System.Collections.Generic.LinkedListNode<T>
#endif
                node)
#if !WinCopies3
                =>
#else
            {
                if (OnRemoveNode(node))
                {
#endif
                    InnerList.Remove(node);
#if WinCopies3
                    OnNodeRemoved(node);
                }
            }
#endif
            #endregion RemoveNode

            #region RemoveItem
#if !WinCopies3
        protected virtual bool RemoveItem(
#if WinCopies3
                TOut
#else
                T
#endif
                   item) => InnerList.Remove(item);
#else
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
#endif
            #endregion RemoveItem

            public bool
#if WinCopies3
                ?
#endif
                Remove(T item) => RemoveItem(item);

            public void Remove(
#if WinCopies3
                ILinkedListNode<T>
#else
                System.Collections.Generic.LinkedListNode<T>
#endif
                 node) =>
#if WinCopies3
            RemoveNode
#else
            RemoveItem
#endif
            (node);

            #region RemoveFirst Methods
            #region RemoveFirstItem
            protected virtual void RemoveFirstItem() =>
#if WinCopies3
                RemoveNode(InnerList.First,
#endif
                InnerList.RemoveFirst
#if !WinCopies3
                (
#endif
                );
            #endregion

            public void RemoveFirst() => RemoveFirstItem();
            #endregion

            #region RemoveLast Methods
            #region RemoveLastItem
            protected virtual void RemoveLastItem() =>
#if WinCopies3
                RemoveNode(InnerList.First,
#endif
                InnerList.RemoveFirst
#if !WinCopies3
                (
#endif
                );
            #endregion

            public void RemoveLast() => RemoveLastItem();
            #endregion
            #endregion Remove Methods

#if WinCopies3
            bool System.Collections.Generic.ICollection<T>.Remove(T item) => Remove(item) == true;

            bool ICollectionBase<T>.Remove(T item) => Remove(item) == true;

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

            void ILinkedList<T>.Add(T item) => InnerList.Add(item);

            bool ILinkedList<T>.Remove(T item) => InnerList.Remove(item) != null;
#endif
        }
#if WinCopies3
    }
#endif
}
#endif
