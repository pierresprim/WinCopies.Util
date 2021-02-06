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

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    public class SynchronizedQueue : IQueue
    {
        private readonly IQueue _queue;

        public bool IsReadOnly => false;

        public bool IsSynchronized => true;

        public object SyncRoot { get; private set; }

        public SynchronizedQueue(IQueue queue)
        {
            _queue = queue;

            SyncRoot = queue.SyncRoot;
        }

        public uint Count
        {
            get
            {
                lock (SyncRoot)

                    return _queue.Count;
            }
        }

        public void Clear()
        {
            lock (SyncRoot)

                _queue.Clear();
        }

        public object Peek()
        {
            lock (SyncRoot)

                return _queue.Peek();
        }

        public bool TryPeek(out object result)
        {
            lock (SyncRoot)

                return _queue.TryPeek(out result);
        }

        public void Enqueue(object value)
        {
            lock (SyncRoot)

                _queue.Enqueue(value);
        }

        public object Dequeue()
        {
            lock (SyncRoot)

                return _queue.Dequeue();
        }

        public bool TryDequeue(out object result)
        {
            lock (SyncRoot)

                return _queue.TryDequeue(out result);
        }
    }
#endif
}
