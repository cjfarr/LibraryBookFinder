namespace LibraryBookFinder.Views
{
    using LibraryBookFinder.Constants;
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
        private readonly DispatcherTimer idleTimer;

        public SearchBooksView(IRegionManager regionManager)
        {
            this.InitializeComponent();
            this.regionManager = regionManager;

            this.idleTimer = new DispatcherTimer();
            this.idleTimer.Interval = new TimeSpan(0, 1, 0);
            this.idleTimer.Tick += this.OnIdleTimerTick;
            this.idleTimer.Start();

            this.testToggle.Checked += TestToggle_Checked;
            this.testToggle.Unchecked += TestToggle_Unchecked;
        }

        private void TestToggle_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Toggle unchecked");
        }

        private void TestToggle_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Toggle checked");
        }

        private void OnIdleTimerTick(object sender, EventArgs e)
        {
            ////They have been idle too long
            this.idleTimer.Tick -= this.OnIdleTimerTick;
            this.regionManager.Regions[RegionName.MainRegion].RemoveAll();
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(MainView));
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
