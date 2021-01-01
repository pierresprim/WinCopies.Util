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
using System.Globalization;

namespace WinCopies.Util.Data
{
    public class IntConversionResult
    {
        private readonly int? _value;

        public bool ConversionSucceeded { get; private set; }

        public int? Value => ConversionSucceeded ? _value : throw new InvalidOperationException("Conversion did not succeeded.");

        private static IntConversionResult _invalidValue;

        public static IntConversionResult InvalidValue => _invalidValue ?? (_invalidValue = new IntConversionResult());

        private IntConversionResult() { /* Left empty. */ }

        public IntConversionResult(int? value)
        {
            ConversionSucceeded = true;

            _value = value;
        }

        public override string ToString() => ConversionSucceeded ? _value.ToString() : "NaN";
    }

    public class StringToIntConversionResultConverter : AlwaysConvertibleTwoWayConverter<string, object, IntConversionResult>
    {
        public override ConversionOptions ConvertOptions => AllowNull;

        public override ConversionOptions ConvertBackOptions => ParameterCanBeNull;

        protected override IntConversionResult Convert(string value, object parameter, CultureInfo culture) => value == null || value.Length == 0 ? new IntConversionResult(null) : int.TryParse(value, out int _value) ? new IntConversionResult(_value) : IntConversionResult.InvalidValue;

        protected override string ConvertBack(IntConversionResult value, object parameter, CultureInfo culture) => value.ToString();
    }
}
