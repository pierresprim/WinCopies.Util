using System;

using static WinCopies.Resources.ExceptionMessages;

namespace WinCopies
{
    public class ResourceNotFoundException : ArgumentException
    {
        public ResourceNotFoundException() : base(ResourceNotFoundExceptionMessage) { /* Left empty. */ }

        public ResourceNotFoundException(in string paramName) : base(ResourceNotFoundExceptionMessage, paramName) { /* Left empty. */ }
    }
}
