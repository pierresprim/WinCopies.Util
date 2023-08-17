#if CS8
using System.IO;

namespace WinCopies.Util
{
    public static partial class Extensions
    {
        private class HttpStream(in System.IO.Stream stream, in long? length) : System.IO.Stream
        {
            private readonly long? _length = length;
            private readonly System.IO.Stream _stream = stream;

            public override bool CanRead => _stream.CanRead;

            public override bool CanSeek => _stream.CanSeek;

            public override bool CanWrite => _stream.CanWrite;

            public override long Length => _length.HasValue ? _length.Value : -1;

            public override long Position { get => _stream.Position; set => _stream.Position = value; }

            public override void Flush() => _stream.Flush();
            public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
            public override void SetLength(long value) => _stream.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
        }
    }
}
#endif