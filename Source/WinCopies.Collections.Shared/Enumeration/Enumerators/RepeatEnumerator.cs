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
using System.Collections;
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;

#if WinCopies3
using static WinCopies.ThrowHelper;
#else
using WinCopies.Util;

using static WinCopies.Util.Util;
#endif

namespace WinCopies.Collections.Generic
{
    public class ConditionalEnumerator<T> : Enumerator<T>,
#if WinCopies3
IEnumerable<T, ConditionalEnumerator<T>>
#else
        System.Collections.Generic.IEnumerable<T>
#endif
    {
        private NullableGeneric<T> _value;
        private Func<bool> _condition;

        protected T Value => GetValueIfNotDisposed(_value).Value;

        protected Func<bool> Condition => GetValueIfNotDisposed(_condition);

        protected override T CurrentOverride => GetValueIfNotDisposed(_value.Value);

#if WinCopies3
        public override bool? IsResetSupported => GetValueIfNotDisposed(true);

        public bool SupportsReversedEnumeration => GetValueIfNotDisposed(true);
#endif

        public ConditionalEnumerator(in NullableGeneric<T> value, in Func<bool> condition)
        {
            _value = value;

            _condition = condition ?? throw GetArgumentNullException(nameof(condition));
        }

        public ConditionalEnumerator(in T value, in Func<bool> condition) : this(new NullableGeneric<T>(value), condition) { /* Left empty. */ }

        protected TValue GetValueIfNotDisposed<TValue>(in TValue value) => _value == null ? throw GetExceptionForDispose(false) : value;

        protected sealed override bool MoveNextOverride() => _value == null ? throw GetExceptionForDispose(false) : _condition();

        protected override void
#if WinCopies3
            DisposeManaged()
        {
            base.DisposeManaged();
#else
            Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
#endif
                _value = null;
                _condition = null;
#if !WinCopies3
            }
#endif
        }

#if WinCopies3
        private ConditionalEnumerator<T> GetEnumerator() => GetValueIfNotDisposed(this);

        ConditionalEnumerator<T> DotNetFix.Generic.IEnumerable<T, ConditionalEnumerator<T>>.GetEnumerator() => GetEnumerator();

        ConditionalEnumerator<T> IEnumerable<T, ConditionalEnumerator<T>>.GetReversedEnumerator() => GetEnumerator();

#if !CS8
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetReversedEnumerator() => GetEnumerator();

        IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator Enumeration.IEnumerable.GetReversedEnumerator() => GetEnumerator();
#endif
#else
        public IEnumerator<T> GetEnumerator() => GetValueIfNotDisposed(this);

        IEnumerator IEnumerable.GetEnumerator() => GetValueIfNotDisposed(this);
#endif
    }

    public sealed class RepeatEnumerator<T> :
#if WinCopies3
        ConditionalEnumerator<T>
#else
Enumerator<T>, System.Collections.Generic.IEnumerable<T>
#endif
        , IUIntCountableEnumerator<T>, ICountableEnumerator<T>
    {
#if WinCopies3
        private Condition _condition;
#else
        private NullableGeneric<T> _value;
        private readonly uint _count;
        private uint _i;

        private TValue GetValueIfNotDisposed<TValue>(in TValue value) => _value == null ? throw GetExceptionForDispose(false) : value;

        protected override T CurrentOverride => GetValueIfNotDisposed(_value.Value);

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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _value = null;
        }
#endif

        uint IUIntCountable.Count => GetValueIfNotDisposed(
#if WinCopies3
            _condition).Count;
#else
            _count);
#endif

        int ICountable.Count => GetValueIfNotDisposed(
#if WinCopies3
            _condition).Count
#else
            _count)
#endif
        > int.MaxValue ? throw new InvalidOperationException($"The number of items is greater than {nameof(Int32)}.{nameof(int.MaxValue)}.") : (int)
#if WinCopies3
        _condition.Count;
#else
            _count;
#endif

#if WinCopies3
        private RepeatEnumerator(in NullableGeneric<T> value, in Condition condition) : base(value, condition.GetCondition) => _condition = condition;

        private RepeatEnumerator(in T value, in Condition condition) : base(new NullableGeneric<T>(value), condition.GetCondition) => _condition = condition;

        public static RepeatEnumerator<T> Get(in NullableGeneric<T> value, uint count) => new
#if !CS9
            RepeatEnumerator<T>
#endif
            (value, new Condition(count));

        public static RepeatEnumerator<T> Get(in T value, uint count) => new
#if !CS9
            RepeatEnumerator<T>
#endif
            (value, new Condition(count));

        private struct Condition
        {
            public uint Index { get; private set; }

            public uint Count { get; }

            public Condition(in uint count)
            {
                Count = count;

                Index = 0;
            }

            public bool GetCondition()
            {
                if (Index == Count)

                    return false;

                else
                {
                    Index++;

                    return true;
                }
            }

            public void Reset() => Index = 0u;
        }
#else
        public RepeatEnumerator(in NullableGeneric<T> value, in uint count)
        {
            _value = value;

            _count = count;
        }

        public RepeatEnumerator(in T value, in uint count) : this(new NullableGeneric<T>(value), count)
        {
            // Left empty.
        }
#endif

        protected override void ResetOverride()
        {
            base.ResetOverride();

#if WinCopies3
            GetValueIfNotDisposed(_condition).Reset();
#else
            _i = _value == null ? throw GetExceptionForDispose(false) : 0u;
#endif
        }

#if !WinCopies3
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetValueIfNotDisposed(this);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetValueIfNotDisposed(this);
#endif
    }
}
