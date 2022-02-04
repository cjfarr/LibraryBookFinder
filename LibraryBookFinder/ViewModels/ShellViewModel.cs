namespace LibraryBookFinder.ViewModels
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Events;
    using LibraryBookFinder.Models;
    using LibraryBookFinder.Views;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Regions;
    using System;
    using System.Windows.Input;

    public class ShellViewModel : NotifyPropertyChangedModel, IDisposable
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        private SubscriptionToken navigateViewsSubscription;
        private bool canUseBackButton;

        private ICommand returnToMainViewCommand;

        public ShellViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
        { 
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            NavigateViewsEvent navigateViewsEvent = this.eventAggregator.GetEvent<NavigateViewsEvent>();
            this.navigateViewsSubscription = navigateViewsEvent.Subscribe(this.OnNavigateViewsEvent, ThreadOption.UIThread, true);
        }

        public bool CanUseBackButton
        {
            get
            {
                return this.canUseBackButton;
            }
        }

        public ICommand ReturnToMainViewCommand
        {
            get
            {
                return this.returnToMainViewCommand ?? (this.returnToMainViewCommand = new DelegateCommand(this.OnReturnToMainView));
            }
        }

        public void Dispose()
        {
            NavigateViewsEvent navigateViewsEvent = this.eventAggregator.GetEvent<NavigateViewsEvent>();
            navigateViewsEvent?.Unsubscribe(this.navigateViewsSubscription);
        }

        private void OnNavigateViewsEvent(NavigationResult result)
        {
            this.canUseBackButton = result?.Context?.Uri?.OriginalString == nameof(SearchBooksView);
            this.RaisePropertyChange(nameof(this.CanUseBackButton));
        }

        private void OnReturnToMainView()
        {
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(MainView), this.OnNavigateViewsEvent);
        }
    }
}
