namespace LibraryBookFinder.ViewModels
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using LibraryBookFinder.Models;
    using Prism.Commands;
    using Prism.Regions;
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
            this.paginationBlockLength = 10;

            this.IsUsingSearchTitle = true;
            this.IsUsingSearchAuthor = false;
        }

        public ICommand SearchCommand
        {
            get => this.searchCommand ?? (this.searchCommand = new DelegateCommand(this.OnSearchRequest));
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
            get => !this.IsBusy && (this.IsUsingSearchTitle || this.IsUsingSearchAuthor);
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

                return $"Viewing {this.currentPaginationOffset + 1} - {this.currentPaginationOffset + 1 + this.paginationBlockLength} out of {this.colection.TotalItemsExpected}";
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
                bool validation = this.googleBookService.CheckIfInputIsValid(this.searchTitleCriteria);
                System.Diagnostics.Debug.WriteLine(validation);

                this.RaisePropertyChange(nameof(this.SearchTitleCriteria));
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
                this.colection = await this.googleBookService.RequestBooks(this.SearchTitleCriteria, this.currentPaginationOffset, this.paginationBlockLength);
                this.SearchResults.AddRange(colection.Books);
            }
            catch
            {
            }
            finally
            {
                this.IsBusy = false;
                this.RaisePropertyChange(nameof(this.ViewPositionMessage));
                this.RaisePropertyChange(nameof(this.AreSearchResultsAvailable));
            }
        }

        private void OnChangePageResultsRequest(int? direction)
        {
            if (direction.HasValue)
            {
                this.currentPaginationOffset += this.paginationBlockLength * direction.Value;
                this.OnSearchRequest();
            }
        }
    }
}
