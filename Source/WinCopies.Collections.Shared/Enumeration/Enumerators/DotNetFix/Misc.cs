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

using System.Collections.Generic;
using WinCopies.Collections.Generic;

namespace WinCopies.Collections.DotNetFix
{
#if WinCopies3
    public interface IEnumeratorBase
    {
        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the collection.</returns>
        /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        bool MoveNext();

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
        void Reset();
    }

    public interface IEnumeratorInfo
    {
        bool? IsResetSupported { get; }

        bool IsStarted { get; }

        bool IsCompleted { get; }
    }

    public interface IDisposableEnumerator : System.Collections.IEnumerator, WinCopies.DotNetFix.IDisposable
    {
        // Left empty.
    }

    public interface IDisposableEnumeratorInfo : IEnumeratorInfo, WinCopies.DotNetFix.IDisposable
    {
        // Left empty.
    }

    public interface IEnumerator : System.Collections.IEnumerator, IEnumeratorBase
    {
#if WinCopies3
        bool MoveNext();

        void Reset();
#else
        // Left empty.
#endif
    }
#endif

    namespace Generic
    {
#if WinCopies3
        public interface IEnumerator<out T> : System.Collections.Generic.IEnumerator<T>, IEnumeratorBase
        {
            // Left empty.
        }

        public interface IDisposableEnumerator<out T> : System.Collections.Generic.IEnumerator<T>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }
#endif
    }
}
