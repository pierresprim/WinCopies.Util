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
    public class ReadOnlyStack<T> : ReadOnlySimpleLinkedList<T>, IStack<T>
    {
        private readonly IStack<T> _stack;

        public sealed override uint Count => _stack.Count;

#if WinCopies3
        public bool HasItems => _stack.HasItems;
#endif

        public ReadOnlyStack(IStack<T> stack) => _stack = stack;

        public sealed override T Peek() => _stack.Peek();

        void
#if !WinCopies3
IStack
#else
            IStackBase
#endif
            <T>.Push(T item) => throw GetReadOnlyListOrCollectionException();

        T
#if !WinCopies3
IStack
#else
            IStackBase
#endif
            <T>.Pop() => throw GetReadOnlyListOrCollectionException();

#if WinCopies3
        public sealed override bool TryPeek(out T result) => _stack.TryPeek(out result);

        bool
#if !WinCopies3
IStack
#else
            IStackBase
#endif
            <T>.TryPop(out T result)
        {
            result = default;

            return false;
        }
#endif
    }
}
