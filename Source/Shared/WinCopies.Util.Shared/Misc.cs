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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
#if NETCORE || NET
using System.Runtime.Loader;
#endif

using static WinCopies.
#if WinCopies3
    ThrowHelper;
using static WinCopies.UtilHelpers;

using WinCopies.Util;
using System.Linq;
#else
    Util.Util;

using static WinCopies.Util.ThrowHelper;
#endif

namespace WinCopies
#if !WinCopies3
    .Util
#endif
{
    public interface IArrayValueProvider<T>
    {
#if CS5
        System.Collections.Generic.IReadOnlyList<T>
#else
        T[]
#endif
        Items
        { get; }

        int CurrentIndex { get; }

        T CurrentValue { get; }
    }

    public struct ArrayValueProvider<T> : IArrayValueProvider<T>
    {
        public
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            Items
        { get; }

        public int CurrentIndex { get; private set; }

        public T CurrentValue => Items[CurrentIndex++];

        public ArrayValueProvider(in
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            items, in int startIndex)
        {
            Items = items;
            CurrentIndex = startIndex;
        }

        public ArrayValueProvider(in
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            items) : this(items, 0) { /* Left empty. */ }
    }

    public struct ArrayValueProvider2<T> : IArrayValueProvider<T>
    {
        private ArrayValueProvider<T> _values;

        public
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            Items => _values.Items;

        public int CurrentIndex => _values.CurrentIndex;

        public T CurrentValue
        {
            get
            {
                if (Count < Length)
                {
                    Count++;

                    return _values.CurrentValue;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public int Count { get; private set; }

        public int Length { get; }

        public ArrayValueProvider2(in ArrayValueProvider<T> values, in int length)
        {
            _values = values;
            Length = length;
        }

        public ArrayValueProvider2(in
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            items, in int startIndex, in int length) : this(new ArrayValueProvider<T>(items, startIndex), length) { /* Left empty. */ }

        public ArrayValueProvider2(in
#if CS5
            System.Collections.Generic.IReadOnlyList<T>
#else
            T[]
#endif
            items, in int length) : this(new ArrayValueProvider<T>(items), length) { /* Left empty. */ }
    }

#if CS8 && !NETSTANDARD
    public class AssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
    {
        protected AssemblyDependencyResolver Resolver { get; }
        public string Path { get; }

        public AssemblyLoadContext(string path, bool isCollectible = false) : base(isCollectible) => Resolver = new AssemblyDependencyResolver(Path = path);

        protected override Assembly Load(AssemblyName assemblyName) => PerformActionIfNotNull(Resolver.ResolveAssemblyToPath(assemblyName), LoadFromAssemblyPath);

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName) => PerformActionIfNull(Resolver.ResolveUnmanagedDllToPath(unmanagedDllName), IntPtr.Zero, LoadUnmanagedDllFromPath);
    }
#endif

    public static class Convert
    {
#if CS8
        private const string VALUE = "value";
#endif

        public static bool TryChangeType(in object
#if CS8
            ?
#endif
            value, Type type,
#if CS8
            [NotNullIfNotNull(VALUE)]
#endif
            out object
#if CS8
            ?
#endif
            result)
        {
            if (value is IConvertible && (type = System.Nullable.GetUnderlyingType(type) ?? type).IsAssignableTo<IConvertible>())
            {
                result = System.Convert.ChangeType(value, type);

                return true;
            }

            result = value;

            return false;
        }

#if CS8
        [return: NotNullIfNotNull(nameof(VALUE))]
#endif
        public static object
#if CS8
            ?
#endif
            TryChangeType(in object
#if CS8
            ?
#endif
            value, in Type type)
        {
            _ = TryChangeType(value, type, out object
#if CS8
            ?
#endif
            result);

            return result;
        }

        public static bool TryChangeType<T>(in object
#if CS8
            ?
#endif
            value,
#if CS8
        [NotNullIfNotNull(nameof(VALUE))]
#endif
        out T
#if CS9
            ?
#endif
            result)
        {
            if (TryChangeType(value, typeof(T), out object
#if CS8
                ?
#endif
                _result))
            {
                result = (T)_result;

                return true;
            }

            result = default;

            return false;
        }

#if CS8
        [return: NotNullIfNotNull(nameof(VALUE))]
#endif
        public static T
#if CS9
            ?
#endif
            TryChangeType<T>(in object
#if CS8
            ?
#endif
            value) => TryChangeType<T>(value, out T result) ? result : default;

#if CS8
        [return: NotNullIfNotNull(nameof(VALUE))]
#endif
        public static T ChangeType<T>(in object
#if CS8
            ?
#endif
            value) => TryChangeType(value, out T
#if CS9
                ?
#endif
                result) ? result : throw GetInvalidCastException<ulong>(value);
    }

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

    public abstract class DisposableBase : DotNetFix.IDisposable
    {
        public abstract bool IsDisposed { get; }

        protected abstract void DisposeManaged();
        protected abstract void DisposeUnmanaged();

        public void Dispose()
        {
            if (IsDisposed)

                return;

            DisposeUnmanaged();
            DisposeManaged();
            GC.SuppressFinalize(this);
        }

        ~DisposableBase() => DisposeUnmanaged();
    }

    public abstract class Disposable : DisposableBase
    {
        private bool _isDisposed;

        public override bool IsDisposed => _isDisposed;

        protected override void DisposeManaged() => _isDisposed = true;
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
        public T
#if CS9
            ?
#endif
            Value
        { get; }

        public NullableReference(T
#if CS9
            ?
#endif
            value) => Value = value;
    }

    public interface INullableBase
    {
        bool HasValue { get; }

        object
#if CS8
            ?
#endif
            Value
        { get; }
    }

    public interface INullable : INullableBase, IEquatable<INullableBase>
    {
        // Left empty.
    }

    public static class NullableHelper
    {
        public static string ToString(in INullableBase nullable) => nullable.HasValue ? nullable.Value?.ToString() ?? "<Null value>" : "<Null>";

        public static bool Equals<T>(in INullableBase nullable, in T other, in Func<bool> func) => other == null ? !nullable.HasValue : func();

        public static bool Equals(INullableBase nullable, INullableBase other) => Equals(nullable, other, () => nullable.HasValue ? other.HasValue && (nullable.Value == null ? other.Value == null : other.Value?.Equals(nullable.Value) == true) : !other.HasValue);

        public static bool Equals<T>(INullableBase nullable, object obj) => Equals(nullable, obj, () => (obj is INullableBase _nullable && Equals(nullable, _nullable)) || (obj is T _obj && nullable.Value is T value && value.Equals(_obj)));

        public static int GetHashCode(in INullableBase nullable) => nullable.HasValue && nullable.Value != null ? nullable.Value.GetHashCode() : nullable.AsFromType<object>().GetHashCode();
    }

    public struct Nullable : INullable
    {
        private readonly object
#if CS8
            ?
#endif
            _value;

        public bool HasValue { get; }

        public object
#if CS8
            ?
#endif
            Value => HasValue ? _value : throw new InvalidOperationException("This instance does not contain any value.");

        public Nullable(in object
#if CS8
            ?
#endif
            value)
        {
            _value = value;

            HasValue = true;
        }

        public override string ToString() => NullableHelper.ToString(this);

        public bool Equals(INullableBase other) => NullableHelper.Equals(this, other);
        public override bool Equals(object obj) => NullableHelper.Equals<object>(this, obj);

        public override int GetHashCode() => NullableHelper.GetHashCode(this);
    }

    public interface IAsEquatable<T>
    {
        IEquatable<T
#if CS9
            ?
#endif
            > AsEquatable();
    }

    public interface INullable<T> : INullableBase, IEquatable<INullable<T
#if CS9
            ?
#endif
            >
#if CS8
            ?
#endif
            >, IEquatable<T
#if CS9
            ?
#endif
            >, IAsEquatable<INullableBase
#if CS8
            ?
#endif
            >
    {
        T
#if CS9
            ?
#endif
            Value
        { get; }
    }

    public struct Nullable<T> : INullable<T
#if CS9
            ?
#endif
            >
    {
        private readonly T
#if CS9
            ?
#endif
            _value;

        public bool HasValue { get; }

        public T
#if CS9
            ?
#endif
            Value => HasValue ? _value : throw new InvalidOperationException("This instance does not contain any value.");

        object
#if CS8
            ?
#endif
            INullableBase.Value => Value;

        public Nullable(
#if CS8 && (!CS9)
            [AllowNull]
#endif
            in T
#if CS9
            ?
#endif
            value)
        {
            _value = value;

            HasValue = true;
        }

        public override string ToString() => NullableHelper.ToString(this);

        public bool Equals(INullable other) => NullableHelper.Equals(this, other);
        public bool Equals(INullable<T
#if CS9
            ?
#endif
            > other) => NullableHelper.Equals<T>(this, other);
        public bool Equals(T
#if CS9
            ?
#endif
            other) => Equals(Value, other);
        public override bool Equals(object obj) => NullableHelper.Equals<object>(this, obj);

        public IEquatable<INullableBase
#if CS8
            ?
#endif
            > AsEquatable() => new Nullable(Value);

        public override int GetHashCode() => NullableHelper.GetHashCode(this);

        public static implicit operator Nullable<T
#if CS9
            ?
#endif
            >(T
#if CS9
            ?
#endif
            value) => new
#if !CS9
            Nullable<T
#if CS9
            ?
#endif
            >
#endif
            (value);
        public static explicit operator T
#if CS9
            ?
#endif
            (Nullable<T
#if CS9
            ?
#endif
            > value) => value.Value;
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
