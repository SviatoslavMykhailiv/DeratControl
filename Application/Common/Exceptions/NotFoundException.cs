using System;

namespace Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string exceptionMessage) : base(exceptionMessage)
        {

        }
    }
}
