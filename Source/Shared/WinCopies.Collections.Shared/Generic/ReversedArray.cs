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

#if CS7
using System;

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Extensions.Generic;

namespace WinCopies.Collections.Generic
{
    public abstract class ReversedListBase<TItems, TList, TEnumeratorList> : ICountableEnumerable<TItems, ArrayEnumeratorBase<TItems, TEnumeratorList>>, IReadOnlyList<TItems>
    {
        protected TList List { get; }

        public abstract int Count { get; }

        public TItems this[int index] => GetAt(GetIndex(index));

        object
#if CS8
            ?
#endif
            IIndexableR.this[int index] => this[index];

        TItems System.Collections.Generic.IReadOnlyList<TItems>.this[int index] => this[index];
#if !CS7
        object IReadOnlyList.this[int index] => this[index];
#endif
        protected ReversedListBase(in TList list) => List = list == null ? throw new ArgumentNullException(nameof(list)) : list;

        protected abstract int GetIndex(int index);
        protected abstract TItems GetAt(int index);

        public abstract ArrayEnumeratorBase<TItems, TEnumeratorList> GetEnumerator();

        ICountableEnumerator<TItems> IReadOnlyList<TItems>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#if !CS8
        ICountableEnumerator<TItems> Enumeration.IEnumerable<ICountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();
        ICountableEnumerator Enumeration.IEnumerable<ICountableEnumerator>.GetEnumerator() => GetEnumerator();
#endif
    }

    public abstract class ReversedListBase2<TItems, TList, TEnumeratorList> : ReversedListBase<TItems, TList, TEnumeratorList>
    {
        protected ReversedListBase2(in TList list) : base(list) { /* Left empty. */ }

        protected override int GetIndex(int index) => Count - 1 - index;
    }

    public abstract class ReversedListBase<TItems, TList> : ReversedListBase2<TItems, TList, TList>
    {
        protected ReversedListBase(in TList list) : base(list) { /* Left empty. */ }
    }

    public class ReversedReadOnlyList<TItems, TList> : ReversedListBase2<TItems, TList, System.Collections.Generic.IReadOnlyList<TItems>> where TList : System.Collections.Generic.IReadOnlyList<TItems>
    {
        public override int Count => List.Count;

        public ReversedReadOnlyList(in TList list) : base(list) { /* Left empty. */ }

        protected override TItems GetAt(int index) => List[index];

        public override DotNetFix.Generic.
#if CS9
            ArrayEnumerator
#else
            ArrayEnumeratorBase
#endif
            <TItems
#if !CS9
            , System.Collections.Generic.IReadOnlyList<TItems>
#endif
            > GetEnumerator() => new
#if !CS9
            DotNetFix.Generic.ArrayEnumerator<TItems>
#endif
            (List, ArrayEnumerationOptions.Reverse);
    }

    public sealed class ReversedReadOnlyList<T> : ReversedReadOnlyList<T, System.Collections.Generic.IReadOnlyList<T>>
    {
        public ReversedReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list) : base(list) { /* Left empty. */ }
    }

    public sealed class ReversedList<T> : ReversedListBase<T, System.Collections.Generic.IList<T>>
    {
        public override int Count => List.Count;

        protected override T GetAt(int index) => List[index];

        public ReversedList(in System.Collections.Generic.IList<T> list) : base(list) { /* Left empty. */ }

        public override DotNetFix.Generic.
#if CS9
            ListEnumerator
#else
            ArrayEnumeratorBase
#endif
            <T
#if !CS9
                , System.Collections.Generic.IList<T>
#endif
                >
            GetEnumerator() => new
#if !CS9
            DotNetFix.Generic.ListEnumerator<T>
#endif
            (List, ArrayEnumerationOptions.Reverse);
    }

    public sealed class ReversedArray<T> : ReversedReadOnlyList<T, T[]>
    {
        public new T this[int index] { get => base[index]; set => List[GetIndex(index)] = value; }

        public ReversedArray(in T[] array) : base(array) { /* Left empty. */ }
    }
}
#endif
