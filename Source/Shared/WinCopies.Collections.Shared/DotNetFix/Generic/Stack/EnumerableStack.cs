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

namespace WinCopies.Collections.DotNetFix.Generic
{
    public interface IEnumerableStack<T> : IStack<T>, IEnumerableSimpleLinkedList<T>
    {
        // Left empty.
    }

    public class EnumerableStack<T> : EnumerableSimpleLinkedList<T, Stack<T>>, IEnumerableStack<T>
    {
        protected override ISimpleLinkedListNode<T>
#if CS8
            ?
#endif
            FirstNode => List.FirstItem;

        public EnumerableStack() : base(new Stack<T>()) { /* Left empty. */ }

        public void Push(T
#if CS9
            ?
#endif
            item) => Add(item);
        public T
#if CS9
            ?
#endif
            Pop() => Remove();
        public bool TryPop(out T
#if CS9
            ?
#endif
            result) => TryRemove(out result);

        void IListCommon.Add(object item) => Push((T
#if CS9
            ?
#endif
            )item);
        object IListCommon.Remove() => Pop();
        bool IListCommon.TryRemove(out object result) => UtilHelpers.TryGetValue<T>(TryPop, out result);
#if !CS8
        void IStackCore.Push(object item) => Push((T)item);
        object IStackCore.Pop() => Pop();
        public bool TryPop(out object result) => UtilHelpers.TryGetValue<T>(TryPop, out result);
#endif
    }
}
