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

#if CS9 && WinCopies3
using static WinCopies.ThrowHelper;
#endif

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public interface IDisposable :
#if !WinCopies3
        WinCopies.Util.DotNetFix.IDisposable
#else
        WinCopies.DotNetFix.IDisposable
#endif
    {
        bool IsDisposing { get; }
    }

    namespace DotNetFix
    {
        /// <summary>
        /// Provides a mechanism for releasing unmanaged resources.
        /// </summary>
        /// <seealso cref="System.IDisposable"/>
        public interface IDisposable : System.IDisposable
        {
            /// <summary>
            /// Gets a value that indicates whether the current object is disposed.
            /// </summary>
            bool IsDisposed { get; }

#if CS9 && WinCopies3
            T GetOrThrowIfDisposed<T>(in T value) => IsDisposed ? throw GetExceptionForDispose(false) : value;
#endif
        }
    }
}
