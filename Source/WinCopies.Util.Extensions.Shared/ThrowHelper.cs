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

#if WinCopies3
using static WinCopies.ThrowHelper;
#endif

using IfCT = WinCopies.Diagnostics.ComparisonType;

namespace WinCopies.Extensions // To avoid name conflicts.
{
    public static class ThrowHelper
    {
        public static void ThrowIfInvalidFlagsEnumValue<T>(in T value, in IfCT comparisonType, in string argumentName, params T[] values) where T : Enum
        {
            if (!value.IsValidFlagsEnumValue(comparisonType, argumentName, values))

#if WinCopies3
                throw GetInvalidEnumArgumentException(argumentName, value);
#else
                throw WinCopies.Util.Extensions.GetInvalidEnumArgumentException(value, argumentName);
#endif
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="WinCopies.Extensions.IsValidEnumValue(Enum)"/>
        /// <seealso cref="WinCopies.ThrowHelper.ThrowIfNotValidEnumValue(in string, in Enum)"/>
        public static void ThrowIfNotValidFlagsEnumValue(in Enum @enum, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            if (!@enum.IsValidFlagsEnumValue(throwIfNotFlagsEnum, throwIfZero))

#if WinCopies3
                throw GetInvalidEnumArgumentException(nameof(@enum), @enum);
#else
                throw WinCopies.Util.Extensions.GetInvalidEnumArgumentException(@enum, nameof(@enum));
#endif
        }

        /// <summary>
        /// Throws an <see cref="InvalidEnumArgumentException"/> if the enum value is not in the required enum value range.
        /// </summary>
        /// <param name="enum">The enum value to check.</param>
        /// <param name="argumentName">The parameter name.</param>
        /// <param name="throwIfNotFlagsEnum">Whether to throw if the given enum does not have the <see cref="FlagsAttribute"/> attribute.</param>
        /// <param name="throwIfZero">Whether to throw if the given enum is zero.</param>
        /// <exception cref="ArgumentException"><paramref name="enum"/> does not have the <see cref="FlagsAttribute"/> and <paramref name="throwIfNotFlagsEnum"/> is set to <see langword="true"/>.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="enum"/> is equal to zero and the <paramref name="throwIfZero"/> parameter is set to true or <paramref name="enum"/> is lesser than zero.</exception>
        /// <seealso cref="WinCopies.Extensions.IsValidEnumValue(Enum)"/>
        public static void ThrowIfNotValidFlagsEnumValue(in Enum @enum, in string argumentName, in bool throwIfNotFlagsEnum, in bool throwIfZero)
        {
            if (!@enum.IsValidFlagsEnumValue(throwIfNotFlagsEnum, throwIfZero))

                throw new InvalidEnumArgumentException(argumentName, (int)Convert.ChangeType(@enum, TypeCode.Int32), @enum.GetType());
        }
    }
}
#endif
