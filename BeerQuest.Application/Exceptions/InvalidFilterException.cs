namespace BeerQuest.Application.Exceptions
{
    class InvalidFilterException : BadRequestException
    {
        /// <summary>
        /// A specific implementation of the BadRequestException for Data Filters
        /// </summary>
        /// <param name="filterName"></param>
        public InvalidFilterException(string filterName) : base("Invalid Data Filter: " + filterName)
        {
        }
    }
}
