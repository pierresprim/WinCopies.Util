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
        public static void EmptyVoid(object parameter) { }

        public static void EmptyVoid<T>(T parameter) { }

        public static object Null(object parameter) => null;

        public static T Null<T>(T parameter) where T : class => null;

        public static TOut Null<TIn, TOut>(TIn parameter) where TOut : class => null;

        public static T Self<T>(T value) => value;

        public static T SelfIn<T>(in T value) => value;

        public static object NullIn(in object parameter) => null;

        public static T NullIn<T>(in T parameter) where T : class => null;

        public static TOut NullIn<TIn, TOut>(in TIn parameter) where TOut : class => null;

        public static TOut Convert<TIn, TOut>(TIn value) where TOut : TIn => (TOut)value;

        public static TOut ConvertIn<TIn, TOut>(in TIn value) where TOut : TIn => (TOut)value;

        public static TOut ConvertBack<TIn, TOut>(TIn value) where TIn : TOut => value;

        public static TOut ConvertBackIn<TIn, TOut>(in TIn value) where TIn : TOut => value;
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

        public static bool AndIn(in bool x, in bool y) => x && y;

        public static bool OrIn(in bool x, in bool y) => x || y;

        public static bool XOrIn(in bool x, in bool y) => x ^ y;

        public static bool ReversedIn(in bool value) => !value;

#if !WinCopies3
        public static bool True(bool value) => true;

        public static bool False(bool value) => false;
#endif

        public static bool True() => true;

        public static bool False() => false;

        public static bool True(object value) => true;

        public static bool False(object value) => false;

        public static bool True<T>(T value) => true;

        public static bool False<T>(T value) => false;

        public static bool IsTrue(bool? value) => value == true;

        public static bool IsFalse(bool? value) => value == false;

        public static bool TrueIn(in object value) => true;

        public static bool FalseIn(in object value) => false;

        public static bool TrueIn<T>(in T value) => true;

        public static bool FalseIn<T>(in T value) => false;

        public static bool IsTrueIn(in bool? value) => value == true;

        public static bool IsFalseIn(in bool? value) => value == false;
    }
}
