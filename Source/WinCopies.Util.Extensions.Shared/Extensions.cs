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
using System.Diagnostics;

using static WinCopies.ThrowHelper;
using static WinCopies.Collections.ThrowHelper;

using IfCT = WinCopies.Diagnostics.ComparisonType;

namespace WinCopies.Util.Extensions
{
    public static class Extensions
    {
        public static bool IsValidFlagsEnumValue<T>(this T value, in IfCT comparisonType, in string argumentName, params T[] values) where T : Enum

        {
            ThrowIfNull(values, nameof(values));

            ThrowIfNotFlagsEnumType<T>(nameof(T));

            ThrowIfNotValidEnumValue(nameof(comparisonType), comparisonType);

            switch (comparisonType)
            {
                case IfCT.And:

                    foreach (T _value in values)

                        if (!value.HasFlag(_value))

                            return false;

                    return true;

                case IfCT.Or:

                    foreach (T _value in values)

                        if (value.HasFlag(_value))

                            return true;

                    return false;

                case IfCT.Xor:

                    bool oneResultFound = false;

                    foreach (T _value in values)

                        if (value.HasFlag(_value))

                            if (oneResultFound)

                                return false;

                            else

                                oneResultFound = true;

                    return oneResultFound;

                default:

                    Debug.Assert(false);

                    return false;
            }
        }
    }
}
