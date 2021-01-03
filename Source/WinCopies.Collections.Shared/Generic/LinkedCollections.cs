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

#if WinCopies3
using System.Collections;
#endif

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;


namespace WinCopies.Collections.Generic
{
    public class Stack<T> : System.Collections.Generic.Stack<T>, IEnumerableStack<T>
    {
#if WinCopies2
        uint IUIntCountableEnumerable.Count => (uint)Count;
#else
        public bool IsReadOnly => false;

        bool ISimpleLinkedListBase.IsSynchronized => ((ICollection)this).IsSynchronized;

        object ISimpleLinkedListBase.SyncRoot => ((ICollection)this).SyncRoot;
#endif

        uint IUIntCountable.Count => (uint)Count;

        public bool HasItems => Count != 0;

#if CS7
        public  bool TryPeek(out T result)
        {
            if (Count > 0)
            {
                result = Peek();

                return true;
            }

            result = default;

            return false;
        }

        public  bool TryPop(out T result)
        {
            if (Count > 0)
            {
                result = Pop();

                return true;
            }

            result = default;

            return false;
        }
#endif
    }

    public class Queue<T> : System.Collections.Generic.Queue<T>, IEnumerableQueue<T>
    {
#if WinCopies2
        uint IUIntCountableEnumerable.Count => (uint)Count;
#else
        public bool IsReadOnly => false;

        bool ISimpleLinkedListBase.IsSynchronized => ((ICollection)this).IsSynchronized;

        object ISimpleLinkedListBase.SyncRoot => ((ICollection)this).SyncRoot;
#endif

        uint IUIntCountable.Count => (uint)Count;

        public bool HasItems => Count != 0;

#if CS7
        public bool TryPeek(out T result)
        {
            if (Count > 0)
            {
                result = Peek();

                return true;
            }

            result = default;

            return false;
        }

        public bool TryDequeue(out T result)
        {
            if (Count > 0)
            {
                result = Dequeue();

                return true;
            }

            result = default;

            return false;
        }
#endif
    }
}
#endif
