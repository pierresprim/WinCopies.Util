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

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif

    [Serializable]
    public class StackCollection<T> : IEnumerableStack<T>, IReadOnlyCollection<T>, ICollection
    {
        protected internal
#if !WinCopies3
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> InnerStack
        { get; }

        public
#if !WinCopies3
int
#else
                uint
#endif
                Count => InnerStack.Count;

#if WinCopies3
            int ICollection.Count => (int)Count;

            int IReadOnlyCollection<T>.Count => (int)Count;
#endif

        public bool IsReadOnly => false;

        bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

#if WinCopies3
            bool ISimpleLinkedListBase2.IsSynchronized => InnerStack.IsSynchronized;

            object ISimpleLinkedListBase2.SyncRoot => InnerStack.SyncRoot;

            bool ISimpleLinkedListBase.HasItems => InnerStack.HasItems;
#else
        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;
#endif

        public StackCollection() : this(new
#if !WinCopies3
            System.Collections.Generic.Stack
#else
            EnumerableStack
#endif
            <T>())
        { }

        public StackCollection(in
#if !WinCopies3
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) => InnerStack = stack;

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(T item) => InnerStack.Contains(item);

        public T Peek() => InnerStack.Peek();

        protected virtual T PopItem() => InnerStack.Pop();

        public T Pop() => PopItem();

        protected virtual void PushItem(T item) => InnerStack.Push(item);

        public void Push(T item) => PushItem(item);

        public T[] ToArray() => InnerStack.ToArray();

#if !WinCopies3
        public void TrimExcess() => InnerStack.TrimExcess();
#endif

        public bool TryPeek(out T result)
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

        protected virtual bool TryPopItem(out T result)
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

        public bool TryPop(out T result) => TryPopItem(out result);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

        public void CopyTo(T[] array, int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

        public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

        System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
    }
#if WinCopies3
    }
#endif
}

#endif
