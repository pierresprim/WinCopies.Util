﻿/* Copyright © Pierre Sprimont, 2019
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
using System.Windows;
using System.Windows.Data;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    // todo: already exists in the System.Windows.Controls namespace with a bit of features less.

    /// <summary>
    /// Provides a converter for conversion from a <see langword="bool"/> value to a <see cref="Visibility"/> value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(Visibility))]
    public class BooleanToVisibilityConverter : AlwaysConvertibleTwoWayConverter<bool, Visibility?, Visibility>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;
        public override IReadOnlyConversionOptions ConvertBackOptions => ParameterCanBeNull;

        /// <summary>
        /// Converts a <see langword="bool"/> value to a <see cref="Visibility"/> value. If the value is <see langword="true"/>, the returned value will be the <see cref="Visibility.Visible"/> value, if not and if parameter is not null, it will be the value of the parameter, otherwise it will be <see cref="Visibility.Collapsed"/>.
        /// </summary>
        /// <param name="value">The <see langword="bool"/> value to convert.</param>
        /// <param name="targetType">The target type of the value. This parameter isn't evaluated in this converter.</param>
        /// <param name="parameter">The value to return if the value to convert is false. This parameter can't be the <see cref="Visibility.Visible"/> value. This parameter can be null.</param>
        /// <param name="culture">The culture used for the conversion. This parameter isn't evaluated in this converter.</param>
        /// <returns><see cref="Visibility.Visible"/> if the value to convert is <see langword="true"/>, if not, the value of the parameter if it is not null, otherwise <see cref="Visibility.Collapsed"/>.</returns>
        protected override Visibility Convert(bool value, Visibility? parameter, CultureInfo culture) => parameter != null && parameter == Visibility.Visible ?

                // todo:

                throw ThrowHelper.GetArgumentMustBeFromEnumAndNotValueException(nameof(parameter), $"{nameof(System)}.{nameof(System.Windows)}.{nameof(Visibility)}", parameter.Value, Visibility.Visible) : value ? Visibility.Visible : parameter ?? Visibility.Collapsed;

        /// <summary>
        /// Converts a <see cref="Visibility"/> value to a <see langword="bool"/> value. If the value is <see cref="Visibility.Visible"/>, the returned value will be <see langword="true"/>, otherwise false.
        /// </summary>
        /// <param name="value">The <see cref="Visibility"/> value to convert.</param>
        /// <param name="targetType">The target type of the value. This parameter isn't evaluated in this converter.</param>
        /// <param name="parameter">The parameter of this converter. This parameter isn't evaluated in this converter.</param>
        /// <param name="culture">The culture used for the conversion. This parameter isn't evaluated in this converter.</param>
        /// <returns><see langword="true"/> if the value to convert is <see cref="Visibility.Visible"/>, otherwise false.</returns>
        protected override bool ConvertBack(Visibility value, Visibility? parameter, CultureInfo culture) =>            value == Visibility.Visible;
    }

    [ValueConversion(typeof(bool?), typeof(bool), ParameterType = typeof(bool))]
    public class NullableBooleanToBooleanConverter : AlwaysConvertibleTwoWayConverter<bool?, bool, bool>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ValueCanBeNull;

        public override IReadOnlyConversionOptions ConvertBackOptions => ValueCanBeNull;

        protected override bool Convert(bool? value, bool parameter, CultureInfo culture) => value ?? parameter;
        protected override bool? ConvertBack(bool value, bool parameter, CultureInfo culture) => value;
    }

    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(Visibility))]
    public class ReversedBooleanToVisibilityConverter :        BooleanToVisibilityConverter
    {
        protected override Visibility Convert(bool value, Visibility? parameter, CultureInfo culture) => base.Convert(!value, parameter, culture);
        protected override bool ConvertBack(Visibility value, Visibility? parameter, CultureInfo culture) => !base.ConvertBack(value, parameter, culture);
    }
}
