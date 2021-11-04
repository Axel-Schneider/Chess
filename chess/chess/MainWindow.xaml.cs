using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board board;
        public MainWindow()
        {
            InitializeComponent();
            Height = BoardGrid.Height + 60;

            

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GraphicPath.Show.Background);
            bitmap.EndInit();
            Background = new ImageBrush(bitmap);

            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GraphicPath.Show.BackgroundObject);
            bitmap.EndInit();
            BoardGrid.Background = new ImageBrush(bitmap);

            board = new Board()
            //board = new Board("r   k  r/pppppppp")
            //board = new Board("    k/r      r")
            //board = new Board("rnbqk  r/ppp  ppp/b  p")
            {
                Name = "Board",
                Height = BoardGrid.Height - 20,
                Width = BoardGrid.Width - 20,
            };
            board.onTurnChanged += Board_onTurnChanged;
            board.onGameEnded += Board_onGameEnded;
            BoardGrid.Children.Add(board);
            board.RegenerateBoard();

            TimerDark = new ChessTimer()
            {
                Header = "Dark",
            };
            TimerDark.onTimerEnded += Dark_onTimerEnded;
            TimerLight = new ChessTimer()
            {
                Header = "Light",
            }; 
            TimerLight.onTimerEnded += Light_onTimerEnded;

            DarkGrid.Background = new ImageBrush(bitmap);
            DarkGrid.Children.Add(TimerDark);
            LightGrid.Background = new ImageBrush(bitmap);
            LightGrid.Children.Add(TimerLight);

            TimerLight.Start();
        }

        private void Board_onGameEnded(object? sender, EventArgs e)
        {
            TimerLight.Stop();
            TimerDark.Stop();
        }

        private void Light_onTimerEnded(object? sender, EventArgs e)
        {
            board.Nulle(NullReason.TIMER);
        }

        private void Dark_onTimerEnded(object? sender, EventArgs e)
        {
            board.Nulle(NullReason.TIMER);
        }

        ChessTimer TimerDark;
        ChessTimer TimerLight;
        private void Board_onTurnChanged(object? sender, EventArgs e)
        {
            if (((ChangeTurn)e).Turn)
            {
                TimerDark.Pause();
                TimerLight.Start();
            }
            else
            {
                TimerLight.Pause();
                TimerDark.Start();
            }
        }
    }
}
