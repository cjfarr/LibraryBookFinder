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
    using System.Windows.Threading;

    public class SearchBooksViewModel : NotifyPropertyChangedModel
    {
        private readonly IRegionManager regionManager;
        private readonly IGoogleBookService googleBookService;

        private ICommand searchCommand;
        private ICommand changePageResultsCommand;

        private bool isBusy;
        private ObservableCollection<Book> searchResults;
        private int currentPaginationOffset;
        private int paginationBlockLength;
        private BookCollection colection;

        public SearchBooksViewModel(
            IRegionManager regionManager,
            IGoogleBookService googleBookService)
        {
            this.regionManager = regionManager;
            this.googleBookService = googleBookService;

            this.currentPaginationOffset = 0;
            this.paginationBlockLength = 10;
        }

        public ICommand SearchCommand
        {
            get
            {
                return this.searchCommand ?? (this.searchCommand = new DelegateCommand(this.OnSearchRequest));
            }
        }

        public ICommand ChangePageResultsCommand
        {
            get
            {
                return this.changePageResultsCommand ?? (this.changePageResultsCommand = new DelegateCommand<int?>(this.OnChangePageResultsRequest));
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                this.RaisePropertyChange(nameof(this.IsBusy));
            }
        }

        public ObservableCollection<Book> SearchResults
        {
            get
            {
                return this.searchResults ?? (this.searchResults = new ObservableCollection<Book>());
            }

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
            get
            {
                return this.colection?.TotalItemsExpected > 0;
            }
        }

        private async void OnSearchRequest()
        {
            this.SearchResults.Clear();
            this.IsBusy = true;

            try
            {
                this.colection = await this.googleBookService.RequestBooks("The", this.currentPaginationOffset, this.paginationBlockLength);
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
