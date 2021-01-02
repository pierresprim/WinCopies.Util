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
using System.Windows.Markup;

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Provides a base-class for any data <see cref="Binding"/> converter.
    /// </summary>
    [MarkupExtensionReturnType(typeof(System.Windows.Data.IValueConverter))]
    public abstract class ConverterBase : MarkupExtension, System.Windows.Data.IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

    public class ConversionOptions
    {
        public bool AllowNullValue { get; }

        public bool AllowNullParameter { get; }

        public ConversionOptions(in bool allowNullValue, in bool allowNullParameter)
        {
            AllowNullValue = allowNullValue;

            AllowNullParameter = allowNullParameter;
        }
    }

    [Flags]
    public enum ConversionWays : byte
    {
        OneWay = 1,

        OneWayToSource = 2,

        TwoWays = OneWay | OneWayToSource
    }

    public abstract class ConverterBase<TSource, TParam, TDestination> : ConverterBase
    {
        public static ConversionOptions AllowNull { get; } = new ConversionOptions(true, true);

        public static ConversionOptions ValueCanBeNull { get; } = new ConversionOptions(true, false);

        public static ConversionOptions ParameterCanBeNull { get; } = new ConversionOptions(false, true);

        public static ConversionOptions NotNull { get; } = new ConversionOptions(false, false);

        public abstract ConversionOptions ConvertOptions { get; }

        public abstract ConversionOptions ConvertBackOptions { get; }

        public abstract ConversionWays Direction { get; }

        protected abstract bool Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result);

        protected abstract bool ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result);

        public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWay))
            {
                if (!ConvertOptions.AllowNullValue)

                    throw new ArgumentNullException(nameof(value));

                if (!ConvertOptions.AllowNullParameter)

                    throw new ArgumentNullException(nameof(parameter));

                if (value != null && !(value is TSource))

                    throw new ArgumentException($"{nameof(value)} must be null or from {nameof(TSource)}.");

                if (parameter != null && !(parameter is TParam))

                    throw new ArgumentException($"{nameof(parameter)} must be null or from {nameof(TParam)}.");

                object convert(in TSource _value, in TParam _parameter) => Convert(_value, _parameter, culture, out TDestination _result) ? _result : Binding.DoNothing;

                return value == null
                    ? parameter == null ? convert(default, default) : convert(default, (TParam)parameter)
                    : parameter == null ? convert((TSource)value, default) : convert((TSource)value, (TParam)parameter);
            }

            throw new InvalidOperationException();
        }

        public sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWayToSource))
            {
                if (!ConvertOptions.AllowNullValue)

                    throw new ArgumentNullException(nameof(value));

                if (!ConvertOptions.AllowNullParameter)

                    throw new ArgumentNullException(nameof(parameter));

                if (value != null && !(value is TDestination))

                    throw new ArgumentException($"{nameof(value)} must be null or from {typeof(TDestination).Name}. {nameof(value)} is {(value == null ? "null" : value.GetType().Name)}.");

                if (parameter != null && !(parameter is TParam))

                    throw new ArgumentException($"{nameof(parameter)} must be null or from {nameof(TParam)}.");

                object convertBack(in TDestination _value, in TParam _parameter) => ConvertBack(_value, _parameter, culture, out TSource _result) ? _result : Binding.DoNothing;

                return value == null
                    ? parameter == null ? convertBack(default, default) : convertBack(default, (TParam)parameter)
                    : parameter == null ? convertBack((TDestination)value, default) : convertBack((TDestination)value, (TParam)parameter);
            }

            throw new InvalidOperationException();
        }
    }

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
    }

    public abstract class OneWayToSourceConverterBase<TSource, TParam, TDestination> : ConverterBase<TSource, TParam, TDestination>, IOneWayToSourceConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.OneWayToSource;

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);
    }

    public abstract class TwoWayConverterBase<TSource, TParam, TDestination> : ConverterBase<TSource, TParam, TDestination>, IValueConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.TwoWays;

        bool IOneWayConverter<TSource, TParam, TDestination>.Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result) => Convert(value, parameter, culture, out result);

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);
    }

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
    }

    public abstract class AlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination> : AlwaysConvertibleConverterBase<TSource, TParam, TDestination>, IAlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination>
    {
        public sealed override ConversionWays Direction => ConversionWays.OneWayToSource;

        bool IOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result) => ConvertBack(value, parameter, culture, out result);

        TSource IAlwaysConvertibleOneWayToSourceConverter<TSource, TParam, TDestination>.ConvertBack(TDestination value, TParam parameter, CultureInfo culture) => ConvertBack(value, parameter, culture);
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
