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

using System.Globalization;

namespace WinCopies.Util.Data
{
    public interface IAlwaysConvertibleOneWayConverter<in TSource, in TParam, TDestination> : IOneWayConverter<TSource, TParam, TDestination>
    {
        TDestination Convert(TSource value, TParam parameter, CultureInfo culture);
    }

    public interface IAlwaysConvertibleOneWayToSourceConverter<TSource, in TParam, in TDestination> : IOneWayToSourceConverter<TSource, TParam, TDestination>
    {
        TSource ConvertBack(TDestination value, TParam parameter, CultureInfo culture);
    }

    public interface IAlwaysConvertibleValueConverter<TSource, in TParam, TDestination> : IAlwaysConvertibleOneWayConverter<TSource, TParam, TDestination>, IAlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination>, IValueConverter<TSource, TParam, TDestination>
    {
        // Left empty.
    }

    public abstract class AlwaysConvertibleConverterBase<TSource, TParam, TDestination> : ConverterBase<TSource, TParam, TDestination>
    {
        protected abstract TDestination Convert(TSource value, TParam parameter, CultureInfo culture);

        protected sealed override bool Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result)
        {
            result = Convert(value, parameter, culture);

            return true;
        }

        protected abstract TSource ConvertBack(TDestination value, TParam parameter, CultureInfo culture);

        protected sealed override bool ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result)
        {
            result = ConvertBack(value, parameter, culture);

            return true;
        }
    }

    public abstract class AlwaysConvertibleOneWayConverter<TSource, TParam, TDestination> : AlwaysConvertibleConverterBase<TSource, TParam, TDestination>, IAlwaysConvertibleOneWayConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.OneWay;

        bool IOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result) => Convert(value, parameter, culture, out result);

        TDestination IAlwaysConvertibleOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture) => Convert(value, parameter, culture);

        public sealed override IReadOnlyConversionOptions ConvertBackOptions => throw GetException(BackConversionExceptionMessageFormat);

        protected sealed override TSource ConvertBack(TDestination value, TParam parameter, CultureInfo culture) => throw GetException(BackConversionExceptionMessageFormat);
    }

    public abstract class AlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination> : AlwaysConvertibleConverterBase<TSource, TParam, TDestination>, IAlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.OneWayToSource;

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);

        TSource IAlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture) => ConvertBack(value, parameter, culture);

        public sealed override IReadOnlyConversionOptions ConvertOptions => throw GetException(string.Empty);

        protected sealed override TDestination Convert(TSource value, TParam parameter, CultureInfo culture) => throw GetException(string.Empty);
    }

    public abstract class AlwaysConvertibleTwoWayConverter<TSource, TParam, TDestination> : AlwaysConvertibleConverterBase<TSource, TParam, TDestination>, IAlwaysConvertibleValueConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.TwoWays;

        bool IOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result) => Convert(value, parameter, culture, out result);

        TDestination IAlwaysConvertibleOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture) => Convert(value, parameter, culture);

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);

        TSource IAlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture) => ConvertBack(value, parameter, culture);
    }
}
