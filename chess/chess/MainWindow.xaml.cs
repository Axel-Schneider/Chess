using chess.ChessBoardGUI;
using SharpVectors.Converters;
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
using System.Windows.Media.Effects;
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
        private UIBoard board;
        private UIUserPanel TimerDark;
        private UIUserPanel TimerLight;
        private HistoryGrid history;
        public MainWindow()
        {
            InitializeComponent();


            board = new UIBoard()
            {
                Name = "Board",
                Height = BoardGrid.Height - 20,
                Width = BoardGrid.Width - 20,
            };
            board.onTurnChanged += Board_onTurnChanged;
            board.onGameEnded += Board_onGameEnded;
            BoardGrid.Children.Add(board);
            board.GenerateBoard();

            BoardGrid.Effect = new DropShadowEffect()
            {
                BlurRadius = 5,
                Direction = -45,
                ShadowDepth = 5,
                Opacity = 0.35
            };

            TimerDark = new UIUserPanel(new TimeSpan(0,10,0))
            {
                Name = "Dark",
                Icon = GraphicPath.Pieces.King(false)

            };
            TimerDark.onTimerEnded += onTimerEnded;

            TimerLight = new UIUserPanel(new TimeSpan(0, 10, 0))
            {
                Name = "Light",
                Icon = GraphicPath.Pieces.King(true)

            };
            TimerLight.onTimerEnded += onTimerEnded;
            TimerLight.TimerStart();
            LightGrid.Children.Add(TimerLight);
            DarkGrid.Children.Add(TimerDark);

            history = new HistoryGrid();
            Historique.Children.Add(history);
        }

        private void Board_onGameEnded(object? sender, EventArgs e)
        {
            TimerLight.TimerStop();
            TimerDark.TimerStop();
        }

        private void onTimerEnded(object? sender, EventArgs e)
        {
            TimerLight.TimerStop();
            TimerDark.TimerStop();
            board.Nulle(NullReason.TIMER);
        }

        private void Board_onTurnChanged(object? sender, EventArgs e)
        {
            string move = "";
            MoveLog cs = ((Board)sender).Turn
                ? ((Board)sender).LastMovesDark.Last()
                : ((Board)sender).LastMovesLight.Last();
            move = cs.ToString();
            TimerLight.TimerInverse();
            TimerDark.TimerInverse();

            if (cs.Kill)
            {
                if (((Board)sender).Turn)
                {
                    TimerDark.AddKill(cs.PieceKilled.UIPiece);
                }
                else
                {
                    TimerLight.AddKill(cs.PieceKilled.UIPiece);
                }
            }

            history.AddChildren(new HistoryItem(cs));

        }
    }
}
