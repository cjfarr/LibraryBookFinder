namespace LibraryBookFinder.ViewModels
{
    using Prism.Mvvm;
    using Prism.Regions;
    using System;

    public class ShellViewModel
    {
        private readonly IRegionManager regionManager;

        public ShellViewModel(IRegionManager regionManager)
        { 
            this.regionManager = regionManager;
        }
    }
}
