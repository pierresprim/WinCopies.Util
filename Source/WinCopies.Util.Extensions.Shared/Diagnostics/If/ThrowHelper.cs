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

#if CS7

using System;

using WinCopies.Collections;
using WinCopies.Util;

using static WinCopies.
#if WinCopies3
    ThrowHelper;
#else
    Extensions.Extensions;
#endif

using IfCT = WinCopies.Diagnostics.ComparisonType;
using IfCM = WinCopies.Diagnostics.ComparisonMode;
using IfComp = WinCopies.Diagnostics.Comparison;

#if WinCopies3
using WinCopies.Collections.Generic;
#endif

namespace WinCopies.Diagnostics
{
    internal static class ThrowHelper
    {
        internal static void ThrowOnInvalidIfMethodArg(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison)
        {
            ThrowIfNotValidEnumValue(nameof(comparisonType), comparisonType);
            ThrowIfNotValidEnumValue(nameof(comparisonMode), comparisonMode);
            ThrowIfNotValidEnumValue(nameof(comparison), comparison);

            if (comparison == IfComp.ReferenceEqual)

                throw new InvalidEnumArgumentException(nameof(comparison), (int)IfComp.ReferenceEqual, typeof(IfComp));
        }

        internal static void ThrowOnInvalidEqualityIfMethodEnumValue(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison)
        {
            ThrowIfNotValidEnumValue(nameof(comparisonType), comparisonType);
            ThrowIfNotValidEnumValue(nameof(comparisonMode), comparisonMode);

            if (!(comparison == IfComp.Equal || comparison == IfComp.NotEqual || comparison == IfComp.ReferenceEqual))

                // todo:

                throw new ArgumentException($"{comparison} must be equal to {nameof(IfComp.Equal)}, {nameof(IfComp.NotEqual)} or {nameof(IfComp.ReferenceEqual)}");
        }

        internal static void ThrowOnInvalidEqualityIfMethodArg(in IfCT comparisonType, in IfCM comparisonMode, in IfComp comparison, in Type valueType, in EqualityComparison comparisonDelegate)
        {
            ThrowOnInvalidEqualityIfMethodEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual && comparisonDelegate != null)

                throw new ArgumentException($"{nameof(comparisonDelegate)} have to be set to null in order to use this method with the {nameof(IfComp.ReferenceEqual)} enum value.");

            if (comparison == IfComp.ReferenceEqual && !valueType.GetType().IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");
        }

        internal static void ThrowOnInvalidEqualityIfMethodArg<T>(IfCT comparisonType, IfCM comparisonMode, IfComp comparison, EqualityComparison<T> comparisonDelegate)
        {
            ThrowOnInvalidEqualityIfMethodEnumValue(comparisonType, comparisonMode, comparison);

            if (comparison == IfComp.ReferenceEqual && comparisonDelegate != null)

                throw new ArgumentException($"{nameof(comparisonDelegate)} have to be set to null in order to use this method with the {nameof(IfComp.ReferenceEqual)} enum value.");

            if (comparison == IfComp.ReferenceEqual && !typeof(T).IsClass) throw new InvalidOperationException("ReferenceEqual comparison is only valid with class types.");
        }
    }
}

#endif
