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
using System.Runtime.Serialization;
using WinCopies
#if WinCopies2
    .Util
#else
    .Collections
#endif
    .Resources;

namespace WinCopies.Collections.DotNetFix
{
#if !WinCopies2
    namespace Generic
    {
#endif
        public interface IReadOnlyLinkedList<T> : ICollection<T>, ICountableEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
        {
            System.Collections.Generic.LinkedListNode<T> Last { get; }

            System.Collections.Generic.LinkedListNode<T> First { get; }

            new int Count { get; }

            System.Collections.Generic.LinkedListNode<T> Find(T value);

            System.Collections.Generic.LinkedListNode<T> FindLast(T value);

            // todo: to remove

            new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
        }

        public interface ILinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback
        {
            System.Collections.Generic.LinkedListNode<T> Last { get; }

            System.Collections.Generic.LinkedListNode<T> First { get; }

            new int Count { get; }

            System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value);

            void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);

            System.Collections.Generic.LinkedListNode<T> AddBefore(System.Collections.Generic.LinkedListNode<T> node, T value);

            void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);

            System.Collections.Generic.LinkedListNode<T> AddFirst(T value);

            void AddFirst(System.Collections.Generic.LinkedListNode<T> node);

            System.Collections.Generic.LinkedListNode<T> AddLast(T value);

            void AddLast(System.Collections.Generic.LinkedListNode<T> node);

            System.Collections.Generic.LinkedListNode<T> Find(T value);

            System.Collections.Generic.LinkedListNode<T> FindLast(T value);

            //todo: to remove

            new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();

            void Remove(System.Collections.Generic.LinkedListNode<T> node);

            void RemoveFirst();

            void RemoveLast();
        }

        public interface ILinkedList2<T> : ILinkedList<T>
        {
            bool IsReadOnly { get; }
        }

        [DebuggerDisplay("Count = {Count}")]
        public class LinkedList<T> : System.Collections.Generic.LinkedList<T>, ILinkedList2<T>
        {
            // todo: remove the explicit interface implementation and make this property public.

            bool ILinkedList2<T>.IsReadOnly => false;

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is empty.
            /// </summary>
            public LinkedList() : base() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that contains elements copied from the specified <see cref="IEnumerable"/> and has sufficient capacity to accommodate the number of elements copied.
            /// </summary>
            /// <param name="collection">The <see cref="IEnumerable"/> whose elements are copied to the new <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
            public LinkedList(IEnumerable<T> collection) : base(collection) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="System.Collections.Generic.LinkedList{T}"/> class that is serializable with the specified <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>.
            /// </summary>
            /// <param name="info">A <see cref="SerializationInfo"/> object containing the information required to serialize the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            /// <param name="context">A <see cref="StreamingContext"/> object containing the source and destination of the serialized stream associated with the <see cref="System.Collections.Generic.LinkedList{T}"/>.</param>
            protected LinkedList(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }

        [DebuggerDisplay("Count = {Count}")]
        public class ReadOnlyLinkedList<T> : IReadOnlyLinkedList<T>, ILinkedList2<T>
        {
            public bool IsReadOnly => true;

            protected ILinkedList<T> InnerList { get; }

            public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

            public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

            public int Count => InnerList.Count;

            bool ICollection<T>.IsReadOnly => true;

            object ICollection.SyncRoot => InnerList.SyncRoot;

            bool ICollection.IsSynchronized => InnerList.IsSynchronized;

            public bool Contains(T value) => InnerList.Contains(value);

            public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

            public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

            public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

            // todo: to remove

            public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

            public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

            public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

            void ICollection<T>.Add(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            void ICollection<T>.Clear() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => InnerList.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public System.Collections.Generic.LinkedListNode<T> AddBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public System.Collections.Generic.LinkedListNode<T> AddFirst(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public System.Collections.Generic.LinkedListNode<T> AddLast(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void Remove(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void RemoveFirst() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

            public void RemoveLast() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);
        }
#if !WinCopies2
    }
#endif
}
