using System;

namespace Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string exceptionMessage) : base(exceptionMessage)
        {

        }
    }
}
