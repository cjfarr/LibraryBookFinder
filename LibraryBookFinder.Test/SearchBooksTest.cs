namespace LibraryBookFinder.Test
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using LibraryBookFinder.Infrastructure.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class SearchBooksTest
    {
        [TestMethod]
        public void SearchByTitle()
        {
            IGoogleBookService service = new GoogleBookService();

            string title = "Z for Zachariah";
            Task<BookCollection> result = service.RequestBooks(title, 0);

            BookCollection collection = result.Result;            
            Assert.IsTrue(collection?.Books?.Count > 0);

            System.Diagnostics.Debug.WriteLine(collection?.Books?.FirstOrDefault()?.Authors[0]);
        }
    }
}
