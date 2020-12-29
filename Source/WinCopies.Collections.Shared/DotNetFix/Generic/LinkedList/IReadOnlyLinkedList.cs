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

#if WinCopies2
using System.Runtime.Serialization;
#endif

namespace WinCopies.Collections.DotNetFix
#if WinCopies3
.Generic
#endif
{
    public interface IReadOnlyLinkedList<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>,
#if WinCopies2
            ICountableEnumerable<T>, ISerializable, IDeserializationCallback
#else
            IUIntCountableEnumerable<T>, Collections.Generic.IEnumerable<T>
#endif
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
                <T> Find(T value);


#if WinCopies2
        System.Collections.Generic.LinkedListNode
#else
            ILinkedListNode
#endif
                <T> FindLast(T value);

#if WinCopies2
        new System.Collections.Generic.LinkedList<T>.Enumerator GetEnumerator();
#else
        // Not available because the GetNodeEnumerator() is now in ILinkedList3<T> for better compatibility.

        //System.Collections.Generic.IEnumerator<ILinkedListNode<T>> GetNodeEnumerator(EnumerationDirection enumerationDirection);
#endif
    }

    public interface IReadOnlyLinkedList2<T>:IReadOnlyLinkedList<T>
    {
        T FirstValue { get; }

        T LastValue { get; }

        System.Collections.Generic.IEnumerator<T> GetEnumerator(EnumerationDirection enumerationDirection);
    }
}
#endif
