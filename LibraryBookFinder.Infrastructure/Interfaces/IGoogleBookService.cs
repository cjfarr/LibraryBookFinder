namespace LibraryBookFinder.Infrastructure.Interfaces
{
    using LibraryBookFinder.Infrastructure.JsonModels;
    using System.Threading.Tasks;

    public interface IGoogleBookService
    {
        string LastJsonResponse 
        { 
            get; 
        }

        string BuildGetRequestUrl(string titleSearchCriteria, string authorSearchCriteria, int paginationOffset, int paginationLength = 10);

        Task<BookCollection> RequestBooks(string url);

        bool CheckIfInputIsValid(string input);
    }
}
