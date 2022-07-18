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
    public class ConditionalEnumerator<T> : Enumerator<T
#if CS9
            ?
#endif
            >,
#if WinCopies3
        IEnumerable<T
#if CS9
            ?
#endif
            , ConditionalEnumerator<T
#if CS9
            ?
#endif
            >>
#else
        System.Collections.Generic.IEnumerable<T
#if CS9
            ?
#endif
            >
#endif
    {
        private
#if WinCopies3
            Nullable
#else
            NullableGeneric
#endif
            <T
#if CS9
            ?
#endif
            > _value;
        private Func<bool> _condition;

        protected T
#if CS9
            ?
#endif
             Value => GetValueIfNotDisposed(_value).Value;

        protected Func<bool> Condition => GetValueIfNotDisposed(_condition);

        protected override T CurrentOverride => GetValueIfNotDisposed(_value.Value);

#if WinCopies3
        public override bool? IsResetSupported => GetValueIfNotDisposed(true);

        public bool SupportsReversedEnumeration => GetValueIfNotDisposed(true);
#endif

        public ConditionalEnumerator(in
#if WinCopies3
            Nullable
#else
            NullableGeneric
#endif
            <T
#if CS9
            ?
#endif
            > value, in Func<bool> condition)
        {
            _value = value;

            _condition = condition ?? throw GetArgumentNullException(nameof(condition));
        }

        public ConditionalEnumerator(in T
#if CS9
            ?
#endif
            value, in Func<bool> condition) : this(
#if WinCopies3
            (Nullable
#else
            new NullableGeneric
#endif
            <T
#if CS9
                ?
#endif
                >
#if WinCopies3
            )
#endif
            (value), condition)
        { /* Left empty. */ }

        protected TValue
#if CS9
            ?
#endif
            GetValueIfNotDisposed<TValue>(in TValue
#if CS9
            ?
#endif
            value) => _value
#if WinCopies3
            .HasValue
#else
            == null
#endif
            ?
#if WinCopies3
            value :
#endif
            throw GetExceptionForDispose(false)
#if !WinCopies3
            : value
#endif
            ;

        protected sealed override bool MoveNextOverride() => GetValueIfNotDisposed(_condition)();

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
            _value =
#if WinCopies3
                new Nullable<T>()
#else
                    null
#endif
                ;
            _condition = null;
#if !WinCopies3
            }
#endif
        }

#if WinCopies3
        protected override void ResetOverride2() { /* Left empty. */ }

        private ConditionalEnumerator<T
#if CS9
            ?
#endif
            > GetEnumerator() => GetValueIfNotDisposed(this);

        ConditionalEnumerator<T
#if CS9
            ?
#endif
            > DotNetFix.Generic.IEnumerable<T
#if CS9
            ?
#endif
            , ConditionalEnumerator<T
#if CS9
            ?
#endif
            >>.GetEnumerator() => GetEnumerator();

        ConditionalEnumerator<T
#if CS9
            ?
#endif
            > IEnumerable<T
#if CS9
            ?
#endif
            , ConditionalEnumerator<T
#if CS9
            ?
#endif
            >>.GetReversedEnumerator() => GetEnumerator();

#if !CS8
        System.Collections.Generic.IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        System.Collections.Generic.IEnumerator<T> Extensions.Generic.IEnumerable<T>.GetReversedEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

        protected override T CurrentOverride => GetValueIfNotDisposed(_value.Value);

        private TValue GetValueIfNotDisposed<TValue>(in TValue value) => _value == null ? throw GetExceptionForDispose(false) : value;

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
        private RepeatEnumerator(in
#if WinCopies3
            Nullable
#else
            NullableGeneric
#endif
            <T> value, in Condition condition) : base(value, condition.GetCondition) => _condition = condition;

        private RepeatEnumerator(in T value, in Condition condition) : base(new
#if WinCopies3
            Nullable
#else
            NullableGeneric
#endif
            <T>(value), condition.GetCondition) => _condition = condition;

        public static RepeatEnumerator<T> Get(in
#if WinCopies3
            Nullable
#else
            NullableGeneric
#endif
            <T> value, uint count) => new
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

        public RepeatEnumerator(in T value, in uint count) : this(new NullableGeneric<T>(value), count) { /* Left empty. */ }

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetValueIfNotDisposed(this);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetValueIfNotDisposed(this);
#endif

        protected override void
#if WinCopies3
            ResetOverride2
#else
            ResetOverride
#endif
            ()
        {
#if WinCopies3
            GetValueIfNotDisposed(_condition).Reset();
#else
            base.ResetOverride();

            _i = _value == null ? throw GetExceptionForDispose(false) : 0u;
#endif
        }
    }

    public abstract class PredicateEnumerator<TItems, TEnumerator> : EnumeratorInfo<TItems, TEnumerator> where TEnumerator : System.Collections.Generic.IEnumerator<TItems>
    {
        public Predicate<TItems> Predicate { get; }

        public PredicateEnumerator(in TEnumerator enumerator, in Predicate<TItems> predicate) : base(enumerator) => Predicate = predicate;
        public PredicateEnumerator(in IEnumerable<TItems, TEnumerator> enumerable, in Predicate<TItems> predicate) : this(GetEnumerator(enumerable, nameof(enumerable)), predicate) { /* Left empty. */ }

        protected override bool MoveNextOverride() => base.MoveNextOverride() && Predicate(CurrentOverride);
    }

    public class PredicateEnumerator<T> : PredicateEnumerator<T, System.Collections.Generic.IEnumerator<T>>
    {
        public override bool? IsResetSupported => null;

        public PredicateEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator, in Predicate<T> predicate) : base(enumerator, predicate) { /* Left empty. */ }
        public PredicateEnumerator(in System.Collections.Generic.IEnumerable<T> enumerable, in Predicate<T> predicate) : this(GetEnumerator(enumerable, nameof(enumerable)), predicate) { /* Left empty. */ }
    }

    public class PredicateEnumeratorInfo<T> : PredicateEnumerator<T, IEnumeratorInfo<T>>
    {
        public override bool? IsResetSupported => InnerEnumerator.IsResetSupported;

        public PredicateEnumeratorInfo(in IEnumeratorInfo<T> enumerator, in Predicate<T> predicate) : base(enumerator, predicate) { /* Left empty. */ }

        public PredicateEnumeratorInfo(in IEnumerable<T, IEnumeratorInfo<T>> enumerable, in Predicate<T> predicate) : base(GetEnumerator(enumerable, nameof(enumerable)), predicate) { /* Left empty. */ }
    }
}
