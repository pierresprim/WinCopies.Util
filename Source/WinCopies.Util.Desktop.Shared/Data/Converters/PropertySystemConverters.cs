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

#if WinCopies3

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util.Data;

namespace WinCopies.PropertySystem
{
    public class PropertyArrayToListConverter : MultiConverterBase<object, IProperty, object, IList<IProperty>>
    {
        public override ConversionOptions ConvertOptions => ConverterHelper.ParameterCanBeNull;

        public override ConversionOptions ConvertBackOptions => throw new NotSupportedException();

        public override ConversionWays Direction => ConversionWays.OneWay;

        protected override ArrayBuilder<IProperty> Convert(System.Collections.Generic.IEnumerable<object> values, object parameter, CultureInfo culture)
        {
            foreach (object value in values)

                if (!(value is System.Collections.Generic.IEnumerable<IProperty>))

                    return new ArrayBuilder<IProperty>();

            return new ArrayBuilder<IProperty>(values.To<System.Collections.Generic.IEnumerable<IProperty>>().TryMerge());
        }

        protected override bool Convert(ArrayBuilder<IProperty> value, object parameter, CultureInfo culture, out IList<IProperty> result)
        {
            result = value.ToList();

            return true;
        }

        protected override bool[] ConvertBack(IList<IProperty> value, object parameter, CultureInfo culture, out IQueue<IProperty> result) => throw new InvalidOperationException();
    }

    [ValueConversion(typeof(object), typeof(IPropertySystemCollection<ReflectionPropertyId, object>))]
    public class ObjectToPropertyCollectionConverter : AlwaysConvertibleOneWayConverter<object, object, IPropertySystemCollection<ReflectionPropertyId, object>>
    {
        public override ConversionOptions ConvertOptions => ConverterHelper.ParameterCanBeNull;

        protected override IPropertySystemCollection<ReflectionPropertyId, object> Convert(object value, object parameter, CultureInfo culture) => new ReflectionPropertyCollection(value);
    }
}
#endif
