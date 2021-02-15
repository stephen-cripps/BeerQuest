using System;

namespace BeerQuest.Application.Exceptions
{
    /// <summary>
    /// To be thrown when invalid inputs are passed into an API request
    /// </summary>
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        { }
    }
}
