namespace LibraryBookFinder.Views
{
    using Prism.Regions;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView(IRegionManager regionManager)
        {
            this.InitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            this.DataContextChanged -= this.OnDataContextChanged;
            System.Diagnostics.Debug.WriteLine($"MainView.DataContext = { this.DataContext?.GetType()?.ToString() }");
        }
    }
}
