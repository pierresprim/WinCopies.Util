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

using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Util;

namespace WinCopies.Collections
{
    namespace DotNetFix.Generic
    {
        public class UIntCountableEnumerable<T> : Enumeration.Generic.UIntCountableEnumerable<IUIntCountableEnumerable<T>, T>
        {
            public UIntCountableEnumerable(in IUIntCountableEnumerable<T> enumerable) : base(enumerable) { /* Left empty. */ }
        }
    }

    namespace Generic
    {
        public abstract class UIntCountableEnumerable<TEnumerable, TEnumerator, TItems> : IUIntCountableEnumerable<TItems> where TEnumerable : ICountableEnumerable<TItems, TEnumerator> where TEnumerator : ICountableEnumerator<TItems>
        {
            protected TEnumerable Enumerable { get; }
            protected ICountable Countable => Enumerable;

            public uint Count => (uint)Countable.Count;

            public UIntCountableEnumerable(in TEnumerable enumerable) => Enumerable = enumerable;

            public IUIntCountableEnumeratorInfo<TItems> GetEnumerator() => new UIntCountableEnumeratorInfo<TItems>(new EnumeratorInfo<TItems>(Enumerable), () => (uint)Countable.Count);

            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            IUIntCountableEnumerator<TItems> Enumeration.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();
        }

        public class UIntCountableEnumerable<T> : UIntCountableEnumerable<ICountableEnumerable<T>, ICountableEnumerator<T>, T>
        {
            public UIntCountableEnumerable(in ICountableEnumerable<T> enumerable) : base(enumerable)
            {
                // Left empty.
            }
        }

        public class UIntCountableEnumerableInfo<T> : UIntCountableEnumerable<ICountableEnumerableInfo<T>, ICountableEnumeratorInfo<T>, T>, IUIntCountableEnumerableInfo<T>
        {
            protected Extensions.IEnumerable<System.Collections.Generic.IEnumerator<T>> Enumerable2 => Enumerable;

            public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

            public UIntCountableEnumerableInfo(in ICountableEnumerableInfo<T> enumerable) : base(enumerable)
            {
                // Left empty.
            }

            public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => new UIntCountableEnumeratorInfo<T>(new EnumeratorInfo<T>(Enumerable.AsFromType<Extensions.IEnumerable<ICountableEnumeratorInfo<T>>>().GetReversedEnumerator()), () => (uint)Countable.Count);
            System.Collections.Generic.IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => Enumerable2.GetReversedEnumerator();
            IEnumerator Extensions.IEnumerable.GetReversedEnumerator() => Enumerable2.GetReversedEnumerator();
        }
    }
}
