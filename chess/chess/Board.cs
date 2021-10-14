using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chess
{
    public class Board : Grid
    {
        public const int BOARD_SIZE = 8;
        public const int BOARD_BORDER = 2;
        
        public double CaseWith { get; private set; } = -1;
        public double CaseHeight { get; private set; } = -1;

        public List<Case> Cases { get; private set; }
        public List<Piece> Pieces { get; private set; }

        private TypeSwitch ts;
        private List<Image> moves;
        public Board() : base()
        { 
            Pieces = new List<Piece>();
            ts = new TypeSwitch()
                .Case((Pawn x) => CalculPawn(x))
                .Case((Bishop x) => CalculBishop(x))
                .Case((Knight x) => CalculKnight(x))
                .Case((Rook x) => CalculRook(x))
                .Case((Queen x) => CalculQueen(x))
                .Case((King x) => CalculKing(x));
        }

        public void RegenerateBoard()
        {
            GenerateBoard();
        }
        
        private void Calculate()
        {
            CaseWith = Width / BOARD_SIZE;
            CaseHeight = Height / BOARD_SIZE;
        }

        private void GenerateBoard()
        {
            Calculate();
            if(CaseHeight > 0 && CaseWith > 0)
            {
                Cases = new List<Case>();
                for(int i = 0; i < BOARD_SIZE; i++)
                {
                    for(int j = 0; j < BOARD_SIZE; j++)
                    {
                        int id = i * BOARD_SIZE + j;
                        Case @case = new Case(id)
                        {
                            Width = CaseWith,
                            Height = CaseHeight,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness((CaseWith * j), (CaseHeight * i), 0, 0),
                            Background = GetBrush(i + j),
                            x = id % BOARD_SIZE,
                            y = (id - (id % BOARD_SIZE)) / BOARD_SIZE,
                        };
                        Children.Add(@case);
                        Cases.Add(@case);

                        if((i+j) % 3 == 0)
                        {
                            Piece piece = rdmPiece();
                            Pieces.Add(piece);
                            @case.AddPiece(piece);
                        }
                    }
                }
            }
            Border brd = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(BOARD_BORDER, BOARD_BORDER, BOARD_BORDER, BOARD_BORDER),
            };
            Children.Add(brd);
        }
        private Brush GetBrush(int id)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GraphicPath.Cases.GetCase(id % 2 == 0));
            bitmap.EndInit();

            return new ImageBrush(bitmap);
        }

        public void tmp()
        {
            Random rdm = new Random();
            Piece pc = Pieces.Where(p => rdm.Next(2) % 2 == 0).First();
            Cases.Where(p => rdm.Next(2) % 2 == 0).First().AddPiece(pc);
        }
        private Piece rdmPiece()
        {
            Random rdm = new Random();
            Piece pc;
            switch (rdm.Next(6))
            {
                case 0:
                    pc = new Pawn(rdm.Next() % 2 == 0);
                    break;
                case 1:
                    pc = new Bishop(rdm.Next() % 2 == 0);
                    break;
                case 2:
                    pc = new Knight(rdm.Next() % 2 == 0);
                    break;
                case 3:
                    pc = new Rook(rdm.Next() % 2 == 0);
                    break;
                case 4:
                    pc = new Queen(rdm.Next() % 2 == 0);
                    break;
                case 5:
                    pc = new King(rdm.Next() % 2 == 0);
                    break;
                default:
                    pc = new Pawn(rdm.Next() % 2 == 0);
                    break;
            }
            pc.MouseDown += Piece_MouseDown;
            return pc;
        }

        private void Piece_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CalculPosition((Piece)sender);
        }
        private void CalculPosition(Piece sender)
        {
            ts.Switch(sender);
        }

        private void showMoves(Piece piece, List<Case> cases)
        {
            if (cases.Count <= 0) return;
            if(moves != null)
            {
                foreach (Image i in moves)
                {
                    if(i.Parent != null)
                        ((Grid)i.Parent).Children.Remove(i);
                }
            }
            moves = new List<Image>();

            foreach(Case c in cases)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(GraphicPath.Show.move);
                bitmap.EndInit();

                Image img = new Image()
                {
                    Source = bitmap,
                    Opacity = 0.5
                };
                c.Children.Add(img);
                moves.Add(img);
                img.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                {
                    foreach(Image i in moves)
                    {
                        ((Grid)i.Parent).Children.Remove(i);
                    }
                    piece.Case.RemovePiece();
                    c.AddPiece(piece);
                };
            }
        }

        private void CalculKing(King sender)
        {
            var mvs = new List<int>();
            foreach (int i in new int[] { 1, 7, 8, 9 })
            {
                mvs.Add(i);
                mvs.Add(-i);
            }
            List<Case> res = new List<Case>();

            foreach (int mv in mvs)
            {
                Case obj = Cases.Where(c => c.Id == sender.Case.Id + mv && Math.Abs(c.y - sender.Case.y) < 2 && Math.Abs(c.x - sender.Case.x) < 2).FirstOrDefault();
                if (obj != null)
                {
                    if(obj.Piece == null || obj.Piece.Color != sender.Color)
                    {
                        res.Add(obj);
                    }
                }
            }

            showMoves(sender, res);
        }

        private void CalculQueen(Queen sender)
        {
            List<Case> res = new List<Case>();
            res.AddRange(CalculDiagonal(sender));
            res.AddRange(CalculLine(sender));

            showMoves(sender, res);
        }

        private void CalculRook(Rook sender)
        {
            List<Case> res = CalculLine(sender);

            showMoves(sender, res);
        }

        private void CalculBishop(Bishop sender)
        {
            List<Case> res = CalculDiagonal(sender);

            showMoves(sender, res);
        }

        private void CalculKnight(Knight sender)
        {
            List<Case> res = new List<Case>();
            var mvs = new List<int>();
            foreach (int i in new int[] { BOARD_SIZE + 2, BOARD_SIZE - 2, BOARD_SIZE * 2 - 1, BOARD_SIZE * 2 + 1 })
            {
                mvs.Add(i);
                mvs.Add(-i);
            }

            foreach (int mv in mvs)
            {
                Case obj = Cases.Where(c => c.Id == sender.Case.Id + mv && Math.Abs(c.y - sender.Case.y) < 3 && Math.Abs(c.x - sender.Case.x) < 3).FirstOrDefault();
                if (obj != null)
                {
                    if(obj.Piece == null || obj.Piece.Color != sender.Color) res.Add(obj);
                }
            }
            showMoves(sender, res);
        }

        private void CalculPawn(Pawn sender)
        {
            int mv = sender.Color ? 1 : -1;
            List<Case> res = new List<Case>();
            Case obj = null;

            obj = Cases.Where(c => c.Id == sender.Case.Id + BOARD_SIZE * mv).FirstOrDefault();
            if (obj != null && obj.Piece == null)
            {   
                res.Add(obj);

                if (!sender.AlreadyMoved)
                {
                    obj = Cases.Where(c => c.Id == sender.Case.Id + BOARD_SIZE * (mv * 2)).FirstOrDefault();
                    if (obj != null && obj.Piece == null) res.Add(obj);
                }
            }

            foreach (int i in new int[] { 1, -1})
            {
                obj = Cases.Where(c => c.Id == sender.Case.Id + BOARD_SIZE * mv + i && c.y == sender.Case.y+ mv).FirstOrDefault();
                if(obj != null && (obj.Piece != null && obj.Piece.Color != sender.Color)) 
                    res.Add(obj);
            }

            showMoves(sender, res);
        }

        private List<Case> CalculLine(Piece sender)
        {
            List<Case> res = new List<Case>();
            var mvs = new int[] { 1, BOARD_SIZE };
            mvs = new int[] { mvs[0], -mvs[0], mvs[1], -mvs[1] };

            foreach (int mv in mvs)
            {
                for (int i = sender.Case.Id + mv; i >= 0 && i <= BOARD_SIZE * BOARD_SIZE; i += mv)
                {
                    Case obj = Cases.Where(c => c.Id == i && (c.y == sender.Case.y || c.x == sender.Case.x)).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.Piece == null || obj.Piece.Color != sender.Color) res.Add(obj);
                        if (obj.Piece != null) break;
                    }
                }
            }
            return res;
        }

        private List<Case> CalculDiagonal(Piece sender)
        {
            List<Case> res = new List<Case>();
            var mvs = new int[] { BOARD_SIZE - 1, BOARD_SIZE + 1 };
            mvs = new int[] { mvs[0], -mvs[0], mvs[1], -mvs[1] };
            foreach (int mv in mvs)
            {
                Case last = sender.Case;
                for (int i = sender.Case.Id + mv; i >= 0 && i <= BOARD_SIZE * BOARD_SIZE; i += mv)
                {
                    Case obj = Cases.Where(c => c.Id == i).FirstOrDefault();

                    if (obj != null)
                    {

                        if (Math.Abs(last.x - obj.x) == 1 && Math.Abs(last.y - obj.y) == 1)
                        {
                            if (obj.Piece == null || obj.Piece.Color != sender.Color)
                            {
                                res.Add(obj);
                                last = obj;
                            }
                        }
                        if (obj.Piece != null) break;
                    }
                }
            }
            return res;
        }
    }

    public class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }
    }
}
