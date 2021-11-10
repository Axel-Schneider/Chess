﻿using chess.ChessBoardGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chess.ChessBoardGUI
{
    class UIBoard : Grid
    {

        public event EventHandler onTurnChanged;
        public event EventHandler onGameEnded;

        public Board BoardChess { get; private set; }
        public double CaseWith { get; private set; } = -1;
        public double CaseHeight { get; private set; } = -1;

        public const int BOARD_BORDER = 2;

        private List<Image> moves = new List<Image>();

        public UIBoard(string pattern = Board.DEFAULT_PATTERN)
        {
            BoardChess = new Board(pattern);
            BoardChess.onGameEnded += BoardChess_onGameEnded;
            BoardChess.onTurnChanged += BoardChess_onTurnChanged;
            BoardChess.onShowMoves += BoardChess_onShowMoves;
            BoardChess.onUnshowMoves += BoardChess_onUnshowMoves;
            BoardChess.onPromotion += BoardChess_onPromotion;
            BoardChess.onPromotionCallback += BoardChess_onPromotionCallback;
        }

        public void Nulle(NullReason reason)
        {
            BoardChess.Nulle(reason);
        }
        private void BoardChess_onPromotion(object? sender, EventArgs e)
        {
            PromotionArgs arg = (PromotionArgs)e;
            Promotion prm = new Promotion(arg.Callback, arg.Piece)
            {
                Width = Width,
                Height = Height,
                Background = new SolidColorBrush()
                {
                    Color = Color.FromRgb(0, 0, 0),
                    Opacity = 0.5
                }
            };
            prm.generateUI();
            Children.Add(prm);
        }

        private void BoardChess_onShowMoves(object? sender, EventArgs e)
        {
            showMoves(((ShowMoves)e).Piece, ((ShowMoves)e).Cases);
        }

        #region UI
        private void CalculateUISize()
        {
            CaseWith = Width / Board.BOARD_SIZE;
            CaseHeight = Height / Board.BOARD_SIZE;
        }
        public void GenerateBoard()
        {
            CalculateUISize();
            if (CaseHeight > 0 && CaseWith > 0)
            {
                BoardChess.initCases();
                for (int i = 0; i < Board.BOARD_SIZE; i++)
                {
                    for (int j = 0; j < Board.BOARD_SIZE; j++)
                    {
                        int id = i * Board.BOARD_SIZE + j;
                        UICase @case = new UICase(id)
                        {
                            Width = CaseWith,
                            Height = CaseHeight,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness((CaseWith * j), (CaseHeight * i), 0, 0),
                            Background = GetBrush(i + j),
                            x = id % Board.BOARD_SIZE,
                            y = (id - (id % Board.BOARD_SIZE)) / Board.BOARD_SIZE,
                        };
                        Children.Add(@case);
                        BoardChess.addCase(@case.CaseChess);
                    }
                }
            }
            GeneratePieces();
            Border brd = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(BOARD_BORDER, BOARD_BORDER, BOARD_BORDER, BOARD_BORDER),
            };
            Children.Add(brd);
        }
        private void GeneratePieces()
        {
            int i = 0;
            int y = 0;
            foreach (char c in BoardChess.Pattern.ToLower())
            {
                if (c == Board.CHAR_LINE)
                {
                    y++;
                    i = (Board.BOARD_SIZE) * y;
                    continue;
                }
                UIPiece black = null;
                UIPiece white = null;
                Case cblack = null;
                Case cwhite = null;
                int wx, wy;
                int bx, by;
                switch (c)
                {
                    case Board.CHAR_ROOK:
                        black = new UIRook(false);
                        white = new UIRook(true);
                        break;
                    case Board.CHAR_KNIGHT:
                        black = new UIKnight(false);
                        white = new UIKnight(true);
                        break;
                    case Board.CHAR_BISHOP:
                        black = new UIBishop(false);
                        white = new UIBishop(true);
                        break;
                    case Board.CHAR_QUEEN:
                        black = new UIQueen(false);
                        white = new UIQueen(true);
                        break;
                    case Board.CHAR_KING:
                        black = new UIKing(false);
                        white = new UIKing(true);
                        break;
                    case Board.CHAR_EMPTY:
                        break;
                    case Board.CHAR_PAWN:
                    default:
                        black = new UIPawn(false);
                        white = new UIPawn(true);
                        break;
                }
                bx = i;
                while (bx >= Board.BOARD_SIZE)
                {
                    bx -= Board.BOARD_SIZE;
                }
                wx = bx;
                by = y;
                wy = Board.BOARD_SIZE - y - 1;
                cwhite = BoardChess.Cases.Where(c => c.x == wx && c.y == wy).FirstOrDefault();
                cblack = BoardChess.Cases.Where(c => c.x == bx && c.y == by).FirstOrDefault();
                if (black != null && cblack != null) addPiece(black, cblack);
                if (white != null && cwhite != null) addPiece(white, cwhite);
                i++;
            }


        }
        private void addPiece(UIPiece piece, Case @case)
        {
            BoardChess.addPiece(piece.PieceChess, @case);
            piece.MouseDown += Piece_MouseDown; ;
        }
        private Brush GetBrush(int id)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GraphicPath.Cases.GetCase(id % 2 == 0));
            bitmap.EndInit();

            return new ImageBrush(bitmap);
        }

        #endregion
        private void unshowMoves()
        {
            foreach (Image i in moves)
            {
                if (i.Parent != null)
                    ((Grid)i.Parent).Children.Remove(i);
            }

        }
        private void showMoves(Piece piece, List<Case> cases)
        {
            unshowMoves();
            foreach (Case c in cases)
            {
                if (piece.IsAlive && BoardChess.CanMoveTo(piece, c))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(GraphicPath.Show.move);
                    bitmap.EndInit();

                    Image img = new Image()
                    {
                        Source = bitmap,
                        Opacity = 0.4
                    };
                    c.addChild(img);
                    moves.Add(img);

                    img.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                    {
                        unshowMoves();
                        BoardChess.played(piece, c);

                    };
                }
            }
        }


        private void Piece_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BoardChess.Click((UIPiece)sender);
        }
        private void BoardChess_onTurnChanged(object? sender, EventArgs e)
        {
            onTurnChanged.Invoke(sender, e);
        }

        private void BoardChess_onGameEnded(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Background = new SolidColorBrush()
                {
                    Color = Color.FromRgb(0, 0, 0),
                    Opacity = 0.2
                };
                Grid grid = new Grid()
                {
                    Background = Background
                };
                Children.Add(grid);
                Label lbl = new Label()
                {
                    Content = ((EndGame)e).Message,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 0),
                    FontSize = 36
                };
                grid.Children.Add(lbl);

            });
        }

        private void BoardChess_onUnshowMoves(object? sender, EventArgs e)
        {
            unshowMoves();
        }
        private void BoardChess_onPromotionCallback(object? sender, EventArgs e)
        {
            ((PromotionCallbackArgs)e).Piece.MouseDown += Piece_MouseDown;
        }
    }


}
