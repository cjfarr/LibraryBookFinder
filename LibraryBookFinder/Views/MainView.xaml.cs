namespace LibraryBookFinder.Views
{
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
    public partial class MainView : BaseTimerView
    {
        private RotateTransform clockArtMinuteHand;
        private RotateTransform clockArtHourHand;

        public MainView(IRegionManager regionManager) : base()
        {
            this.ClockMinuteHandAngle = 190;
            this.InitializeComponent();

            Canvas canvas = this.libraryArtContentPresenter.Content as Canvas;
            FrameworkElement[] canvasChildren = canvas.Children.Cast<FrameworkElement>().ToArray();

            Rectangle minuteHand = canvasChildren.FirstOrDefault(c => c.Name == "clockMinuteHand") as Rectangle;
            this.clockArtMinuteHand = minuteHand.RenderTransform as RotateTransform;

            this.OnTimerTick(this, null);
        }

        public double ClockMinuteHandAngle
        {
            get;
            set;
        }

        protected override TimeSpan GetTimeInterval()
        {
            return new TimeSpan(0, 0, 1);
        }

        protected override void OnTimerTick(object sender, EventArgs e)
        {
            double minutePercentage = DateTime.Now.Second / 60d;
            this.clockArtMinuteHand.Angle = Math.Floor(360d * minutePercentage);

            this.currentDateTextBlock.Text = DateTime.Now.ToString("dddd, MM/dd");
            this.currentTimeTextBlock.Text = DateTime.Now.ToString("h:mm tt");
        }
    }
}
