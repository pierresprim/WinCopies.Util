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
using System.Collections;

namespace WinCopies.Collections.DotNetFix
{
    public interface IEnumerableStack : IStack, IEnumerableSimpleLinkedList, ICollection
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableStack : EnumerableSimpleLinkedList<Stack>, IEnumerableStack
    {
                protected override ISimpleLinkedListNode2 FirstNode => InnerList.FirstItem;

        public EnumerableStack() : base(new Stack()) { /* Left empty. */ }

        public void Push(object item)
        {
            InnerList.Push(item);

            UpdateEnumerableVersion();
        }

        public object Pop()
        {
            object result = InnerList.Pop();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryPop(out object result)
        {
            if (InnerList.TryPop(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }
    }
}
