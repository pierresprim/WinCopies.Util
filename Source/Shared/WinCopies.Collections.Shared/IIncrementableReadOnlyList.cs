﻿/* Copyright © Pierre Sprimont, 2021
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

#if CS7
using System;
using System.Collections;
using System.Collections.Generic;

using WinCopies.Collections.DotNetFix.Generic;

using static WinCopies.ThrowHelper;

namespace WinCopies.Collections.Generic
{
    public interface IIncrementableReadOnlyList<T> : ICountableEnumerable<T>, ICountableEnumerator<T>
    {
        int CurrentIndex { get; }

        void IncrementIndex(long length);
    }

    public interface IDecrementableReadOnlyList<T> : ICountableEnumerable<T>
    {
        void DecrementIndex(long length);
    }

    public sealed class IncrementableReadOnlyList<T> : Enumerator<T>, IIncrementableReadOnlyList<T>, WinCopies.DotNetFix.IDisposable
    {
        private System.Collections.Generic.IReadOnlyList<T> _list;
        private int _currentIndex = -1;

        int IIncrementableReadOnlyList<T>.CurrentIndex => GetOrThrowIfDisposed(_currentIndex);

        int System.Collections.Generic.IReadOnlyCollection<T>.Count => GetOrThrowIfDisposed(_list).Count;

        int ICountable.Count => GetOrThrowIfDisposed(_list).Count;

        protected override T CurrentOverride => _list[_currentIndex];

        public override bool? IsResetSupported => true;

        public IncrementableReadOnlyList(in System.Collections.Generic.IReadOnlyList<T> list) => _list = list ?? throw GetArgumentNullException(nameof(list));

        void IIncrementableReadOnlyList<T>.IncrementIndex(long length)
        {
            ThrowIfDisposed();

            if (length > int.MaxValue || length > _list.Count - 1 - _currentIndex)

                throw new ArgumentOutOfRangeException(nameof(length));

            _currentIndex += (int)length;
        }

        //ICountableEnumerator<T> ICountableEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => IsDisposed ? throw GetExceptionForDispose(false) : new IncrementableIReadOnlyList<T>(_list);

        protected override bool MoveNextOverride() => ++_currentIndex < _list.Count;

        protected override void ResetCurrent() => _currentIndex = -1;

        protected override void ResetOverride2() { /* Left empty. */ }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            _list = null;
        }

        public ICountableEnumerator<T> GetEnumerator() => GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
#if !CS9
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() => GetEnumerator();
#endif
    }

    // TODO:
#if IncrementableStreamCursor
    public sealed class IncrementableStreamCursor<T> : Enumerator<T>, IIncrementableReadOnlyList<T>, WinCopies.DotNetFix.IDisposable
    {
        private System.IO.Stream _stream;
        private T[] _buffer;
        private int _bufferSize;
        private int _currentIndex;

        public override bool? IsResetSupported => true;

        protected override T CurrentOverride => _buffer[_currentIndex];

        int IIncrementableReadOnlyList<T>.CurrentIndex => GetOrThrowIfDisposed(_currentIndex);

        int ICountableEnumerable<T, ICountableEnumerator<T>>.Count => _stream.Length;

        protected override bool MoveNextOverride() =>  ;

        ICountableEnumerator<T> ICountableEnumerable<T, ICountableEnumerator<T>>.GetEnumerator() => IsDisposed ? throw GetExceptionForDispose(false) : new IncrementableStreamCursor<T>(_stream);

        void IIncrementableReadOnlyList<T>.IncrementIndex(long length) => _stream.Seek(length, SeekOrigin.Current);
    }
#endif
}
#endif
