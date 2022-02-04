namespace LibraryBookFinder.Views
{
    using LibraryBookFinder.Constants;
    using Prism.Regions;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        private readonly IRegionManager regionManager;

        public ShellView(IRegionManager regionManager)
        {
            this.InitializeComponent();

            this.regionManager = regionManager;
            this.Loaded += this.OnLoaded;

            this.DataContextChanged += this.OnDataContextChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;
            this.regionManager.RequestNavigate(RegionName.MainRegion, nameof(MainView));
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.DataContextChanged -= this.OnDataContextChanged;
        }

        private void OnQuitButtonClick(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
