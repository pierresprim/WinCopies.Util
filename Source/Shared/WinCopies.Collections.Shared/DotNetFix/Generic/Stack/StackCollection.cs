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

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif
        [Serializable]
        public class StackCollection
                <
#if WinCopies3
        TStack, TItems
#else
        T
#endif
       > :
#if WinCopies3
        IStack<TItems>, IUIntCountable
#else
        IEnumerableStack<T>, System.Collections.Generic.IReadOnlyCollection<
#if WinCopies3
TItems
#else
            T
#endif
            >, ICollection
#endif
#if WinCopies3
            where TStack : IStack<TItems>
#endif
        {
            protected internal
#if WinCopies3
            TStack
#else
            System.Collections.Generic.Stack<T>
#endif
            InnerStack
            { get; }

            public
#if !WinCopies3
int
#else
                uint
#endif
                Count => InnerStack.Count;

            public bool IsReadOnly => false;

#if WinCopies3
            bool ISimpleLinkedListBase2.IsSynchronized => InnerStack.IsSynchronized;

            object ISimpleLinkedListBase2.SyncRoot => InnerStack.SyncRoot;

            bool ISimpleLinkedListBase.HasItems => InnerStack.HasItems;
#else
            bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;

            public StackCollection() : this(new
#if !WinCopies3
            System.Collections.Generic.Stack
#else
            EnumerableStack
#endif
            <T>())
            { }
#endif

            public StackCollection(in
#if WinCopies3
                TStack
#else
            System.Collections.Generic.Stack<T>
#endif
     stack) => InnerStack = stack;

            protected virtual void ClearItems() => InnerStack.Clear();

            public void Clear() => ClearItems();

            public
#if WinCopies3
TItems
#else
            T
#endif
            Peek() => InnerStack.Peek();

            protected virtual
#if WinCopies3
TItems
#else
            T
#endif
            PopItem() => InnerStack.Pop();

            public
#if WinCopies3
TItems
#else
            T
#endif
            Pop() => PopItem();

            protected virtual void PushItem(
#if WinCopies3
TItems
#else
            T
#endif
            item) => InnerStack.Push(item);

            public void Push(
#if WinCopies3
TItems
#else
            T
#endif
            item) => PushItem(item);

#if !WinCopies3
        public void Contains(T item) => InnerStack.Contains(item);

        public T[] ToArray() => InnerStack.ToArray();

        public void TrimExcess() => InnerStack.TrimExcess();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => InnerStack.GetEnumerator();
#endif

            public bool TryPeek(out
#if WinCopies3
TItems
#else
            T
#endif
            result)
#if CS8
            => InnerStack.TryPeek(out result);
#else
            {
                if (Count == 0)
                {
                    result = default;

                    return false;
                }

                result = InnerStack.Peek();

                return true;
            }
#endif

            protected virtual bool TryPopItem(out
#if WinCopies3
TItems
#else
            T
#endif
            result)
#if CS8
            => InnerStack.TryPop(out result);
#else
            {
                if (IsReadOnly || Count == 0)
                {
                    result = default;

                    return false;
                }

                result = InnerStack.Pop();

                return true;
            }
#endif

            public bool TryPop(out
#if WinCopies3
TItems
#else
            T
#endif
            result) => TryPopItem(out result);

#if WinCopies3 && !CS8
            bool ISimpleLinkedList.TryPeek(out object result) => InnerStack.TryPeek(out result);

            object ISimpleLinkedList.Peek() => ((ISimpleLinkedList)InnerStack).Peek();
#endif
        }

#if WinCopies3
        public class StackCollection<T> : StackCollection<IStack<T>, T>
        {
            public StackCollection() : this(new Stack<T>())
            {
                // Left empty.
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="StackCollection{T}"/> class with a custom Stack.
            /// </summary>
            /// <param name="stack">The inner stack for this <see cref="StackCollection{T}"/>.</param>
            public StackCollection(in IStack<T> stack) : base(stack)
            {
                // Left empty.
            }
        }
    }
#endif
}

#endif
