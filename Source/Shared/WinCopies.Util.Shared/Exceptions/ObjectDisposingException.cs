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
using WinCopies.Util.Resources;

namespace WinCopies.Util
#else
using WinCopies.Resources;

namespace WinCopies
#endif
{
    /// <summary>
    /// The exception that is thrown when an operation is performed on a disposing object.
    /// </summary>
    [Serializable]
    public class ObjectDisposingException : InvalidOperationException
    {
        #region

        public ObjectDisposingException() : base(ExceptionMessages.CurrentObjectIsDisposing) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposingException"/> class with a string containing the name of the disposing object.
        /// </summary>
        /// <param name="objectName">A string containing the name of the disposing object.</param>
        public ObjectDisposingException(string objectName) : base(ExceptionMessages.CurrentObjectIsDisposing) => ObjectName = objectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposingException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If <paramref name="innerException"/> is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public ObjectDisposingException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposingException"/> class with the specified object name and message.
        /// </summary>
        /// <param name="objectName">The name of the disposed object.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ObjectDisposingException(string objectName, string message) : base(message) => ObjectName = objectName;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposingException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ObjectDisposingException(SerializationInfo info, StreamingContext context) : base(info, context) => info.GetString(nameof(ObjectName));

        /// <summary>
        /// Gets the message that describes the error.
        /// </summary>
        public override string Message => string.IsNullOrEmpty(ObjectName) ? base.Message : $"{ObjectName} is disposing.";

        /// <summary>
        /// Gets the name of the disposed object.
        /// </summary>
        public string ObjectName { get; }

        /// <summary>
        /// Retrieves the System.Runtime.Serialization.SerializationInfo object with the parameter name and additional exception information.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        [System.Security.SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ObjectName), ObjectName, typeof(string));
        }
    }
}
