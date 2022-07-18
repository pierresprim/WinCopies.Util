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

#if !WinCopies3
using System;
using System.Globalization;
using System.Windows.Controls;

namespace WinCopies.Util.Data
{
    [Obsolete("Use WinCopies.Util.Data.DecimalRangeRule instead.")]
    public class IntRangeRule : ValidationRule<IntConversionResult>
    {
        private int _min = int.MinValue;
        private int _max = int.MaxValue;

        public bool AllowInnerNullValue { get; set; }

        public override bool AllowNullValueOverride => false;

        protected ArgumentOutOfRangeException GetException(in string propertyName) => new ArgumentOutOfRangeException(propertyName, $"{nameof(Min)} must be less than or equal to {nameof(Max)} and {nameof(Max)} must be greater than or equal to {nameof(Min)}.");

        public int Min { get => _min; set => _min = value <= _max ? value : throw GetException(nameof(Min)); }

        public int Max { get => _max; set => _max = value >= _min ? value : throw GetException(nameof(Max)); }

        protected override ValidationResult Validate(IntConversionResult value, CultureInfo cultureInfo) => value.ConversionSucceeded
            ? value.Value.HasValue
                ? value.Value >= _min && value.Value <= _max
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, $"The value must be less than {nameof(Max)} and greater than {nameof(Min)}.")
                : AllowInnerNullValue ? ValidationResult.ValidResult : new ValidationResult(false, "The value cannot be empty.")
            : new ValidationResult(false, "Illegal characters.");
    }
}
#endif
