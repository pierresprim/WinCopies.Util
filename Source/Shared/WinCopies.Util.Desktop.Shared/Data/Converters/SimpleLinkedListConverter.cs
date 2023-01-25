/* Copyright © Pierre Sprimont, 2021
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

using WinCopies.Collections.DotNetFix;
using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    [ValueConversion(typeof(ISimpleLinkedList), typeof(object))]
    public class SimpleLinkedListConverter : AlwaysConvertibleOneWayConverter<ISimpleLinkedList, object, object>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;

        protected override object Convert(ISimpleLinkedList value, object parameter, CultureInfo culture) => value.Peek();
    }

    public class SimpleLinkedListConverter<T> : AlwaysConvertibleOneWayConverter<ISimpleLinkedList<T>, object, T>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;

        protected override T Convert(ISimpleLinkedList<T> value, object parameter, CultureInfo culture) => value.Peek();
    }
}
