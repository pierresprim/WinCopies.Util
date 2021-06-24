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

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    public class CoalesceConverter : AlwaysConvertibleTwoWayConverter<object, object, object>
    {
        public override
#if WinCopies3
            IReadOnlyConversionOptions
#else
            ConversionOptions
#endif
            ConvertOptions => AllowNull;

        public override
#if WinCopies3
            IReadOnlyConversionOptions
#else
            ConversionOptions
#endif
           ConvertBackOptions => AllowNull;

        protected override object Convert(object value, object parameter, CultureInfo culture) => value ?? parameter;

        protected override object ConvertBack(object value, object parameter, CultureInfo culture) => value ?? parameter;
    }

    public class CoalesceMultiConverter :
#if WinCopies3
        AlwaysConvertibleOneWayMultiConverter<object, object>
#else
        MultiConverterBase
#endif
    {
#if WinCopies3
        public override IReadOnlyConversionOptions ConvertOptions => AllowNull;
#endif

#if WinCopies3
protected
#else
        public
#endif
            override object Convert(object[] values,
#if !WinCopies3
        Type targetType,
#endif
            object parameter, CultureInfo culture)
        {
            foreach (object value in values)

                if (value != null)

                    return value;

            return parameter;
        }

#if !WinCopies3
        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotSupportedException();
#endif
    }
}
