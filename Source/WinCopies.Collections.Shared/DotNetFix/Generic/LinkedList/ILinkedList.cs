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

using WinCopies.Collections.Generic;

#if !WinCopies3
using System.Runtime.Serialization;
#endif

namespace WinCopies.Collections.DotNetFix
#if WinCopies3
.Generic
#endif
{
    public interface ILinkedList<T> :
        // TODO:
#if WinCopies3
        IReadOnlyLinkedList2<T>, Collections.Generic.IEnumerable<ILinkedListNode<T>>,
#else
        ISerializable, IDeserializationCallback, System.Collections.Generic.IEnumerable<T>,
#endif
        ICollection<T>, ICollection
    {
#if WinCopies3
        new bool SupportsReversedEnumeration { get; }

        new System.Collections.Generic.IEnumerator<T> GetEnumerator();

        new System.Collections.Generic.IEnumerator<T> GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetReversedNodeEnumerator();
#else
        System.Collections.Generic.LinkedListNode<T> Last { get; }

        System.Collections.Generic.LinkedListNode<T> First { get; }

        new int Count { get; }

        void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
        
        void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
        
        void AddFirst(System.Collections.Generic.LinkedListNode<T> node);
        
        void AddLast(System.Collections.Generic.LinkedListNode<T> node);

        System.Collections.Generic.LinkedListNode<T> Find(T value);

        System.Collections.Generic.LinkedListNode<T> FindLast(T value);

        new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
#endif



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
                <T> node, T value);

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
                <T> node, T value);


#if !WinCopies3
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> AddFirst(T value);


#if !WinCopies3
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> AddLast(T value);



        void Remove(
#if !WinCopies3
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node);

        void RemoveFirst();

        void RemoveLast();
    }

    [Obsolete("The IsReadOnly property is available through the ICollection<T> interface.")]
    public interface ILinkedList2<T> : ILinkedList<T>
    {
        new bool IsReadOnly { get; }
    }

    public interface ILinkedList3<T> :
#if WinCopies4
        ILinkedList
#else
        ILinkedList2
#endif
        <T>
#if !WinCopies3
, IUIntCountableEnumerable<ILinkedListNode<T>>
#endif
    {
#if !WinCopies3
        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection);
#endif

        new ILinkedListNode<T> Remove(T item);

        bool MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after);

        bool MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before);

        void Swap(ILinkedListNode<T> x, ILinkedListNode<T> y);

#if WinCopies3 && CS8
        System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
    }

#if WinCopies3
    public interface IEnumerableInfoLinkedList<T> : ILinkedList3<T>, IReadOnlyEnumerableInfoLinkedList<T>, Collections.Generic.IEnumerableInfo<ILinkedListNode<T>>
    {
        new IEnumeratorInfo2<T> GetEnumerator();

        new IEnumeratorInfo2<T> GetReversedEnumerator();

        new IEnumeratorInfo2<ILinkedListNode<T>> GetNodeEnumerator();

        new IEnumeratorInfo2<ILinkedListNode<T>> GetReversedNodeEnumerator();

#if CS8
        System.Collections.Generic.IEnumerator<T> ILinkedList<T>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> ILinkedList<T>.GetReversedEnumerator() => GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetNodeEnumerator() => GetNodeEnumerator();

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> ILinkedList<T>.GetReversedNodeEnumerator() => GetReversedNodeEnumerator();

        IEnumeratorInfo2<ILinkedListNode<T>> IEnumerable<ILinkedListNode<T>, IEnumeratorInfo2<ILinkedListNode<T>>>.GetEnumerator() => GetNodeEnumerator();

        System.Collections.IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();

        IEnumeratorInfo2<ILinkedListNode<T>> Collections.Generic.IEnumerable<ILinkedListNode<T>, IEnumeratorInfo2<ILinkedListNode<T>>>.GetReversedEnumerator() => GetReversedNodeEnumerator();
#endif
    }
#endif
}
#endif
