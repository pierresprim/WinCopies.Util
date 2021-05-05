﻿/* Copyright © Pierre Sprimont, 2019
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
using System.Globalization;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    public interface IMultiConverterConverters<TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {
        TParamOut GetDefaultParameter();

        TParamOut ConvertParameter(in TParamIn parameter);

        TDestinationOut GetDefaultDestinationValue();

        TDestinationOut ConvertDestinationValue(in TDestinationIn value);
    }

    public sealed class MultiConverterConverters<TParam, TDestination> : IMultiConverterConverters<TParam, TParam, TDestination, TDestination>
    {
        public TParam GetDefaultParameter() => default;

        public TParam ConvertParameter(in TParam parameter) => parameter;

        public TDestination GetDefaultDestinationValue() => default;

        public TDestination ConvertDestinationValue(in TDestination value) => value;
    }

    public abstract class MultiConverterBase3<TParamIn, TParamOut, TDestinationIn, TDestinationOut> : MultiConverterBase
    {
        public abstract ConversionOptions ConvertOptions { get; }

        public abstract ConversionOptions ConvertBackOptions { get; }

        public abstract ConversionWays Direction { get; }

        protected abstract IMultiConverterConverters<TParamIn, TParamOut, TDestinationIn, TDestinationOut> Converters { get; }

        protected abstract object Convert(object[] values, TParamOut _parameter, CultureInfo culture);

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWay))
            {
                Check(values, ConvertOptions.AllowNullValue, nameof(values));

                Check<TParamIn>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                return Convert(values, parameter == null ? Converters.GetDefaultParameter() : Converters.ConvertParameter((TParamIn)parameter), culture);
            }

            throw new InvalidOperationException("The OneWay conversion direction is not supported.");
        }

        protected abstract object[] ConvertBack(TDestinationOut _value, TParamOut _parameter, CultureInfo culture);

        public sealed override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWayToSource))
            {
                Check<TDestinationIn>(value, ConvertOptions.AllowNullValue, nameof(value));

                Check<TParamIn>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                return ConvertBack(value == null ? Converters.GetDefaultDestinationValue() : Converters.ConvertDestinationValue((TDestinationIn)value), parameter == null ? Converters.GetDefaultParameter() : Converters.ConvertParameter((TParamIn)parameter), culture);
            }

            throw new InvalidOperationException("The OneWayToSource conversion direction is not supported.");
        }
    }

    public abstract class MultiConverterBase4<TParamIn, TParamOut, TDestinationIn, TDestinationOut> : MultiConverterBase3<TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {
        protected abstract TDestinationOut ConvertOverride(object[] values, TParamOut _parameter, CultureInfo culture);

        protected sealed override object Convert(object[] values, TParamOut _parameter, CultureInfo culture) => ConvertOverride(values, _parameter, culture);
    }
}
#else
using System;
using System.Globalization;
using System.Windows.Data;

using WinCopies.Collections;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Linq;

using static WinCopies.Util.Data.ConverterHelper;
using static WinCopies.Util.Util;

namespace WinCopies.Util.Data
{
    public abstract class MultiConverterBase<TSourceIn, TSourceOut, TParam, TDestination> : MultiConverterBase
    {
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

        public abstract ConversionOptions ConvertOptions { get; }

        public abstract ConversionOptions ConvertBackOptions { get; }

        public abstract ConversionWays Direction { get; }

        protected abstract ArrayBuilder<TSourceOut> Convert(System.Collections.Generic.IEnumerable<TSourceIn> values, TParam parameter, CultureInfo culture);

        protected abstract bool Convert(ArrayBuilder<TSourceOut> value, TParam parameter, CultureInfo culture, out TDestination result);

        protected abstract bool[] ConvertBack(TDestination value, TParam parameter, CultureInfo culture, out IQueue<TSourceOut> result);

        public static bool CheckForNullItem(in object value, in bool methodParameter) => !methodParameter && value == null;

        private static System.Collections.Generic.IEnumerable<T> To<T>(in System.Collections.IEnumerable enumerable) => enumerable.SelectConverter(value => (T)value);

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Direction.HasFlag(ConversionWays.OneWay))
            {
                Check(values, ConvertOptions.AllowNullValue, nameof(values));

                Check<TParam>(parameter, ConvertOptions.AllowNullParameter, nameof(parameter));

                object convert(in TParam _parameter)
                {
                    ArrayBuilder<TSourceOut> arrayBuilder = Convert(values == null ? null : To<TSourceIn>(values), _parameter, culture);

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
#endif
