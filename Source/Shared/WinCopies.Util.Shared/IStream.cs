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

#if !WinCopies4
using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

using static WinCopies.ThrowHelper;

namespace WinCopies
{
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

    /// <summary>
    /// The <see cref="StreamCommit"/> enumeration constants specify the conditions for performing the commit operation in the IStorage::Commit and <see cref="System.Runtime.InteropServices.ComTypes.IStream.Seek(long, int, IntPtr)"/> methods.
    /// </summary>
    [Flags]
    public enum StreamCommit
    {
        /// <summary>
        /// You can specify this condition with <see cref="Consolidate"/>, or some combination of the other three flags in this list of elements. Use this value to increase the readability of code.
        /// </summary>
        Default = 0,

        /// <summary>
        /// <para>The commit operation can overwrite existing data to reduce overall space requirements. This value is not recommended for typical usage because it is not as robust as the default value. In this case, it is possible for the commit operation to fail after the old data is overwritten, but before the new data is completely committed. Then, neither the old version nor the new version of the storage object will be intact.</para>
        /// <para>You can use this value in the following cases:</para>
        /// <para>The user is willing to risk losing the data.</para>
        /// <para>The low-memory save sequence will be used to safely save the storage object to a smaller file.</para>
        /// <para>A previous commit returned STG_E_MEDIUMFULL, but overwriting the existing data would provide enough space to commit changes to the storage object.</para>
        /// <para>Be aware that the commit operation verifies that adequate space exists before any overwriting occurs. Thus, even with this value specified, if the commit operation fails due to space requirements, the old data is safe. It is possible, however, for data loss to occur with the <see cref="Overwrite"/> value specified if the commit operation fails for any reason other than lack of disk space.</para>
        /// </summary>
        Overwrite = 1,

        /// <summary>
        /// Prevents multiple users of a storage object from overwriting each other's changes. The commit operation occurs only if there have been no changes to the saved storage object because the user most recently opened it. Thus, the saved version of the storage object is the same version that the user has been editing. If other users have changed the storage object, the commit operation fails and returns the STG_E_NOTCURRENT value. To override this behavior, call the IStorage::Commit or <see cref="System.Runtime.InteropServices.ComTypes.IStream.Commit(int)"/> method again using the <see cref="Default"/> value.
        /// </summary>
        OnlyIfCurrent = 2,

        /// <summary>
        /// <para>Commits the changes to a write-behind disk cache, but does not save the cache to the disk. In a write-behind disk cache, the operation that writes to disk actually writes to a disk cache, thus increasing performance. The cache is eventually written to the disk, but usually not until after the write operation has already returned. The performance increase comes at the expense of an increased risk of losing data if a problem occurs before the cache is saved and the data in the cache is lost.</para>
        /// <para>If you do not specify this value, then committing changes to root-level storage objects is robust even if a disk cache is used. The two-phase commit process ensures that data is stored on the disk and not just to the disk cache.</para>
        /// </summary>
        DangerouslyCommitMerelyToDiskCache = 4,

        /// <summary>
        /// Windows 2000 and Windows XP: Indicates that a storage should be consolidated after it is committed, resulting in a smaller file on disk. This flag is valid only on the outermost storage object that has been opened in transacted mode. It is not valid for streams. The <see cref="Consolidate"/> flag can be combined with any other <see cref="StreamCommit"/> flags.
        /// </summary>
        Consolidate = 8
    }

    public interface IStream : DotNetFix.IDisposable
    {
        ulong Position { get; }

        int Read(byte[] bytes, int length);
#if CS8
        byte[] Read(int length, out int read)
        {
            byte[] bytes = new byte[length];

            read = Read(bytes, length);

            return bytes;
        }

        int Write(byte[] bytes) => Write(bytes ?? throw GetArgumentNullException(nameof(bytes)), bytes.Length);
#endif
        int Write(byte[] bytes, int cb);

        void Seek(long move, SeekOrigin origin);

        void Flush();
    }

    public interface ICountableStream : IStream
    {
        ulong Length { get; }
    }

    public class NativeStream : ICountableStream
    {
        private System.Runtime.InteropServices.ComTypes.IStream _stream;
        private ulong _position;

        public System.Runtime.InteropServices.ComTypes.IStream InnerStream => _stream ?? throw GetExceptionForDispose(false);

        public ulong Position { get => IsDisposed ? throw GetExceptionForDispose(false) : _position; private set => _position = IsDisposed ? throw GetExceptionForDispose(false) : value; }

        public bool IsDisposed => InnerStream == null;

        public ulong Length
        {
            get
            {
                InnerStream.Stat(out STATSTG stat, 1);

                return (ulong)stat.cbSize;
            }
        }

        public NativeStream(in System.Runtime.InteropServices.ComTypes.IStream stream) => _stream = stream ?? throw GetArgumentNullException(nameof(stream));

        unsafe int IStream.Read(byte[] bytes, int length)
        {
            ThrowIfDisposed(this);

            uint _read;

            InnerStream.Read(bytes, length, (IntPtr)(&_read));

            Position += _read;

            return (int)_read;
        }

        unsafe int IStream.Write(byte[] bytes, int cb)
        {
            ThrowIfDisposed(this);

            uint _written;

            InnerStream.Write(bytes, cb, (IntPtr)(&_written));

            return (int)_written;
        }

        unsafe void IStream.Seek(long move, SeekOrigin origin)
        {
            ThrowIfDisposed(this);

            ulong result;

            InnerStream.Seek(move, (int)origin, (IntPtr)(&result));

            Position = result;
        }

        public void Flush()
        {
            ThrowIfDisposed(this);

            InnerStream.Commit((int)StreamCommit.Default);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)

                Flush();

            _stream = null;
        }

        void System.IDisposable.Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        ~NativeStream() => Dispose(false);
    }

    public class Stream : ICountableStream
    {
        private System.IO.Stream _stream;

        public System.IO.Stream InnerStream => _stream ?? throw GetExceptionForDispose(false);

        ulong IStream.Position => (ulong)InnerStream.Position;

        public bool IsDisposed => _stream == null;

        ulong ICountableStream.Length => (ulong)InnerStream.Length;

        public Stream(in System.IO.Stream stream) => _stream = stream;

        void IStream.Flush() => InnerStream.Flush();

        int IStream.Read(byte[] bytes, int length) => InnerStream.Read(bytes, 0, length);

        void IStream.Seek(long move, SeekOrigin origin) => InnerStream.Seek(move, origin);

        int IStream.Write(byte[] bytes, int cb)
        {
            InnerStream.Write(bytes, 0, cb);

            return cb;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerStream.Flush();
                InnerStream.Dispose();

                _stream = null;
            }
        }

        void System.IDisposable.Dispose()
        {
            if (!IsDisposed)
            {
                Dispose(true);

                GC.SuppressFinalize(this);
            }
        }

        ~Stream() => Dispose(false);
    }

    public abstract class Stream<T> : System.IO.Stream where T : System.IO.Stream
    {
        protected T InnerStream { get; }

        public override bool CanRead => InnerStream.CanRead;

        public override bool CanSeek => InnerStream.CanSeek;

        public override bool CanWrite => InnerStream.CanWrite;

        public override long Length => InnerStream.Length;

        public override long Position
        {
            get => InnerStream.Position;

            set
            {
                long old = InnerStream.Position;

                InnerStream.Position = value;

                OnPositionChanged(old, value);
            }
        }

        public Stream(in T stream) => InnerStream = stream;

        protected abstract void OnPositionChanged(in long old, in long @new);

        public override void Flush() => InnerStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            long old = Position;

            int result = InnerStream.Read(buffer, offset, count);

            OnPositionChanged(old, Position);

            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long old = Position;

            long result = InnerStream.Seek(offset, origin);

            OnPositionChanged(old, Position);

            return result;
        }

        public override void SetLength(long value) => InnerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
        {
            long old = Position;

            InnerStream.Write(buffer, offset, count);

            OnPositionChanged(old, Position);
        }
    }

    public class ActionStream<T> : Stream<T> where T : System.IO.Stream
    {
        protected Action<long, long> Action { get; }

        public ActionStream(in T stream, in Action<long, long> action) : base(stream) => Action = action;

        protected override void OnPositionChanged(in long old, in long @new) => Action(old, @new);
    }

    namespace DotNetFix
    {
        public interface IWriterStream : System.IDisposable
        {
            ulong Length { get; }

            void Write(byte[] buffer, uint offset, uint count);
#if CS8
            void Write(ReadOnlySpan<byte> buffer);
#endif
        }

        public interface IDeleteableWriterStream : IWriterStream
        {
            bool CanDelete { get; }

            void Delete();
        }

        public abstract class ExtendedStream : System.IO.Stream, IWriterStream
        {
            public abstract bool CanDelete { get; }

            ulong IWriterStream.Length => (ulong)Length;

            void IWriterStream.Write(byte[] buffer, uint offset, uint count) => Write(buffer, (int)offset, (int)count);
#if CS8
            void IWriterStream.Write(ReadOnlySpan<byte> buffer) => Write(buffer);
#endif

            public abstract void Delete();
        }

        public abstract class ReadOnlyStream : ExtendedStream
        {
            public override bool CanRead => true;
            public override bool CanWrite => false;
            public override bool CanDelete => false;

            public override void Delete() => throw new NotSupportedException("This stream does not support deletion.");
        }

        public abstract class Stream<T> : ExtendedStream where T : System.IO.Stream
        {
            protected T InnerStream { get; }

            public override bool CanSeek => InnerStream.CanSeek;
            public override bool CanRead => InnerStream.CanRead;
            public override bool CanWrite => InnerStream.CanWrite;
            public override long Length => InnerStream.Length;
            public override long Position { get => InnerStream.Position; set => InnerStream.Position = value; }

            protected Stream(in T stream) => InnerStream = stream;

            public override void Flush() => InnerStream.Flush();
            public override int Read(byte[] buffer, int offset, int count) => InnerStream.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => InnerStream.Seek(offset, origin);
            public override void SetLength(long value) => InnerStream.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => InnerStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                InnerStream.Dispose();

                base.Dispose(disposing);
            }
        }

        public class FileStream : Stream<System.IO.FileStream>
        {
            public string Path => InnerStream.Name;

            public override bool CanDelete => true;

            public FileStream(in System.IO.FileStream fileStream) : base(fileStream) { /* Left empty. */ }

            public override void Delete() => File.Delete(Path);
        }
    }
}
#endif
