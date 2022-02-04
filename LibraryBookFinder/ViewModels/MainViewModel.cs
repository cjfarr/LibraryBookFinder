namespace LibraryBookFinder.ViewModels
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Events;
    using LibraryBookFinder.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using System.Windows.Input;

    public class MainViewModel
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        private ICommand searchBooksCommand;

        public MainViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
        }

        public ICommand SearchBooksCommand
        {
            get
            {
                
                return this.searchBooksCommand ?? (this.searchBooksCommand = new DelegateCommand(this.OnSearchBooksRequest, () => true));
            }
        }

        private void OnSearchBooksRequest()
        {
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(SearchBooksView), this.OnNavigateToSearchBooksCallback);
        }

        private void OnNavigateToSearchBooksCallback(NavigationResult result)
        {
            NavigateViewsEvent navigationEvent = this.eventAggregator.GetEvent<NavigateViewsEvent>();
            navigationEvent?.Publish(result);
        }
    }
}
