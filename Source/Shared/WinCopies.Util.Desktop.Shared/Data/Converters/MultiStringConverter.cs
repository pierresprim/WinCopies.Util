/* Copyright © Pierre Sprimont, 2020
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
using System.Linq;

using WinCopies.Collections;
using WinCopies.Linq;

namespace WinCopies.Util.Data
{
    [MultiValueConversion(typeof(string), ParameterType = typeof(string))]
    public class MultiStringConverter : AlwaysConvertibleOneWayMultiConverter<string, string>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ConverterHelper.AllowNull;

        protected override string Convert(object[] values, string parameter, CultureInfo culture) => values == null ? parameter : parameter == null ? values.ConcatenateString2() : string.Format(culture, parameter, values);
    }

    public class StringMergerConverter : AlwaysConvertibleOneWayMultiConverter<string, string>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ConverterHelper.AllowNull;

        protected override string Convert(object[] values, string parameter, CultureInfo culture) => values?.Cast<string>().Join(false, parameter);
    }
}
