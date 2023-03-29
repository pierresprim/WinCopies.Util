using System;

using WinCopies.Util;

namespace WinCopies
{
    public sealed class NullableGeneric<T>
    {
        public T Value { get; }

        public NullableGeneric(T value) => Value = value;
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

    public static class NullableHelper
    {
        public static string ToString(in INullableBase nullable) => nullable.HasValue ? nullable.Value?.ToString() ?? "<Null value>" : "<Null>";

        public static bool Equals<T>(in INullableBase nullable, in T other, in Func<bool> func) => other == null ? !nullable.HasValue : func();

        public static bool Equals(INullableBase nullable, INullableBase
#if CS8
            ?
#endif
            other) => Equals(nullable, other, () => nullable.HasValue ? other.HasValue && (nullable.Value == null ? other.Value == null : other.Value?.Equals(nullable.Value) == true) : !other.HasValue);

        public static bool Equals<T>(INullableBase nullable, object
#if CS8
            ?
#endif
            obj) => Equals(nullable, obj, () => (obj is INullableBase _nullable && Equals(nullable, _nullable)) || (obj is T _obj && nullable.Value is T value && value.Equals(_obj)));

        public static int GetHashCode(in INullableBase nullable) => nullable.HasValue && nullable.Value != null ? nullable.Value.GetHashCode() : nullable.AsFromType<object>().GetHashCode();
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

    public readonly struct Nullable : INullable
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

        public bool Equals(INullableBase
#if CS8
            ?
#endif
            other) => NullableHelper.Equals(this, other);
        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => NullableHelper.Equals<object>(this, obj);

        public override int GetHashCode() => NullableHelper.GetHashCode(this);

        public static bool operator ==(Nullable left, Nullable right) => left.Equals(right);
        public static bool operator !=(Nullable left, Nullable right) => !(left == right);

        public static bool operator ==(Nullable left, INullableBase right) => left.Equals(right);
        public static bool operator !=(Nullable left, INullableBase right) => !(left == right);
    }

    public readonly struct Nullable<T> : INullable<T
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

        public bool Equals(INullable
#if CS8
            ?
#endif
            other) => NullableHelper.Equals(this, other);
        public bool Equals(INullable<T
#if CS9
            ?
#endif
            >
#if CS8
            ?
#endif
            other) => NullableHelper.Equals<T>(this, other);
        public bool Equals(T
#if CS9
            ?
#endif
            other) => Equals(Value, other);
        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => NullableHelper.Equals<object>(this, obj);

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

        public static bool operator ==(Nullable<T> left, Nullable<T> right) => left.Equals(right);
        public static bool operator !=(Nullable<T> left, Nullable<T> right) => !(left == right);

        public static bool operator ==(Nullable<T> left, INullable right) => left.Equals(right);
        public static bool operator !=(Nullable<T> left, INullable right) => !(left == right);

        public static bool operator ==(Nullable<T> left, INullable<T> right) => left.Equals(right);
        public static bool operator !=(Nullable<T> left, INullable<T> right) => !(left == right);
    }

    public struct NullableDisposable : IValueProvider
    {
        private object
#if CS8
            ?
#endif
            _value;

        public object Value => _value ?? throw new ObjectDisposedException(null);

        public bool IsDisposed => _value == null;

        public NullableDisposable(in object
#if CS8
            ?
#endif
            value) => _value = value;

        private bool _Equals(object
#if CS8
            ?
#endif
            obj) => _value == obj;
        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => obj == null ? IsDisposed : obj is IValueProviderBase value ? Equals(value) : _Equals(obj);
        public bool Equals<T>(T
#if CS9
            ?
#endif
            other) => _Equals(other.AsObject());
        public bool Equals(IValueProviderBase
#if CS9
            ?
#endif
            other) => other == null || other.IsDisposed ? IsDisposed : !IsDisposed && _Equals(other.Value);

        public override int GetHashCode() => Value.GetHashCode();

        public static NullableDisposable<T> Create<T>(in T
#if CS9
            ?
#endif
            value) where T : class => new
#if !CS9
            NullableDisposable<T>
#endif
            (value);
        public static NullableDisposable2<T> Create<T>(in NullableDisposable<T> value) where T : class, System.IDisposable => new
#if !CS9
            NullableDisposable2<T>
#endif
            (value);
        public static NullableDisposable2<T> Create2<T>(in T
#if CS9
            ?
#endif
            value) where T : class, System.IDisposable => new
#if !CS9
            NullableDisposable2<T>
#endif
            (value);

        public void Dispose() => _value = null;

        public static bool operator ==(NullableDisposable left, IValueProviderBase right) => left.Equals(right);
        public static bool operator !=(NullableDisposable left, IValueProviderBase right) => !(left == right);
    }

    public struct NullableDisposable<T> : IValueProvider<T> where T : class
    {
        private T
#if CS9
            ?
#endif
            _value;

        public T Value => _value ?? throw new ObjectDisposedException(null);
#if !CS8
        object IValueProviderBase.Value => Value;
#endif

        public bool IsDisposed => _value == null;

        public NullableDisposable(in T
#if CS9
            ?
#endif
            value) => _value = value;

        private bool _Equals(object
#if CS8
            ?
#endif
            obj) => _value == obj;
        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => obj == null ? IsDisposed : obj is IValueProviderBase value ? Equals(value) : _Equals(obj);
        public bool Equals(T
#if CS9
            ?
#endif
            other) => _Equals(other.AsObject());
        public bool Equals(IValueProviderBase
#if CS8
            ?
#endif
            other) => other == null || other.IsDisposed ? IsDisposed : !IsDisposed && _Equals(other.Value);

        public override int GetHashCode() => Value.GetHashCode();

        public void Dispose() => _value = null;

        public static bool operator ==(NullableDisposable<T> left, IValueProviderBase right) => left.Equals(right);
        public static bool operator !=(NullableDisposable<T> left, IValueProviderBase right) => !(left == right);
#if !WinCopies4
        public static bool operator ==(NullableDisposable<T> left, object
#if CS8
            ?
#endif
            right) => left.Equals(right);
        public static bool operator !=(NullableDisposable<T> left, object
#if CS8
            ?
#endif
            right) => !(left == right);
#endif
    }

    public struct NullableDisposable2<T> : IValueProvider<T> where T : class, System.IDisposable
    {
        private NullableDisposable<T> _value;

        public T Value => _value.Value;
#if !CS8
        object IValueProviderBase.Value => Value;
#endif
        public bool IsDisposed => _value.IsDisposed;

        public NullableDisposable2(NullableDisposable<T> value) => _value = value;
        public NullableDisposable2(T
#if CS9
            ?
#endif
            value) => this = new
#if !CS9
            NullableDisposable2<T>
#endif
            (new NullableDisposable<T>(value));

        public bool Equals(T
#if CS8
            ?
#endif
            other) => _value.Equals(other);
        public bool Equals(IValueProviderBase
#if CS8
            ?
#endif
            other) => _value.Equals(other);
        public override bool Equals(object
#if CS8
            ?
#endif
            obj) => (obj is IValueProviderBase valueProvider && Equals(valueProvider)) || (obj is T value && Equals(value));

        public override int GetHashCode() => Value.GetHashCode();

        public bool Dispose()
        {
            if (IsDisposed)

                return false;

            Value.Dispose();
            _value.Dispose();

            return true;
        }

        void System.IDisposable.Dispose() => Dispose();

        public static bool operator ==(NullableDisposable2<T> left, T right) => left.Equals(right);
        public static bool operator !=(NullableDisposable2<T> left, T right) => !(left == right);

        public static bool operator ==(NullableDisposable2<T> left, IValueProviderBase right) => left.Equals(right);
        public static bool operator !=(NullableDisposable2<T> left, IValueProviderBase right) => !(left == right);
    }
}
