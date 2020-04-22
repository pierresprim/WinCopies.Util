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

using System;

namespace WinCopies.Util.Data
{
    public delegate void EventHandler<T>(object sender, EventArgs<T> e);

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);

    public delegate void ValueChangedEventHandler<TOldValue, TNewValue>(object sender, ValueChangedEventArgs<TOldValue, TNewValue> e);

    public class EventArgs<T> : EventArgs
    {

        public T Value { get; } = default;

        public EventArgs(T value) => Value = value;

    }

    public class ValueChangedEventArgs : EventArgs
    {

        public object OldValue { get; } = null;

        public object NewValue { get; } = null;

        public ValueChangedEventArgs(object oldValue, object newValue)

        {

            OldValue = oldValue;

            NewValue = newValue;

        }

    }

    public class ValueChangedEventArgs<T> : EventArgs
    {

        public T OldValue { get; } = default;

        public T NewValue { get; } = default;

        public ValueChangedEventArgs(T oldValue, T newValue)

        {

            OldValue = oldValue;

            NewValue = newValue;

        }

    }

    public class ValueChangedEventArgs<TOldValue, TNewValue> : EventArgs
    {

        public TOldValue OldValue { get; } = default;

        public TNewValue NewValue { get; } = default;

        public ValueChangedEventArgs(TOldValue oldValue, TNewValue newValue)

        {

            OldValue = oldValue;

            NewValue = newValue;

        }

    }
}
