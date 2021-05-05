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

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    /// <summary>
    /// This class contains static methods that can be used as delegates.
    /// </summary>
    public static class Delegates
    {
        public static T Self<T>(T value) => value;
    }

    /// <summary>
    /// This class contains static methods that can be used as delegates for <see cref="bool"/> values.
    /// </summary>
    public static class Bool
    {
        public static bool And(bool x, bool y) => x && y;

        public static bool Or(bool x, bool y) => x || y;

        public static bool XOr(bool x, bool y) => x ^ y;

        public static bool Reversed(bool value) => !value;

        public static bool True(bool value) => true;

        public static bool False(bool value) => false;

        public static bool IsTrue(bool? value) => value == true;

        public static bool IsFalse(bool? value) => value == false;
    }
}
