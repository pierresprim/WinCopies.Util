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
        public class LinkedCollection<T> : ICollection<T>, IEnumerable, IReadOnlyCollection<T>, ICollection,
#if !WinCopies3
            IDeserializationCallback, ISerializable, ILinkedList2<T>, System.Collections.Generic.IEnumerable<T>
#else
            IEnumerableInfoLinkedList<T>
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
#if !WinCopies3
System.Collections.Generic.LinkedList
#else
                IEnumerableInfoLinkedList
#endif
                <T> InnerList
            { get; }

            public
#if !WinCopies3
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Last => InnerList.Last;

            public T LastValue => Last.Value;

            public
#if !WinCopies3
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
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

            int ICollection<T>.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

            bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

            bool ICollection<T>.IsReadOnly => false;

            public LinkedCollection() : this(new LinkedList<T>()) { }

            public LinkedCollection(in
#if !WinCopies3
                System.Collections.Generic.LinkedList
#else
                IEnumerableInfoLinkedList
#endif
                <T> list) => InnerList = list
#if WinCopies3
                ?? throw GetArgumentNullException(nameof(list))
#endif
                ;

#if WinCopies3
            protected virtual void OnNodeAdded(ILinkedListNode<T> node) { /* Left empty. */ }
#endif

            protected virtual void AddItem(T item)
            {
                ((ICollection<T>)InnerList).Add(item);

#if WinCopies3
                OnNodeAdded(InnerList.First<ILinkedListNode<T>>(node => node.Value.Equals(item)));
#endif
            }

            void ICollection<T>.Add(T item) => AddItem(item);

#if !WinCopies3
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
#endif

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
                <T> node, T value)
#if !WinCopies3
=>
#else
            {
                ILinkedListNode<T> newNode =
#endif
                InnerList.AddAfter(node, value);

#if WinCopies3
                OnNodeAdded(newNode);

                return newNode;
            }
#endif

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
                <T> node, T value)
#if !WinCopies3
                =>
#else
            {
                ILinkedListNode<T> newNode =
#endif
                InnerList.AddBefore(node, value);
#if WinCopies3
                OnNodeAdded(newNode);

                return newNode;
            }
#endif

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

            protected virtual
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirstItem(T value)
#if !WinCopies3
                =>
#else
            {
                ILinkedListNode<T> node =
#endif
                    InnerList.AddFirst(value);
#if WinCopies3
                OnNodeAdded(node);

                return node;
            }
#endif

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

            protected virtual
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLastItem(T value)
#if !WinCopies3
                =>
#else
            {
                ILinkedListNode<T> node =
#endif
                    InnerList.AddLast(value);
#if WinCopies3
                OnNodeAdded(node);

                return node;
            }
#endif

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

            protected virtual void ClearItems() => InnerList.Clear();

            public void Clear() => ClearItems();

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            public
#if !WinCopies3
            System.Collections.Generic.LinkedList<T>.Enumerator
#else
IUIntCountableEnumeratorInfo<T>
#endif
            GetEnumerator() => InnerList.GetEnumerator();

#if !WinCopies3
        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

#if !WinCopies3
        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
#else
            public bool SupportsReversedEnumeration => InnerList.SupportsReversedEnumeration;

            public IUIntCountableEnumeratorInfo<T> GetEnumerator(EnumerationDirection enumerationDirection) => InnerList.GetEnumerator(enumerationDirection);

            public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

            public IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection) => InnerList.GetNodeEnumerator(enumerationDirection);

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => ((Collections.Generic.IEnumerable<ILinkedListNode<T>>)InnerList).GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => ((Collections.Generic.IEnumerable<T>)InnerList).GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList3<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) => GetNodeEnumerator(enumerationDirection);

            IUIntCountableEnumerator<T> ILinkedList<T>.GetEnumerator() => ((ILinkedList<T>)InnerList).GetEnumerator();

            IUIntCountableEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => ((ILinkedList<T>)InnerList).GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => ((System.Collections.Generic.IEnumerable<ILinkedListNode<T>>)InnerList).GetEnumerator();

            System.Collections.Generic.IEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator(EnumerationDirection enumerationDirection) => ((IReadOnlyLinkedList2<T>)InnerList).GetEnumerator(enumerationDirection);

            IUIntCountableEnumerator<T> IUIntCountableEnumerable<T>.GetEnumerator() => ((IUIntCountableEnumerable<T>)InnerList).GetEnumerator();

            IEnumeratorInfo2<T>
#if WinCopies3
                WinCopies.Collections.DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo2<T>>
#else
                IEnumerableInfo<T>
#endif
                .GetEnumerator() => ((IEnumerableInfo<T>)InnerList).GetEnumerator();

            IEnumeratorInfo2<T>
#if WinCopies3
               WinCopies.Collections.Generic.IEnumerable<T, IEnumeratorInfo2<T>>
#else
                IEnumerableInfo<T>
#endif
                .GetReversedEnumerator() => ((IEnumerableInfo<T>)InnerList).GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => ((System.Collections.Generic.IEnumerable<T>)InnerList).GetEnumerator();

            protected virtual void OnNodeRemoved(ILinkedListNode<T> node) { /* Left empty. */ }
#endif

            protected virtual bool RemoveItem(T item)
#if !WinCopies3
                =>
#else
            {
                ILinkedListNode<T> node =
#endif
                    InnerList.Remove(item);
#if WinCopies3
                if (node == null)

                    return false;

                OnNodeRemoved(node);

                return true;
            }

            protected virtual ILinkedListNode<T> RemoveItem2(T item)
            {
                ILinkedListNode<T> node = InnerList.Remove(item);

                if (node == null)

                    return null;

                OnNodeRemoved(node);

                return node;
            }
#endif

            public bool Remove(T item) => RemoveItem(item);

#if WinCopies3
            public ILinkedListNode<T> Remove2(T item) => RemoveItem2(item);

            ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => Remove2(item);
#endif

            protected virtual void RemoveItem(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node)
#if !WinCopies3
                =>
#else
            {
#endif
                InnerList.Remove(node);
#if WinCopies3
                OnNodeRemoved(node);
            }
#endif

            public void Remove(
#if !WinCopies3
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node)
#if !WinCopies3
                =>
#else
            {
#endif
                RemoveItem(node);
#if WinCopies3

                OnNodeRemoved(node);
            }
#endif

            protected virtual void RemoveFirstItem()
#if !WinCopies3
                =>
#else
            {
                ILinkedListNode<T> node = InnerList.First;
#endif
                InnerList.RemoveFirst();
#if WinCopies3
                OnNodeRemoved(node);
            }
#endif

            public void RemoveFirst() => RemoveFirstItem();

            protected virtual void RemoveLastItem()
#if !WinCopies3
                =>
#else
            {
                ILinkedListNode<T> node = InnerList.Last;
#endif
                InnerList.RemoveLast();
#if WinCopies3
                OnNodeRemoved(node);
            }
#endif

            public void RemoveLast() => RemoveLastItem();

#if WinCopies3
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
#endif
        }
#if WinCopies3
    }
#endif
}
#endif
