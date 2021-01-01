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

using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WinCopies.Util.Data
{
    public abstract class ValidationRule<T> : ValidationRule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRule"/> class.
        /// </summary>
        protected ValidationRule() { /* Left empty. */ }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRule"/> class with the specified validation step and a value that indicates whether the validation rule runs when the target is updated.
        /// </summary>
        /// <param name="validationStep">One of the enumeration values that specifies when the validation rule runs.</param>
        /// <param name="validatesOnTargetUpdated"><see langword="true"/> to have the validation rule run when the target of the <see cref="Binding"/> is updated; otherwise, <see langword="false"/>.</param>
        protected ValidationRule(ValidationStep validationStep, bool validatesOnTargetUpdated) : base(validationStep, validatesOnTargetUpdated) { /* Left empty. */ }

        /// <summary>
        /// Gets a value indicating whether null values are allowed. When this property is set to <see langword="false"/>, validation will fail for null values.
        /// </summary>
        public abstract bool AllowNullValueOverride { get; }

        /// <summary>
        /// When overridden in a derived class, performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>A <see cref="ValidationResult"/> object.</returns>
        protected abstract ValidationResult Validate(T value, CultureInfo cultureInfo);

        /// <summary>
        /// Performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>A <see cref="ValidationResult"/> object.</returns>
        public sealed override ValidationResult Validate(object value, CultureInfo cultureInfo) => value == null ? AllowNullValue ? ValidationResult.ValidResult : new ValidationResult(false, "The value cannot be null or empty.") : value is T _value ? Validate(_value, cultureInfo): new ValidationResult(false, $"The value is not a {typeof(T).Name} value.");
    }
}
