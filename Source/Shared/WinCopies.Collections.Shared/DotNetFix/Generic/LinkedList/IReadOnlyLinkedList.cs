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
using System.Collections;
using System.Collections.Generic;

using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IReadOnlyLinkedListCore<T>
    {
        bool HasItems { get; }

        T FirstValue { get; }
        T LastValue { get; }
    }

    public interface IReadOnlyLinkedListBase<T> : IReadOnlyLinkedListCore<T>, System.Collections.Generic.ICollection<T>, ICollection, System.Collections.Generic.IReadOnlyCollection<T>, IReadOnlyUIntCollection<T>, IUIntCountable
    {
        // Left empty.
    }

    public interface IReadOnlyLinkedListBase2<T>
    {
        T
#if CS9
            ?
#endif
            First
        { get; }
        T
#if CS9
            ?
#endif
            Last
        { get; }
    }

    public interface IReadOnlyLinkedListBase<TValue, TNode> : IReadOnlyLinkedListBase<TValue>, IReadOnlyLinkedListBase2<TNode> where TNode : IReadOnlyLinkedListNode<TValue>
    {
        TNode
#if CS9
            ?
#endif
            Find(TValue value);
        TNode
#if CS9
            ?
#endif
            FindLast(TValue value);
    }

    public interface IReadOnlyLinkedList<TValue, TNode> : IReadOnlyLinkedListBase<TValue, TNode>, Collections.Extensions.Generic.IEnumerable<TValue, IUIntCountableEnumeratorInfo<TValue>> /* In order to have the GetReversedEnumerator() method through the Collections.Generic.IEnumerable<T> interface. */ where TNode : IReadOnlyLinkedListNode<TValue>
    {
#if !WinCopies3
        new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
#else
        // Not available because the GetNodeEnumerator() is now in ILinkedList3<T> for better compatibility.

        //System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection);
#endif
    }

    public interface IReadOnlyLinkedList<T> : IReadOnlyLinkedList<T, IReadOnlyLinkedListNode<T>>
    {
        // Left empty.
    }

    public interface IReadOnlyEnumerableInfoLinkedList<TValue, TNode> : IReadOnlyLinkedList<TValue, TNode>, IUIntCountableEnumerableInfo<TValue> where TNode : IReadOnlyLinkedListNode<TValue>
    {
        // Left empty.
    }

    public interface IReadOnlyEnumerableInfoLinkedList<T> : IReadOnlyEnumerableInfoLinkedList<T, IReadOnlyLinkedListNode<T>>, IReadOnlyLinkedList<T>
    {
        // Left empty.
    }
}
#endif
