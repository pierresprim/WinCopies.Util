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
        IEnumerableStack<T>, IReadOnlyCollection<
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

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
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

        public class EnumerableStackCollection<TStack, TItems> : IEnumerableStack<TItems>, IReadOnlyCollection<TItems>, ICollection where TStack : StackCollection<IEnumerableStack<TItems>, TItems>
        {
            protected TStack InnerStack { get; }

            bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

            int ICollection.Count => ((ICollection)InnerStack.InnerStack).Count;

            int IReadOnlyCollection<TItems>.Count => ((IReadOnlyCollection<TItems>)InnerStack.InnerStack).Count;

            object ISimpleLinkedListBase2.SyncRoot => ((ISimpleLinkedListBase2)InnerStack.InnerStack).SyncRoot;

            bool ISimpleLinkedListBase2.IsSynchronized => ((ISimpleLinkedListBase2)InnerStack.InnerStack).IsSynchronized;

            bool ISimpleLinkedListBase.IsReadOnly => InnerStack.IsReadOnly;

            bool ISimpleLinkedListBase.HasItems => InnerStack.InnerStack.HasItems;

            uint IUIntCountable.Count => InnerStack.Count;

            /// <summary>
            /// Initializes a new instance of the <see cref="StackCollection{T}"/> class with a custom Stack.
            /// </summary>
            /// <param name="Stack">The inner stack for this <see cref="StackCollection{T}"/>.</param>
            public EnumerableStackCollection(in TStack Stack) => InnerStack = Stack;

            /// <summary>
            /// Determines whether an element is in the <see cref="StackCollection{T}"/>.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="StackCollection{T}"/>. The value can be <see langword="null"/> for reference types.</param>
            /// <returns><see langword="true"/> if <paramref name="item"/> is found in the Stack; otherwise, <see langword="false"/>.</returns>
            public bool Contains(TItems item) => InnerStack.InnerStack.Contains(item);

            /// <summary>
            /// Copies the <see cref="StackCollection{T}"/> elements to a new array.
            /// </summary>
            /// <returns>A new array containing elements copied from the <see cref="StackCollection{T}"/>.</returns>
            public TItems[] ToArray() => InnerStack.InnerStack.ToArray();

            /// <summary>
            /// Returns an enumerator that iterates through the <see cref="StackCollection{T}"/>.
            /// </summary>
            /// <returns>An <see cref="System.Collections.Generic.IEnumerator{T}"/> for the <see cref="StackCollection{T}"/>.</returns>
            public
#if WinCopies3
               System.Collections.Generic.IEnumerator
#else
IUIntCountableEnumerator
#endif
                <TItems> GetEnumerator() => InnerStack.InnerStack.GetEnumerator();

            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => ((System.Collections.Generic.IEnumerable<TItems>)InnerStack.InnerStack).GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack.InnerStack).GetEnumerator();

            void ICollection.CopyTo(Array array, int index) => InnerStack.InnerStack.CopyTo(array, index);

            /// <summary>
            /// Copies the <see cref="StackCollection{T}"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="StackCollection{T}"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in array to start copying.</param>
            /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than zero.</exception>
            /// <exception cref="ArgumentException">The number of elements in the source <see cref="StackCollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
            public void CopyTo(TItems[] array, int arrayIndex) => InnerStack.InnerStack.CopyTo(array, arrayIndex);

            TItems IStack<TItems>.Peek() => InnerStack.Peek();

            bool IStack<TItems>.TryPeek(out TItems result) => InnerStack.TryPeek(out result);

            void IStack<TItems>.Clear() => InnerStack.Clear();

            void IStackBase<TItems>.Push(TItems item) => InnerStack.Push(item);

            TItems IStackBase<TItems>.Pop() => InnerStack.Pop();

            bool IStackBase<TItems>.TryPop(out TItems result) => InnerStack.TryPop(out result);

            TItems IStackBase<TItems>.Peek() => InnerStack.Peek();

            bool IStackBase<TItems>.TryPeek(out TItems result) => InnerStack.TryPeek(out result);

            void IStackBase<TItems>.Clear() => InnerStack.Clear();

            TItems ISimpleLinkedListBase<TItems>.Peek() => InnerStack.Peek();

            bool ISimpleLinkedListBase<TItems>.TryPeek(out TItems result) => InnerStack.TryPeek(out result);

            void ISimpleLinkedListBase2.Clear() => InnerStack.Clear();

#if WinCopies3
            TItems ISimpleLinkedList<TItems>.Peek() => ((ISimpleLinkedList<TItems>)InnerStack).Peek();
#if !CS8
            bool ISimpleLinkedList.TryPeek(out object result) => ((ISimpleLinkedList)InnerStack).TryPeek(out result);

            object ISimpleLinkedList.Peek() => ((ISimpleLinkedList)InnerStack).Peek();
#endif
#endif
        }

        public class EnumerableStackCollection<T> : EnumerableStackCollection<StackCollection<IEnumerableStack<T>, T>, T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StackCollection{T}"/> class with a custom stack.
            /// </summary>
            /// <param name="stack">The inner stack for this <see cref="StackCollection{T}"/>.</param>
            public EnumerableStackCollection(in StackCollection<IEnumerableStack<T>, T> stack) : base(stack) { }
        }
    }
#endif
}

#endif
