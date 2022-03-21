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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ReadOnlyQueue
#if WinCopies3
<TQueue, TItems>
#else
        <T>
#endif
        : ReadOnlySimpleLinkedList<

#if WinCopies3
            TItems
#else
            T
#endif
            >, IQueue<

#if WinCopies3
            TItems
#else
                T
#endif
            >
#if WinCopies3
        where TQueue : IQueue<TItems>
#endif
    {
        private readonly
#if WinCopies3
            TQueue
#else
IQueue<T>
#endif
            _queue;

        public sealed override uint Count => _queue.Count;

#if WinCopies3
        public bool HasItems => _queue.HasItems;
#endif

        public ReadOnlyQueue(
#if WinCopies3
            TQueue
#else
IQueue<T>
#endif
           queue) => _queue = queue;

        public sealed override
#if WinCopies3
            TItems
#else
T
#endif
            Peek() => _queue.Peek();

        void
#if !WinCopies3
IQueue
#else
            IQueueBase
#endif
            <
#if WinCopies3
            TItems
#else
T
#endif
            >.Enqueue(
#if WinCopies3
            TItems
#else
T
#endif
             item) => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        TItems
#else
        T
#endif

#if !WinCopies3
IQueue
#else
            IQueueBase
#endif
            <
#if WinCopies3
            TItems
#else
T
#endif
            >.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        public sealed override bool TryPeek(out TItems result) => _queue.TryPeek(out result);

        bool IQueueBase<TItems>.TryDequeue(out TItems result)
        {
            result = default;

            return false;
        }
#endif
    }

#if WinCopies3
    public class ReadOnlyQueue<T> : ReadOnlyQueue<IQueue<T>, T>
    {
        public ReadOnlyQueue(in IQueue<T> queue) : base(queue)
        {
            // Left empty.
        }
    }
#endif
}
