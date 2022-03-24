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

            public uint Count => (uint)Enumerable.Count;

            public UIntCountableEnumerable(in TEnumerable enumerable) => Enumerable = enumerable;

            public IUIntCountableEnumeratorInfo<TItems> GetEnumerator() => new UIntCountableEnumeratorInfo<TItems>(new EnumeratorInfo<TItems>(Enumerable), () => (uint)Enumerable.Count);

            System.Collections.Generic.IEnumerator<TItems> System.Collections.Generic.IEnumerable<TItems>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            IUIntCountableEnumerator<TItems> IUIntCountableEnumerable<TItems, IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

#if !CS8
            IUIntCountableEnumerator<TItems> Enumeration.DotNetFix.IEnumerable<IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();

            IUIntCountableEnumerator<TItems> DotNetFix.Generic.IEnumerable<TItems, IUIntCountableEnumerator<TItems>>.GetEnumerator() => GetEnumerator();
#endif
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
            public bool SupportsReversedEnumeration => Enumerable.SupportsReversedEnumeration;

            public UIntCountableEnumerableInfo(in ICountableEnumerableInfo<T> enumerable) : base(enumerable)
            {
                // Left empty.
            }

            // public IEnumeratorInfo2<T> GetReversedEnumerator() => Enumerable.GetReversedEnumerator();

            public IUIntCountableEnumeratorInfo<T> GetReversedEnumerator() => new UIntCountableEnumeratorInfo<T>(new EnumeratorInfo<T>(Enumerable.GetReversedEnumerator()), () => (uint)Enumerable.Count);

            System.Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetReversedEnumerator() => GetReversedEnumerator();

            IUIntCountableEnumeratorInfo<T> IEnumerable<T, IUIntCountableEnumeratorInfo<T>>.GetReversedEnumerator() => GetReversedEnumerator();

#if !CS8
            IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetReversedEnumerator();
#endif
        }
    }
}
#endif
