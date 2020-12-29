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

#if WinCopies2
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
        ILinkedList3
#else
IReadOnlyLinkedList<T>, ILinkedList
#endif
        <T>
    {
#if !WinCopies3
        public bool IsReadOnly => true;
#endif

        protected ILinkedList<T> InnerList { get; } // Already was ILinkedList<T> in WinCopies 2.

#if WinCopies3
        public T LastValue => InnerList.LastValue;

        ILinkedListNode<T> IReadOnlyLinkedList<T>.Last => throw GetReadOnlyListOrCollectionException();
#else
        public System.Collections.Generic.LinkedListNode <T> Last => InnerList.Last;
#endif

#if WinCopies3
        public T FirstValue => InnerList.FirstValue;

        ILinkedListNode<T> IReadOnlyLinkedList<T>.First => throw GetReadOnlyListOrCollectionException();
#else
        public System.Collections.Generic.LinkedListNode<T> First => InnerList.First;
#endif

        public
#if WinCopies2
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

        bool ILinkedList2<T>.IsReadOnly => true;

        public bool Contains(T value) => InnerList.Contains(value);

        public void CopyTo(T[] array, int index) => InnerList.CopyTo(array, index);

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

        public
#if !WinCopies3
         System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();  
#endif

            System.Collections.Generic.IEnumerator<T>
#if !WinCopies3
System.Collections.Generic.IEnumerable<T>.
#endif
                GetEnumerator() => InnerList.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() =>
#if WinCopies2
InnerList
#else
                ((IEnumerable)InnerList)
#endif
                .GetEnumerator();

#if WinCopies3
        public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator() => throw GetReadOnlyListOrCollectionException();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> System.Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetEnumerator() => throw GetReadOnlyListOrCollectionException();

        public System.Collections.Generic.IEnumerator<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>>.GetReversedEnumerator() => throw GetReadOnlyListOrCollectionException();

        // Not available because the GetNodeEnumerator() is now in ILinkedList3<T> for better compatibility.

        //public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection) => InnerList.GetNodeEnumerator(enumerationDirection);
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
System.Collections.Generic.LinkedListNode<T>                
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

        System.Collections.Generic.IEnumerator<T> IReadOnlyLinkedList2<T>.GetEnumerator(EnumerationDirection enumerationDirection) => InnerList.GetEnumerator(enumerationDirection);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator(EnumerationDirection enumerationDirection) => throw GetReadOnlyListOrCollectionException();

        bool ILinkedList3<T>.MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after) => throw GetReadOnlyListOrCollectionException();

        bool ILinkedList3<T>.MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before) => throw GetReadOnlyListOrCollectionException();

        void ILinkedList3<T>.Swap(ILinkedListNode<T> x, ILinkedListNode<T> y) => throw GetReadOnlyListOrCollectionException();
#endif
    }
}

#endif
