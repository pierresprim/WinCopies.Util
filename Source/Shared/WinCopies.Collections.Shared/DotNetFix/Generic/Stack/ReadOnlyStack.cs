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
    public class ReadOnlyStack<TStack, TItems> : ReadOnlySimpleLinkedList<TStack, TItems>, IStack<TItems> where TStack : IStack<TItems>
    {
        public ReadOnlyStack(in TStack stack) : base(stack) { /* Left empty. */ }

        void IStackCore<TItems>.Push(TItems item) => throw GetReadOnlyListOrCollectionException();
        TItems IStackCore<TItems>.Pop() => throw GetReadOnlyListOrCollectionException();
        bool IStackCore<TItems>.TryPop(out TItems result)
        {
            result = default;

            return false;
        }
#if !CS8
        void IStackCore.Push(object item) => throw GetReadOnlyListOrCollectionException();
        object IStackCore.Pop() => throw GetReadOnlyListOrCollectionException();
        bool IStackCore.TryPop(out object result) => throw GetReadOnlyListOrCollectionException();
#endif
    }

    public class ReadOnlyStack<T> : ReadOnlyStack<IStack<T>, T>
    {
        public ReadOnlyStack(in IStack<T> stack) : base(stack) { /* Left empty. */ }
    }
}
