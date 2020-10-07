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

namespace WinCopies.Diagnostics
{
    /// <summary>
    /// Comparison types for the If functions.
    /// </summary>
    public enum ComparisonType
    {
        /// <summary>
        /// Check if all conditions are checked.
        /// </summary>
        And,

        /// <summary>
        /// Check if at least one condition is checked.
        /// </summary>
        Or,

        /// <summary>
        /// Check if exactly one condition is checked.
        /// </summary>
        Xor
    }

    /// <summary>
    /// Comparison modes for the If functions.
    /// </summary>
    public enum ComparisonMode
    {
        /// <summary>
        /// Use a binary comparison
        /// </summary>
        Binary,

        /// <summary>
        /// Use a logical comparison
        /// </summary>
        Logical
    }

    /// <summary>
    /// Comparison to perform.
    /// </summary>
    public enum Comparison
    {
        /// <summary>
        /// Check for values equality
        /// </summary>
        Equal,

        /// <summary>
        /// Check for values non-equality
        /// </summary>
        NotEqual,

        /// <summary>
        /// Check if a value is lesser than a given value. This field only works for methods that use lesser/greater/equal comparers.
        /// </summary>
        Lesser,

        /// <summary>
        /// Check if a value is lesser than or equal to a given value. This field only works for methods that use lesser/greater/equal comparers.
        /// </summary>
        LesserOrEqual,

        /// <summary>
        /// Check if a value is greater than a given value. This field only works for methods that use lesser/greater/equal comparers.
        /// </summary>
        Greater,

        /// <summary>
        /// Check if a value is greater than or equal to a given value. This field only works for methods that use lesser/greater/equal comparers.
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// Check if an object reference is equal to a given object reference. This field only works for methods that use equality comparers (not lesser/greater ones).
        /// </summary>
        ReferenceEqual
    }
}
