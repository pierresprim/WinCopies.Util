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

#if WinCopies2
using WinCopies.Util;
#endif

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    [Serializable]
    public class ReadOnlyEnumerableQueue : ReadOnlySimpleLinkedList,
#if WinCopies2
            IEnumerableStack,
#endif
            IEnumerableQueue
    {
        private readonly IEnumerableQueue _queue;

        public sealed override uint Count =>
#if WinCopies2
                ((IUIntCountable)
#endif
                _queue
#if WinCopies2
                )
#endif
                .Count;

        public ReadOnlyEnumerableQueue(IEnumerableQueue queue) => _queue = queue;

        public sealed override object Peek() => _queue.Peek();

        public void CopyTo(Array array, int arrayIndex) =>
#if WinCopies2
                WinCopies.Util.Extensions
#else
            EnumerableExtensions
#endif
                .CopyTo(this, array, arrayIndex, Count);

        public object[] ToArray() => _queue.ToArray();

#if WinCopies2
        void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

        object IStack.Pop() => throw GetReadOnlyListOrCollectionException();
#endif

        void IQueue.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();

        object IQueue.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out object result) => TryPeek(out result);

        bool IQueue.TryDequeue(out object result)
        {
            result = null;

            return false;
        }
#endif

        public System.Collections.IEnumerator GetEnumerator() => _queue.GetEnumerator();
    }
}

#endif
