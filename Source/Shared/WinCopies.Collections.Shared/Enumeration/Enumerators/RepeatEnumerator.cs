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
using WinCopies.Collections.Enumeration.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    public class ConditionalEnumerator<T> : Enumerator<T
#if CS9
        ?
#endif
        >,
        IEnumerable<T
#if CS9
            ?
#endif
            , ConditionalEnumerator<T
#if CS9
            ?
#endif
            >>
    {
        private Nullable<T
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

        public override bool? IsResetSupported => GetValueIfNotDisposed(true);

        public bool SupportsReversedEnumeration => GetValueIfNotDisposed(true);

        public ConditionalEnumerator(in Nullable<T
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
            value, in Func<bool> condition) : this((Nullable<T
#if CS9
                ?
#endif
                >)value, condition)
        { /* Left empty. */ }

        protected TValue
#if CS9
            ?
#endif
            GetValueIfNotDisposed<TValue>(in TValue
#if CS9
            ?
#endif
            value) => _value.HasValue ? value : throw GetExceptionForDispose(false);

        protected sealed override bool MoveNextOverride() => GetValueIfNotDisposed(_condition)();

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _value = new Nullable<T>();
            _condition = null;
        }

        protected override void ResetOverride2() { /* Left empty. */ }

        public ConditionalEnumerator<T
#if CS9
            ?
#endif
            > GetEnumerator() => GetValueIfNotDisposed(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#if !CS8
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif
    }

    public sealed class RepeatEnumerator<T> : ConditionalEnumerator<T>, IUIntCountableEnumerator<T>, ICountableEnumerator<T>
    {
        private Condition _condition;

        uint IUIntCountable.Count => GetValueIfNotDisposed(_condition).Count;

        int ICountable.Count => GetValueIfNotDisposed(_condition).Count > int.MaxValue ? throw new InvalidOperationException($"The number of items is greater than {nameof(Int32)}.{nameof(int.MaxValue)}.") : (int)_condition.Count;

        private RepeatEnumerator(in Nullable<T> value, in Condition condition) : base(value, condition.GetCondition) => _condition = condition;

        private RepeatEnumerator(in T value, in Condition condition) : base(new Nullable<T>(value), condition.GetCondition) => _condition = condition;

        public static RepeatEnumerator<T> Get(in Nullable<T> value, uint count) => new
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

        protected override void ResetOverride2() => GetValueIfNotDisposed(_condition).Reset();
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
