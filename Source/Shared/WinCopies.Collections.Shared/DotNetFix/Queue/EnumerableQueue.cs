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
    public interface IEnumerableQueue : IQueue, IEnumerableSimpleLinkedList, ICollection
    {
        // Left empty.
    }

    [Serializable]
    public class EnumerableQueue : EnumerableSimpleLinkedList<Queue>, IEnumerableQueue
    {
        protected override ISimpleLinkedListNode2 FirstNode => InnerList.FirstItem;

        public EnumerableQueue() : base(new Queue()) { /* Left empty. */ }

        public void Enqueue(object
#if CS8
            ?
#endif
            item)
        {
            InnerList.Enqueue(item);

            UpdateEnumerableVersion();
        }

        public object
#if CS8
            ?
#endif
            Dequeue()
        {
            object
#if CS8
                ?
#endif
                result = InnerList.Dequeue();

            UpdateEnumerableVersion();

            return result;
        }

        public bool TryDequeue(out object
#if CS8
            ?
#endif
            result)
        {
            if (InnerList.TryDequeue(out result))
            {
                UpdateEnumerableVersion();

                return true;
            }

            return false;
        }
    }
}
