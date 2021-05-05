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

#if WinCopies3
using System;
using System.Globalization;
using System.Windows.Data;

using WinCopies.Collections.Abstraction.Generic;
using WinCopies.Linq;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    public abstract class MultiConverterBase<TSource, TParamIn,
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
      > where TSource : class
    {
        protected sealed override object Convert(object[] values, TParamOut _parameter, CultureInfo culture)
        {
            Collections.Abstraction.Generic.IArrayEnumerable<TSource> array = Convert(
#if WinCopies3
                    values?.
#else
                    values == null ? null :
#endif
                    To<TSource>(
#if !WinCopies3
                        values
#endif
                    ), _parameter, culture);

            try
            {
                return array != null && Convert(array, _parameter, culture, out TDestinationOut _result) ? _result : Binding.DoNothing;
            }

            finally
            {
                for (int i = 0; i < array.Count; i++)

                    array[i] = null;
            }
        }

        protected abstract Collections.Abstraction.Generic.IArrayEnumerable<TSource> Convert(System.Collections.Generic.IEnumerable<object> values, TParamOut parameter, CultureInfo culture);

        protected abstract bool Convert(Collections.Abstraction.Generic.IArrayEnumerable<TSource> values, TParamOut parameter, CultureInfo culture, out TDestinationOut result);

        protected sealed override object[] ConvertBack(TDestinationOut _value, TParamOut _parameter, CultureInfo culture)
        {
            bool[] results = ConvertBack(_value, _parameter, culture, out Collections.Abstraction.Generic.IArrayEnumerable<TSource> _result);

            if (results.Length == _result.Count)
            {
                object[] resultArray = new object[results.Length];

                for (int i = 0; i < results.Length; i++)

                    resultArray[i] = results[i] ? _result[i] : Binding.DoNothing;
            }

            throw new InvalidOperationException("The number of items in _result must be equal to ConvertBack result's.");
        }

        protected abstract bool[] ConvertBack(TDestinationOut value, TParamOut parameter, CultureInfo culture, out Collections.Abstraction.Generic.IArrayEnumerable<TSource> result);
    }
}

#endif
