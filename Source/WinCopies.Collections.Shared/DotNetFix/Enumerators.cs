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
using System.Collections;
using System.Collections.Generic;
using static WinCopies.Util.Util;

namespace WinCopies.Collections.DotNetFix
{
    public interface ICountableEnumerator<out T> : IEnumerator<T>
    {
        int Count { get; }
    }

    public interface ICountableDisposableEnumerator<out T> : ICountableEnumerator<T>, WinCopies.Util.DotNetFix.IDisposable
    {
        // Left empty.
    }

    public class ArrayEnumerator<T> : ICountableDisposableEnumerator<T>
    {
        private T[] _array;
        private T _current;
        private int _currentIndex = -1;

        protected T[] Array => IsDisposed ? throw GetExceptionForDispose(false) : _array;

        public int Count => IsDisposed ? throw GetExceptionForDispose(false) : _array.Length;

        public bool IsStarted { get; private set; }

        public bool IsCompleted { get; private set; }

        public T Current => IsStarted && !IsDisposed ? _current : throw new InvalidOperationException("The enumeration is not started or the enumerator is disposed.");

        object IEnumerator.Current => Current;

        protected int CurrentIndex => IsDisposed ? throw GetExceptionForDispose(false) : _currentIndex;

        public bool IsDisposed { get; private set; }

        public ArrayEnumerator(T[] array) => _array = array ?? throw GetArgumentNullException(nameof(array));

        public bool MoveNext()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
                
            if (IsCompleted)

                return false;

            if (_currentIndex < _array.Length)
            {
                IsStarted = true;

                _current = _array[++_currentIndex];

                return true;
            }

            Reset();

            return false;
        }

        public void Reset()
        {
            if (IsDisposed)

                throw GetExceptionForDispose(false);
                
            _currentIndex = -1;

            _current = default;

            IsStarted = false;

            IsCompleted = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            
                return;
                
            if (disposing)
            {
                Reset();

                _array = null;

                IsDisposed = true;
            }
        }

        public void Dispose() => Dispose(true);
    }
}
