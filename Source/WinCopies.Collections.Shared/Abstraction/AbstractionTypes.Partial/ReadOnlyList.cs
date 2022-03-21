/* Copyright © Pierre Sprimont, 2021
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

#if CS6
using System.Collections;
using System.Collections.Generic;

using WinCopies.Util;

namespace WinCopies.Collections.AbstractionInterop.Generic
{
    public static partial class AbstractionTypes<TSource, TDestination> 
    {
        public class ReadOnlyList<TList> : IReadOnlyList<TDestination> where TList : IReadOnlyList<TSource>
        {
            protected TList List { get; }

            public TDestination this[int index] => List[index];

            public int Count => List.Count;

            public ReadOnlyList(in TList list) => List = list;

            public IEnumerator<TDestination> GetEnumerator() => List.ToEnumerable<TSource, TDestination>().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
        }

        public class ReadOnlyList : ReadOnlyList<IReadOnlyList<TSource>>
        {
            public ReadOnlyList(in IReadOnlyList<TSource> list) : base(list) { /* Left empty. */ }
        }
    }
}
#endif
