using System;
using System.Collections.Generic;
using System.Text;

namespace WinCopies.Collections
{
    public interface INullableValueEntry<T> where T : struct
    {
        T? Value { get; }
    }

    public interface INullableRefEntry<T> where T : class
    {
        T Value { get; }
    }

    public static class NullableValueEntryHelper
    {

        public static bool Equals<T>(in INullableValueEntry<T> value, in object obj) where T : struct
        {
            if (value == null) return obj == null;

            return value.Value.HasValue ? value.Value.Equals(obj) : obj == null;
        }

        public static bool Equals<T>(in INullableValueEntry<T> left, in T right) where T : struct, IEquatable<T> => left == null ? false : left.Value.HasValue && left.Value.Equals(right);

        public static bool Equals<T>(in INullableValueEntry<T> left, in T? right) where T : struct, IEquatable<T> => right.HasValue ? Equals(left, right.Value) : left == null || !left.Value.HasValue;

        public static bool Equals<T>(in INullableValueEntry<T> left, in INullableValueEntry<T> right) where T : struct, IEquatable<T> => right == null ? left == null || !left.Value.HasValue : Equals(left, right.Value);

        public static int Compare<T>(in INullableValueEntry<T> left, in T right) where T : struct, IComparable<T> => left != null && left.Value.HasValue ? left.Value.Value.CompareTo(right) : -1;

        public static int Compare<T>(in T? left, in T? right) where T : struct, IComparable<T>
        {
            if (left.HasValue) return right.HasValue ? left.Value.CompareTo(right.Value) : 1;

            return right.HasValue ? -1 : 0;
        }

        public static int Compare<T>(in INullableValueEntry<T> left, in T? right) where T : struct, IComparable<T>
        {
            if (right.HasValue) return Compare(left, right.Value);

            if (left == null) return 0;

            return left.Value.HasValue ? 1 : 0;
        }

        public static int Compare<T>(in INullableValueEntry<T> left, in INullableValueEntry<T> right) where T : struct, IComparable<T>
        {
            if (right == null)
            {
                if (left == null) return 0;

                return left.Value.HasValue ? 1 : 0;
            }

            return Compare(left, right.Value);
        }
    }

    public struct NullableValueEntry<T> : INullableValueEntry<T> where T : struct
    {
        public T? Value { get; }

        public NullableValueEntry(in T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public static bool operator ==(in NullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in NullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in NullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in NullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in NullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in NullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);
    }

    public struct EquatableNullableValueEntry<T> : INullableValueEntry<T>, IEquatable<T>, IEquatable<T?>, IEquatable<INullableValueEntry<T>> where T : struct, IEquatable<T>
    {
        public T? Value { get; }

        public EquatableNullableValueEntry(in T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(T? valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public static bool operator ==(in EquatableNullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableNullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in EquatableNullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);

    }

    public struct ComparableNullableValueEntry<T> : INullableValueEntry<T>, IComparable<T>, IComparable<T?>, IComparable<INullableValueEntry<T>> where T : struct, IComparable<T>
    {
        public T? Value { get; }

        public ComparableNullableValueEntry(in T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public int CompareTo(T valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(T? valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in ComparableNullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in ComparableNullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);

        public static bool operator <(in ComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) < 0;

        public static bool operator <=(in ComparableNullableValueEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in ComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) > 0;

        public static bool operator >=(in ComparableNullableValueEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in ComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in ComparableNullableValueEntry<T> left, in T? right) => !(left > right);

        public static bool operator >(in ComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in ComparableNullableValueEntry<T> left, in T? right) => !(left < right);

        public static bool operator <(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) < 0;

        public static bool operator <=(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left > right);

        public static bool operator >(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) > 0;

        public static bool operator >=(in ComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left < right);
    }

    public struct EquatableComparableNullableValueEntry<T> : INullableValueEntry<T>, IEquatable<T>, IEquatable<T?>, IEquatable<INullableValueEntry<T>>, IComparable<T>, IComparable<T?>, IComparable<INullableValueEntry<T>> where T : struct, IEquatable<T>, IComparable<T>
    {
        public T? Value { get; }

        public EquatableComparableNullableValueEntry(T? value) => Value = value;

        public override bool Equals(object obj) => NullableValueEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(T? valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Equals(this, valueToCompare);

        public int CompareTo(T valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(T? valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableValueEntry<T> valueToCompare) => NullableValueEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in EquatableComparableNullableValueEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableValueEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableComparableNullableValueEntry<T> left, in T? right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableValueEntry<T> left, in T? right) => !(left == right);

        public static bool operator ==(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left == right);

        public static bool operator <(in EquatableComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) < 0;

        public static bool operator <=(in EquatableComparableNullableValueEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableValueEntry<T> left, in T right) => left.Value.HasValue && left.Value.Value.CompareTo(right) > 0;

        public static bool operator >=(in EquatableComparableNullableValueEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in EquatableComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in EquatableComparableNullableValueEntry<T> left, in T? right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableValueEntry<T> left, in T? right) => left.Value.HasValue && right.HasValue && left.Value.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in EquatableComparableNullableValueEntry<T> left, in T? right) => !(left < right);

        public static bool operator <(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) < 0;

        public static bool operator <=(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => right == null ? false : left.Value.HasValue && right.Value.HasValue && left.Value.Value.CompareTo(right.Value.Value) > 0;

        public static bool operator >=(in EquatableComparableNullableValueEntry<T> left, in INullableValueEntry<T> right) => !(left < right);

    }



    public static class NullableRefEntryHelper
    {
        public static bool Equals<T>(in INullableRefEntry<T> value, in object obj) where T : class => value == null || value.Value == null ? obj == null : value.Value.Equals(obj);

        public static bool Equals<T>(in INullableRefEntry<T> left, in T right) where T : class, IEquatable<T> => left == null || left.Value == null ? right == null : left.Value.Equals(right);

        public static bool Equals<T>(in INullableRefEntry<T> left, in INullableRefEntry<T> right) where T : class, IEquatable<T> => right == null ? left == null || left.Value == null : Equals(left, right.Value);

        public static int Compare<T>(in INullableRefEntry<T> left, in T right) where T : class, IComparable<T> => left == null || left.Value == null ? -1 : left.Value.CompareTo(right);

        public static int Compare<T>(in INullableRefEntry<T> left, in INullableRefEntry<T> right) where T : class, IComparable<T>
        {
            if (right == null) return left == null || left.Value == null ? 0 : 1;

            return Compare(left, right.Value);
        }
    }

    public struct NullableRefEntry<T> : INullableRefEntry<T> where T : class
    {
        public T Value { get; }

        public NullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public static bool operator ==(in NullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in NullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in NullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in NullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);
    }

    public struct EquatableNullableRefEntry<T> : INullableRefEntry<T>, IEquatable<T>, IEquatable<INullableRefEntry<T>> where T : class, IEquatable<T>
    {
        public T Value { get; }

        public EquatableNullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public static bool operator ==(in EquatableNullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);

    }

    public struct ComparableNullableRefEntry<T> : INullableRefEntry<T>, IComparable<T>, IComparable<INullableRefEntry<T>> where T : class, IComparable<T>
    {
        public T Value { get; }

        public ComparableNullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public int CompareTo(T valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in ComparableNullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);

        public static bool operator <(in ComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) < 0;

        public static bool operator <=(in ComparableNullableRefEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in ComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) > 0;

        public static bool operator >=(in ComparableNullableRefEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left > right);

        public static bool operator >(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in ComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left < right);
    }

    public struct EquatableComparableNullableRefEntry<T> : INullableRefEntry<T>, IEquatable<T>, IEquatable<INullableRefEntry<T>>, IComparable<T>, IComparable<INullableRefEntry<T>> where T : class, IEquatable<T>, IComparable<T>
    {
        public T Value { get; }

        public EquatableComparableNullableRefEntry(in T value) => Value = value;

        public override bool Equals(object obj) => NullableRefEntryHelper.Equals(this, obj);

        public override int GetHashCode() => Value == null ? base.GetHashCode() : Value.GetHashCode();

        public override string ToString() => Value == null ? base.ToString() : Value.ToString();

        public bool Equals(T valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public bool Equals(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Equals(this, valueToCompare);

        public int CompareTo(T valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public int CompareTo(INullableRefEntry<T> valueToCompare) => NullableRefEntryHelper.Compare(this, valueToCompare);

        public static bool operator ==(in EquatableComparableNullableRefEntry<T> left, in T right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableRefEntry<T> left, in T right) => !(left == right);

        public static bool operator ==(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Equals(right);

        public static bool operator !=(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left == right);

        public static bool operator <(in EquatableComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) < 0;

        public static bool operator <=(in EquatableComparableNullableRefEntry<T> left, in T right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableRefEntry<T> left, in T right) => left.Value == null || right == null ? false : left.Value.CompareTo(right) > 0;

        public static bool operator >=(in EquatableComparableNullableRefEntry<T> left, in T right) => !(left < right);

        public static bool operator <(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) < 0;

        public static bool operator <=(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left > right);

        public static bool operator >(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => left.Value == null || right == null || right.Value == null ? false : left.Value.CompareTo(right.Value) > 0;

        public static bool operator >=(in EquatableComparableNullableRefEntry<T> left, in INullableRefEntry<T> right) => !(left < right);

    }
}
