namespace LibraryBookFinder.ViewModels
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Views;
    using Prism.Commands;
    using Prism.Regions;
    using System.Windows.Input;

    public class MainViewModel
    {
        private readonly IRegionManager regionManager;

        private ICommand searchBooksCommand;

        public MainViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
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
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(SearchBooksView));
        }
    }
}
