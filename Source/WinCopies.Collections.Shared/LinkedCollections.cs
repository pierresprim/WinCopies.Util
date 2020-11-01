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

#if !WinCopies2
using System.Collections;
#endif

using WinCopies.Collections.DotNetFix;

namespace WinCopies.Collections
{
    public class Stack : System.Collections.Stack, IEnumerableStack
    {
        public bool IsReadOnly => false;

#if WinCopies2
        uint IUIntCountableEnumerable.Count => (uint)Count;
#endif

        uint IUIntCountable.Count => (uint)Count;

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
    }

    public class Queue : System.Collections.Queue, IEnumerableQueue
    {
        public bool IsReadOnly => false;

#if WinCopies2
        uint IUIntCountableEnumerable.Count => (uint)Count;
#endif

        uint IUIntCountable.Count => (uint)Count;

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
    }
}
