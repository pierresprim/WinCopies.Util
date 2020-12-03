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

using System.Collections;
using System.Collections.Generic;

#if WinCopies2
using System.Runtime.Serialization;
#endif

namespace WinCopies.Collections.DotNetFix
#if !WinCopies2
.Generic
#endif
{
    public interface ILinkedList<T> :
            // TODO:
#if WinCopies2
            ISerializable, IDeserializationCallback, System.Collections.Generic.IEnumerable<T>,
#else
            IUIntCountableEnumerable<T>, Collections.Generic.IEnumerable<T> /* In order to have the GetReversedEnumerator() method. */,
#endif
            ICollection<T>, ICollection, IReadOnlyCollection<T>
    {
#if WinCopies2
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> Last
        { get; }

#if WinCopies2
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> First
        { get; }

        new
#if WinCopies2
                int
#else
                uint
#endif
                Count
        { get; }

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
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> Find(T value);


#if WinCopies2
        System.Collections.Generic.LinkedListNode
#else
        ILinkedListNode
#endif
                <T> FindLast(T value);

        //todo: to remove

        new
#if WinCopies2
System.Collections.Generic.LinkedList<T>.Enumerator
#else
              System.Collections.Generic.IEnumerator<T>
#endif
                GetEnumerator();

        void Remove(
#if WinCopies2
System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> node);

        void RemoveFirst();

        void RemoveLast();
    }

    public interface ILinkedList2<T> : ILinkedList<T>
    {
        bool IsReadOnly { get; }
    }

#if !WinCopies2
    public interface ILinkedList3<T> : ILinkedList2<T>, IUIntCountableEnumerable<ILinkedListNode<T>>
    {
        System.Collections.Generic.IEnumerator<T> GetEnumerator(EnumerationDirection enumerationDirection);

        System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection);

        ILinkedListNode<T> Remove(T item);

        bool MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after);

        bool MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before);

        void Swap(ILinkedListNode<T> x, ILinkedListNode<T> y);
    }
#endif
}
