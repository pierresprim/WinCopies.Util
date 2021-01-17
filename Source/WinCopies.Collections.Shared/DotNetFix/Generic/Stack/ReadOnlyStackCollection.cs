﻿/* Copyright © Pierre Sprimont, 2020
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

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    namespace Generic
    {
#endif

        [Serializable]
        public class ReadOnlyStackCollection<T> : System.Collections.Generic.IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
        {
            protected
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

            public bool IsReadOnly => true;

            bool ICollection.IsSynchronized => ((ICollection)InnerStack).IsSynchronized;

            object ICollection.SyncRoot => ((ICollection)InnerStack).SyncRoot;

            public ReadOnlyStackCollection(in
#if !WinCopies3
            System.Collections.Generic.Stack
#else
            IEnumerableStack
#endif
            <T> stack) => InnerStack = stack;

            public ReadOnlyStackCollection(in StackCollection<T> stackCollection) : this(stackCollection.InnerStack) { }

            public void Contains(T item) => InnerStack.Contains(item);

            public T Peek() => InnerStack.Peek();

            public T[] ToArray() => InnerStack.ToArray();

#if NETCORE

            public bool TryPeek(out T result) => InnerStack.TryPeek(out result);

#endif

            void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerStack).CopyTo(array, index);

            public void CopyTo(in T[] array, in int arrayIndex) => InnerStack.CopyTo(array, arrayIndex);

            public System.Collections.Generic.IEnumerator<T> GetEnumerator() => InnerStack.GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)InnerStack).GetEnumerator();
        }
#if WinCopies3
    }
#endif
}

#endif
