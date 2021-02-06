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
    public interface IEnumerableSimpleLinkedList : ISimpleLinkedList,
#if !WinCopies3
            IUIntCountableEnumerable
#else
            IEnumerableSimpleLinkedListBase, IEnumerable
#endif
    {
#if WinCopies3
        void CopyTo(Array array, int index);

        object[] ToArray();
#endif
    }

    public abstract class EnumerableSimpleLinkedList : EnumerableSimpleLinkedListBase, IEnumerableSimpleLinkedList
    {
        public abstract object Peek();

        public abstract bool TryPeek(out object result);

        public abstract System.Collections.IEnumerator GetEnumerator();

        public void CopyTo(Array array, int arrayIndex) =>
#if !WinCopies3
                WinCopies.Util.Extensions
#else
            EnumerableExtensions
#endif
                .CopyTo(this, array, arrayIndex, Count);

        public object[] ToArray()
        {
            if (Count > int.MaxValue)

                throw new ArgumentOutOfRangeException("Too many items in list or collection.");

            object[] result = new object[Count];

            int i = -1;

            foreach (object value in this)

                result[++i] = value;

            return result;
        }
    }
}
