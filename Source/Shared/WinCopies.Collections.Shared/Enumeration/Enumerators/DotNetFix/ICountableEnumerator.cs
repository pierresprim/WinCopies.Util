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

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerator : System.Collections.IEnumerator, ICountable
    {
        // Left empty.
    }

    public interface ICountableDisposableEnumerator : ICountableEnumerator, WinCopies.DotNetFix.IDisposable
    {
        // Left empty.
    }

    public interface IUIntCountableEnumerator : System.Collections.IEnumerator, IUIntCountable
    {
        // Left empty.
    }

    public interface IUIntCountableDisposableEnumerator : IUIntCountableEnumerator, WinCopies.DotNetFix.IDisposable
    {
        // Left empty.
    }

    namespace Generic
    {
        public interface ICountableEnumerator<
#if CS5
            out
#endif
            T> : System.Collections.Generic.IEnumerator<T>, ICountableEnumerator
        {
            // Left empty.
        }

        public interface ICountableDisposableEnumerator<
#if CS5
            out
#endif
             T> : ICountableEnumerator<T>, ICountableDisposableEnumerator
        {
            // Left empty.
        }

        public interface IUIntCountableEnumerator<
#if CS5
            out
#endif
            T> : System.Collections.Generic.IEnumerator<T>, IUIntCountableEnumerator
        {
            // Left empty.
        }

        public interface IUIntCountableDisposableEnumerator<
#if CS5
            out
#endif
             T> : IUIntCountableEnumerator<T>, IUIntCountableDisposableEnumerator
        {
            // Left empty.
        }
    }
}
