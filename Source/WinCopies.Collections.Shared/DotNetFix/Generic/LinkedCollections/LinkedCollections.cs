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

#if WinCopies2
using System.Runtime.Serialization;
#endif

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif

        [Serializable]
        public class LinkedCollection<T> : ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
#if WinCopies2
            , IDeserializationCallback, ISerializable
#endif
        {
            protected internal
#if WinCopies2
System.Collections.Generic.LinkedList
#else
                ILinkedList
#endif
                <T> InnerList
            { get; }

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

            bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

            bool ICollection<T>.IsReadOnly => false;

            public LinkedCollection() : this(new LinkedList<T>()) { }

            public LinkedCollection(in
#if WinCopies2
                System.Collections.Generic.LinkedList
#else
                ILinkedList
#endif
                <T> list) => InnerList = list;

            protected virtual void AddItem(T item) => ((ICollection<T>)InnerList).Add(item);

            void ICollection<T>.Add(T item) => AddItem(item);

#if WinCopies2
        protected virtual void AddItemAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        /// <summary>
        /// Adds the specified new node after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddAfter(System.Collections.Generic.LinkedListNode{T}, T)"/>
        public void AddAfter(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemAfter(node, newNode);

        protected virtual void AddItemBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        /// <summary>
        /// Adds the specified new node before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert <paramref name="newNode"/>.</param>
        /// <param name="newNode">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add to the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>. -or- <paramref name="newNode"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>. -or- <paramref name="newNode"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in T)"/>
        public void AddBefore(in System.Collections.Generic.LinkedListNode<T> node, in System.Collections.Generic.LinkedListNode<T> newNode) => AddItemBefore(node, newNode);

        protected virtual void AddFirstItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

        /// <summary>
        /// Adds the specified new node at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddFirst(in T)"/>
        public void AddFirst(in System.Collections.Generic.LinkedListNode<T> node) => AddFirstItem(node);

        protected virtual void AddLastItem(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

        /// <summary>
        /// Adds the specified new node at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(System.Collections.Generic.LinkedListNode{T})"/> method to provide a custom implementation.
        /// </summary>
        /// <param name="node">The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="node"/> belongs to another <see cref="System.Collections.Generic.LinkedList{T}"/>.</exception>
        /// <seealso cref="AddLast(in T)"/>
        public void AddLast(in System.Collections.Generic.LinkedListNode<T> node) => AddLastItem(node);
#endif

            protected virtual
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemAfter(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => InnerList.AddAfter(node, value);

            /// <summary>
            /// Adds a new node containing the specified value after the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemAfter(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> after which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            /// <seealso cref="AddAfter(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
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
                <T> node, T value) => AddItemAfter(node, value);

            protected virtual
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddItemBefore(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => InnerList.AddBefore(node, value);

            /// <summary>
            /// Adds a new node containing the specified value before the specified existing node in the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddItemBefore(System.Collections.Generic.LinkedListNode{T}, T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="node">The <see cref="System.Collections.Generic.LinkedListNode{T}"/> before which to insert a new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</param>
            /// <param name="value">The value to add to the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
            /// <exception cref="InvalidOperationException"><paramref name="node"/> is not in the current <see cref="LinkedCollection{T}"/>.</exception>
            /// <seealso cref="AddBefore(in System.Collections.Generic.LinkedListNode{T}, in System.Collections.Generic.LinkedListNode{T})"/>
            public
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddBefore(in
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, in T value) => AddItemBefore(node, value);

            protected virtual
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirstItem(T value) => InnerList.AddFirst(value);

            /// <summary>
            /// Adds a new node containing the specified value at the start of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddFirstItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the start of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <seealso cref="AddFirst(in T)"/>
            public
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirst(in T value) => AddFirstItem(value);

            protected virtual
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLastItem(T value) => InnerList.AddLast(value);

            /// <summary>
            /// Adds a new node containing the specified value at the end of the <see cref="LinkedCollection{T}"/>. Override the <see cref="AddLastItem(T)"/> method to provide a custom implementation.
            /// </summary>
            /// <param name="value">The value to add at the end of the <see cref="LinkedCollection{T}"/>.</param>
            /// <returns>The new <see cref="System.Collections.Generic.LinkedListNode{T}"/> containing value.</returns>
            /// <seealso cref="AddLast(in System.Collections.Generic.LinkedListNode{T})"/>
            public
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLast(in T value) => AddLastItem(value);

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

            protected virtual void ClearItems() => InnerList.Clear();

            public void Clear() => ClearItems();

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

#if WinCopies2
        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
#endif

            protected virtual bool RemoveItem(T item) => InnerList.Remove(item);

            public bool Remove(T item) => RemoveItem(item);

            protected virtual void RemoveItem(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node) => InnerList.Remove(node);

            public void Remove(
#if WinCopies2
                System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node) => RemoveItem(node);

            protected virtual void RemoveFirstItem() => InnerList.RemoveFirst();

            public void RemoveFirst() => RemoveFirstItem();

            protected virtual void RemoveLastItem() => InnerList.RemoveLast();

            public void RemoveLast() => RemoveLastItem();

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
        }
#if !WinCopies2
    }
#endif
}
