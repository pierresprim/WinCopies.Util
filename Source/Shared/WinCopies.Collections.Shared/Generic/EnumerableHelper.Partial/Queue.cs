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
        public interface IEnumerableQueue : IQueueBase<T>, IEnumerableInfo<T>
        {
            // Left empty.
        }

        internal class Queue : IQueueBase<T>
        {
            private protected IEnumerableQueue Enumerable { get; } = new Enumerable();

            public bool IsReadOnly => false;

            public bool HasItems => Enumerable.HasItems;

            public T Peek() => Enumerable.Peek();

            public bool TryPeek(out T result) => Enumerable.TryPeek(out result);

            public void Enqueue(T item) => Enumerable.Enqueue(item);

            public T Dequeue() => Enumerable.Dequeue();

            public bool TryDequeue(out T item) => Enumerable.TryDequeue(out item);

            public void Clear() => Enumerable.Clear();
        }

        internal class EnumerableQueue : Queue, IEnumerableQueue
        {
            public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

            public IEnumeratorInfo<T> GetEnumerator() => Enumerable.GetEnumerator();

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => Enumerable.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => Enumerable.GetEnumerator();

            public IEnumeratorInfo<T> GetReversedEnumerator() => Enumerable.GetReversedEnumerator();

            System.Collections.Generic.IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => Enumerable.GetReversedEnumerator();
#if !CS8
            IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }
}
#endif
