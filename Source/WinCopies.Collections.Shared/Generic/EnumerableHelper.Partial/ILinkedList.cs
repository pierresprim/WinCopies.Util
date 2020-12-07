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

using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Generic
{
    public static partial class EnumerableHelper<T>
    {
        public interface ILinkedList : IQueueBase<T>, IStackBase<T>
        {
            T First { get; }

            T Last { get; }

            bool TryGetFirst(out T result);

            bool TryGetLast(out T result);

            void AddFirst(T item);

            void AddLast(T item);

            void RemoveFirst();

            T GetAndRemoveFirst();

            bool TryGetAndRemoveFirst(out T result);

            void RemoveLast();

            T GetAndRemoveLast();

            bool TryGetAndRemoveLast(out T result);
        }
    }
}

#endif
