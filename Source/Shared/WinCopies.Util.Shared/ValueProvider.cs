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

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    public interface IValueProvider<T1, T2>
    {
        T1 ProvideValues(out T2 result);
    }

    public abstract class ValueProvider<T1, T2> : IValueProvider<T1, T2>
    {
        private readonly T1 _value;

        public ValueProvider(in T1 value) => _value = value;

        protected abstract T2 ProvideSecondValue(in T1 value);

        protected virtual void ValidateValue(in T1 value) { /* Left empty. */ }

        protected virtual void ValidateResultValue(in T2 value) { /* Left empty. */ }

        public T1 ProvideValues(out T2 result)
        {
            ValidateValue(_value);

            T2 _result = ProvideSecondValue(_value);

            ValidateResultValue(_result);

            result = _result;

            return _value;
        }

    }

    public class DelegateValueProvider<T1, T2> : ValueProvider<T1, T2>
    {
        private readonly Func<T1, T2> _func;

        public DelegateValueProvider(in T1 value, in Func<T1, T2> func) : base(value) => _func = func ?? throw
#if WinCopies3
            ThrowHelper
#else
            Util
#endif
            .GetArgumentNullException(nameof(func));

        protected sealed override T2 ProvideSecondValue(in T1 value) => _func(value);
    }
}
