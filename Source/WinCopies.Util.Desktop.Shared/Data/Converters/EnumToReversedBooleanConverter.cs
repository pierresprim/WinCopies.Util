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
    /// Data converter for checking whether an enum is not equal to a parameter.
    /// </summary>
#if WinCopies2
    /// <remarks>This class can also work for numeric types (int, ...)</remarks>
#else
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
#endif
    public class EnumToReversedBooleanConverter :
#if WinCopies3
        EnumToBooleanConverter
    {
        protected override bool Convert(Enum value, Enum parameter, CultureInfo culture, out bool result)
        {
            bool returnValue = base.Convert(value, parameter, culture, out result);

            result = !result;

            return returnValue;
        }

        protected override bool ConvertBack(bool value, Enum parameter, CultureInfo culture, out Enum result) => base.ConvertBack(!value, parameter, culture, out result);
#else
        ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !value.Equals(parameter);

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value ? parameter : Binding.DoNothing;
#endif
    }
}
