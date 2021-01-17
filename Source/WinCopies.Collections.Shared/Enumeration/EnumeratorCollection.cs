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

#if !WinCopies3 // Removed in WinCopies 3.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WinCopies.Collections
{
    // todo: to linked list?

    public class EnumeratorCollection : Collection<System.Collections.IEnumerator>
    {
        public int EnumerableVersion { get; private set; }

        public EnumeratorCollection() : base() { }

        public EnumeratorCollection(IList<System.Collections.IEnumerator> list) : base(list) { }

        protected override void ClearItems()
        {
            base.ClearItems();

            EnumerableVersion = 0;
        }

        protected override void RemoveItem(int index)
        {
            RemoveItem(index);

            if (Count == 0)

                EnumerableVersion = 0;
        }

        public void OnCollectionUpdated()
        {
            if (Count > 0)

                EnumerableVersion++;
        }
    }
}

#endif
