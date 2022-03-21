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

namespace WinCopies.Collections.DotNetFix
{
    public class ReadOnlyQueue : ReadOnlySimpleLinkedList, IQueue
    {
        private readonly IQueue _queue;

        public sealed override uint Count => _queue.Count;

        public ReadOnlyQueue(IQueue queue) => _queue = queue;

#if !WinCopies3
        public ReadOnlyQueue(IStack stack)
        {
            // Left empty.
        }
#endif

        public sealed override object Peek() => _queue.Peek();

        void IQueue.Enqueue(object item) => GetReadOnlyListOrCollectionException();

        object IQueue.Dequeue() => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        public sealed override bool TryPeek(out object result) => _queue.TryPeek(out result);

        bool IQueue.TryDequeue(out object result)
        {
            result = null;

            return false;
        }
#endif
    }
}
