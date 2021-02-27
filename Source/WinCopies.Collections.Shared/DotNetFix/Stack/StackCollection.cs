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

namespace WinCopies.Collections.DotNetFix
{

    [Serializable]
    public class StackCollection : IEnumerableStack, ICollection
#if !WinCopies3
        , ICloneable
#endif
    {
        protected internal
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

#if !WinCopies3
        uint IUIntCountable.Count => (uint)Count;

        uint IUIntCountableEnumerable.Count => (uint)Count;
#else
        int ICollection.Count => (int)Count;
#endif

        public bool HasItems => Count != 0;

        public bool IsReadOnly => false;

        public bool IsSynchronized => InnerStack.IsSynchronized;

        public object SyncRoot => InnerStack.SyncRoot;

        public StackCollection() : this(new
#if !WinCopies3
            System.Collections.Stack
#else
            EnumerableStack
#endif
            ())
        { }

        public StackCollection(in
#if !WinCopies3
            System.Collections.Stack
#else
            IEnumerableStack
#endif
            stack) => InnerStack = stack;

        protected virtual void ClearItems() => InnerStack.Clear();

        public void Clear() => ClearItems();

        public void Contains(object item) => InnerStack.Contains(item);

        public object Peek() => InnerStack.Peek();

        protected virtual object PopItem() => InnerStack.Pop();

        public object Pop() => PopItem();

        protected virtual void PushItem(object item) => InnerStack.Push(item);

        public void Push(object item) => PushItem(item);

        public object[] ToArray() => InnerStack.ToArray();

        public void CopyTo(Array array, int index) => InnerStack.CopyTo(array, index);

        public System.Collections.IEnumerator GetEnumerator() => InnerStack.GetEnumerator();

#if !WinCopies3
        public object Clone() => InnerStack.Clone();
#else
        public bool TryPeek(out object result) => InnerStack.TryPeek(out result);

        public bool TryPop(out object result) => InnerStack.TryPop(out result);
#endif
    }
}
