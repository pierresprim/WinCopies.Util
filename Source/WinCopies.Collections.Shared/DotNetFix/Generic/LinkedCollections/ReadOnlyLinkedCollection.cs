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

using static WinCopies
#if !WinCopies3
    .Util.Util;

using System.Runtime.Serialization;

using WinCopies.Collections.Generic;
using WinCopies.Util;
#else
    .ThrowHelper;
#endif

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        [Serializable]
        public class ReadOnlyLinkedCollection<T> : ICollection<T>, System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
#if WinCopies3
, IReadOnlyLinkedList2<T>
#else
, IDeserializationCallback, ISerializable
#endif
        {
            protected
#if !WinCopies3
                System.Collections.Generic.LinkedList
#else
                ILinkedList
#endif
                <T> InnerList
            { get; }

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

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

#if WinCopies3
            public T FirstValue => InnerList.FirstValue;

            public T LastValue => InnerList.LastValue;

            public bool SupportsReversedEnumeration => InnerList.SupportsReversedEnumeration;

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.First => throw GetReadOnlyListOrCollectionException();

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Last => throw GetReadOnlyListOrCollectionException();
#endif

            bool ICollection<T>.IsReadOnly => true;

            public ReadOnlyLinkedCollection(in
#if !WinCopies3
                System.Collections.Generic.LinkedList
#else
                ILinkedList
#endif
                <T> list) => InnerList = list;

#if WinCopies3
            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.Find(T value) => throw GetReadOnlyListOrCollectionException();

            IReadOnlyLinkedListNode<T> IReadOnlyLinkedList<T>.FindLast(T value) => throw GetReadOnlyListOrCollectionException();
#else
        public ReadOnlyLinkedCollection(in LinkedCollection<T> listCollection) : this(listCollection.InnerList) { /* Left empty. */ }

        public System.Collections.Generic.LinkedListNode<T> Find(T value) => InnerList.Find(value);

        public System.Collections.Generic.LinkedListNode<T> FindLast(T value) => InnerList.FindLast(value);
#endif

            public bool Contains(T item) => InnerList.Contains(item);

            public void CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

            public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerList).GetEnumerator();

#if !WinCopies3
        public void GetObjectData(SerializationInfo info, StreamingContext context) => InnerList.GetObjectData(info, context);

        public void OnDeserialization(object sender) => InnerList.OnDeserialization(sender);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
#endif

            void ICollection<T>.Add(T item) => throw GetReadOnlyListOrCollectionException();

            void ICollection<T>.Clear() => throw GetReadOnlyListOrCollectionException();

            bool ICollection<T>.Remove(T item) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
            public IUIntCountableEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();

            public IUIntCountableEnumerator<T> GetReversedEnumerator() => InnerList.GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => ((Collections.Generic.IEnumerable<T>)InnerList).GetReversedEnumerator();

#if !CS8
            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

            System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
#endif
        }
#if WinCopies3
    }
#endif
}

#endif
