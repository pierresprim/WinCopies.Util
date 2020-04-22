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
using System.Text;
using WinCopies.Collections.DotNetFix;
using WinCopies.Util.Resources;

namespace WinCopies.Collections.DotNetFix
{

    // todo: add 'in' parameter keyword?

    public interface IReadOnlyLinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback

    {

        LinkedListNode<T> Last { get; }

        LinkedListNode<T> First { get; }

        new int Count { get; }

        LinkedListNode<T> Find(T value);

        LinkedListNode<T> FindLast(T value);

        // todo: to remove

        new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();

    }

    public interface ILinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback

    {

        LinkedListNode<T> Last { get; }

        LinkedListNode<T> First { get; }

        new int Count { get; }

        LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value);

        void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode);

        LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value);

        void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode);

        LinkedListNode<T> AddFirst(T value);

        void AddFirst(LinkedListNode<T> node);

        LinkedListNode<T> AddLast(T value);

        void AddLast(LinkedListNode<T> node);

        LinkedListNode<T> Find(T value);

        LinkedListNode<T> FindLast(T value);

        //todo: to remove

        new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();

        void Remove(LinkedListNode<T> node);

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

    }

    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyLinkedList<T> : IReadOnlyLinkedList<T>, ILinkedList2<T>

    {

        public bool IsReadOnly => true;

        protected ILinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => InnerList.SyncRoot;

        bool ICollection.IsSynchronized => InnerList.IsSynchronized;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

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

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddFirst(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddFirst(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddLast(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddLast(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void Remove(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void RemoveFirst() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void RemoveLast() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

    }

}

namespace WinCopies.Collections
{

    [Obsolete("Please use the WinCopies.Collections.DotNetFix.LinkedList<T> instead.")]
    [DebuggerDisplay("Count = {Count}")]
    public class LinkedList<T> : ILinkedList<T>
    {

        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedList(System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => false;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        void ICollection<T>.Add(T item) => ((ICollection<T>)InnerList).Add(item);

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        public LinkedListNode<T> AddFirst(T value) => InnerList.AddFirst(value);

        public void AddFirst(LinkedListNode<T> node) => InnerList.AddFirst(node);

        public LinkedListNode<T> AddLast(T value) => InnerList.AddLast(value);

        public void AddLast(LinkedListNode<T> node) => InnerList.AddLast(node);

        public void Clear() => InnerList.Clear();

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        public bool Remove(T value) => InnerList.Remove(value);

        public void Remove(LinkedListNode<T> node) => InnerList.Remove(node);

        public void RemoveFirst() => InnerList.RemoveFirst();

        public void RemoveLast() => InnerList.RemoveLast();

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

    }

    [Obsolete("Please use the WinCopies.Collections.DotNetFix.ReadOnlyLinkedList<T> instead.")]
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyLinkedList<T> : ILinkedList<T>

    {

        protected ILinkedList<T> InnerList { get; }

        public LinkedListNode<T> Last => InnerList.Last;

        public LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

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

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddFirst(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddFirst(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public LinkedListNode<T> AddLast(T value) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddLast(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void Remove(LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void RemoveFirst() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void RemoveLast() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

    }

}
