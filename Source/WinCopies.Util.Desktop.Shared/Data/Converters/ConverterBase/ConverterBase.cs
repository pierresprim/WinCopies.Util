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

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
#if WinCopies3
    public interface IReadOnlyConversionOptions
    {
        bool AllowNullValue { get; }

        bool AllowNullParameter { get; }
    }

    public interface IConversionOptions : IReadOnlyConversionOptions
    {
        new bool AllowNullValue { get; set; }

        new bool AllowNullParameter { get; set; }

#if CS8
        bool IReadOnlyConversionOptions.AllowNullValue => AllowNullValue;

        bool IReadOnlyConversionOptions.AllowNullParameter => AllowNullParameter;
#endif
    }
#endif

    public class
#if WinCopies3
        ReadOnlyConversionOptions : IReadOnlyConversionOptions
#else
        ConversionOptions
#endif
    {
        public bool AllowNullValue { get; }

        public bool AllowNullParameter { get; }

        public
#if WinCopies3
        ReadOnlyConversionOptions
#else
        ConversionOptions
#endif
            ()
        { /* Left empty. */ }

        public
#if WinCopies3
        ReadOnlyConversionOptions
#else
        ConversionOptions
#endif
            (in bool allowNullValue, in bool allowNullParameter)
        {
            AllowNullValue = allowNullValue;

            AllowNullParameter = allowNullParameter;
        }
    }

#if WinCopies3
    public class ConversionOptions : IConversionOptions
    {
        public bool AllowNullValue { get; set; }

        public bool AllowNullParameter { get; set; }

        public ConversionOptions() { /* Left empty. */ }

        public ConversionOptions(in bool allowNullValue, in bool allowNullParameter)
        {
            AllowNullValue = allowNullValue;

            AllowNullParameter = allowNullParameter;
        }

#if !CS8
        bool IReadOnlyConversionOptions.AllowNullValue => AllowNullValue;

        bool IReadOnlyConversionOptions.AllowNullParameter => AllowNullParameter;
#endif
    }
#endif

    [Flags]
    public enum ConversionWays : byte
    {
        OneWay = 1,

        OneWayToSource = 2,

        TwoWays = OneWay | OneWayToSource
    }

    public static class ConverterHelper
    {
#if WinCopies3
        public static ReadOnlyConversionOptions AllowNull { get; } = new ReadOnlyConversionOptions(true, true);

        public static ReadOnlyConversionOptions ValueCanBeNull { get; } = new ReadOnlyConversionOptions(true, false);

        public static ReadOnlyConversionOptions ParameterCanBeNull { get; } = new ReadOnlyConversionOptions(false, true);

        public static ReadOnlyConversionOptions NotNull { get; } = new ReadOnlyConversionOptions(false, false);
#endif

        public static bool CheckForNullItem(in object value, in bool methodParameter) => !methodParameter && value == null;

        public static void Check(in object obj, in bool methodParameter, in string argumentName)
        {
            if (CheckForNullItem(obj, methodParameter))

                throw new ArgumentNullException(argumentName);
        }

        public static void Check<T>(in object obj, in bool methodParameter, in string argumentName)
        {
            Check(obj, methodParameter, argumentName);

            if (obj != null && !(obj is T))

                throw new ArgumentException($"{argumentName} must be null or from {typeof(T).Name}. {argumentName} is {(obj == null ? "null" : obj.GetType().Name)}.");
        }
    }

    /// <summary>
    /// Provides a base class for any data <see cref="Binding"/> converter.
    /// </summary>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ConverterBase : MarkupExtension, IValueConverter
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

        internal const string BackConversionExceptionMessageFormat = "back ";

        internal static InvalidOperationException GetException(in string format) => new InvalidOperationException($"The current converter does not support {format}conversion.");
    }

    public abstract class ConverterBase<TSource, TParam, TDestination
#if WinCopies3
        , TConversionOptions
#endif
        > : ConverterBase
#if WinCopies3
where TConversionOptions : IReadOnlyConversionOptions
#endif
    {
#if !WinCopies3
        public static ConversionOptions AllowNull { get; } = new ConversionOptions(true, true);

        public static ConversionOptions ValueCanBeNull { get; } = new ConversionOptions(true, false);

        public static ConversionOptions ParameterCanBeNull { get; } = new ConversionOptions(false, true);

        public static ConversionOptions NotNull { get; } = new ConversionOptions(false, false);
#endif

        public abstract
#if WinCopies3
            TConversionOptions
#else
            ConversionOptions
#endif
            ConvertOptions { get; }

        public abstract
#if WinCopies3
            TConversionOptions
#else
            ConversionOptions
#endif
           ConvertBackOptions
        { get; }

        public abstract ConversionWays Direction { get; }

        protected abstract bool Convert(TSource value, TParam parameter, CultureInfo culture, out TDestination result);

        protected abstract bool ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out TSource result);

#if !WinCopies3
        public bool CheckForNullItem(in object value, in bool methodParameter) => !methodParameter && value == null;
#endif

        public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWay))
            {
                Check<TSource>(value, ConvertOptions.AllowNullValue, nameof(value));

                Check<TParam>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                object convert(in TSource _value, in TParam _parameter) => Convert(_value, _parameter, culture, out TDestination _result) ? _result : Binding.DoNothing;

                return value == null
                    ? parameter == null ? convert(default, default) : convert(default, (TParam)parameter)
                    : parameter == null ? convert((TSource)value, default) : convert((TSource)value, (TParam)parameter);
            }

            throw new InvalidOperationException("The OneWay conversion direction is not supported.");
        }

        public sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWayToSource))
            {
                Check<TDestination>(value, ConvertOptions.AllowNullValue, nameof(value));

                Check<TDestination>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                object convertBack(in TDestination _value, in TParam _parameter) => ConvertBack(_value, _parameter, culture, out TSource _result) ? _result : Binding.DoNothing;

                return value == null
                    ? parameter == null ? convertBack(default, default) : convertBack(default, (TParam)parameter)
                    : parameter == null ? convertBack((TDestination)value, default) : convertBack((TDestination)value, (TParam)parameter);
            }

            throw new InvalidOperationException("The OneWayToSource conversion direction is not supported.");
        }
    }
}
