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
namespace WinCopies.Collections.DotNetFix
{
    public interface IReadOnlyLinkedListNode
    {
        object Value { get; }

        IReadOnlyLinkedListNode Previous { get; }

        IReadOnlyLinkedListNode Next { get; }
    }

#if WinCopies3
    namespace Generic
    {
#endif
        public interface IReadOnlyLinkedListNodeBase<out T> : IReadOnlyLinkedListNode
        {
            T Value { get; }

#if CS8
            object IReadOnlyLinkedListNode.Value => Value;
#endif
        }

        public interface IReadOnlyLinkedListNode<out TItems, out TList> : IReadOnlyLinkedListNodeBase<TItems> where TList : IReadOnlyLinkedList<TItems>
        {
            TList List { get; }
        }

        public interface IReadOnlyLinkedListNode<out TItems, out TNodes, out TList> : IReadOnlyLinkedListNode<TItems, TList> where TList : IReadOnlyLinkedList<TItems> where TNodes : IReadOnlyLinkedListNode<TItems, TList>
        {
            TNodes Previous { get; }

            TNodes Next { get; }

#if CS8
            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Previous => Previous;

            IReadOnlyLinkedListNode IReadOnlyLinkedListNode.Next => Next;
#endif
        }

        public interface IReadOnlyLinkedListNode<T> : IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>
        {
#if CS8
            IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Previous => Previous;

            IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>> IReadOnlyLinkedListNode<T, IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>, IReadOnlyLinkedList<T>>.Next => Next;

            IReadOnlyLinkedList<T> IReadOnlyLinkedListNode<T, IReadOnlyLinkedList<T>>.List => List;
#else
            // Left empty.
#endif
        }

        public interface
#if WinCopies3
            ILinkedListNodeBase<T> : IReadOnlyLinkedListNodeBase<T>
        {
            bool IsReadOnly { get; }

            T Value { get; set; }
        }

        public interface ILinkedListNode<TItems, out TList> : ILinkedListNodeBase<TItems>, IReadOnlyLinkedListNode<TItems, TList> where TList : ILinkedList<TItems>
        {
            // Left empty.
        }

        public interface ILinkedListNode<TItems, out TNodes, out TList> : ILinkedListNode<TItems, TList>, IReadOnlyLinkedListNode<TItems, TNodes, TList> where TList : ILinkedList<TItems> where TNodes : ILinkedListNode<TItems, TList>
        {
#if !WinCopies3
            TNodes Previous { get; }

            TNodes Next { get; }
#endif

#if CS8
            TNodes IReadOnlyLinkedListNode<TItems, TNodes, TList>.Previous => Previous;

            TNodes IReadOnlyLinkedListNode<TItems, TNodes, TList>.Next => Next;
#endif
        }

        public interface ILinkedListNode<T> : ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>, IReadOnlyLinkedListNode<T>
        {
            new ILinkedList<T> List { get; }

            new ILinkedListNode<T> Previous { get; }

            new ILinkedListNode<T> Next { get; }

#if CS8
            ILinkedList<T> IReadOnlyLinkedListNode<T, ILinkedList<T>>.List => List;

#if !WinCopies3
            ILinkedListNode<T, ILinkedList<T>> ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Previous => Previous;

            ILinkedListNode<T, ILinkedList<T>> ILinkedListNode<T, ILinkedListNode<T, ILinkedList<T>>, ILinkedList<T>>.Next => Next;
#endif
#endif
        }
#else
            ILinkedListNode<T>
        {
            bool IsReadOnly { get; }

            ILinkedList<T> List { get; }

            ILinkedListNode<T> Previous { get; }

            ILinkedListNode<T> Next { get; }

            T Value { get; }
#endif
    }
}
#endif
