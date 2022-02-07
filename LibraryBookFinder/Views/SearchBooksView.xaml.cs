namespace LibraryBookFinder.Views
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Controls;
    using LibraryBookFinder.Events;
    using LibraryBookFinder.ViewModels;
    using Prism.Events;
    using Prism.Regions;
    using System;
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SearchBooksView.xaml
    /// </summary>
    public partial class SearchBooksView : BaseTimerControl
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        private INotifyPropertyChanged viewModel;

        public SearchBooksView(
            IRegionManager regionManager,
            IEventAggregator eventAggregator) 
            : base()
        {
            this.InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.Unloaded += this.OnUnloaded;
            this.DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext is INotifyPropertyChanged dataContext)
            {
                this.viewModel = dataContext;
                this.viewModel.PropertyChanged += this.OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                ////The reason I'm using CanUsePreviousButton property is because it only gets
                ////raised after a search is performed.  Some of the others get raised in places
                ////I may not want to set focus back to a search field.
                case nameof(SearchBooksViewModel.CanUsePreviousButton):
                    if (this.titleSearchField.IsUsingField)
                    {
                        this.titleSearchField.FocusSearchTextBox();
                    }
                    else if (this.authorSearchField.IsUsingField)
                    {
                        this.authorSearchField.FocusSearchTextBox();
                    }

                    break;
            }
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.viewModel != null)
            {
                this.viewModel.PropertyChanged -= this.OnViewModelPropertyChanged;
            }
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            ////They have been idle too long.
            this.idleTimer.Tick -= this.OnTimerTick;
            this.regionManager.Regions[RegionName.MainRegion].RemoveAll();
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(MainView), this.OnNavigateBackToMainView);
        }

        private void OnNavigateBackToMainView(NavigationResult result)
        {
            NavigateViewsEvent navigateViewsEvent = this.eventAggregator.GetEvent<NavigateViewsEvent>();
            navigateViewsEvent?.Publish(result);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.idleTimer.Stop();
            base.OnKeyDown(e);
            this.idleTimer.Start();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.idleTimer.Stop();
            base.OnMouseMove(e);
            this.idleTimer.Start();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            this.idleTimer.Stop();
            base.OnMouseDown(e);
            this.idleTimer.Start();
        }
    }
}
