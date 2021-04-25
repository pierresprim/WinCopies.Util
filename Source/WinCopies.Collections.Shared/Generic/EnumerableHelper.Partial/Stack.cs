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

#if WinCopies3

using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        public interface IEnumerableStack : IStackBase<T>, IEnumerableInfo<T>
        {
            // Left empty.
        }

        internal class Stack : IStackBase<T>
        {
            private protected IEnumerableStack Enumerable { get; } = new Enumerable();

            public bool IsReadOnly => false;

            public bool HasItems => Enumerable.HasItems;

            public T Peek() => Enumerable.Peek();

            public bool TryPeek(out T result) => Enumerable.TryPeek(out result);

            public void Push(T item) => Enumerable.Push(item);

            public T Pop() => Enumerable.Pop();

            public bool TryPop(out T item) => Enumerable.TryPop(out item);

            public void Clear() => Enumerable.Clear();
        }

        internal class EnumerableStack : Stack, IEnumerableStack
        {
            public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

            public IEnumeratorInfo2<T> GetEnumerator() => Enumerable.GetEnumerator();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => ((System.Collections.Generic.IEnumerable<T>)Enumerable).GetEnumerator();

            System.Collections.IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Enumerable).GetEnumerator();

            public IEnumeratorInfo2<T> GetReversedEnumerator() => Enumerable.GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetReversedEnumerator() => ((IEnumerable<T>)Enumerable).GetReversedEnumerator();

#if !CS8
            IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }
}

#endif
