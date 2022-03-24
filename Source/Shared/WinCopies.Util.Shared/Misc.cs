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
using System.IO;

using static WinCopies.
#if WinCopies3
    ThrowHelper
#else
    Util.Util
#endif
    ;

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

    public sealed class NullableGeneric<T>
    {
        public T Value { get; }

        public NullableGeneric(T value) => Value = value;
    }

    public interface IPropertyObservable : DotNetFix.IDisposable
    {
        void AddPropertyChangedDelegate(Action<string> action);

        void RemovePropertyChangedDelegate(Action<string> action);
    }

    public sealed class NullableReference<T> where T : class
    {
        public T Value { get; }

        public NullableReference(T value) => Value = value;
    }

    public delegate bool TaskAwaiterPredicate(ref bool cancel);

    public class StreamInfo : System.IO.Stream, DotNetFix.IDisposable
    {
        protected System.IO.Stream Stream { get; }

        public override bool CanRead => Stream.CanRead;

        public override bool CanSeek => Stream.CanSeek;

        public override bool CanWrite => Stream.CanWrite;

        public override long Length => Stream.Length;

        public override long Position { get => Stream.Position; set => Stream.Position = value; }

        public bool IsDisposed { get; private set; }

        public StreamInfo(in System.IO.Stream stream) => Stream = stream ?? throw GetArgumentNullException(nameof(stream));

        public override void Flush() => Stream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => Stream.Seek(offset, origin);

        public override void SetLength(long value) => Stream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => Stream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            IsDisposed = true;
        }
    }

    public class BooleanEventArgs : EventArgs
    {
        public bool Value { get; }

        public BooleanEventArgs(in bool value) => Value = value;
    }

    public interface ISplitFactory<T, U, TContainer>
    {
        TContainer Container { get; }

        int SubCount { get; }

        void SubClear();
    }

    public interface IValueSplitFactory<T, U, TContainer> : ISplitFactory<T, U, TContainer>
    {
        void Add(U enumerable);

        U GetEnumerable();

        void SubAdd(T value);
    }

    public interface IRefSplitFactory<T, U, V, TContainer> : ISplitFactory<T, U, TContainer> where T : class
    {
        void Add(V enumerable);

        V GetEnumerable();

        void SubAdd(U value);

        U GetValueContainer(T value);
    }

    public interface IPopable<TIn, TOut>
    {
        TOut Pop(TIn key);
    }

    public interface IAsEnumerable<T>
    {
        System.Collections.Generic.IEnumerable<T> AsEnumerable();
    }
}
