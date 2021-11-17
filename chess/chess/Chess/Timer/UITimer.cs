using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using chess.ChessBoardGUI;
using System.Drawing;
using SharpVectors.Converters;

namespace chess
{
    class UIUserPanel : Border
    {
        public ChessTimer timer { get; private set; }
        public double Fontsize { get;  set; } = 40;
        public string Name { 
            get { return sName; }
            set { setName(value); }
        }
        private string sName { get; set; } = "No Name";

        public string Icon {
            get {
                if (imgIcon == null) return null;
                return imgIcon.Source.ToString(); } 
            set { setIcon(value); }
        }
        private SvgViewbox imgIcon { get; set; }

        private Label lblName;
        private Label lblTimer;
        private StackPanel stkPawn;
        private StackPanel stkOther;
        private Grid mainGrid;
        private Grid head;
        private Border btnDraw;
        private System.Windows.Media.Brush Bakground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(250, 250, 250));

        public event EventHandler onTimerEnded;
        public event EventHandler onDraw;

        private void setName(string Name)
        {
            sName = Name;
            lblName.Content = sName;
        }
        private void setIcon(string Name)
        {
            if (imgIcon != null) head.Children.Remove(imgIcon);

            imgIcon = new SvgViewbox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Source = new Uri(Name)
            };

            head.Children.Add(imgIcon);
        }
        public UIUserPanel() : base()
        {
            Contructor(TimeDefault.time);
        }
        public UIUserPanel(TimeSpan time)
        {
            Contructor(time);
        }

        private void Contructor(TimeSpan time)
        {
            mainGrid = new Grid()
            {
                Margin = new Thickness(0,0,0,0)
            };

            Margin = new Thickness(0, 0, 0, 0);
            Background = Bakground;
            CornerRadius = new CornerRadius(10);
            Effect = new DropShadowEffect()
            {
                BlurRadius = 5,
                Direction = -45,
                ShadowDepth = 5,
                Opacity = 0.35,
                Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
            };
            Child = mainGrid;
            timer = new ChessTimer(time);
            timer.onTimerEnded += Timer_onTimerEnded;
            timer.Elapsed += Timer_Elapsed;
            initUI();

        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (Application.Current == null) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                lblTimer.Content = timer.Time.ToString(@"hh\:mm\:ss");
            });
        }

        private void Timer_onTimerEnded(object? sender, EventArgs e)
        {
            onTimerEnded?.Invoke(this, e);
        }

        private void initUI()
        {
            HeadUI();
            TimerUI();
            PiecesUI();
            DrawButtonUI();
        }

        private void HeadUI()
        {
            head = new Grid()
            {
                Margin = new Thickness(20, 20, 20, 391),
                Background = Bakground
            };
            lblName = new Label()
            {
                Content = Name,
                Margin = new Thickness(38, 0, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 16,
                FontFamily = new System.Windows.Media.FontFamily("Nirmala UI")
            };

            head.Children.Add(lblName);
            if(imgIcon != null) head.Children.Add(imgIcon);
            
            ChildrenAdd(head);
        }

        private void TimerUI()
        {
            Grid Timer = new Grid();

            lblTimer = new Label()
            {
                Content = timer.Time.ToString(@"hh\:mm\:ss"),
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 40,
                FontFamily = new System.Windows.Media.FontFamily("Nirmala UI")
            };
            Border border = new Border()
            {
                Margin = new Thickness(20, 68, 20, 250),
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(10),
                Child = Timer,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 5,
                    Direction = -45,
                    ShadowDepth = 7,
                    Opacity = 0.35,
                    Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
                }
            };
            Timer.Children.Add(lblTimer);
            ChildrenAdd(border);
        }

        private void PiecesUI()
        {

            Grid Pieces = new Grid();

            Border border = new Border()
            {
                Margin = new Thickness(20, 225, 20, 100),
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(10),
                Child = Pieces,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 5,
                    Direction = -45,
                    ShadowDepth = 7,
                    Opacity = 0.35,
                    Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
                }
            };

            Grid Pawn = new Grid()
            {
                Margin = new Thickness(20,20,20,69),
            };
            Grid Other = new Grid()
            {
                Margin = new Thickness(20, 69, 20, 20),
            };
            stkPawn = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stkOther = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Pawn.Children.Add(stkPawn);
            Other.Children.Add(stkOther);

            ChildrenAdd(border);
            Pieces.Children.Add(Pawn);
            Pieces.Children.Add(Other);
        }

        private void DrawButtonUI()
        {
            Label label = new Label()
            {
                Content = "Draw",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 16,
                FontFamily = new System.Windows.Media.FontFamily("Nirmala UI")
            };
            btnDraw = new Border()
            {
                Margin = new Thickness(329, 385, 20, 20),
                Background = System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(20),
                Child = label,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 5,
                    Direction = -45,
                    ShadowDepth = 7,
                    Opacity = 0.35,
                    Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
                }
            };
            btnDraw.MouseEnter += BtnDraw_MouseEnter;
            btnDraw.MouseLeave += BtnDraw_MouseLeave;
            btnDraw.MouseUp += BtnDraw_MouseUp;
            ChildrenAdd(btnDraw);
        }

        public void AddKill(UIPiece piece)
        {
            SvgViewbox pc = new SvgViewbox()
            {
                Margin = new Thickness(5, 0, 5, 0),
                Source = piece.Image.Source,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 7,
                    Direction = -45,
                    ShadowDepth = 7,
                    Opacity = 0.35,
                    Color = System.Windows.Media.Color.FromRgb(128, 128, 128)
                }
            };
            if (piece.PieceChess is Pawn)
                stkPawn.Children.Add(pc);
            else
                stkOther.Children.Add(pc);

        }

        public void TimerStart() => timer.Start();
        public void TimerStop() => timer.Stop();
        public void TimerInverse() => timer.Inverse();
        private void ChildrenAdd(UIElement element)
        {
            mainGrid.Children.Add(element);
        }

        private void BtnDraw_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            onDraw?.Invoke(this, e);
        }

        private void BtnDraw_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnDraw.Margin = new Thickness(329, 385, 20, 20);
            ((DropShadowEffect)btnDraw.Effect).Color = System.Windows.Media.Color.FromRgb(128, 128, 128);
        }

        private void BtnDraw_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnDraw.Margin = new Thickness(324, 380, 15, 15);
            ((DropShadowEffect)btnDraw.Effect).Color = System.Windows.Media.Color.FromRgb(200, 0, 0);
        }
    }
}
