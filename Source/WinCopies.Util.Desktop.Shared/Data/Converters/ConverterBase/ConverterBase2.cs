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
    public interface IOneWayConverter<in TSource, in TParam, TDestination>
    {
        bool Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result);
    }

    public interface IOneWayToSourceConverter<TSource, in TParam, in TDestination>
    {
        bool ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result);
    }

    public interface IValueConverter<TSource, in TParam, TDestination> : IOneWayConverter<TSource, TParam, TDestination>, IOneWayToSourceConverter<TSource, TParam, TDestination>
    {
        // Left empty.
    }

    public abstract class OneWayConverterBase<TSource, TParam, TDestination> : ConverterBase<TSource, TParam, TDestination>, IOneWayConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.OneWay;

        bool IOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result) => Convert(value, parameter, culture, out result);

#if WinCopies3
        public sealed override ConversionOptions ConvertBackOptions => throw GetException(BackConversionExceptionMessageFormat);

        protected sealed override bool ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => throw GetException(BackConversionExceptionMessageFormat);
#endif
    }

    public abstract class OneWayToSourceConverterBase<TSource, TParam, TDestination> : ConverterBase<TSource, TParam, TDestination>, IOneWayToSourceConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.OneWayToSource;

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);

#if WinCopies3
        public sealed override ConversionOptions ConvertOptions => throw GetException(string.Empty);

        protected sealed override bool Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result) => throw GetException(string.Empty);
#endif
    }

    public abstract class TwoWayConverterBase<TSource, TParam, TDestination> : ConverterBase<TSource, TParam, TDestination>, IValueConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.TwoWays;

        bool IOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result) => Convert(value, parameter, culture, out result);

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);
    }
}
