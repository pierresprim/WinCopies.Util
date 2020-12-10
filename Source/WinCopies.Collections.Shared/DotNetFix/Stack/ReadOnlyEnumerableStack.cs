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

using System;

#if WinCopies2
using WinCopies.Util;
#endif

using static WinCopies.Collections.ThrowHelper;

namespace WinCopies.Collections.DotNetFix
{
    [Serializable]
    public class ReadOnlyEnumerableStack : ReadOnlySimpleLinkedList, IEnumerableStack
    {
        private readonly IEnumerableStack _stack;

        public sealed override uint Count =>
#if WinCopies2
                ((IUIntCountable)
#endif
                _stack
#if WinCopies2
                )
#endif
                .Count;

        public ReadOnlyEnumerableStack(IEnumerableStack stack) => _stack = stack;

        public sealed override object Peek() => _stack.Peek();

        public void CopyTo(Array array, int arrayIndex) =>
#if WinCopies2
                WinCopies.Util.Extensions
#else
            EnumerableExtensions
#endif
                .CopyTo(this, array, arrayIndex, Count);

        public object[] ToArray() => _stack.ToArray();

        void IStack.Push(object item) => throw GetReadOnlyListOrCollectionException();

        object IStack.Pop() => throw GetReadOnlyListOrCollectionException();

#if !WinCopies2
        public sealed override bool TryPeek(out object result) => _stack.TryPeek(out result);

        bool IStack.TryPop(out object result)
        {
            result = null;

            return false;
        }
#endif

        public System.Collections.IEnumerator GetEnumerator() => _stack.GetEnumerator();
    }
}
