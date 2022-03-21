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
    /// Determines whether an object is null. You can set to <see langword="true"/> the parameter of the <see cref="Binding"/> that will use this converter to get a reversed <see cref="bool"/>.
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter :
#if WinCopies3
        AlwaysConvertibleOneWayConverter<object, bool, bool>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ConverterHelper.AllowNull;

        protected override bool Convert(object value, bool parameter, CultureInfo culture)
        {
            bool result = value is null;

            if (parameter)

                result = !result;

            return result;
        }
#else
        ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value is null;
            
            if (parameter is bool _parameter && _parameter == true)
            
                result = !result;

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
#endif
    }
}
