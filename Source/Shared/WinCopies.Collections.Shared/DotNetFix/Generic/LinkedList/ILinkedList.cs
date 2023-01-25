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
using WinCopies.Util;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface ILinkedList<T> :
        // TODO:
        IReadOnlyLinkedList<T, ILinkedListNode<T>>, IUIntCountableEnumerableInfo<ILinkedListNode<T>>, IUIntCollection<T>, System.Collections.Generic.ICollection<T>, ICollection, IUIntCountableEnumerableInfo<T>, IQueueCommon<T>, IStackCommon<T>, IPeekable<T>, ISimpleLinkedListBase2
    {
        IQueue<T> AsQueue();
        IStack<T> AsStack();

        IUIntCountableEnumerable<ILinkedListNode<T>> AsNodeEnumerable();

        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetNodeEnumerator();
        IUIntCountableEnumeratorInfo<ILinkedListNode<T>> GetReversedNodeEnumerator();

        ILinkedListNode<T> AddAfter(ILinkedListNode<T> node, T value);
        ILinkedListNode<T> AddBefore(ILinkedListNode<T> node, T value);

        ILinkedListNode<T> AddFirst(T value);
        ILinkedListNode<T> AddLast(T value);



        void Remove(ILinkedListNode<T> node);

        void RemoveFirst();
        void RemoveLast();
#if CS8
        new IUIntCountableEnumeratorInfo<T> GetEnumerator() => this.AsFromType<Enumeration.IEnumerable<IUIntCountableEnumeratorInfo<T>>>().GetEnumerator();
        IUIntCountableEnumerator<T> Enumeration.IEnumerable<IUIntCountableEnumerator<T>>.GetEnumerator() => GetEnumerator();
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
        System.Collections.IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        System.Collections.IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => this.AsFromType<Extensions.Generic.IEnumerable<T>>().GetReversedEnumerator();

        void ICollectionBase<T>.Add(T item) => AddLast(item);
        bool ICollectionBase<T>.Remove(T item) => this.AsFromType<System.Collections.Generic.ICollection<T>>().Remove(item) != null;
#endif
    }

    public interface ILinkedList3<T> : ILinkedList<T>
    {
        bool MoveAfter(ILinkedListNode<T> node, ILinkedListNode<T> after);
        bool MoveBefore(ILinkedListNode<T> node, ILinkedListNode<T> before);

        new ILinkedListNode<T>
#if CS8
            ?
#endif
            Remove(T item);

        void Swap(ILinkedListNode<T> x, ILinkedListNode<T> y);
    }

    public interface IEnumerableInfoLinkedList<T> : ILinkedList3<T>, IReadOnlyEnumerableInfoLinkedList<T, ILinkedListNode<T>>
    {
        // Left empty.
    }
}
#endif
