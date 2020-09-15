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

#if WinCopies2
using WinCopies.Collections.DotNetFix;
using WinCopies.Util.Resources;
#else
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Resources;
#endif

namespace WinCopies.Collections
{
#if WinCopies2
    [Obsolete("Use the WinCopies.Collections.DotNetFix.ILinkedListNode<T> (WinCopies.Collections.DotNetFix.Generic.ILinkedListNode<T> in WinCopies 3) interface instead.")]
    public interface ILinkedListNode<T>
    {
        ILinkedList<T> List { get; }

        ILinkedListNode<T> Next { get; }

        ILinkedListNode<T> Previous { get; }

        T Value { get; }
    }

    [DebuggerDisplay("Count = {Count}")]
    public class LinkedList<T> : ILinkedList<T>
    {
        protected System.Collections.Generic.LinkedList<T> InnerList { get; }

        public LinkedList(System.Collections.Generic.LinkedList<T> list) => InnerList = list;

        public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

        public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => false;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        void ICollection<T>.Add(T item) => ((ICollection<T>)InnerList).Add(item);

        public System.Collections.Generic.LinkedListNode<T> AddAfter(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddAfter(node, value);

        public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddAfter(node, newNode);

        public System.Collections.Generic.LinkedListNode<T> AddBefore(System.Collections.Generic.LinkedListNode<T> node, T value) => InnerList.AddBefore(node, value);

        public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => InnerList.AddBefore(node, newNode);

        public System.Collections.Generic.LinkedListNode<T> AddFirst(T value) => InnerList.AddFirst(value);

        public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddFirst(node);

        public System.Collections.Generic.LinkedListNode<T> AddLast(T value) => InnerList.AddLast(value);

        public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => InnerList.AddLast(node);

        public void Clear() => InnerList.Clear();

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)InnerList).GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        public bool Remove(T value) => InnerList.Remove(value);

        public void Remove(System.Collections.Generic.LinkedListNode<T> node) => InnerList.Remove(node);

        public void RemoveFirst() => InnerList.RemoveFirst();

        public void RemoveLast() => InnerList.RemoveLast();

        public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);
    }
#endif

#if WinCopies2
    [Obsolete("Please use the WinCopies.Collections.DotNetFix.ReadOnlyLinkedList<T> instead.")]
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyLinkedList<T> : ILinkedList<T>
    {
        protected ILinkedList<T> InnerList { get; }

        public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;

        public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;

        public int Count => InnerList.Count;

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);

        public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        void ICollection<T>.Add(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        void ICollection<T>.Clear() => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool ICollection<T>.Remove(T item) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetEnumerator() => InnerList.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

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
#endif
}
