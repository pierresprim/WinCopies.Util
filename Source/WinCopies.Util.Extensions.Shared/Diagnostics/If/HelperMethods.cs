/* Copyright © Pierre Sprimont, 2020
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

using WinCopies.Collections;
using WinCopies.Collections.Generic;

using IfComp = WinCopies.Diagnostics.Comparison;

namespace WinCopies.Diagnostics
{
    internal static class HelperMethods
    {
        internal static bool CheckIfComparison(in IfComp comparison, in Func<bool> predicateResult, in int result)
        {
            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

            switch (comparison)
            {
                case IfComp.Equal:
                case IfComp.ReferenceEqual:

                    return result == 0;

                case IfComp.LesserOrEqual:

                    return result <= 0;

                case IfComp.GreaterOrEqual:

                    return result >= 0;

                case IfComp.Lesser:

                    return result < 0;

                case IfComp.Greater:

                    return result > 0;

                case IfComp.NotEqual:

                    return !predicateResult() || result != 0;

                default:

                    return false;//: comparisonType == ComparisonType.Or ?//(result == 0 && (comparison == Comparison.Equals || comparison == Comparison.LesserOrEquals || comparison == Comparison.GreaterOrEquals)) ||//    (result < 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.LesserThan || comparison == Comparison.LesserOrEquals)) ||//    (result > 0 && (comparison == Comparison.DoesNotEqual || comparison == Comparison.GreaterThan || comparison == Comparison.GreaterOrEquals))
            }
        }

        internal static bool CheckEqualityComparison(in IfComp comparison, in object value, in object valueToCompare, in Func<bool> predicateResult, in EqualityComparison comparisonDelegate)
        {
            if (comparison == IfComp.ReferenceEqual && !value.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");

            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

#if CS7

            switch (comparison)
            {

                case IfComp.Equal:

                    return comparisonDelegate(value, valueToCompare);

                case IfComp.NotEqual:
            
                    return !predicateResult() || !comparisonDelegate(value, valueToCompare);

#pragma warning disable IDE0002

                case IfComp.ReferenceEqual:
            
                    return object.ReferenceEquals(value, valueToCompare);

#pragma warning restore IDE0002

                default:
            
                    return false;

            }

#else

            return comparison switch
            {
                IfComp.Equal => comparisonDelegate(value, valueToCompare),

                IfComp.NotEqual => !predicateResult() || !comparisonDelegate(value, valueToCompare),

#pragma warning disable IDE0002

                IfComp.ReferenceEqual => object.ReferenceEquals(value, valueToCompare),

#pragma warning restore IDE0002

                _ => false
            };

#endif
        }

        internal static bool CheckEqualityComparison<T>(in IfComp comparison, in T value, in T valueToCompare, in Func<bool> predicateResult, in EqualityComparison<T> comparisonDelegate)
        {
            // Because we've already checked that for the 'T' type in the 'If' method and assuming that 'T' is the base type of all the values to test, if 'T' is actually a class, we don't need to check here if the type of the current value is actually a class when comparison is set to ReferenceEqual.

            if (comparison != IfComp.NotEqual && !predicateResult()) return false;

#if CS7

            switch (comparison)
            {
                case IfComp.Equal:
            
                    return comparisonDelegate(value, valueToCompare);

                case IfComp.NotEqual:
            
                    return !predicateResult() || !comparisonDelegate(value, valueToCompare);

#pragma warning disable IDE0002

                case IfComp.ReferenceEqual:
            
                    return object.ReferenceEquals(value, valueToCompare);

#pragma warning restore IDE0002

                default:
            
                    return false;
            }

#else

            return comparison switch
            {
                IfComp.Equal => comparisonDelegate(value, valueToCompare),

                IfComp.NotEqual => !predicateResult() || !comparisonDelegate(value, valueToCompare),

#pragma warning disable IDE0002

                IfComp.ReferenceEqual => object.ReferenceEquals(value, valueToCompare),

#pragma warning restore IDE0002

                _ => false
            };

#endif
        }

        internal delegate bool CheckIfComparisonDelegate(in object value, in Func<bool> predicate);

        internal delegate bool CheckIfComparisonDelegate<T>(in T value, in Func<bool> predicate);
    }
}
