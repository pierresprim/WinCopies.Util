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

namespace WinCopies.Util.Data
{

    /// <summary>
    /// Provides a converter for conversion from a <see langword="bool"/> value to a <see cref="Visibility"/> value.
    /// </summary>
    [ValueConversion(typeof(Visibility), typeof(bool), ParameterType = typeof(Visibility))]
    public class VisibilityToBooleanConverter : ConverterBase
    {

        /// <summary>
        /// Converts a <see cref="Visibility"/> value to a <see langword="bool"/> value. If the value is <see cref="Visibility.Visible"/>, the returned value will be <see langword="true"/>, otherwise false.
        /// </summary>
        /// <param name="value">The <see cref="Visibility"/> value to convert.</param>
        /// <param name="targetType">The target type of the value. This parameter isn't evaluated in this converter.</param>
        /// <param name="parameter">The parameter of this converter. This parameter isn't evaluated in this converter.</param>
        /// <param name="culture">The culture used for the conversion. This parameter isn't evaluated in this converter.</param>
        /// <returns><see langword="true"/> if the value to convert is <see cref="Visibility.Visible"/>, otherwise false.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (Visibility)value == Visibility.Visible;

        /// <summary>
        /// Converts a <see langword="bool"/> value to a <see cref="Visibility"/> value. If the value is <see langword="true"/>, the returned value will be the <see cref="Visibility.Visible"/> value, if not and if parameter is not null, it will be the value of the parameter, otherwise it will be <see cref="Visibility.Collapsed"/>.
        /// </summary>
        /// <param name="value">The <see langword="bool"/> value to convert.</param>
        /// <param name="targetType">The target type of the value. This parameter isn't evaluated in this converter.</param>
        /// <param name="parameter">The value to return if the value to convert is false. This parameter can't be the <see cref="Visibility.Visible"/> value. This parameter can be null.</param>
        /// <param name="culture">The culture used for the conversion. This parameter isn't evaluated in this converter.</param>
        /// <returns><see cref="Visibility.Visible"/> if the value to convert is <see langword="true"/>, if not, the value of the parameter if it is not null, otherwise <see cref="Visibility.Collapsed"/>.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter != null && (!(parameter is Visibility) || (Visibility)parameter == Visibility.Visible))

                // todo:

                throw new ArgumentException("parameter must be a value of the System.Windows.Visibility enum and can't be the System.Windows.Visibility.Visible value.");

            return (bool)value ? Visibility.Visible : parameter ?? Visibility.Collapsed;

        }
    }
}

 
