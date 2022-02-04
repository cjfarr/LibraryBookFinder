namespace LibraryBookFinder.ViewModels
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.JsonModels;
    using LibraryBookFinder.Models;
    using LibraryBookFinder.Views;
    using Prism.Commands;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class SearchBooksViewModel : NotifyPropertyChangedModel
    {
        private readonly IRegionManager regionManager;
        private readonly IGoogleBookService googleBookService;

        private ICommand returnToMainViewCommand;
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

        public ICommand ReturnToMainViewCommand
        {
            get
            {
                return this.returnToMainViewCommand ?? (this.returnToMainViewCommand = new DelegateCommand(this.OnReturnToMainViewRequest, () => true));
            }
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

        private void OnReturnToMainViewRequest()
        {
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(MainView));
        }

        private async void OnSearchRequest()
        {
            this.IsBusy = true;
            try
            {
                this.colection = await this.googleBookService.RequestBooks("The", this.currentPaginationOffset, this.paginationBlockLength);
                this.SearchResults = new ObservableCollection<Book>(colection.Books);
                this.RaisePropertyChange(nameof(this.ViewPositionMessage));
            }
            catch
            {
            }
            finally
            {
                this.isBusy = false;
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
