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
    }

    public abstract class ConverterBase<TSource, TParam, TDestination> : ConverterBase
    {
        public abstract TDestination Convert(TSource value, TParam parameter, CultureInfo culture);

        public abstract TSource ConvertBack(TDestination value, TParam parameter, CultureInfo culture);

        public sealed override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is TSource))

                throw new ArgumentException($"{nameof(value)} must be null or from {nameof(TSource)}.");

            if (parameter != null && !(parameter is TParam))

                throw new ArgumentException($"{nameof(parameter)} must be null or from {nameof(TParam)}.");

            TDestination convert(in TSource _value, in TParam _parameter) => Convert(_value, _parameter, culture);

            if (value == null)

                if (parameter == null)

                    return convert(default, default);

                else

                    return convert(default, (TParam)parameter);

            else if (parameter == null)

                return convert((TSource)value, default);

            else

                return convert((TSource)value, (TParam)parameter);
        }

        public sealed override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !(value is TDestination))

                throw new ArgumentException($"{nameof(value)} must be null or from {nameof(TDestination)}.");

            if (parameter != null && !(parameter is TParam))

                throw new ArgumentException($"{nameof(parameter)} must be null or from {nameof(TParam)}.");

            TSource convertBack(in TDestination _value, in TParam _parameter) => ConvertBack(_value, _parameter, culture);

            if (value == null)

                if (parameter == null)

                    return convertBack(default, default);

                else

                    return convertBack(default, (TParam)parameter);

            else if (parameter == null)

                return convertBack((TDestination)value, default);

            else

                return convertBack((TDestination)value, (TParam)parameter);
        }
    }
}
