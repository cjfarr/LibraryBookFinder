namespace LibraryBookFinder.Test
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using LibraryBookFinder.Infrastructure.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class SearchBooksTest
    {
        [TestMethod]
        public void SearchByTitle_Test()
        {
            IGoogleBookService service = new GoogleBookService(ConfigurationManager.AppSettings["BaseGoogleBookApiUrl"]);

            string title = "Z for Zachariah";
            string url = service.BuildGetRequestUrl(title, string.Empty, 0);
            Task<BookCollection> result = service.RequestBooks(url);

            BookCollection collection = result.Result;            
            Assert.IsTrue(collection?.Books?.Count > 0);
        }

        [TestMethod]
        public void BuildGetRequestUrl_Test()
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseGoogleBookApiUrl"];
            IGoogleBookService service = new GoogleBookService(baseUrl);

            try
            {
                service.BuildGetRequestUrl(string.Empty, string.Empty, 0);
                throw new Exception("I want to make sure the empty strings are caught as a MissingSearchCriteriaException");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Infrastructure.Exceptions.MissingSearchCriteriaException);
            }

            try
            {
                string title = "Little house on the prarie";
                string author = "Laura Ingalls Wilder";

                string url = service.BuildGetRequestUrl(title, string.Empty, 0);
                Assert.IsTrue(url.Contains(baseUrl) && url.Contains(title));
                
                url = service.BuildGetRequestUrl(string.Empty, author, 0);
                Assert.IsTrue(url.Contains(baseUrl) && url.Contains(author));

                url = service.BuildGetRequestUrl(title, author, 0);
                Assert.IsTrue(url.Contains(baseUrl) && url.Contains(title) && url.Contains(author));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
        }

        [TestMethod]
        public void CheckInputValidation_Test()
        {
            IGoogleBookService service = new GoogleBookService(ConfigurationManager.AppSettings["BaseGoogleBookApiUrl"]);

            string input = string.Empty;
            Assert.IsFalse(service.CheckIfInputIsValid(input));

            input = "        ";
            Assert.IsFalse(service.CheckIfInputIsValid(input));

            input = "This is a ridculously long search title that should fail, simply because it's way too long.";
            Assert.IsFalse(service.CheckIfInputIsValid(input));

            input = "123456789";
            Assert.IsFalse(service.CheckIfInputIsValid(input));

            input = "Fundamentals of programming";
            Assert.IsTrue(service.CheckIfInputIsValid(input));
        }
    }
}
