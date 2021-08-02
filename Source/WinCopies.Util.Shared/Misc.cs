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

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
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

    public abstract class DisposableValue<T> : System.IDisposable where T :
#if !WinCopies3
        WinCopies.Util.
#endif
        DotNetFix.IDisposable
    {
        protected T Value { get; }

        protected DisposableValue(T value) => Value = value == null ? throw
#if WinCopies3
            ThrowHelper
#else
            Util
#endif
            .GetArgumentNullException(nameof(value)) : value;

        protected virtual void Dispose(in bool disposing)
        {
            if (!Value.IsDisposed)

                Value.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
