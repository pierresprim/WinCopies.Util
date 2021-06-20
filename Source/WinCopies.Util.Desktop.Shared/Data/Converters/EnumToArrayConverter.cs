﻿/* Copyright © Pierre Sprimont, 2021
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
    [ValueConversion(typeof(object), typeof(Array), ParameterType = typeof(Type))]
    public class EnumToArrayConverter :
#if WinCopies3
        AlwaysConvertibleOneWayConverter<object, Type, Array>
    {
        /// <summary>
        /// <para>Value: ignored.</para>
        /// <para>Parameter: cannot be <see langword="null"/>.</para>
        /// </summary>
        public override IReadOnlyConversionOptions ConvertOptions => ConverterHelper.ValueCanBeNull;

        protected override Array Convert(object value, Type parameter, CultureInfo culture) => parameter.IsEnum ? parameter.GetEnumValues() : null;
#else
        ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => parameter is Type t && t.IsEnum ? t.GetEnumValues() : null;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
#endif
    }
}
