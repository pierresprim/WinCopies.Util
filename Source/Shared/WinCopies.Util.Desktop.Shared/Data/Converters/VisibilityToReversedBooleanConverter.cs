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
using System.Windows;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(Visibility), typeof(bool), ParameterType = typeof(Visibility))]
    public class VisibilityToReversedBooleanConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (Visibility)value == Visibility.Visible;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => parameter != null && !(parameter is Visibility) ?

            // todo:

            throw new ArgumentException("parameter must be a value of the System.Windows.Visibility enum.") : (bool)value ? parameter ?? Visibility.Collapsed : Visibility.Visible;
    }
}
