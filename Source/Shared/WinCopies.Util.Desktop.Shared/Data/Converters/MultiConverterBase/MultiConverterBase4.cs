﻿/* Copyright © Pierre Sprimont, 2021
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
using System.Linq;
using System.Windows.Data;

using WinCopies.Collections;
using WinCopies.Collections.Abstraction.Generic;
using WinCopies.Collections.DotNetFix.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Util.Data
{
    public abstract class MultiConverterBase4<TSourceIn, TSourceOut, TSourceEnumerableIn, TSourceEnumerableOut, TParamIn, TParamOut, TDestinationIn, TDestinationOut> : MultiConverterBase3<TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {
        protected abstract int Count(in TSourceEnumerableOut values);

        protected abstract void Convert(in bool[] conversionsSucceeded, in TSourceEnumerableOut results, in object[] resultArray);

        protected abstract void Clear(in TSourceEnumerableIn values);

        protected sealed override bool Convert(object[] values, TParamOut _parameter, CultureInfo culture, out TDestinationIn result)
        {
            TSourceEnumerableIn array = Convert(values?.Cast<TSourceIn>(), values.Length, _parameter, culture);

            if (array != null)

                try
                {
                    if (Convert(array, _parameter, culture, out result))

                        return true;
                }

                finally
                {
                    Clear(array);
                }

            result = DestinationConverters.GetDefaultValue<TDestinationIn>();

            return false;
        }

        protected abstract TSourceEnumerableIn Convert(System.Collections.Generic.IEnumerable<TSourceIn> values, in int valuesCount, TParamOut parameter, CultureInfo culture);

        protected abstract bool Convert(TSourceEnumerableIn values, TParamOut parameter, CultureInfo culture, out TDestinationIn result);

        protected InvalidOperationException GetLengthMismatchException() => new
#if !CS9
            InvalidOperationException
#endif
            ("The number of items in _result must be equal to ConvertBack result's.");

        protected sealed override object[] ConvertBack(TDestinationIn _value, TParamOut _parameter, CultureInfo culture)
        {
            bool[] results = ConvertBack(_value, _parameter, culture, out TSourceEnumerableOut _result);

            if (results.Length == Count(_result))
            {
                object[] resultArray = new object[results.Length];

                Convert(results, _result, resultArray);

                return resultArray;
            }

            throw GetLengthMismatchException();
        }

        protected abstract bool[] ConvertBack(TDestinationIn value, TParamOut parameter, CultureInfo culture, out TSourceEnumerableOut result);
    }

    public abstract class MultiConverterBase5<TSourceIn, TSourceOut, TSourceEnumerable, TParamIn, TParamOut, TDestinationIn, TDestinationOut> : MultiConverterBase4<TSourceIn, TSourceOut, TSourceEnumerable, TSourceEnumerable, TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {

    }

    public abstract class MultiConverterBase6<TSourceIn, TSourceOut, TParamIn, TParamOut, TDestinationIn, TDestinationOut> : MultiConverterBase5<TSourceIn, TSourceOut, IArrayEnumerable<TSourceOut>, TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {
        protected sealed override int Count(in IArrayEnumerable<TSourceOut> values) => values.AsFromType<ICountable>().Count;

        protected sealed override void Convert(in bool[] conversionsSucceeded, in IArrayEnumerable<TSourceOut> results, in object[] resultArray)
        {
            for (int i = 0; i < conversionsSucceeded.Length; i++)

                resultArray[i] = conversionsSucceeded[i] ? results[i] : Binding.DoNothing;
        }

        protected sealed override void Clear(in IArrayEnumerable<TSourceOut> values) => values.Clear();
    }
#if CS7
    public abstract class MultiConverterBase7<TSourceIn, TSourceOut, TParamIn, TParamOut, TDestinationIn, TDestinationOut> : MultiConverterBase4<TSourceIn, TSourceOut, ArrayBuilder<TSourceOut>, IQueue<TSourceOut>, TParamIn, TParamOut, TDestinationIn, TDestinationOut>
    {
        protected sealed override int Count(in IQueue<TSourceOut> values) => values.Count <= int.MaxValue ? (int)values.Count : throw GetLengthMismatchException();

        protected sealed override void Convert(in bool[] conversionsSucceeded, in IQueue<TSourceOut> results, in object[] resultArray)
        {
            int i = 0;

            bool[] _conversionsSucceeded = conversionsSucceeded;
            IQueue<TSourceOut> _results = results;

            object getValue()
            {
                if (_conversionsSucceeded[i])

                    return _results.Dequeue();

                _ = _results.Dequeue();

                return Binding.DoNothing;
            }

            for (; i < conversionsSucceeded.Length; i++)

                resultArray[i] = getValue();
        }

        protected sealed override void Clear(in ArrayBuilder<TSourceOut> values) => values.Clear();
    }
#endif
}
