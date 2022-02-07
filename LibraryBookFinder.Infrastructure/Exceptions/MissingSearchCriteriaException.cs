namespace LibraryBookFinder.Infrastructure.Exceptions
{
    using System;

    public class MissingSearchCriteriaException : Exception
    {
        public MissingSearchCriteriaException(
            string message = "No title or author was used to search on.") 
            : base(message)
        {
        }
    }
}
