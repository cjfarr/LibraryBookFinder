namespace LibraryBookFinder.Views
{
    using LibraryBookFinder.Constants;
    using LibraryBookFinder.Events;
    using LibraryBookFinder.ViewModels;
    using Prism.Events;
    using Prism.Regions;
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for SearchBooksView.xaml
    /// </summary>
    public partial class SearchBooksView : UserControl
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly DispatcherTimer idleTimer;

        public SearchBooksView(
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            this.InitializeComponent();
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.idleTimer = new DispatcherTimer();
            this.idleTimer.Interval = new TimeSpan(0, 1, 0);
            this.idleTimer.Tick += this.OnIdleTimerTick;
            this.idleTimer.Start();
        }

        private void OnIdleTimerTick(object sender, EventArgs e)
        {
            ////They have been idle too long
            this.idleTimer.Tick -= this.OnIdleTimerTick;
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
