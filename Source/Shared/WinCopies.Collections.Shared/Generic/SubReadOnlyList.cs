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
using System.Collections.Generic;
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

    public abstract class ArrayArrayBase<T, U, V> : IULongCountableEnumerable<T> where U : System.Collections.Generic.IReadOnlyList<T> where V : System.Collections.Generic.IEnumerable<U>
    {
        protected abstract V InnerArrays { get; }

        public abstract ulong Count { get; }

        protected W GetValue<W>(in ulong index, in Func<U, ulong, W> func)
        {
            ulong length = index >= Count ? throw new IndexOutOfRangeException(nameof(index)) : 0ul;

            foreach (U array in InnerArrays)

                if (index < (length += (ulong)array.Count))

                    return func(array, index - length);

            // This code should not be reached.

            Debug.Assert(false);

            return default;
        }
        protected W GetValue<W>(in ulong index, Func<U, int, W> func) => GetValue(index, (U array, ulong i) => func(array, (int)i));

        protected Enumerator<W> GetEnumerator<W>(in W arrays) where W : IULongCountableEnumerable<T>, IULongIndexableR<T> => new
#if !CS9
            Enumerator<W>
#endif
            (arrays);
        public abstract IULongCountableEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#if !CS8
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif
        protected class Enumerator<W> : Generic.Enumerator<T>, IULongCountableEnumerator<T> where W : IULongCountableEnumerable<T>, IULongIndexableR<T>
        {
            private T _current;
            private ulong _currentIndex;

            protected W InnerArrays { get; }

            public ulong Count => InnerArrays.Count;

            public override bool? IsResetSupported => true;

            protected override T CurrentOverride => _current;

            public Enumerator(in W arrays) => InnerArrays = arrays;

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

    public abstract class ArrayArray<T, U, V> : ArrayArrayBase<T, U, V> where U : System.Collections.Generic.IReadOnlyList<T> where V : System.Collections.Generic.IEnumerable<U>
    {
        protected sealed override V InnerArrays { get; }

        public sealed override ulong Count { get; }

        protected ArrayArray(in V arrays) => Count = UtilHelpers.GetLength(array => array.Count, InnerArrays = arrays);
    }

    public abstract class ArrayArray<T, U> : ArrayArray<T, U, System.Collections.Generic.IEnumerable<U>> where U : System.Collections.Generic.IReadOnlyList<T>
    {
        protected ArrayArray(in System.Collections.Generic.IEnumerable<U> arrays) : base(arrays) { /* Left empty. */ }
        protected ArrayArray(params U[] arrays) : this(arrays.AsEnumerable()) { /* Left empty. */ }
#if !WinCopies4
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
#endif
    }

    public class BufferedArray<T> : ArrayArrayBase<T, T[], System.Collections.Generic.IReadOnlyList<T[]>>, IULongIndexable<T>
    {
        private struct ArrayArray : System.Collections.Generic.IReadOnlyList<T[]>, IIndexableR<T[]>
        {
            private T[][]
#if CS8
                ?
#endif
                _array;

            public T[] this[int index] => _array == null ? throw GetError(nameof(index)) : _array[index];

            public T this[ulong index] { get => GetArray(index, out int position)[position]; set => GetArray(index, out int position)[position] = value; }

            public int Count => _array == null ? 0 : _array.Length;

            public int BufferLength { get; private set; }
#if !CS8
            object IIndexableR.this[int index] => this[index];
#endif
            public ArrayArray(in T[][]
#if CS8
                ?
#endif
                array, in int bufferLength)
            {
                _array = array;

                BufferLength = bufferLength;
            }

            private readonly T[] GetArray(in ulong index, out int position)
            {
                var bufferLength = _array == null ? throw GetError(nameof(index)) : (ulong)BufferLength;

                position = (int)(index % bufferLength);

                return _array[(int)(index / bufferLength)];
            }

            public void Resize(int bufferCount)
            {
                if (bufferCount == 0)
                {
                    _array = null;

                    return;
                }

                T[][] create() => new T[bufferCount][];

                if (_array == null)
                {
                    _array = create();

                    return;
                }

                if (bufferCount == _array.Length)

                    return;

                T[][] values = create();

                System.Array.Copy(_array, values, bufferCount > _array.Length ? _array.Length : bufferCount);

                _array = values;
            }

            public System.Collections.Generic.IEnumerator<T[]> GetEnumerator() => _array == null ? Linq.Enumerator.Empty<T[]>() : _array.AsEnumerable().GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private ArrayArray _arrays;
        private ulong _count;

        protected sealed override System.Collections.Generic.IReadOnlyList<T[]> InnerArrays => _arrays;

        public override ulong Count => _count;
#if !CS8
        object IULongIndexableR.this[ulong index] => this[index];
        object IULongIndexableW.this[ulong index] { set => this[index] = (T)value; }
#endif
        public T this[ulong index] { get => ValidateIndex(index) ? _arrays[index] : throw GetError(nameof(index)); set => _arrays[ValidateIndex(index) ? index : throw GetError(nameof(index))] = value; }

        public BufferedArray(in int bufferLength, in int bufferCount, in ulong length)
        {
            if (bufferLength <= 0)

                OnError(nameof(bufferLength));

            if (bufferCount < 0)

                OnError(nameof(bufferCount));

            T[][]
#if CS8
                ?
#endif
                array;

            _count = length;

            if (bufferCount == 0)

                array = length == 0 ?
#if !CS9
                    (T[][])
#endif
                    null : throw GetError(nameof(length));

            else
            {
                if (length > ((ulong)bufferLength * (ulong)bufferCount))

                    OnError(nameof(length));

                array = new T[bufferCount][];

                for (int i = 0; i < array.Length; i++)

                    array[i] = new T[bufferLength];
            }

            _arrays = new ArrayArray(array, bufferLength);
        }
        public BufferedArray(in int bufferLength, in ulong length) : this(bufferLength, (int)Math.GetBufferCount(length, bufferLength), length) { }

        protected static Exception GetError(in string argumentName) => new ArgumentOutOfRangeException(argumentName);
        protected static void OnError(in string argumentName) => throw GetError(argumentName);

        protected bool ValidateIndex(in ulong index) => index < _count;

        public void SetLength(in ulong length) => _arrays.Resize((int)Math.GetBufferCount(_count = length, _arrays.BufferLength));

        public override IULongCountableEnumerator<T> GetEnumerator() => GetEnumerator(this);
    }

    public class ReadOnlyArrayArray<T> : ArrayArray<T, System.Collections.Generic.IReadOnlyList<T>>, IULongIndexableR<T>
    {
        public T this[ulong index] => GetValue(index, (System.Collections.Generic.IReadOnlyList<T> array, int i) => array[i]);
#if !CS8
        object IULongIndexableR.this[ulong index] => this[index];
#endif
        public ReadOnlyArrayArray(in System.Collections.Generic.IEnumerable<System.Collections.Generic.IReadOnlyList<T>> arrays) : base(arrays) { /* Left empty. */ }
        public ReadOnlyArrayArray(params System.Collections.Generic.IReadOnlyList<T>[] arrays) : base(arrays) { /* Left empty. */ }

        public override IULongCountableEnumerator<T> GetEnumerator() => GetEnumerator(this);
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

        public override IULongCountableEnumerator<T> GetEnumerator() => GetEnumerator(this);
    }

    public class ReadOnlyArrayArray2<T> : ReadOnlyArrayArray<T>, Extensions.Generic.IReadOnlyList<T>
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
        ICountableEnumerator<T> Extensions.Generic.IReadOnlyList<T>.GetEnumerator() => GetEnumerator2();
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#if WinCopies4
        protected
#else
        public
#endif
            class Enumerator2<U> : Enumerator<U>, ICountableEnumerator<T> where U : IULongCountableEnumerable<T>, Extensions.Generic.IReadOnlyList<T>, IULongIndexableR<T>
        {
            int ICountable.Count => (int)Count;

            public Enumerator2(in U arrays) : base(arrays) { /* Left empty. */ }
        }
    }
}
#endif
