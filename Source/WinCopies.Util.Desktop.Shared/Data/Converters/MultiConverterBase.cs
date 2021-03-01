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

using WinCopies.Collections;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;

using static WinCopies.Util.Data.ConverterHelper;

#if WinCopies3
using WinCopies.Collections.Generic;

using static WinCopies.ThrowHelper;
#else
using static WinCopies.Util.Util;
#endif

namespace WinCopies.Util.Data
{
    /// <summary>
    /// Provides a base-class for any data-<see cref="MultiBinding"/> converter.
    /// </summary>
    [MarkupExtensionReturnType(typeof(IMultiValueConverter))]
    public abstract class MultiConverterBase : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="MultiBinding"/> produces. The value <see cref="System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

    public abstract class MultiConverterBase<TSourceIn, TSourceOut, TParam, TDestination> : MultiConverterBase
    {
#if !WinCopies3
        public interface IQueue : IUIntCountable
        {
            TSourceOut Dequeue();
        }

        public sealed class SimpleQueue : IQueue
        {
            private readonly IQueue<TSourceOut> _queue = new Collections.DotNetFix.Generic.Queue<TSourceOut>();

            public uint Count => _queue.Count;

            public void Enqueue(in TSourceOut item) => _queue.Enqueue(item);

            public TSourceOut Dequeue() => _queue.Dequeue();
        }

        public sealed class ArrayQueue : IQueue
        {
            private readonly object[] _array;
            private int _index;

            public uint Count => (uint)(_array.Length - _index);

            public ArrayQueue(in object[] array) => _array = array ?? throw GetArgumentNullException(nameof(array));

            public TSourceOut Dequeue()
            {
                var item = (TSourceOut)_array[_index];

                _array[_index++] = null;

                return item;
            }
        }

        public sealed class Queue : IQueue
        {
            private readonly IQueue<TSourceOut> _queue;

            public uint Count => _queue.Count;

            public Queue(IQueue<TSourceOut> queue) => _queue = queue ?? throw GetArgumentNullException(nameof(queue));

            public TSourceOut Dequeue() => _queue.Dequeue();
        }

        public sealed class Stack : IQueue
        {
            private readonly IStack<TSourceOut> _stack;

            public uint Count => _stack.Count;

            public Stack(IStack<TSourceOut> stack) => _stack = stack ?? throw GetArgumentNullException(nameof(stack));

            public TSourceOut Dequeue() => _stack.Pop();
        }
#endif

        public abstract ConversionOptions ConvertOptions { get; }

        public abstract ConversionOptions ConvertBackOptions { get; }

        public abstract ConversionWays Direction { get; }

        protected abstract ArrayBuilder<TSourceOut> Convert(System.Collections.Generic.IEnumerable<TSourceIn> values, TParam parameter, CultureInfo culture);

        protected abstract bool Convert(ArrayBuilder<TSourceOut> value, TParam parameter, CultureInfo culture, out TDestination result);

        protected abstract bool[] ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out IQueue<TSourceOut> result);

#if !WinCopies3
        public static bool CheckForNullItem(in object value, in bool methodParameter) => !methodParameter && value == null;
        
        private static System.Collections.Generic.IEnumerable<T> To<T>(in System.Collections.IEnumerable enumerable) => enumerable.SelectConverter(value => (T)value);
#endif

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWay))
            {
                Check(values, ConvertOptions.AllowNullValue, nameof(values));

                Check<TParam>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                object convert(in TParam _parameter)
                {
                    ArrayBuilder<TSourceOut> arrayBuilder = Convert(
#if WinCopies3
                    values?.
#else
                    values == null ? null :
#endif
                    To<TSourceIn>(
#if !WinCopies3
                        values
#endif
                    ), _parameter, culture);

                    try
                    {
                        return arrayBuilder != null && Convert(arrayBuilder, _parameter, culture, out TDestination _result) ? _result : Binding.DoNothing;
                    }

                    finally
                    {
                        arrayBuilder?.Clear();
                    }
                }

                return convert(parameter == null ? default : (TParam)parameter);
            }

            throw new InvalidOperationException("The OneWay conversion direction is not supported.");
        }

        public sealed override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWayToSource))
            {
                Check<TDestination>(value, ConvertOptions.AllowNullValue, nameof(value));

                Check<TParam>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                object[] convertBack(in TDestination _value, in TParam _parameter)
                {
                    bool[] results = ConvertBack(_value, _parameter, culture, out IQueue<TSourceOut> _result);

                    if (results.Length == _result.Count)
                    {
                        object[] resultArray = new object[results.Length];

                        for (int i = 0; i < results.Length; i++)

                            resultArray[i] = results[i] ? _result.Dequeue() : Binding.DoNothing;
                    }

                    throw new InvalidOperationException("The number of items in _result must be equal to ConvertBack result's.");
                }

                return value == null
                    ? parameter == null ? convertBack(default, default) : convertBack(default, (TParam)parameter)
                    : parameter == null ? convertBack((TDestination)value, default) : convertBack((TDestination)value, (TParam)parameter);
            }

            throw new InvalidOperationException("The OneWayToSource conversion direction is not supported.");
        }
    }
}
