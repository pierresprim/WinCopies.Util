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
using System.Runtime.Serialization;
using System.Security;

using WinCopies.Collections;
using WinCopies.Util;

namespace WinCopies
{
    public class InvalidEnumArgumentException : System.ComponentModel.InvalidEnumArgumentException
    {
#if !WinCopies4
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class.
        /// </summary>
        public InvalidEnumArgumentException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidEnumArgumentException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a <see langword="null"/> reference (<see langword="Nothing"/> in Visual Basic) if no inner exception is specified.</param>
        public InvalidEnumArgumentException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">info is null.</exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="Exception.HResult"/> is zero (0).</exception>
        [SecuritySafeCritical]
        protected InvalidEnumArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a message generated from the argument, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        /// <param name="enumClass">A <see cref="Type"/> that represents the enumeration class with the valid values.</param>
        /// <remarks>This constructor will create a localized message saying that the <paramref name="argumentName"/> parameter was invalid because the value passed in was invalid, and that the value should be one of the values in the enumeration class.</remarks>
        public InvalidEnumArgumentException(string argumentName, int invalidValue, Type enumClass) : this(null, argumentName, invalidValue, enumClass) { }
#endif

        // todo: also for ulong and uint

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a message generated from the argument, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        /// <param name="enumClass">A <see cref="Type"/> that represents the enumeration class with the valid values.</param>
        public InvalidEnumArgumentException(string argumentName, long invalidValue, Type enumClass) : this($"The '{argumentName}' parameter was invalid because the value passed in was not in the enumeration class.", argumentName, invalidValue, enumClass) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a message generated from the argument, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        public InvalidEnumArgumentException(string argumentName, Enum invalidValue) : this($"The '{argumentName}' parameter was invalid because the value passed in was not in the enumeration class.", argumentName, invalidValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a custom message, the argument name, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="message">A custom message describing this exception.</param>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        /// <param name="enumClass">A <see cref="Type"/> that represents the enumeration class with the valid values.</param>
        public InvalidEnumArgumentException(string message, string argumentName, int invalidValue, Type enumClass) : base(argumentName, invalidValue, enumClass)
        {
            _message = message;

            _paramName = argumentName;

            InvalidValue = (Enum)Enum.ToObject(enumClass, invalidValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a custom message, the argument name, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="message">A custom message describing this exception.</param>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        /// <param name="enumClass">A <see cref="Type"/> that represents the enumeration class with the valid values.</param>
        public InvalidEnumArgumentException(string message, string argumentName, long invalidValue, Type enumClass) : base(argumentName, invalidValue <= int.MaxValue ? (int)invalidValue : 0, enumClass)
        {
            _message = message;

            _paramName = argumentName;

            InvalidValue = (Enum)Enum.ToObject(enumClass, invalidValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumArgumentException"/> class with a message generated from the argument, the invalid value, and an enumeration class.
        /// </summary>
        /// <param name="message">A custom message describing this exception.</param>
        /// <param name="argumentName">The name of the argument that caused the exception.</param>
        /// <param name="invalidValue">The value of the argument that failed.</param>
        public InvalidEnumArgumentException(string message, string argumentName, Enum invalidValue) : base(argumentName, new EnumComparer().CompareToObject(invalidValue, int.MaxValue) <= 0 ? (int)invalidValue.GetNumValue() : 0, invalidValue.GetType())
        {
            _message = message;

            _paramName = argumentName;

            InvalidValue = invalidValue;
        }

        private readonly string _message = null;

        /// <summary>
        /// Gets the error message and the parameter name, or only the error message if no parameter name is set.
        /// </summary>
        /// <returns>A text string describing the details of the exception. The value of this property takes one of two forms: Condition Value The paramName is a null reference (Nothing in Visual Basic) or of zero length. The message string passed to the constructor. The paramName is not null reference (Nothing in Visual Basic) and it has a length greater than zero. The message string appended with the name of the invalid parameter.</returns>
        public override string Message => _message ?? base.Message;

        private readonly string _paramName;

        /// <summary>
        /// Gets the name of the parameter that causes this exception.
        /// </summary>
        /// <returns>The parameter name.</returns>
        public override string ParamName => _paramName;

        /// <summary>
        /// The value of the argument that failed.
        /// </summary>
        public Enum InvalidValue { get; }
    }
}
