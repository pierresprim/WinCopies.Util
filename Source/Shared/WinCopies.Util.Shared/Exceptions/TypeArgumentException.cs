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
using System.Runtime.Serialization;

#if !WinCopies3
namespace WinCopies.Util
#else
namespace WinCopies
#endif
{
    public class TypeArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class.
        /// </summary>
        public TypeArgumentException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class with a specified
        /// error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TypeArgumentException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this
        /// exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/>
        /// parameter is not a <see langword="null"/> reference, the current exception is raised in a catch
        /// block that handles the inner exception.</param>
        public TypeArgumentException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class with a specified
        /// error message and the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public TypeArgumentException(string message, string paramName) : base(message, paramName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class with a specified
        /// error message, the parameter name, and a reference to the inner exception that
        /// is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/>
        /// parameter is not a <see langword="null"/> reference, the current exception is raised in a catch
        /// block that handles the inner exception.</param>
        public TypeArgumentException(string message, string paramName, Exception innerException) : base(message, paramName, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeArgumentException"/> class with serialized
        /// data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TypeArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    public class TypeArgumentException<TExpected> : ArgumentException
    {
        private const string MESSAGE = "an instance of or an instance of a type that inherits or implement ";

        public TypeArgumentException(in Type
#if CS8
            ?
#endif
            type, in string argumentName) : base($"{argumentName} should be {MESSAGE}{GetTypeFullName(typeof(TExpected))}. {argumentName} was {(type == null ? "null" : MESSAGE + GetTypeFullName(type))}.") { /* Left empty. */ }

        private static string GetTypeFullName(in Type t) => t.FullName ?? t.Name;
    }
}
