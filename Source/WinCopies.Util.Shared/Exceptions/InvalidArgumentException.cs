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
using System.Globalization;
using System.Runtime.Serialization;
using WinCopies
#if WinCopies2
    .Util
#endif
    .Resources;

namespace WinCopies
#if WinCopies2
.Util
#endif
{
    public class InvalidArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class.
        /// </summary>
        public InvalidArgumentException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class with the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public InvalidArgumentException(in string paramName) : base(GetErrorMessage(paramName)) { }

        private static string GetErrorMessage(in string paramName) => string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidArgument, paramName);

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArgumentException"/> class with the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidArgumentException(in string paramName, in Exception innerException) : base(GetErrorMessage(paramName), innerException) { }

        /// <summary>
        /// Initializes a new instance of the System.ArgumentException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidArgumentException(in SerializationInfo info, in StreamingContext context) : base(info, context) { }
    }
}
