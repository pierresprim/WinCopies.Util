using System;
using System.Collections.Generic;
using System.Text;

namespace
#if WinCopies2
WinCopies.Util
#else
WinCopies
#endif
{
    public interface INullableValueEntry<T> where T : struct
    {
        T? Value { get; }
    }

    public interface INullableRefEntry<T> where T : class
    {
        T Value { get; }
    }

    public struct NullableValueEntry<T> : INullableValueEntry<T> where T : struct
    {
        public T? Value { get; }

        public NullableValueEntry(T? value) => Value = value;

        public static bool Equals(INullableValueEntry<T> value, object obj)
        {
            if (value == null)

                return obj == null;

            else if (value.Value.HasValue)

                return value.Value.Equals(obj);

            else

                return obj == null;
        }

        public static bool Equals<U>(INullableValueEntry<U> left, U right) where U : struct, IEquatable<U> => left == null ? false : left.Value.HasValue && left.Value.Equals(right);

        public static bool Equals<U>(INullableValueEntry<U> left, U? right) where U : struct, IEquatable<U> => right.HasValue ? Equals(left, right.Value) : left == null || !left.Value.HasValue;

        public static bool Equals<U>(INullableValueEntry<U> left, INullableValueEntry<U> right) where U : struct, IEquatable<U> => right == null ? left == null || !left.Value.HasValue : Equals(left, right.Value);

        public static int Compare<U>(INullableValueEntry<U> left, U right) where U : struct, IComparable<U> => left != null && left.Value.HasValue ? left.Value.Value.CompareTo(right) : -1;

        public static int Compare<U>(INullableValueEntry<U> left, U? right) where U : struct, IComparable<U>
        {
            if (right.HasValue)

                return Compare(left, right.Value);

            else if (left == null)

                return 0;

            else if (left.Value.HasValue)

                return 1;

            else

                return 0;
        }

        public static int Compare<U>(INullableValueEntry<U> left, INullableValueEntry<U> right) where U : struct, IComparable<U>
        {
            if (right == null)

                if (left == null)

                    return 0;

                else if (left.Value.HasValue)

                    return 1;

                else

                    return 0;

            else

                return Compare(left, right.Value);
        }

        public override bool Equals(object obj) => Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public static bool operator ==(NullableValueEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(NullableValueEntry<T> left, T right) => !(left == right);

        public static bool operator ==(NullableValueEntry<T> left, T? right) => left.Equals(right);

        public static bool operator !=(NullableValueEntry<T> left, T? right) => !(left == right);

        public static bool operator ==(NullableValueEntry<T> left, INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(NullableValueEntry<T> left, INullableValueEntry<T> right) => !(left == right);
    }

    public struct EquatableNullableValueEntry<T> : INullableValueEntry<T>, IEquatable<T>, IEquatable<T?>, IEquatable<INullableValueEntry<T>> where T : struct, IEquatable<T>
    {
        public T? Value { get; }

        public EquatableNullableValueEntry(T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntry<T>.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableValueEntry<T>.Equals(this, valueToCompare);

        public bool Equals(T? valueToCompare) => NullableValueEntry<T>.Equals(this, valueToCompare);

        public bool Equals(INullableValueEntry<T> valueToCompare) => NullableValueEntry<T>.Equals(this, valueToCompare);

        public static bool operator ==(EquatableNullableValueEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(EquatableNullableValueEntry<T> left, T right) => !(left == right);

        public static bool operator ==(EquatableNullableValueEntry<T> left, T? right) => left.Equals(right);

        public static bool operator !=(EquatableNullableValueEntry<T> left, T? right) => !(left == right);

        public static bool operator ==(EquatableNullableValueEntry<T> left, INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(EquatableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left == right);

    }

    public struct ComparableNullableValueEntry<T> : INullableValueEntry<T>, IComparable<T>, IComparable<T?>, IComparable<INullableValueEntry<T>> where T : struct, IComparable<T>
    {
        public T? Value { get; }

        public ComparableNullableValueEntry(T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntry<T>.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public int CompareTo(T valueToCompare) => NullableValueEntry<T>.Compare(this, valueToCompare);

        public int CompareTo(T? valueToCompare) => NullableValueEntry<T>.Compare(this, valueToCompare);

        public int CompareTo(INullableValueEntry<T> valueToCompare) => NullableValueEntry<T>.Compare(this, valueToCompare);

        public static bool operator ==(ComparableNullableValueEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(ComparableNullableValueEntry<T> left, T right) => !(left == right);

        public static bool operator ==(ComparableNullableValueEntry<T> left, T? right) => left.Equals(right);

        public static bool operator !=(ComparableNullableValueEntry<T> left, T? right) => !(left == right);

        public static bool operator ==(ComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(ComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left == right);

        public static bool operator <(ComparableNullableValueEntry<T> left, T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) < 0;

        public static bool operator <=(ComparableNullableValueEntry<T> left, T right) => !(left > right);

        public static bool operator >(ComparableNullableValueEntry<T> left, T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) > 0;

        public static bool operator >=(ComparableNullableValueEntry<T> left, T right) => !(left < right);

        public static bool operator <(ComparableNullableValueEntry<T> left, T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(ComparableNullableValueEntry<T> left, T? right) => !(left > right);

        public static bool operator >(ComparableNullableValueEntry<T> left, T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(ComparableNullableValueEntry<T> left, T? right) => !(left < right);

        public static bool operator <(ComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) < 0;

        public static bool operator <=(ComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left > right);

        public static bool operator >(ComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) > 0;

        public static bool operator >=(ComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left < right);
    }

    public struct EquatableComparableNullableValueEntry<T> : INullableValueEntry<T>, IEquatable<T>, IEquatable<T?>, IEquatable<INullableValueEntry<T>>, IComparable<T>, IComparable<T?>, IComparable<INullableValueEntry<T>> where T : struct, IEquatable<T>, IComparable<T>
    {
        public T? Value { get; }

        public EquatableComparableNullableValueEntry(T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntry<T>.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableValueEntry<T>.Equals(this, valueToCompare);

        public bool Equals(T? valueToCompare) => NullableValueEntry<T>.Equals(this, valueToCompare);

        public bool Equals(INullableValueEntry<T> valueToCompare) => NullableValueEntry<T>.Equals(this, valueToCompare);

        public int CompareTo(T valueToCompare) => NullableValueEntry<T>.Compare(this, valueToCompare);

        public int CompareTo(T? valueToCompare) => NullableValueEntry<T>.Compare(this, valueToCompare);

        public int CompareTo(INullableValueEntry<T> valueToCompare) => NullableValueEntry<T>.Compare(this, valueToCompare);

        public static bool operator ==(EquatableComparableNullableValueEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(EquatableComparableNullableValueEntry<T> left, T right) => !(left == right);

        public static bool operator ==(EquatableComparableNullableValueEntry<T> left, T? right) => left.Equals(right);

        public static bool operator !=(EquatableComparableNullableValueEntry<T> left, T? right) => !(left == right);

        public static bool operator ==(EquatableComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(EquatableComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left == right);

        public static bool operator <(EquatableComparableNullableValueEntry<T> left, T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) < 0;

        public static bool operator <=(EquatableComparableNullableValueEntry<T> left, T right) => !(left > right);

        public static bool operator >(EquatableComparableNullableValueEntry<T> left, T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) > 0;

        public static bool operator >=(EquatableComparableNullableValueEntry<T> left, T right) => !(left < right);

        public static bool operator <(EquatableComparableNullableValueEntry<T> left, T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(EquatableComparableNullableValueEntry<T> left, T? right) => !(left > right);

        public static bool operator >(EquatableComparableNullableValueEntry<T> left, T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(EquatableComparableNullableValueEntry<T> left, T? right) => !(left < right);

        public static bool operator <(EquatableComparableNullableValueEntry<T> left, INullableValueEntry<T> right) =>right==null?false: left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) < 0;

        public static bool operator <=(EquatableComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left > right);

        public static bool operator >(EquatableComparableNullableValueEntry<T> left, INullableValueEntry<T> right) =>right==null?false: left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) > 0;

        public static bool operator >=(EquatableComparableNullableValueEntry<T> left, INullableValueEntry<T> right) => !(left < right);

    }



    public struct NullableRefEntry<T> : INullableRefEntry<T> where T : class
    {
        public T Value { get; }

        public NullableRefEntry(T value) => Value = value;

        public static bool Equals(INullableRefEntry<T> value, object obj) => value == null || value.Value == null ? obj == null : value.Value.Equals(obj);

        public static bool Equals<U>(INullableRefEntry<U> left, U right) where U : class, IEquatable<U> => left == null || left.Value == null ? right == null : left.Value.Equals(right);

        public static bool Equals<U>(INullableRefEntry<U> left, INullableRefEntry<U> right) where U : class, IEquatable<U> => right == null ? left == null || left.Value == null : Equals(left, right.Value);

        public static int Compare<U>(INullableRefEntry<U> left, U right) where U : class, IComparable<U> => left == null || left.Value == null ? -1 : left.Value.CompareTo(right);

        public static int Compare<U>(INullableRefEntry<U> left, INullableRefEntry<U> right) where U : class, IComparable<U>
        {
            if (right == null)

                if (left == null || left.Value == null)

                    return 0;

                else

                    return 1;

            else

                return Compare(left, right.Value);
        }

        public override bool Equals(object obj) => Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public static bool operator ==(NullableRefEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(NullableRefEntry<T> left, T right) => !(left == right);

        public static bool operator ==(NullableRefEntry<T> left, INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(NullableRefEntry<T> left, INullableRefEntry<T> right) => !(left == right);
    }

    public struct EquatableNullableRefEntry<T> : INullableRefEntry<T>, IEquatable<T>, IEquatable<INullableRefEntry<T>> where T : class, IEquatable<T>
    {
        public T Value { get; }

        public EquatableNullableRefEntry(T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntry<T>.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableRefEntry<T>.Equals(this, valueToCompare);

        public bool Equals(INullableRefEntry<T> valueToCompare) => NullableRefEntry<T>.Equals(this, valueToCompare);

        public static bool operator ==(EquatableNullableRefEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(EquatableNullableRefEntry<T> left, T right) => !(left == right);

        public static bool operator ==(EquatableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(EquatableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left == right);

    }

    public struct ComparableNullableRefEntry<T> : INullableRefEntry<T>, IComparable<T>,  IComparable<INullableRefEntry<T>> where T : class, IComparable<T>
    {
        public T Value { get; }

        public ComparableNullableRefEntry(T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntry<T>.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public int CompareTo(T valueToCompare) => NullableRefEntry<T>.Compare(this, valueToCompare);

        public int CompareTo(INullableRefEntry<T> valueToCompare) => NullableRefEntry<T>.Compare(this, valueToCompare);

        public static bool operator ==(ComparableNullableRefEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(ComparableNullableRefEntry<T> left, T right) => !(left == right);

        public static bool operator ==(ComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(ComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left == right);

        public static bool operator <(ComparableNullableRefEntry<T> left, T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) < 0;

        public static bool operator <=(ComparableNullableRefEntry<T> left, T right) => !(left > right);

        public static bool operator >(ComparableNullableRefEntry<T> left, T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) > 0;

        public static bool operator >=(ComparableNullableRefEntry<T> left, T right) => !(left < right);

        public static bool operator <(ComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(ComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left > right);

        public static bool operator >(ComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(ComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left < right);
    }

    public struct EquatableComparableNullableRefEntry<T> : INullableRefEntry<T>, IEquatable<T>,  IEquatable<INullableRefEntry<T>>, IComparable<T>,  IComparable<INullableRefEntry<T>> where T : class, IEquatable<T>, IComparable<T>
    {
        public T Value { get; }

        public EquatableComparableNullableRefEntry(T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntry<T>.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableRefEntry<T>.Equals(this, valueToCompare);

        public bool Equals(INullableRefEntry<T> valueToCompare) => NullableRefEntry<T>.Equals(this, valueToCompare);

        public int CompareTo(T valueToCompare) => NullableRefEntry<T>.Compare(this, valueToCompare);

        public int CompareTo(INullableRefEntry<T> valueToCompare) => NullableRefEntry<T>.Compare(this, valueToCompare);

        public static bool operator ==(EquatableComparableNullableRefEntry<T> left, T right) => left.Equals(right);

        public static bool operator !=(EquatableComparableNullableRefEntry<T> left, T right) => !(left == right);

        public static bool operator ==(EquatableComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(EquatableComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left == right);

        public static bool operator <(EquatableComparableNullableRefEntry<T> left, T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) < 0;

        public static bool operator <=(EquatableComparableNullableRefEntry<T> left, T right) => !(left > right);

        public static bool operator >(EquatableComparableNullableRefEntry<T> left, T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) > 0;

        public static bool operator >=(EquatableComparableNullableRefEntry<T> left, T right) => !(left < right);

        public static bool operator <(EquatableComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(EquatableComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left > right);

        public static bool operator >(EquatableComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(EquatableComparableNullableRefEntry<T> left, INullableRefEntry<T> right) => !(left < right);

    }
}
