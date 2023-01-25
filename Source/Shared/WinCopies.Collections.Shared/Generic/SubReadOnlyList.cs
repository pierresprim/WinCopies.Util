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
using System.Collections;
using System.Diagnostics;
using System.Linq;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Extensions.Generic;
using WinCopies.Util;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    public abstract class SubReadOnlyListBase<TList, TItems, TEnumerator>
#if CS7
        : System.Collections.Generic.IReadOnlyList<TItems>
#endif
        where TList : System.Collections.Generic.IReadOnlyList<TItems>
        where TEnumerator : ICountableEnumeratorInfo<TItems>
    {
        protected TList InnerList { get; }

        public abstract bool SupportsReversedEnumeration { get; }

        protected int StartIndex { get; }

        public TItems this[int index] => index.Between(0, Count - 1) ? InnerList[index += StartIndex] : throw new IndexOutOfRangeException($"{nameof(index)} must be between 0 and {nameof(Count)} - 1.");

        public int Count { get; }

#if WinCopies3
        protected
#else
        public
#endif
        SubReadOnlyListBase(in TList list, in int startIndex, in int count)
        {
            int arrayLength = (list
#if CS8
                ??
#else
                == null ?
#endif
                throw GetArgumentNullException(nameof(list))
#if !CS8
                : list
#endif
                ).Count;

            if (!(arrayLength == 0 && arrayLength == startIndex && arrayLength == count))
            {
                if (startIndex >= arrayLength)

                    throw new ArgumentOutOfRangeException(nameof(startIndex));

                if (count > arrayLength - startIndex)

                    throw new ArgumentOutOfRangeException(nameof(count));
            }

            InnerList = list;

            StartIndex = startIndex;

            Count = count;
        }

        protected SubReadOnlyListBase(in TList list, in int startIndex) : this(list, startIndex, (list
#if CS8
                ??
#else
                == null ?
#endif
                throw GetArgumentNullException(nameof(list))
#if !CS8
                : list
#endif
                ).Count - startIndex)
        { /* Left empty. */ }

        public abstract TEnumerator GetEnumerator();

        public abstract TEnumerator GetReversedEnumerator();

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class SubReadOnlyList<T> : SubReadOnlyListBase<System.Collections.Generic.IReadOnlyList<T>, T, ArrayEnumerator<T>>
    {
        public override bool SupportsReversedEnumeration => true;

        public SubReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list, in int startIndex, in int count) : base(list, startIndex, count) { /* Left empty. */ }

        public SubReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list, in int startIndex) : base(list, startIndex) { /* Left empty. */ }

        public override ArrayEnumerator<T> GetEnumerator() => new
#if !CS9
            ArrayEnumerator<T>
#endif
            (this);

        public override ArrayEnumerator<T> GetReversedEnumerator() => new
#if !CS9
            ArrayEnumerator<T>
#endif
            (this, DotNetFix.ArrayEnumerationOptions.Reverse);
    }

    public abstract class ArrayArray<T, U> : IULongCountableEnumerable<T> where U : System.Collections.Generic.IReadOnlyList<T>
    {
        protected System.Collections.Generic.IEnumerable<U> InnerArrays { get; }

        public ulong Count { get; }

        public ArrayArray(in System.Collections.Generic.IEnumerable<U> arrays) => Count = UtilHelpers.GetLength(array => array.Count, InnerArrays = arrays);

        public ArrayArray(params U[] arrays) : this(arrays.AsEnumerable()) { /* Left empty. */ }

        protected V GetValue<V>(in ulong index, in Func<U, ulong, V> func)
        {
            if (index >= Count)

                throw new IndexOutOfRangeException(nameof(index));

            ulong length = 0;

            foreach (U array in InnerArrays)

                if (index < (length += (ulong)array.Count))

                    return func(array, index - length);

            // This code should not be reached.

            Debug.Assert(false);

            return default;
        }

        protected V GetValue<V>(in ulong index, Func<U, int, V> func) => GetValue(index, (U array, ulong i) => func(array, (int)i));

        public abstract IULongCountableEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#if !CS8
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif

        public class Enumerator<V> : Generic.Enumerator<T>, IULongCountableEnumerator<T> where V : IULongCountableEnumerable<T>, IULongIndexableR<T>
        {
            private T _current;
            private ulong _currentIndex;

            protected V InnerArrays { get; }

            public ulong Count => InnerArrays.Count;

            public override bool? IsResetSupported => true;

            protected override T CurrentOverride => _current;

            public Enumerator(in V arrays) => InnerArrays = arrays;

            protected override bool MoveNextOverride()
            {
                if (_currentIndex < Count)
                {
                    _current = InnerArrays[_currentIndex++];

                    return true;
                }

                return false;
            }

            protected override void ResetCurrent()
            {
                _current = default;
                _currentIndex = 0;

                base.ResetCurrent();
            }

            protected override void ResetOverride2() { /* Left empty. */ }
        }
    }

    public class ReadOnlyArrayArray<T> : ArrayArray<T, System.Collections.Generic.IReadOnlyList<T>>, IULongIndexableR<T>
    {
        public T this[ulong index] => GetValue(index, (System.Collections.Generic.IReadOnlyList<T> array, int i) => array[i]);
#if !CS8
        object IULongIndexableR.this[ulong index] => throw new NotImplementedException();
#endif

        public ReadOnlyArrayArray(in System.Collections.Generic.IEnumerable<System.Collections.Generic.IReadOnlyList<T>> arrays) : base(arrays) { /* Left empty. */ }

        public ReadOnlyArrayArray(params System.Collections.Generic.IReadOnlyList<T>[] arrays) : base(arrays) { /* Left empty. */ }

        public override IULongCountableEnumerator<T> GetEnumerator() => new Enumerator<ReadOnlyArrayArray<T>>(this);
    }

    public class ArrayArray<T> : ArrayArray<T, T[]>, IULongIndexable<T>
    {
        public T this[ulong index] { get => GetValue(index, (T[] array, int i) => array[i]); set => GetValue(index, (T[] array, int i) => array[i] = value); }
#if !CS8
        object IULongIndexableR.this[ulong index] => this[index];

        object IULongIndexableW.this[ulong index] { set => this[index] = (T)value; }
#endif

        public ArrayArray(in System.Collections.Generic.IEnumerable<T[]> arrays) : base(arrays) { /* Left empty. */ }

        public ArrayArray(params T[][] arrays) : base(arrays) { /* Left empty. */ }

        public override IULongCountableEnumerator<T> GetEnumerator() => new Enumerator<ArrayArray<T>>(this);
    }

    public class ReadOnlyArrayArray2<T> : ReadOnlyArrayArray<T>, IReadOnlyList<T>
    {
        public T this[int index] => this[(ulong)index];

        int ICountable.Count => (int)Count;
#if !CS8
        int System.Collections.Generic.IReadOnlyCollection<T>.Count => (int)Count;

        object IIndexableR.this[int index] => this[index];

        ICountableEnumerator<T> Enumeration.IEnumerable<ICountableEnumerator<T>>.GetEnumerator() => GetEnumerator2();
        DotNetFix.ICountableEnumerator Enumeration.IEnumerable<DotNetFix.ICountableEnumerator>.GetEnumerator() => GetEnumerator2();
#endif
        public ReadOnlyArrayArray2(in System.Collections.Generic.IEnumerable<System.Collections.Generic.IReadOnlyList<T>> arrays) : base(arrays)
        {
            if (Count > int.MaxValue)

                ThrowOverflowException(nameof(arrays));
        }

        public ReadOnlyArrayArray2(params System.Collections.Generic.IReadOnlyList<T>[] arrays) : this(arrays.AsEnumerable()) { /* Left empty. */ }

        public ICountableEnumerator<T> GetEnumerator2() => new Enumerator2<ReadOnlyArrayArray2<T>>(this);
        ICountableEnumerator<T> IReadOnlyList<T>.GetEnumerator() => GetEnumerator2();
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        public class Enumerator2<U> : Enumerator<U>, ICountableEnumerator<T> where U : IULongCountableEnumerable<T>, IReadOnlyList<T>, IULongIndexableR<T>
        {
            int ICountable.Count => (int)Count;

            public Enumerator2(in U arrays) : base(arrays) { /* Left empty. */ }
        }
    }
}
#endif
