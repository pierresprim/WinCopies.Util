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
    public class ReadOnlyLinkedList<T> : IReadOnlyLinkedList<T>, ILinkedList2<T>
    {
        public bool IsReadOnly => true;

        protected ILinkedList<T> InnerList { get; } // Already was ILinkedList<T> in WinCopies 2.

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

#if WinCopies3
        int ICollection.Count => (int)Count;

        int ICollection<T>.Count => (int)Count;

        int IReadOnlyCollection<T>.Count => (int)Count;
#endif

        bool ICollection<T>.IsReadOnly => true;

        object ICollection.SyncRoot => InnerList.SyncRoot;

        bool ICollection.IsSynchronized => InnerList.IsSynchronized;

        public bool SupportsReversedEnumeration => true;



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

#if WinCopies2
        public System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator() => InnerList.GetEnumerator();
#else
        public
#endif

    System.Collections.Generic.IEnumerator<T>
#if WinCopies2
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

#if WinCopies2
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public virtual void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);
#else
        public System.Collections.Generic.IEnumerator<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

        // Not available because the GetNodeEnumerator() is now in ILinkedList3<T> for better compatibility.

        //public System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection) => InnerList.GetNodeEnumerator(enumerationDirection);
#endif

        void ICollection<T>.Add(T item) => throw GetReadOnlyListOrCollectionException();

        void ICollection<T>.Clear() => throw GetReadOnlyListOrCollectionException();

        bool ICollection<T>.Contains(T item) => InnerList.Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

        bool ICollection<T>.Remove(T item) => throw GetReadOnlyListOrCollectionException
            ();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

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
                <T> node, T value) => throw GetReadOnlyListOrCollectionException();

#if WinCopies2
        public void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddFirst(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);

        public void AddLast(System.Collections.Generic.LinkedListNode<T> node) => throw new InvalidOperationException(ExceptionMessages.ReadOnlyCollection);
#endif

        public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddBefore(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node, T value) => throw GetReadOnlyListOrCollectionException();

        public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddFirst(T value) => throw GetReadOnlyListOrCollectionException();

        public
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> AddLast(T value) => throw GetReadOnlyListOrCollectionException();

        public void Remove(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
                ILinkedListNode
#endif
                <T> node) => throw GetReadOnlyListOrCollectionException();

        public void RemoveFirst() => throw GetReadOnlyListOrCollectionException();

        public void RemoveLast() => throw GetReadOnlyListOrCollectionException();
    }
}

#endif
