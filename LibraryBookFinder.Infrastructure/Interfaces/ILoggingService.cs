namespace LibraryBookFinder.Infrastructure.Interfaces
{
    using System;

    public interface ILoggingService
    {
        void LogException(Exception ex);
    }
}
