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

using static WinCopies.Util.Data.ConverterHelper;
using static WinCopies.Desktop.Resources.ExceptionMessages;

namespace WinCopies.Util.Data
{
    public abstract class MultiConverterBase2<TParam, TDestination, TParameterConverters, TDestinationConverters> : MultiConverterBase where TParameterConverters : IConverterConverter where TDestinationConverters : IConverterConverter
    {
        public abstract IReadOnlyConversionOptions ConvertOptions { get; }

        public abstract IReadOnlyConversionOptions ConvertBackOptions { get; }

        public abstract ConversionWays Direction { get; }

        protected abstract TParameterConverters ParameterConverters { get; }

        protected abstract TDestinationConverters DestinationConverters { get; }

        protected abstract bool Convert(object[] values, TParam _parameter, CultureInfo culture, out TDestination result);

        protected Exception GetNotSupportedConversionWayException(in ConversionWays conversionWay) => throw new InvalidOperationException(string.Format(ConversionDirectionNotSupported, conversionWay.ToString()));

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWay))
            {
                Check(values, ConvertOptions.AllowNullValue, nameof(values));

                Check<TParam>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                return Convert(values, parameter == null ? ParameterConverters.GetDefaultValue<TParam>() : (TParam)parameter, culture, out TDestination result) ? result : Binding.DoNothing;
            }

            throw GetNotSupportedConversionWayException(ConversionWays.OneWay);
        }

        protected abstract object[] ConvertBack(TDestination value, TParam parameter, CultureInfo culture);

        public sealed override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWayToSource))
            {
                Check<TDestination>(value, ConvertOptions.AllowNullValue, nameof(value));

                Check<TParam>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                return ConvertBack(value == null ? DestinationConverters.GetDefaultValue<TDestination>() : (TDestination)value, parameter == null ? ParameterConverters.GetDefaultValue<TParam>() : (TParam)parameter, culture);
            }

            throw GetNotSupportedConversionWayException(ConversionWays.OneWayToSource);
        }
    }

    public abstract class MultiConverterBase2<TParam, TDestination> : MultiConverterBase2<TParam, TDestination, IConverterConverter, IConverterConverter>
    {
        protected override IConverterConverter ParameterConverters => ConverterConverter.Instance;

        protected override IConverterConverter DestinationConverters => null;
    }

    public abstract class OneWayMultiConverter<TParam, TDestination> : MultiConverterBase2<TParam, TDestination, IConverterConverter, IConverterConverter>
    {
        protected override IConverterConverter ParameterConverters => ConverterConverter.Instance;

        protected sealed override IConverterConverter DestinationConverters => null;

        public sealed override IReadOnlyConversionOptions ConvertBackOptions => default;

        public sealed override ConversionWays Direction => ConversionWays.OneWay;

        protected sealed override object[] ConvertBack(TDestination _value, TParam _parameter, CultureInfo culture) => null;
    }

    public abstract class OneWayToSourceMultiConverter<TParam, TDestination> : MultiConverterBase2<TParam, TDestination, IConverterConverter, IConverterConverter>
    {
        protected override IConverterConverter ParameterConverters => ConverterConverter.Instance;

        protected override IConverterConverter DestinationConverters => ConverterConverter.Instance;

        public sealed override IReadOnlyConversionOptions ConvertOptions => default;

        public sealed override ConversionWays Direction => ConversionWays.OneWayToSource;

        protected sealed override bool Convert(object[] values, TParam _parameter, CultureInfo culture, out TDestination result)
        {
            result = DestinationConverters.GetDefaultValue<TDestination>();

            return false;
        }
    }

    public abstract class TwoWayMultiConverter<TParam, TDestination> : MultiConverterBase2<TParam, TDestination, IConverterConverter, IConverterConverter>
    {
        protected override IConverterConverter ParameterConverters => ConverterConverter.Instance;

        protected override IConverterConverter DestinationConverters => ConverterConverter.Instance;

        public sealed override ConversionWays Direction => ConversionWays.TwoWays;
    }

    public abstract class AlwaysConvertibleOneWayMultiConverter<TParam, TDestination> : OneWayMultiConverter<TParam, TDestination>
    {
        protected abstract TDestination Convert(object[] values, TParam parameter, CultureInfo culture);

        protected sealed override bool Convert(object[] values, TParam parameter, CultureInfo culture, out TDestination result)
        {
            result = Convert(values, parameter, culture);

            return true;
        }
    }

    public abstract class AlwaysConvertibleTwoWayMultiConverter<TParam, TDestination> : TwoWayMultiConverter<TParam, TDestination>
    {
        protected abstract TDestination Convert(object[] values, TParam parameter, CultureInfo culture);

        protected sealed override bool Convert(object[] values, TParam parameter, CultureInfo culture, out TDestination result)
        {
            result = Convert(values, parameter, culture);

            return true;
        }
    }
}
