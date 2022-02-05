namespace LibraryBookFinder.Infrastructure.Interfaces
{
    using LibraryBookFinder.Infrastructure.JsonModels;
    using System.Threading.Tasks;

    public interface IGoogleBookService
    {
        Task<BookCollection> RequestBooks(string titleSearch, int paginationOffset, int paginationLenth = 10);

        bool CheckIfInputIsValid(string input);
    }
}
