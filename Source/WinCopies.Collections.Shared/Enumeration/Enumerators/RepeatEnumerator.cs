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

using System;

using WinCopies.Collections.DotNetFix.Generic;

#if WinCopies3
using System.Collections;
using System.Collections.Generic;

using static WinCopies.ThrowHelper;
#else
using WinCopies.Util;

using static WinCopies.Util.Util;
#endif

namespace WinCopies.Collections.Generic
{
    public sealed class RepeatEnumerator<T> : Enumerator<T>, IUIntCountableEnumerator<T>, ICountableEnumerator<T>
#if WinCopies3
, IUIntCountableEnumerableInfo<T>, ICountableEnumerableInfo<T>
#endif
    {
        private NullableGeneric<T> _value;
        private readonly uint _count;
        private uint _i;

        private TValue GetValueIfNotDisposed<TValue>(in TValue value) => _value == null ? throw GetExceptionForDispose(false) : value;

        protected override T CurrentOverride => GetValueIfNotDisposed(_value.Value);

        uint IUIntCountable.Count => GetValueIfNotDisposed(_count);

        private int Count => GetValueIfNotDisposed(_count > int.MaxValue ? throw new InvalidOperationException($"The number of items is greater than {nameof(Int32)}.{nameof(int.MaxValue)}.") : (int)_count);

        int ICountable.Count => Count;

#if WinCopies3
#if CS7
        int IReadOnlyCollection<T>.Count => Count;

        int ICountableEnumerable<T>.Count => Count;
#endif

        public override bool? IsResetSupported => GetValueIfNotDisposed(true);

        bool Collections.Generic.IEnumerable<T>.SupportsReversedEnumeration => GetValueIfNotDisposed(true);
#endif

        public RepeatEnumerator(in NullableGeneric<T> value, in uint count)
        {
            _value = value;

            _count = count;
        }

        public RepeatEnumerator(in T value, in uint count) : this(new NullableGeneric<T>(value), count)
        {
            // Left empty.
        }

        protected override bool MoveNextOverride()
        {
            if (_value == null)

                throw GetExceptionForDispose(false);

            if (_i == _count)

                return false;

            else
            {
                _i++;

                return true;
            }
        }

        protected override void ResetOverride()
        {
            base.ResetOverride();

            _i = _value == null ? throw GetExceptionForDispose(false) : 0u;
        }

        void System.IDisposable.Dispose() => _value = null;

        private RepeatEnumerator<T> GetEnumerator() => GetValueIfNotDisposed(this);

        private ICountableEnumerator<T> GetCountableEnumerator() => GetEnumerator();

#if WinCopies3
        private IEnumeratorInfo2<T> GetEnumeratorInfo() => new EnumeratorInfo<T>(GetCountableEnumerator());

        ICountableEnumerator<T> ICountableEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumeratorInfo2<T> IEnumerable<T, IEnumeratorInfo2<T>>.GetReversedEnumerator() => GetEnumeratorInfo();

        IEnumeratorInfo2<T> DotNetFix.Generic.IEnumerable<T, IEnumeratorInfo2<T>>.GetEnumerator() => GetEnumeratorInfo();

        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetReversedEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IUIntCountableEnumerator<T> IUIntCountableEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif
    }
}
