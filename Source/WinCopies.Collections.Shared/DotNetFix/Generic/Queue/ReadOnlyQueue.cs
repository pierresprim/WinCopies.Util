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

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix.Generic
{
    public class ReadOnlyQueue<T> : ReadOnlySimpleLinkedList<T>, IQueue<T>
    {
        private readonly IQueue<T> _queue;

        public sealed override uint Count => _queue.Count;

        public ReadOnlyQueue(IQueue<T> queue) => _queue = queue;

        public sealed override T Peek() => _queue.Peek();

        void IQueue<T>.Enqueue(T item) => GetReadOnlyListOrCollectionException();

        T IQueue<T>.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out T result) => _queue.TryPeek(out result);

        bool IQueue<T>.TryDequeue(out T result)
        {
            result = default;

            return false;
        }
#endif
    }
}
