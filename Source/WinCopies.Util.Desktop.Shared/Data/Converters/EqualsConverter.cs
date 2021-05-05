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

using WinCopies.Util.Data;

using static WinCopies.Util.Data.ConverterHelper;
using static WinCopies.Bool;

namespace WinCopies
{
    public class EqualsConverter : MultiConverterBase4<bool, bool, bool, bool>
    {
        public override ConversionOptions ConvertOptions => ParameterCanBeNull;

        public override ConversionOptions ConvertBackOptions => AllowNull;

        public override ConversionWays Direction => ConversionWays.OneWay;

        protected override IMultiConverterConverters<bool, bool, bool, bool> Converters => new MultiConverterConverters<bool, bool>();

        protected override bool ConvertOverride(object[] values, bool parameter, CultureInfo culture)
        {
            if (values == null || values.Length <= 1)

                return false;

            Func<bool?, bool> func;

            if (parameter is bool _parameter && _parameter) // We check if the parameter of func is true when parameter is true itself and if the parameter of func is false when parameter is false because the loop return false for the first value that is false.

                func = IsTrue;

            else

                func = IsFalse;

            for (int i = 0; i < values.Length; i++)

                for (int j = i + 1; j < values.Length; j++)

                    if (func(values[i]?.Equals(values[j])))

                        return false;

            return true;
        }

        protected override object[] ConvertBack(bool _value, bool _parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
#endif
