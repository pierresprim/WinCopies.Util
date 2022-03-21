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

using System;
using System.Collections;

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    [Serializable]
    public class ReadOnlyStackCollection : IEnumerableStack, ICollection
#if !WinCopies3
        , ICloneable
#endif
    {
        protected
#if !WinCopies3
            System.Collections.Stack
#else
IEnumerableStack
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

        public bool HasItems => Count !=
#if WinCopies3
            0u
#else
            0
#endif
            ;

#if !WinCopies3
        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;
#else
        int ICollection.Count => (int)Count;
#endif

        public bool IsReadOnly => true;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public ReadOnlyStackCollection(in
#if !WinCopies3
            System.Collections.Stack
#else
            IEnumerableStack
#endif
            stack) => InnerStack = stack;

        public ReadOnlyStackCollection(in StackCollection stackCollection) : this(stackCollection.InnerStack) { }

#if !WinCopies3
        public object Clone() => InnerStack.Clone();
#endif

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public System.Collections.IEnumerator GetEnumerator() => InnerStack.GetEnumerator();

        void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

        object IStack.Pop() => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        bool ISimpleLinkedList.TryPeek(out object result) => InnerStack.TryPeek(out result);

        bool IStack.TryPop(out object result)
        {
            result = null;

            return false;
        }

        void ISimpleLinkedListBase2.Clear() => throw GetReadOnlyListOrCollectionException();
#endif
    }
}
