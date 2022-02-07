namespace LibraryBookFinder.ViewModels
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using LibraryBookFinder.Models;
    using Prism.Commands;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class SearchBooksViewModel : NotifyPropertyChangedModel
    {
        private readonly IRegionManager regionManager;
        private readonly IGoogleBookService googleBookService;

        private ICommand searchCommand;
        private ICommand changePageResultsCommand;
        private ICommand showJsonResponseCommand;

        private bool isBusy;
        private ObservableCollection<Book> searchResults;
        private int currentPaginationOffset;
        private int paginationBlockLength;
        private BookCollection colection;
        private string searchTitleCriteria;
        private bool isUsingSearchTitle;
        private string searchAuthorCriteria;
        private bool isUsingSearchAuthor;
        private bool showJsonResults;

        public SearchBooksViewModel(
            IRegionManager regionManager,
            IGoogleBookService googleBookService)
        {
            this.regionManager = regionManager;
            this.googleBookService = googleBookService;

            this.currentPaginationOffset = 0;
            this.paginationBlockLength = 40;

            this.IsUsingSearchTitle = true;
            this.IsUsingSearchAuthor = false;
        }

        public ICommand SearchCommand
        {
            get => this.searchCommand ?? (this.searchCommand = new DelegateCommand(() =>
            {
                ////Treat the main search command as a new search with pagination going back to zero.
                this.currentPaginationOffset = 0;
                this.OnSearchRequest();
            }));
        }

        public ICommand ChangePageResultsCommand
        {
            get => this.changePageResultsCommand ?? (this.changePageResultsCommand = new DelegateCommand<int?>(this.OnChangePageResultsRequest));
        }

        public ICommand ShowJsonResponseCommand
        {
            get => this.showJsonResponseCommand ?? (this.showJsonResponseCommand = new DelegateCommand(
                () =>
                {
                    this.ShowJsonResults = !this.ShowJsonResults;
                }));
        }

        public bool IsBusy
        {
            get => this.isBusy;

            set
            {
                this.isBusy = value;
                this.RaisePropertyChange(nameof(this.IsBusy));
                this.RaisePropertyChange(nameof(this.CanUseSearchButton));
            }
        }

        public bool CanUseSearchButton
        {
            get => !this.IsBusy && 
                (this.ValidateFieldInput(this.IsUsingSearchTitle, this.SearchTitleCriteria) || 
                this.ValidateFieldInput(this.IsUsingSearchAuthor, this.SearchAuthorCriteria));
        }

        public bool CanUsePreviousButton
        {
            get => this.currentPaginationOffset > 0 && this.colection != null;
        }

        public bool CanUseNextButton
        {
            get => this.colection?.TotalItemsExpected > 0 && this.currentPaginationOffset + this.paginationBlockLength < this.colection.TotalItemsExpected;
        }

        public ObservableCollection<Book> SearchResults
        {
            get => this.searchResults ?? (this.searchResults = new ObservableCollection<Book>());

            set
            {
                this.searchResults = value;
                this.RaisePropertyChange(nameof(this.SearchResults));
            }
        }

        public string ViewPositionMessage
        {
            get
            {
                if (this.colection == null)
                {
                    return string.Empty;
                }

                int highestResult = this.currentPaginationOffset + this.paginationBlockLength;
                if (highestResult > this.colection.TotalItemsExpected)
                {
                    highestResult = this.colection.TotalItemsExpected;
                }

                return $"Viewing {this.currentPaginationOffset + 1} - {highestResult} out of {this.colection.TotalItemsExpected}";
            }
        }

        public bool AreSearchResultsAvailable
        {
            get => this.colection?.TotalItemsExpected > 0;
        }

        public string SearchTitleCriteria
        {
            get => this.searchTitleCriteria;

            set
            {
                this.searchTitleCriteria = value;
                this.RaisePropertyChange(nameof(this.SearchTitleCriteria));
                this.RaisePropertyChange(nameof(this.CanUseSearchButton));
            }
        }

        public bool IsUsingSearchTitle
        {
            get => this.isUsingSearchTitle;

            set
            {
                this.isUsingSearchTitle = value;
                this.RaisePropertyChange(nameof(this.IsUsingSearchTitle));
                this.RaisePropertyChange(nameof(this.CanUseSearchButton));
            }
        }

        public string SearchAuthorCriteria
        {
            get => this.searchAuthorCriteria;

            set
            {
                this.searchAuthorCriteria = value;
                this.RaisePropertyChange(nameof(this.SearchAuthorCriteria));
                this.RaisePropertyChange(nameof(this.CanUseSearchButton));
            }
        }

        public bool IsUsingSearchAuthor
        {
            get => this.isUsingSearchAuthor;

            set
            {
                this.isUsingSearchAuthor = value;
                this.RaisePropertyChange(nameof(this.IsUsingSearchAuthor));
                this.RaisePropertyChange(nameof(this.CanUseSearchButton));
            }
        }

        public string LastJsonResponse
        {
            get => this.googleBookService.LastJsonResponse;
        }

        public bool CanViewJsonResponse
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public bool ShowJsonResults
        {
            get => this.showJsonResults;

            set
            {
                this.showJsonResults = value;
                this.RaisePropertyChange(nameof(this.ShowJsonResults));
                this.RaisePropertyChange(nameof(this.LastJsonResponse));
            }
        }

        private async void OnSearchRequest()
        {
            this.SearchResults.Clear();
            this.IsBusy = true;

            try
            {
                string url = this.googleBookService.BuildGetRequestUrl(
                    this.SearchTitleCriteria, 
                    this.SearchAuthorCriteria, 
                    this.currentPaginationOffset, 
                    this.paginationBlockLength);

                this.colection = await this.googleBookService.RequestBooks(url);
                this.SearchResults.AddRange(colection.Books);
            }
            catch (Exception ex)
            {
                ////TODO.  Exception message
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                this.IsBusy = false;
                this.RaisePropertyChange(nameof(this.ViewPositionMessage));
                this.RaisePropertyChange(nameof(this.AreSearchResultsAvailable));
                this.RaisePropertyChange(nameof(this.LastJsonResponse));
                this.RaisePropertyChange(nameof(this.CanUsePreviousButton));
                this.RaisePropertyChange(nameof(this.CanUseNextButton));
            }
        }

        private void OnChangePageResultsRequest(int? direction)
        {
            if (direction.HasValue)
            {
                this.currentPaginationOffset += this.paginationBlockLength * direction.Value;
                if (this.currentPaginationOffset < 0)
                {
                    this.currentPaginationOffset = 0;
                }

                this.OnSearchRequest();
            }
        }

        private bool ValidateFieldInput(bool isUsingField, string input)
        {
            if (!isUsingField)
            {
                return false;
            }

            return this.googleBookService.CheckIfInputIsValid(input);
        }
    }
}
