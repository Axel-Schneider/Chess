using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace chess
{
    class ChessTimer : GroupBox
    {
        public double Fontsize { get;  set; } = 40;
        public TimeSpan Time { get; set; } = new TimeSpan(0, 10, 0);
        private Timer Timer;
        private TextBlock textBlock;
        public event EventHandler onTimerEnded;
        public ChessTimer() : base()
        {
            Header = "NO NAME";
            Foreground  = Brushes.LightGray;
            textBlock = new TextBlock();
            Timer = new Timer(20);
            Timer.Elapsed += Timer_Elapsed;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Margin = new Thickness(5, 5, 5, 5);

            textBlock = new TextBlock()
            {
                Foreground = Foreground,
                FontSize = Fontsize,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            this.Content = textBlock;
            textBlock.Text = Time.ToString(@"hh\:mm\:ss");

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time = Time.Subtract(new TimeSpan(0, 0, 0, 0, 20));
            if(Time.TotalMilliseconds <= 0)
            {
                Timer.Stop();
                onTimerEnded?.Invoke(this, null);
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                    textBlock.Text = Time.ToString(@"hh\:mm\:ss");
            });
        }
        public void Start()
        {
            Timer.Start();
        }
        public void Stop()
        {
            Timer.Stop();
        }
        public void Pause()
        {
            Timer.Stop();
        }
    }
}
