namespace LibraryBookFinder
{
    using LibraryBookFinder.Infrastructure.Interfaces;
    using LibraryBookFinder.Infrastructure.Services;
    using LibraryBookFinder.ViewModels;
    using LibraryBookFinder.Views;
    using Prism.Ioc;
    using Prism.Unity;
    using System.Configuration;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            ShellView window = this.Container.Resolve<ShellView>();

            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(typeof(IGoogleBookService), new GoogleBookService(ConfigurationManager.AppSettings["BaseGoogleBookApiUrl"]));
            containerRegistry.RegisterInstance(typeof(ILoggingService), new LoggingService());
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<SearchBooksView, SearchBooksViewModel>();
        }
    }
}
