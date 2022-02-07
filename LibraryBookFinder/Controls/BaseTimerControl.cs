namespace LibraryBookFinder.Controls
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Threading;

    public abstract class BaseTimerControl : UserControl
    {
        protected readonly DispatcherTimer idleTimer;

        public BaseTimerControl()
        {
            this.idleTimer = new DispatcherTimer();
            this.idleTimer.Interval = this.GetTimeInterval();
            this.idleTimer.Tick += this.OnTimerTick;
            this.idleTimer.Start();
        }

        protected virtual TimeSpan GetTimeInterval()
        {
            return new TimeSpan(0, 1, 0);
        }

        protected abstract void OnTimerTick(object sender, EventArgs e);
    }
}
