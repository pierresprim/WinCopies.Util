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

using System;
using System.Globalization;
using System.Windows.Markup;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    public enum Comparison
    {
        Equal = 0,

        NotEqual = 1,

        Lesser = 2,

        Greater = 4,

        ReferenceEqual = 6
    }

    public abstract class ComparisonConverterParameter<T> : MarkupExtension
    {
        public WinCopies.Diagnostics.Comparison ExpectedComparisonResult { get; set; }

        public T Parameter { get; set; }

        public abstract Comparison Compare(T value);

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }

    public sealed class IntComparsionConverterParameter : ComparisonConverterParameter<int>
    {
        public override Comparison Compare(int value) => value < Parameter ? Comparison.Lesser : value > Parameter ? Comparison.Greater : Comparison.Equal;
    }

    public class ComparisonConverter<T> : AlwaysConvertibleOneWayConverter<T, ComparisonConverterParameter<T>, bool>
    {
        public override ConversionOptions ConvertOptions => ValueCanBeNull;

#if !WinCopies3
        public override ConversionOptions ConvertBackOptions => throw new InvalidOperationException();
#endif

        protected override bool Convert(T value, ComparisonConverterParameter<T> parameter, CultureInfo culture)
        {
            Comparison result = parameter.Compare(value);

            switch (result)
            {
                case Comparison.Equal:

                    switch (parameter.ExpectedComparisonResult)
                    {
                        case Diagnostics.Comparison.Equal:
                        case Diagnostics.Comparison.LesserOrEqual:
                        case Diagnostics.Comparison.GreaterOrEqual:

                            return true;
                    }

                    return false;

                case Comparison.Lesser:

                    switch (parameter.ExpectedComparisonResult)
                    {
                        case Diagnostics.Comparison.Lesser:
                        case Diagnostics.Comparison.LesserOrEqual:
                        case Diagnostics.Comparison.NotEqual:

                            return true;
                    }

                    return false;

                case Comparison.Greater:

                    switch (parameter.ExpectedComparisonResult)
                    {
                        case Diagnostics.Comparison.Greater:
                        case Diagnostics.Comparison.GreaterOrEqual:
                        case Diagnostics.Comparison.NotEqual:

                            return true;
                    }

                    return false;

                case Comparison.NotEqual:

                    return parameter.ExpectedComparisonResult == Diagnostics.Comparison.NotEqual;

                case Comparison.ReferenceEqual:

                    return parameter.ExpectedComparisonResult == Diagnostics.Comparison.ReferenceEqual;
            }

            throw new InvalidOperationException("The comparison result value returned by the comparison method of the given parameter is not supported.");
        }

#if !WinCopies3
        protected override T ConvertBack(bool value, ComparisonConverterParameter<T> parameter, CultureInfo culture) => throw new InvalidOperationException();
#endif
    }

    public sealed class IntComparisonConverter : ComparisonConverter<int>
    {

    }
}
