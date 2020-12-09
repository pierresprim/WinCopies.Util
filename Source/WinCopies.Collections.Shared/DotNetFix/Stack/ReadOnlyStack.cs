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
    public class ReadOnlyStack : ReadOnlySimpleLinkedList, IStack
    {
        private readonly IStack _stack;

        public sealed override uint Count => _stack.Count;

        public ReadOnlyStack(IStack stack) => _stack = stack;

        public sealed override object Peek() => _stack.Peek();

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
    }
}
