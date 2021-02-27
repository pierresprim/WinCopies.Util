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
    public class SynchronizedStack : IStack
    {
        private readonly IStack _stack;

        public bool IsReadOnly => false;

        public bool HasItems => Count != 0;

        public bool IsSynchronized => true;

        public object SyncRoot { get; private set; }

        public SynchronizedStack(IStack stack)
        {
            _stack = stack;

            SyncRoot = stack.SyncRoot;
        }

        public uint Count
        {
            get
            {
                lock (SyncRoot)

                    return _stack.Count;
            }
        }

        public void Clear()
        {
            lock (SyncRoot)

                _stack.Clear();
        }

        public object Peek()
        {
            lock (SyncRoot)

                return _stack.Peek();
        }

        public bool TryPeek(out object result)
        {
            lock (SyncRoot)

                return _stack.TryPeek(out result);
        }

        public void Push(object value)
        {
            lock (SyncRoot)

                _stack.Push(value);
        }

        public object Pop()
        {
            lock (SyncRoot)

                return _stack.Pop();
        }

        public bool TryPop(out object result)
        {
            lock (SyncRoot)

                return _stack.TryPop(out result);
        }
    }
#endif
}
