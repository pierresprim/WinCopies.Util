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
using System.Globalization;

namespace WinCopies.Util.Data
{
    public abstract class MultiConverterBase3<TParamIn, TParamOut, TDestinationIn, TDestinationOut, TConversionOptions> : MultiConverterBase2<TParamIn, TDestinationOut, IConverterConverter<TParamIn, TParamOut>, IMultiConverterConverter<TDestinationOut, TDestinationIn>, TConversionOptions> where TConversionOptions : IReadOnlyConversionOptions
    {
        protected abstract bool Convert(object[] values, TParamOut parameter, CultureInfo culture, out TDestinationIn result);

        protected sealed override bool Convert(object[] values, TParamIn _parameter, CultureInfo culture, out TDestinationOut result)
        {
            if (Convert(values, ParameterConverters.Convert(_parameter), culture, out TDestinationIn _result))
            {
                result = DestinationConverters.ConvertBack(_result);

                return true;
            }

            result = DestinationConverters.GetDefaultValue<TDestinationOut>();

            return false;
        }

        protected abstract object[] ConvertBack(TDestinationIn _value, TParamOut _parameter, CultureInfo culture);

        protected sealed override object[] ConvertBack(TDestinationOut _value, TParamIn _parameter, CultureInfo culture) => ConvertBack(DestinationConverters.Convert(_value), ParameterConverters.Convert(_parameter), culture);
    }
}
#endif
