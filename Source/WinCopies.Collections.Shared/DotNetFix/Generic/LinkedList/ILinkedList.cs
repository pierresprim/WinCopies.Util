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

#if WinCopies2
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
         IReadOnlyLinkedList2<T>,   IUIntCountableEnumerable<T>, Collections.Generic.IEnumerable<T> /* In order to have the GetReversedEnumerator() method. */, Collections.Generic.IEnumerable<ILinkedListNode<T>>, 
#else
            ISerializable, IDeserializationCallback, System.Collections.Generic.IEnumerable<T>,
#endif
            ICollection<T>, ICollection, IReadOnlyCollection<T>
    {
#if WinCopies2
        System.Collections.Generic.LinkedListNode<T> Last{ get; }
#endif

#if WinCopies2
        System.Collections.Generic.LinkedListNode<T> First { get; }
#endif

#if WinCopies2
           new     int  Count { get; }
#endif
              
       

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
                <T> node, T value);

#if WinCopies2
        void AddAfter(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
#endif

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
                <T> node, T value);

#if WinCopies2
        void AddBefore(System.Collections.Generic.LinkedListNode<T> node, System.Collections.Generic.LinkedListNode<T> newNode);
#endif


#if WinCopies2
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> AddFirst(T value);

#if WinCopies2
        void AddFirst(System.Collections.Generic.LinkedListNode<T> node);
#endif


#if WinCopies2
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> AddLast(T value);

#if WinCopies2
        void AddLast(System.Collections.Generic.LinkedListNode<T> node);
#endif


#if WinCopies2
        System.Collections.Generic.LinkedListNode  <T> Find(T value);
#endif

#if WinCopies2
        System.Collections.Generic.LinkedListNode   <T> FindLast(T value);
#endif

        //todo: to remove

#if WinCopies2
        new System.Collections.Generic.LinkedList<T>.Enumerator    GetEnumerator();
#endif
            


        void Remove(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node);

        void RemoveFirst();

        void RemoveLast();

#if WinCopies3
        new System.Collections.Generic.IEnumerator<T> GetEnumerator();

        new System.Collections.Generic.IEnumerator<T> GetReversedEnumerator();

        new bool SupportsReversedEnumeration { get; } 
#endif
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
        <T>, IUIntCountableEnumerable<ILinkedListNode<T>>
    {
        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection);

        new ILinkedListNode<T> Remove(T item);

        bool MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after);

        bool MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before);

        void Swap(ILinkedListNode<T> x, ILinkedListNode<T> y);
    }
}

#endif
