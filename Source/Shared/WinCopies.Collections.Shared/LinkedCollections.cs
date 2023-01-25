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

using WinCopies.Collections.DotNetFix;

namespace WinCopies.Collections
{
    public class Stack : System.Collections.Stack, IEnumerableStack
    {
        bool ISimpleLinkedListCore.IsReadOnly => false;

        uint IUIntCountable.Count => (uint)Count;

        public bool HasItems => Count != 0u;

        public bool TryPeek(out object result)
        {
            if (Count > 0)
            {
                result = Peek();

                return true;
            }

            result = null;

            return false;
        }

        public bool TryPop(out object result)
        {
            if (Count > 0)
            {
                result = Pop();

                return true;
            }

            result = null;

            return false;
        }

        void IListCommon.Add(object value) => Push(value);
        object IListCommon.Remove() => Pop();
        bool IListCommon.TryRemove(out object result) => TryPop(out result);
    }

    public class Queue : System.Collections.Queue, IEnumerableQueue
    {
        bool ISimpleLinkedListCore.IsReadOnly => false;

        uint IUIntCountable.Count => (uint)Count;

        public bool HasItems => Count != 0u;

        public bool TryPeek(out object result)
        {
            if (Count > 0)
            {
                result = Peek();

                return true;
            }

            result = null;

            return false;
        }

        public bool TryDequeue(out object result)
        {
            if (Count > 0)
            {
                result = Peek();

                return true;
            }

            result = null;

            return false;
        }

        void IListCommon.Add(object value) => Enqueue(value);
        object IListCommon.Remove() => Dequeue();
        bool IListCommon.TryRemove(out object result) => TryDequeue(out result);
    }
}
