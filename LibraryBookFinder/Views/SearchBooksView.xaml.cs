namespace LibraryBookFinder.Views
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Events;
    using Prism.Events;
    using Prism.Regions;
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SearchBooksView.xaml
    /// </summary>
    public partial class SearchBooksView : BaseTimerView
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public SearchBooksView(
            IRegionManager regionManager,
            IEventAggregator eventAggregator) 
            : base()
        {
            this.InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            ////They have been idle too long
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
