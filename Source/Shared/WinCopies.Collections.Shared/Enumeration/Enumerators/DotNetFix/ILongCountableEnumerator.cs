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

namespace WinCopies.Collections.DotNetFix
{
    public interface ILongCountableEnumerator : System.Collections.IEnumerator, ILongCountable
    {
        // Left empty.
    }

    public interface ILongCountableDisposableEnumerator : ILongCountableEnumerator, WinCopies.DotNetFix.IDisposable
    {
        // Left empty.
    }

    public interface IULongCountableEnumerator : System.Collections.IEnumerator, IULongCountable
    {
        // Left empty.
    }

    public interface IULongCountableDisposableEnumerator : IULongCountableEnumerator, WinCopies.DotNetFix.IDisposable
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface ILongCountableEnumerator<
#if CS5
            out
#endif
            T> : System.Collections.Generic.IEnumerator<T>, ILongCountableEnumerator
        {
            // Left empty.
        }

        public interface ILongCountableDisposableEnumerator<
#if CS5
            out
#endif
             T> : ILongCountableEnumerator<T>, ILongCountableDisposableEnumerator
        {
            // Left empty.
        }

        public interface IULongCountableEnumerator<
#if CS5
            out
#endif
            T> : System.Collections.Generic.IEnumerator<T>, IULongCountableEnumerator
        {
            // Left empty.
        }

        public interface IULongCountableDisposableEnumerator<
#if CS5
            out
#endif
             T> : IULongCountableEnumerator<T>, IULongCountableDisposableEnumerator
        {
            // Left empty.
        }
    }
}
