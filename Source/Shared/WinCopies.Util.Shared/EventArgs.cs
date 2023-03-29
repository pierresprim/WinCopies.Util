using System;

namespace WinCopies
{
    public class BooleanEventArgs : EventArgs
    {
        public bool Value { get; protected set; }

        public BooleanEventArgs(in bool value) => Value = value;
    }

    public class BooleanEventArgs2 : BooleanEventArgs
    {
        public new bool Value { get => base.Value; set => base.Value = value; }

        public BooleanEventArgs2(in bool value = false) : base(value) { /* Left empty. */ }
    }

    public class ReadOnlyValueBooleanEventArgs<T> : BooleanEventArgs
    {
        public T Item { get; }

        public ReadOnlyValueBooleanEventArgs(in T item, in bool value) : base(value) => Item = item;
    }

    public class ReadOnlyValueBooleanEventArgs2<T> : BooleanEventArgs2
    {
        public T Item { get; }

        public ReadOnlyValueBooleanEventArgs2(in T item, in bool value = false) : base(value) => Item = item;
    }

    public class ValueBooleanEventArgs<T> : BooleanEventArgs
    {
        public T Item { get; set; }

        public ValueBooleanEventArgs(in T item, in bool value) : base(value) => Item = item;
    }

    public class ValueBooleanEventArgs2<T> : BooleanEventArgs2
    {
        public T Item { get; set; }

        public ValueBooleanEventArgs2(in T item, in bool value = false) : base(value) => Item = item;
    }
}
