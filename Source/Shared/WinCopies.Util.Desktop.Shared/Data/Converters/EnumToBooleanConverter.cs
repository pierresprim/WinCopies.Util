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

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Data converter for checking whether an enum equals a parameter.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class EnumToBooleanConverter : TwoWayConverterBase<Enum, Enum, bool>
    {
        /// <summary>
        /// No <see langword="null"/> argument is allowed for value nor parameter.
        /// </summary>
        public override IReadOnlyConversionOptions ConvertOptions => NotNull;

        /// <summary>
        /// No <see langword="null"/> argument is allowed for value nor parameter.
        /// </summary>
        public override IReadOnlyConversionOptions ConvertBackOptions => NotNull;

        /// <summary>
        /// Checks if an enum value equals a parameter.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="parameter">The converter parameter to use. This represents the value to compare with the value.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <param name="result">A value indicating whether <paramref name="value"/> is equal to <paramref name="parameter"/>.</param>
        /// <returns>Always <see langword="true"/>.</returns>
        protected override bool Convert(Enum value, Enum parameter, CultureInfo culture, out bool result)
        {
            // if (targetType != typeof(System.Boolean)) throw new ArgumentException("The targetType is not System.Boolean.");

            result = value.Equals(parameter);

            return true;
        }

        protected override bool ConvertBack(bool value, Enum parameter, CultureInfo culture, out Enum result)
        {
            if (value)
            {
                result = parameter;

                return true;
            }

            result = null;

            return false;
        }
    }

    /// <summary>
    /// Data converter for checking whether an enum is not equal to a parameter.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    public class EnumToReversedBooleanConverter : EnumToBooleanConverter
    {
        protected override bool Convert(Enum value, Enum parameter, CultureInfo culture, out bool result)
        {
            bool returnValue = base.Convert(value, parameter, culture, out result);

            result = !result;

            return returnValue;
        }

        protected override bool ConvertBack(bool value, Enum parameter, CultureInfo culture, out Enum result) => base.ConvertBack(!value, parameter, culture, out result);
    }
}
