﻿/* Copyright © Pierre Sprimont, 2020
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

using static WinCopies.Collections.ThrowHelper;

#if !WinCopies3
using System.Runtime.Serialization;

using WinCopies.Util.Resources;
#endif

namespace WinCopies.Collections.DotNetFix
#if WinCopies3
    .Generic
#endif
{
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyLinkedList<T> :
#if WinCopies3
        ILinkedList3<T>, IReadOnlyLinkedList2
#else
ILinkedList2<T>, IReadOnlyLinkedList
#endif
        <T>
    {
#if !WinCopies3
        public bool IsReadOnly => true;
#endif

        protected
#if WinCopies3
            IReadOnlyLinkedList2
#else
ILinkedList
#endif
            <T> InnerList
        { get; } // Already was ILinkedList<T> in WinCopies 2.

#if WinCopies3
        public T FirstValue => InnerList.FirstValue;
#else
        public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;
#endif

#if WinCopies3
        public T LastValue => InnerList.LastValue;
#else
        public System.Collections.Generic.LinkedListNode<T> Last => InnerList.Last;
#endif

        public
#if !WinCopies3
                int
#else
                uint
#endif
                Count => InnerList.Count;

#if WinCopies3
        int ICollection.Count => (int)Count;

        int ICollection<T>.Count => (int)Count;

        int IReadOnlyCollection<T>.Count => (int)Count;
#endif

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => InnerList.SyncRoot;

        bool ICollection.IsSynchronized => InnerList.IsSynchronized;

        public bool SupportsReversedEnumeration => true;

#if WinCopies3
        bool ILinkedList2<T>.IsReadOnly => true;

        ILinkedListNode<T> ILinkedList<T>.First => throw GetReadOnlyListOrCollectionException();

        ILinkedListNode<T> ILinkedList<T>.Last => throw GetReadOnlyListOrCollectionException();

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => throw GetReadOnlyListOrCollectionException();

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => throw GetReadOnlyListOrCollectionException();
#endif

        public ReadOnlyLinkedList(
#if WinCopies3
            IReadOnlyLinkedList2
#else
            ILinkedList
#endif
            <T> list) => InnerList = list;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

        public
#if !WinCopies3
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> Find(T value) => throw GetReadOnlyListOrCollectionException();

        public
#if !WinCopies3
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> FindLast(T value) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        ILinkedListNode<T> ILinkedList<T>.Find(T value) => throw GetReadOnlyListOrCollectionException();

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => throw GetReadOnlyListOrCollectionException();

        IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => throw GetReadOnlyListOrCollectionException();
#endif

        public
#if !WinCopies3
         System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();
#endif


#if WinCopies3
            IUIntCountableEnumerator<T>
#else
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.
#endif
                GetEnumerator() => InnerList.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() =>
#if !WinCopies3
InnerList
#else
                ((IEnumerable)InnerList)
#endif
                .GetEnumerator();

#if WinCopies3
        public IUIntCountableEnumerator<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

        //System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator() => throw GetReadOnlyListOrCollectionException();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => throw GetReadOnlyListOrCollectionException();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => throw GetReadOnlyListOrCollectionException();

        IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => throw GetReadOnlyListOrCollectionException();

        IUIntCountableEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => throw GetReadOnlyListOrCollectionException();

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => throw GetReadOnlyListOrCollectionException();

        System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => throw GetReadOnlyListOrCollectionException();

#if !CS8
        System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
#else
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
#endif

        void ICollection<T>.Add(T item) => throw GetReadOnlyListOrCollectionException();

        void ICollection<T>.Clear() => throw GetReadOnlyListOrCollectionException();

        bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool ICollection<T>.Remove(T item) => throw GetReadOnlyListOrCollectionException
            ();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

#if WinCopies3
        ILinkedListNode<T> ILinkedList<T>.
#else
        public System.Collections.Generic.LinkedListNode<T>
#endif
                AddAfter(
#if WinCopies3
            ILinkedListNode
#else
System.Collections.Generic.LinkedListNode
#endif
                <T> node, T value) => throw GetReadOnlyListOrCollectionException();

#if !WinCopies3
        public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);
#endif

#if WinCopies3
        ILinkedListNode<T> ILinkedList<T>.
#else
        public System.Collections.Generic.LinkedListNode<T>
#endif
                 AddBefore(
#if WinCopies3
                ILinkedListNode
#else
System.Collections.Generic.LinkedListNode
#endif
                <T> node, T value) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        ILinkedListNode<T> ILinkedList<T>.
#else
        public System.Collections.Generic.LinkedListNode<T>
#endif
                 AddFirst(T value) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        ILinkedListNode<T> ILinkedList<T>.
#else
        public System.Collections.Generic.LinkedListNode<T>
#endif
                 AddLast(T value) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        void ILinkedList<T>.Remove(ILinkedListNode
#else
        public void Remove(System.Collections.Generic.LinkedListNode
#endif
                <T> node) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        void ILinkedList<T>.RemoveFirst()
#else
        public void RemoveFirst()
#endif
            => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        void ILinkedList<T>.RemoveLast()
#else
        public void RemoveLast()
#endif
            => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        ILinkedListNode<T> ILinkedList3<T>.Remove(T item) => throw GetReadOnlyListOrCollectionException();

        bool ILinkedList3<T>.MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => throw GetReadOnlyListOrCollectionException();

        bool ILinkedList3<T>.MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => throw GetReadOnlyListOrCollectionException();

        void ILinkedList3<T>.Swap(ILinkedListNode<T> x, ILinkedListNode<T> y) => throw GetReadOnlyListOrCollectionException();
#endif
    }
}

#endif
