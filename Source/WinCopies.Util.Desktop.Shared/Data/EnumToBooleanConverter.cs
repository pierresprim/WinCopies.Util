/* Copyright © Pierre Sprimont, 2019
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
using System.Windows.Data;

namespace WinCopies.Util.Data
{

    /// <summary>
    /// Data converter for checking wheter an enum equals a parameter.
    /// </summary>
    /// <remarks>This class can also work for numeric types (int, ...)</remarks>
    public class EnumToBooleanConverter : ConverterBase
    {

        /// <summary>
        /// Checks if an enum value equals a parameter.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property. This type must be <see cref="System.Boolean"/>.</param>
        /// <param name="parameter">The converter parameter to use. This represents the value to compare with the value.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>

            // if (targetType != typeof(System.Boolean)) throw new ArgumentException("The targetType is not System.Boolean.");

            value.Equals(parameter);

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? parameter : Binding.DoNothing;

    }

}

 
