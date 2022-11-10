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
    public class ReadOnlyQueue<TQueue, TItems> : ReadOnlySimpleLinkedList<TQueue, TItems>, IQueue<TItems> where TQueue : IQueue<TItems>
    {
        public ReadOnlyQueue(TQueue queue) : base(queue) { /* Left empty. */ }

        void IQueueCore<TItems>.Enqueue(TItems
#if CS9
            ?
#endif
            item) => throw GetReadOnlyListOrCollectionException();
        TItems IQueueCore<TItems>.Dequeue() => throw GetReadOnlyListOrCollectionException();
        bool IQueueCore<TItems>.TryDequeue(out TItems
#if CS9
            ?
#endif
            result)
        {
            result = default;

            return false;
        }
#if !CS8
        void IQueueCore.Enqueue(object item) => throw GetReadOnlyListOrCollectionException();
        object IQueueCore.Dequeue() => throw GetReadOnlyListOrCollectionException();
        bool IQueueCore.TryDequeue(out object result) => throw GetReadOnlyListOrCollectionException();
#endif
    }

    public class ReadOnlyQueue<T> : ReadOnlyQueue<IQueue<T>, T>
    {
        public ReadOnlyQueue(in IQueue<T> queue) : base(queue) { /* Left empty. */ }
    }
}
