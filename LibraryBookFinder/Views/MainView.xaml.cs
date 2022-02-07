namespace LibraryBookFinder.Views
{
    using LibraryBookFinder.Controls;
    using Prism.Regions;
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : BaseTimerControl
    {
        private RotateTransform clockArtMinuteHand;
        private RotateTransform clockArtHourHand;

        public MainView(IRegionManager regionManager) : base()
        {
            this.InitializeComponent();

            Canvas canvas = this.libraryArtContentPresenter.Content as Canvas;
            FrameworkElement[] canvasChildren = canvas.Children.Cast<FrameworkElement>().ToArray();

            Rectangle minuteHand = canvasChildren.FirstOrDefault(c => c.Name == "clockMinuteHand") as Rectangle;
            this.clockArtMinuteHand = minuteHand.RenderTransform as RotateTransform;

            Rectangle hourHand = canvasChildren.FirstOrDefault(c => c.Name == "clockHourHand") as Rectangle;
            this.clockArtHourHand = hourHand.RenderTransform as RotateTransform;

            this.OnTimerTick(this, null);
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            double minutePercentage = DateTime.Now.Second / 60d;
            this.clockArtMinuteHand.Angle = Math.Floor(360d * minutePercentage);

            int hour = DateTime.Now.Hour;
            if (hour > 12)
            {
                hour -= 12;
            }

            double hourPercentage = hour / 12d;
            ////the  - .25 is because I originally drew the hour hand on 3 instead of 12.
            this.clockArtHourHand.Angle = Math.Floor(360d * (hourPercentage - .25d));
            
            this.currentDateTextBlock.Text = DateTime.Now.ToString("dddd, MM/dd");
            this.currentTimeTextBlock.Text = DateTime.Now.ToString("h:mm tt");
        }
    }
}
