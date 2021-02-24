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

using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;

using WinCopies.Collections.DotNetFix;

using IEnumerator = System.Collections.IEnumerator;

namespace WinCopies.Collections.Generic
{
#if WinCopies3
    public class UIntCountableEnumerator<T> : Enumerator<T, ICountableEnumeratorInfo<T>, T>, IUIntCountableEnumeratorInfo<T>
    {
        public uint Count => (uint)InnerEnumerator.Count;

        protected override T CurrentOverride => InnerEnumerator.Current;

        public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

        public UIntCountableEnumerator(ICountableEnumeratorInfo<T> enumerator) : base(enumerator) { /* Left empty. */ }

        protected override bool MoveNextOverride() => InnerEnumerator.MoveNext();
    }
#endif

    public interface IReadOnlyList : ICountableEnumerable
    {
        object this[int index] { get; }
    }

    public interface IReadOnlyList<out T> : ICountableEnumerable<T>
#if WinCopies3
        , IReadOnlyList
#endif
#if CS7
, System.Collections.Generic.IReadOnlyList<T>
#endif
    {
#if WinCopies3
        new int Count { get; }

#if !CS7
        T this[int index] { get; }
#endif
#endif
    }

    public class CountableEnumerableArray<T> : IReadOnlyList<T>
    {
        protected T[] Array { get; }

        public int Count => Array.Length;

        public T this[int index] => Array[index];

#if WinCopies3
        object IReadOnlyList.this[int index] => this[index];
#endif

        public CountableEnumerableArray(in T[] array) => Array = array;

        public
#if WinCopies3
            ICountableEnumeratorInfo<T>
#else
         System.Collections.Generic.IEnumerator<T> 
#endif
            GetEnumerator() => new ArrayEnumerator<T>(
#if WinCopies3 && !CS7
                this
#else
                Array
#endif
                );

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies3
        ICountableEnumerator<T> ICountableEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif
    }

    public class UIntCountableEnumerableArray<T> : IUIntCountableEnumerable<T>
    {
        private readonly CountableEnumerableArray<T> _array;

        public UIntCountableEnumerableArray(in T[] array) : this(new CountableEnumerableArray<T>(array)) { /* Left empty. */ }

        public UIntCountableEnumerableArray(in CountableEnumerableArray<T> array) => _array = array;

        public uint Count => (uint)_array.Count;

        public
#if WinCopies3
            IUIntCountableEnumeratorInfo<T>
#else
         System.Collections.Generic.IEnumerator<T> 
#endif
            GetEnumerator() =>
#if WinCopies3
            new UIntCountableEnumerator<T>(
#endif
                _array.GetEnumerator()
#if WinCopies3
                )
#endif
            ;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if WinCopies3
        IUIntCountableEnumerator<T> IUIntCountableEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif
    }
}
