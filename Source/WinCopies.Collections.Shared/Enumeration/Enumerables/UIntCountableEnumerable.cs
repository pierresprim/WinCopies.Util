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

#if WinCopies3

using System.Collections;

using WinCopies.Collections.DotNetFix.Generic;

namespace WinCopies.Collections.Generic
{
    public abstract class UIntCountableEnumerable<TEnumerable, TItems> : IUIntCountableEnumerable<TItems> where TEnumerable : ICountableEnumerable<TItems>
    {
        protected TEnumerable Enumerable { get; }

        public uint Count => (uint)Enumerable.Count;

        public UIntCountableEnumerable(in TEnumerable enumerable) => Enumerable = enumerable;

        public IUIntCountableEnumeratorInfo<TItems> GetEnumerator() => new UIntCountableEnumeratorInfo<TItems>(new EnumeratorInfo<TItems>(Enumerable), () => (uint)Enumerable.Count);

        System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IUIntCountableEnumerator<TItems> IUIntCountableEnumerable<TItems>.GetEnumerator() => GetEnumerator();
    }

    public class UIntCountableEnumerable<T> : UIntCountableEnumerable<ICountableEnumerable<T>, T>
    {
        public UIntCountableEnumerable(in ICountableEnumerable<T> enumerable) : base(enumerable)
        {
            // Left empty.
        }
    }

    public class UIntCountableEnumerableInfo<T> : UIntCountableEnumerable<ICountableEnumerableInfo<T>, T>, IUIntCountableEnumerableInfo<T>
    {
        public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

        public UIntCountableEnumerableInfo(in ICountableEnumerableInfo<T> enumerable) : base(enumerable)
        {
            // Left empty.
        }

        public IEnumeratorInfo2<T> GetReversedEnumerator() => Enumerable.GetReversedEnumerator();

        IEnumeratorInfo2<T> Collections.DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo2<T>>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();
    }
}

#endif
