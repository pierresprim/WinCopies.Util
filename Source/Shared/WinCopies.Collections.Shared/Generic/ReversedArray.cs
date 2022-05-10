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

#if WinCopies3 && CS7
using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Generic
{
    public class ReversedReadOnlyList<TItems, TList> : ICountableEnumerable<TItems, ArrayEnumerator<TItems>>, IReadOnlyList<TItems> where TList : System.Collections.Generic.IReadOnlyList<TItems>
    {
        protected TList List { get; }

        int ICountableEnumerable<TItems, ArrayEnumerator<TItems>>.Count => List.Count;

        public int Count => List.Count;

        int System.Collections.Generic.IReadOnlyCollection<TItems>.Count => List.Count;

        int IReadOnlyList<TItems>.Count => List.Count;

        int ICountableEnumerable<TItems, ICountableEnumerator<TItems>>.Count => List.Count;

        TItems IIndexableR<TItems>.this[int index] => GetAt(index);

        object IIndexableR.this[int index] => GetAt(index);

        protected TItems this[in int index] => List[GetIndex(index)];

        TItems System.Collections.Generic.IReadOnlyList<TItems>.this[int index] => GetAt(index);

#if !(WinCopies3 && CS7)
        object IReadOnlyList.this[int index] => this[index];
#endif

        protected ReversedReadOnlyList(in TList list)
        {
            if (list == null)

                throw WinCopies.
#if WinCopies3
            ThrowHelper
#else
            Util.Util
#endif
            .GetArgumentNullException(nameof(list));

            List = list;
        }

        protected int GetIndex(in int index) => List.Count - 1 - index;

        protected TItems GetAt(in int index) => this[Count - 1 - index];

        public ArrayEnumerator<TItems> GetEnumerator() => new
#if !CS9
            ArrayEnumerator<TItems>
#endif
            (List, true);

        ICountableEnumerator<TItems> IReadOnlyList<TItems>.GetEnumerator() => GetEnumerator();

        ICountableEnumerator<TItems> ICountableEnumerable<TItems, ICountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

#if !CS9
        ICountableEnumerator<TItems> Enumeration.DotNetFix.IEnumerable<ICountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

        ICountableEnumerator<TItems> DotNetFix.Generic.IEnumerable<TItems, ICountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

        ICountableEnumerator Enumeration.DotNetFix.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetEnumerator();
#endif
    }

    public sealed class ReversedReadOnlyList<T> : ReversedReadOnlyList<T, System.Collections.Generic.IReadOnlyList<T>>
    {
        public ReversedReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list) : base(list)
        {
            // Left empty.
        }
    }

    public sealed class ReversedArray<T> : ReversedReadOnlyList<T, T[]>
    {
        public T this[int index] { get => this[index]; set => List[GetIndex(index)] = value; }

        public ReversedArray(in T[] array) : base(array)
        {
            // Left empty.
        }
    }
}
#endif
