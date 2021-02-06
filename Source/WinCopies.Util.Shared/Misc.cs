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

#if WinCopies3
namespace WinCopies
{
#else
    namespace WinCopies.Util
    {
#endif
    /// <summary>
    /// This enum is designed as an extension of the <see cref="bool"/> type.
    /// </summary>
    public enum Result : sbyte
    {
        /// <summary>
        /// An error as occurred.
        /// </summary>
        Error = -3,

        /// <summary>
        /// The operation has been canceled.
        /// </summary>
        Canceled = -2,

        /// <summary>
        /// The operation did not return any particular value. This value is the same as returning a <see langword="null"/> <see cref="Nullable{Boolean}"/>.
        /// </summary>
        None = -1,

        /// <summary>
        /// The operation returned False. This value is the same number as <see langword="false"/>.
        /// </summary>
        False = 0,

        /// <summary>
        /// The operation returned True. This value is the same number as <see langword="true"/>.
        /// </summary>
        True = 1
    }

    public enum XOrResult : sbyte
    {
        MoreThanOneTrueResult = -1,

        NoTrueResult = 0,

        OneTrueResult = 1
    }

    /// <summary>
    /// Delegate for a non-generic predicate.
    /// </summary>
    /// <param name="value">The value to test</param>
    /// <returns><see langword="true"/> if the predicate success, otherwise <see langword="false"/>.</returns>
    public delegate bool Predicate(object value);

    public delegate T Converter<T>(object obj);

    public delegate void ActionParams(params object[] args);

    public delegate void ActionParams<in T>(params T[] args);

    /// <summary>
    /// Represents a delegate that returns an object.
    /// </summary>
    /// <returns>Any object.</returns>
    public delegate object Func();

    public delegate object FuncParams(params object[] args);

    public delegate TResult FuncParams<in TParams, out TResult>(params TParams[] args);
}
