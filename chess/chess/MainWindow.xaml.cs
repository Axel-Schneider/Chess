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
using chess.AI;

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

        private Grid GridBackgroundPopUp = new Grid()
        {
            Margin = new Thickness(0),
            Background = Brushes.Black,
            Opacity = 0.7
        };
        public int BtnDraw_MouseEnter { get; private set; }
        public int BtnDraw_MouseLeave { get; private set; }
        public int BtnDraw_MouseUp { get; private set; }

        public MainWindow()
        {
            InitializeComponent();


            board = new UIBoard(AI:true, AIColor:false)
            {
                Name = "Board",
                Height = BoardGrid.Height - 20,
                Width = BoardGrid.Width - 20,
            };
            board.onTurnChanged += Board_onTurnChanged;
            board.onGameEnded += Board_onGameEnded;
            BoardGrid.Children.Add(board);
            board.GenerateBoard();


            TimerDark = new UIUserPanel(new TimeSpan(0,10,0))
            {
                Name = "Dark",
                Icon = GraphicPath.Pieces.King(false)

            };
            TimerDark.onTimerEnded += onTimerEnded;
            TimerDark.onDraw += TimerDark_onDraw;

            TimerLight = new UIUserPanel(new TimeSpan(0, 10, 0))
            {
                Name = "Light",
                Icon = GraphicPath.Pieces.King(true)

            };
            TimerLight.onTimerEnded += onTimerEnded;
            TimerLight.onDraw += TimerLight_onDraw;
            TimerLight.TimerStart();
            LightGrid.Children.Add(TimerLight);
            DarkGrid.Children.Add(TimerDark);

            history = new HistoryGrid();
            Historique.Children.Add(history);


            ArtificalInteligence ai = new RandomBot(board.BoardChess, false);
        }

        private void TimerDark_onDraw(object? sender, EventArgs e)
        {
            Draw($"{((UIUserPanel)sender).Name} want draw.\nDid you want too?");
        }

        private void TimerLight_onDraw(object? sender, EventArgs e)
        {
            Draw($"{((UIUserPanel)sender).Name} want draw.\nDid you want to?");
        }

        private void Draw(string Name)
        {
            mainGrid.Children.Add(GridBackgroundPopUp);
            PopUp pop = new PopUp(Name);
            pop.onClick += Pop_onClick;
            mainGrid.Children.Add(pop);
        }

        private void Pop_onClick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            mainGrid.Children.Remove((UIElement)sender);
            mainGrid.Children.Remove(GridBackgroundPopUp);
            if (((PopUpArgs)e).Choose)
            {
                board.Nulle(NullReason.ACCORD);
            }
        }

        private void Board_onGameEnded(object? sender, EventArgs e)
        {
            TimerLight.TimerStop();
            TimerDark.TimerStop();

            mainGrid.Children.Add(GridBackgroundPopUp);
            PopUp endGame = new PopUp($"Game ended for reason : {((EndGame)e).Message}", "Replay", "Close");
            endGame.onClick += EndGame_onClick;
            mainGrid.Children.Add(endGame);

        }

        private void EndGame_onClick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            mainGrid.Children.Remove((UIElement)sender);
            mainGrid.Children.Remove(GridBackgroundPopUp);
            if (((PopUpArgs)e).Choose)
            {
                MainWindow newGame = new MainWindow();
                newGame.Show();
                this.Close();
            }
            else
            {
                App.Current.MainWindow.Close();
            }
        }

        private void onTimerEnded(object? sender, EventArgs e)
        {
            TimerLight.TimerStop();
            TimerDark.TimerStop();
            board.Nulle(NullReason.TIMER);
        }

        private void Board_onTurnChanged(object? sender, EventArgs e)
        {

            App.Current.Dispatcher.Invoke(() =>
            {
                TimerLight.TimerInverse();
                TimerDark.TimerInverse();
                MoveLog cs = ((Board)sender).Turn
                    ? ((Board)sender).LastMovesDark.Last()
                    : ((Board)sender).LastMovesLight.Last();

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
            });
        }
    }
}
