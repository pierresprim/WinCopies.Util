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

#if WinCopies3 && CS7
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;
using WinCopies.Util.Data;

namespace WinCopies.PropertySystem
{
    public class PropertyArrayToListConverter : MultiConverterBase7<object, IProperty, object, object, IList<IProperty>, IList<IProperty>>
    {
        private IConversionOptions _convertOptions = new ConversionOptions(false, true);

        public override IReadOnlyConversionOptions ConvertOptions => ConvertOptions2;

        public IConversionOptions ConvertOptions2 { get => _convertOptions; set => _convertOptions = value ?? throw ThrowHelper.GetArgumentNullException(nameof(value)); }

        public override IReadOnlyConversionOptions ConvertBackOptions => throw new NotSupportedException();

        public override ConversionWays Direction => ConversionWays.OneWay;

        protected override IConverterConverter<object, object> ParameterConverters { get; } = new ConverterConverter<object>();

        protected override IMultiConverterConverter<IList<IProperty>, IList<IProperty>> DestinationConverters { get; } = new MultiConverterConverter<IList<IProperty>>();

        protected override ArrayBuilder<IProperty> Convert(System.Collections.Generic.IEnumerable<object> values, in int valuesCount, object parameter, CultureInfo culture)
        {
            if (values == null)

                return new ArrayBuilder<IProperty>();

            foreach (object value in values)

                if (value != null && !(value is System.Collections.Generic.IEnumerable<IProperty>))

                    return new ArrayBuilder<IProperty>();

            return new ArrayBuilder<IProperty>(values.Cast<System.Collections.Generic.IEnumerable<IProperty>>().TryMerge());
        }

        protected override bool Convert(ArrayBuilder<IProperty> value, object parameter, CultureInfo culture, out IList<IProperty> result)
        {
            result = value.ToList();

            return true;
        }

        protected override bool[] ConvertBack(IList<IProperty> value, object parameter, CultureInfo culture, out IQueue<IProperty> result) => throw new NotSupportedException();
    }

    [ValueConversion(typeof(object), typeof(IPropertySystemCollection<ReflectionPropertyId, object>))]
    public class ObjectToPropertyCollectionConverter : AlwaysConvertibleOneWayConverter<object, object, IPropertySystemCollection<ReflectionPropertyId, object>>
    {
        private IConversionOptions _convertOptions = new ConversionOptions(true, true);

        public override IReadOnlyConversionOptions ConvertOptions => ConvertOptions2;

        public IConversionOptions ConvertOptions2 { get => _convertOptions; set => _convertOptions = value ?? throw ThrowHelper.GetArgumentNullException(nameof(value)); }

        protected override IPropertySystemCollection<ReflectionPropertyId, object> Convert(object value, object parameter, CultureInfo culture) => value == null ? null : new ReflectionPropertyCollection(value);
    }
}
#endif
