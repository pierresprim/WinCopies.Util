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

using System;
using System.Windows.Markup;

namespace WinCopies.Markup
{
    public abstract class ValueMarkupExtension<T> : MarkupExtension
    {
        public T Value { get; }

        public ValueMarkupExtension(in T value) => Value = value;

        public override object ProvideValue(IServiceProvider serviceProvider) => Value;
    }

    public class ValueMarkupExtension : MarkupExtension
    {
        public object Value { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider) => Value;
    }

    public class Boolean : ValueMarkupExtension<bool>
    {
        public Boolean(in bool value) : base(value) { /* Left empty. */ }

        public Boolean(in string value) : base(bool.Parse(value)) { /* Left empty. */ }
    }

    /// <summary>
    /// Represents a markup extension that always returns the <see langword="true"/> value.
    /// </summary>
    public class TrueValue : Boolean
    {
        public TrueValue() : base(true) { /* Left empty. */ }
    }

    /// <summary>
    /// Represents a markup extension that always returns the <see langword="false"/> value.
    /// </summary>
    public class FalseValue : Boolean
    {
        public FalseValue() : base(false) { /* Left empty. */ }
    }
}
