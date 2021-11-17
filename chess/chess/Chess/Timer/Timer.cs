using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace chess.ChessBoardGUI
{
    class ChessTimer
    {
        private Timer timer;
        public TimeSpan Time;
        private TimeSpan Interval = new TimeSpan(0,0,0,0,50);

        public event EventHandler onTimerEnded;
        public event EventHandler Elapsed;
        public ChessTimer()
        {
            Contructor(TimeDefault.time);
        }
        public ChessTimer(TimeSpan Time)
        {
            Contructor(Time);
        }

        private void Contructor(TimeSpan Time)
        {
            this.Time = Time;
            timer = new Timer(Interval.TotalMilliseconds);
            timer.Elapsed += ChessTimer_Elapsed;
        }

        private void ChessTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time = Time.Subtract(Interval);
            Elapsed?.Invoke(this, e);
            if (Time.TotalMilliseconds <= 0) onTimerEnded?.Invoke(this, null);
        }

        public void Inverse()
        {
            if (timer.Enabled) { Stop(); }
            else{ Start(); }
        }
        public void Start() => timer.Start(); 
        public void Stop() => timer.Stop();
    }

    public static class TimeDefault
    {
        public static readonly TimeSpan time = new TimeSpan(0, 10, 0);
    }
}
