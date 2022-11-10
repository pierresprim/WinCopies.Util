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
        // Left empty.
    }

    namespace Generic
    {
        public interface IDisposableEnumerator<
#if CS5
            out
#endif
             T> : System.Collections.Generic.IEnumerator<T>, WinCopies.DotNetFix.IDisposable
        {
            // Left empty.
        }

        public sealed class DisposableEnumerator<T> : IDisposableEnumerator<T>
        {
            private System.Collections.Generic.IEnumerator<T> _enumerator;

            private TValue GetOrThrowIfDisposed<TValue>(in TValue value) => WinCopies.ThrowHelper.GetOrThrowIfDisposed(this, value);

            public T Current => GetOrThrowIfDisposed(_enumerator).Current;
            object System.Collections.IEnumerator.Current => Current;

            public bool IsDisposed => _enumerator == null;

            public DisposableEnumerator(in System.Collections.Generic.IEnumerator<T> enumerator) => _enumerator = enumerator;
            public DisposableEnumerator(in System.Collections.Generic.IEnumerable<T> enumerable) : this(enumerable.GetEnumerator()) { /* Left empty. */ }

            public bool MoveNext() => GetOrThrowIfDisposed(_enumerator).MoveNext();

            public void Reset() => GetOrThrowIfDisposed(_enumerator).Reset();

            public void Dispose() => _enumerator = null;
        }
    }
}
