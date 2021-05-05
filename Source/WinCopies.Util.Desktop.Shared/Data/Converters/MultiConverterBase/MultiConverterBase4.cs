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

#if WinCopies3
using System;
using System.Globalization;
using System.Windows.Data;

using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;
using WinCopies.Linq;

namespace WinCopies.Util.Data
{
    public abstract class MultiConverterBase<TSourceIn, TSourceOut, TParamIn,
#if WinCopies3
        TParamOut,
#endif
        TDestinationIn
#if WinCopies3
        , TDestinationOut
#endif
      > : MultiConverterBase3<TParamIn,
#if WinCopies3
        TParamOut,
#endif
        TDestinationIn
#if WinCopies3
        , TDestinationOut
#endif
      >
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

        protected sealed override object Convert(object[] values, TParamOut _parameter, CultureInfo culture)
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
                return arrayBuilder != null && Convert(arrayBuilder, _parameter, culture, out TDestinationOut _result) ? _result : Binding.DoNothing;
            }

            finally
            {
                arrayBuilder?.Clear();
            }
        }

        protected abstract ArrayBuilder<TSourceOut> Convert(System.Collections.Generic.IEnumerable<TSourceIn> values, TParamOut parameter, CultureInfo culture);

        protected abstract bool Convert(ArrayBuilder<TSourceOut> values, TParamOut parameter, CultureInfo culture, out TDestinationOut result);

        protected sealed override object[] ConvertBack(TDestinationOut _value, TParamOut _parameter, CultureInfo culture)
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

        protected abstract bool[] ConvertBack(TDestinationOut value, TParamOut parameter, CultureInfo culture, out IQueue<TSourceOut> result);

#if !WinCopies3
        public static bool CheckForNullItem(in object value, in bool methodParameter) => !methodParameter && value == null;
        
        private static System.Collections.Generic.IEnumerable<T> To<T>(in System.Collections.IEnumerable enumerable) => enumerable.SelectConverter(value => (T)value);
#endif
    }

    public abstract class MultiConverterBase<TSourceIn, TSourceOut, TParam, TDestination> : MultiConverterBase<TSourceIn, TSourceOut, TParam, TParam, TDestination, TDestination>
    {
        protected override IMultiConverterConverters<TParam, TParam, TDestination, TDestination> Converters { get; } = new MultiConverterConverters<TParam, TDestination>();
    }
}
#endif
