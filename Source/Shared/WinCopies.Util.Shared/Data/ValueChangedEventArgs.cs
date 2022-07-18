/* Copyright © Pierre Sprimont, 2019
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

#if WinCopies3
namespace WinCopies.Util.Data
{
    public delegate void EventHandler<T>(object sender, EventArgs<T> e);

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);

    public delegate void ValueChangedEventHandler<TOldValue, TNewValue>(object sender, ValueChangedEventArgs<TOldValue, TNewValue> e);

    public static class EventArgs
    {
        public static EventArgs<T> Construct<T>(T value) => new
#if !CS9
            EventArgs<T>
#endif
            (value);
        public static EventArgs<T1, T2> Construct2<T1, T2>(T1 value, T2 extraInfo) => new
#if !CS9
            EventArgs<T1, T2>
#endif
            (value, extraInfo);

        public static ValueChangedEventArgs<T> Construct<T>(T oldValue, T newValue) => new
#if !CS9
            ValueChangedEventArgs<T>
#endif
            (oldValue, newValue);
        public static ValueChangedEventArgs<TOldValue, TNewValue> Construct<TOldValue, TNewValue>(TOldValue oldValue, TNewValue newValue) => new
#if !CS9
            ValueChangedEventArgs<TOldValue, TNewValue>
#endif
            (oldValue, newValue);
    }

    public class EventArgs<T> : System.EventArgs
    {
        public T Value { get; }

        public EventArgs(in T value) => Value = value;
    }

    public class EventArgs<T1, T2> : EventArgs<T1>
    {
        public T2 ExtraInfo { get; }

        public EventArgs(in T1 value, T2 extraInfo) : base(value) => ExtraInfo = extraInfo;
    }

    public class ValueChangedEventArgs : System.EventArgs
    {
        public object OldValue { get; }

        public object NewValue { get; }

        public ValueChangedEventArgs(in object oldValue, in object newValue)
        {
            OldValue = oldValue;

            NewValue = newValue;
        }
    }

    public class ValueChangedEventArgs<T> : System.EventArgs
    {
        public T OldValue { get; }

        public T NewValue { get; }

        public ValueChangedEventArgs(in T oldValue, in T newValue)
        {
            OldValue = oldValue;

            NewValue = newValue;
        }
    }

    public class ValueChangedEventArgs<TOldValue, TNewValue> : System.EventArgs
    {
        public TOldValue OldValue { get; }

        public TNewValue NewValue { get; }

        public ValueChangedEventArgs(in TOldValue oldValue, in TNewValue newValue)
        {
            OldValue = oldValue;

            NewValue = newValue;
        }
    }
}
#endif
