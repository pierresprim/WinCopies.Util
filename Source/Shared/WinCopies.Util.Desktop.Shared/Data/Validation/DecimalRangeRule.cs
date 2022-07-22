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
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using SystemPropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace WinCopies.Util.Data
{
    /*public class DecimalConversionResult
    {
        private readonly decimal? _value;

        public bool ConversionSucceeded { get; private set; }

        public decimal? Value => ConversionSucceeded ? _value : throw new InvalidOperationException(WinCopies.
#if !WinCopies3
            Util.
#endif
            Desktop.Resources.ExceptionMessages.ConversionDidNotSucceeded);

        private static DecimalConversionResult _invalidValue;

        public static DecimalConversionResult InvalidValue => _invalidValue
#if CS8
            ??=
#else
            ?? (_invalidValue =
#endif
            new DecimalConversionResult()
#if !CS8
            )
#endif
            ;

        private DecimalConversionResult() { /* Left empty. */ /*}

        public DecimalConversionResult(decimal? value)
        {
            ConversionSucceeded = true;

            _value = value;
        }

        public override string ToString() => ConversionSucceeded ? _value.ToString() : "NaN";
    }*/

    public class DecimalRangeRuleData : DependencyObject
    {
        private static DependencyProperty Register(string propertyName, FuncIn<DependencyObject, decimal, bool> predicate) => Desktop.UtilHelpers.Register<decimal, DependencyObject>(propertyName, new PropertyMetadata(0m, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        {
            var value = (decimal)e.NewValue;

            if (predicate(d, value))

                throw GetException(value, nameof(propertyName));
        }));

        public static readonly DependencyProperty MinValueProperty = Register(nameof(MinValue), (in DependencyObject d, in decimal value) => value > (decimal)d.GetValue(MaxValueProperty));

        public decimal MinValue { get => (decimal)GetValue(MinValueProperty); set => SetValue(MinValueProperty, value); }

        public static readonly DependencyProperty MaxValueProperty = Register(nameof(MaxValue), (in DependencyObject d, in decimal value) => value < (decimal)d.GetValue(MinValueProperty));

        public decimal MaxValue { get => (decimal)GetValue(MaxValueProperty); set => SetValue(MaxValueProperty, value); }

        public bool AllowInnerNullValue { get; set; }

        protected static ArgumentOutOfRangeException GetException(in decimal value, in string propertyName) => new ArgumentOutOfRangeException(propertyName, value, $"{nameof(MinValue)} must be less than or equal to {nameof(MaxValue)} and {nameof(MaxValue)} must be greater than or equal to {nameof(MinValue)}.");
    }

    public class DecimalRangeRule : ValidationRule<decimal>
    {
        public DecimalRangeRuleData
#if CS8
            ?
#endif
            Data
        { get; set; }

        public override bool
#if WinCopies3
            AllowNullValue
#else
            AllowNullValueOverride
#endif
            => false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool UpdateValue<T>(ref T value, in T newValue, SystemPropertyChangedEventArgs e) =>
#if WinCopies3
            UtilHelpers
#else
            Util
#endif
            .UpdateValue(ref value, newValue, () => PropertyChanged?.Invoke(this, e));

        protected virtual bool UpdateValue<T>(ref T value, in T newValue, in string propertyName) => UpdateValue(ref value, newValue, new SystemPropertyChangedEventArgs(propertyName));

        protected override ValidationResult Validate(decimal value, CultureInfo cultureInfo) => Data == null ? throw new InvalidOperationException($"{nameof(Data)} was null.") : /*value.ConversionSucceeded
            ? value.Value.HasValue*/
                /*?*/ value/*.Value*/ >= Data.MinValue && value/*.Value*/ <= Data.MaxValue
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, $"The value must be greater than {Data.MinValue} and less than {Data.MaxValue}.")
        /*: Data.AllowInnerNullValue
        ? ValidationResult.ValidResult
        : new ValidationResult(false, "The value cannot be empty.")
    : new ValidationResult(false, "Illegal characters.")*/;
    }
}
