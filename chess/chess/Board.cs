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
        public const char
            CHAR_KING = 'k',
            CHAR_QUEEN = 'q',
            CHAR_ROOK = 'r',
            CHAR_KNIGHT = 'n',
            CHAR_BISHOP = 'b',
            CHAR_PAWN = 'p',
            CHAR_LINE = '/',
            CHAR_EMPTY = ' ';
        public const string DEFAULT_PATTERN = "rnbqkbnr/pppppppp";


        public double CaseWith { get; private set; } = -1;
        public double CaseHeight { get; private set; } = -1;
        public string Pattern { get; private set; }

        public List<Case> Cases { get; private set; }
        public List<Piece> Pieces { get; private set; }

        private TypeSwitch ts;
        private List<Image> moves;
        public Board(string pattern = DEFAULT_PATTERN) : base()
        {
            Pattern = pattern;
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
                    }
                }
            }
            generatePieces();
            Border brd = new Border()
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(BOARD_BORDER, BOARD_BORDER, BOARD_BORDER, BOARD_BORDER),
            };
            Children.Add(brd);
        }
        private void generatePieces()
        {
            int i = 0;
            int y = 0;
            foreach (char c in Pattern.ToLower())
            {
                if(c == CHAR_LINE)
                {
                    y++;
                    i = (BOARD_SIZE) * y;
                    continue;
                }
                Piece black = null;
                Piece white = null;
                Case cblack = null;
                Case cwhite = null;
                int wx, wy;
                int bx, by;
                switch (c)
                {
                    case CHAR_ROOK:
                        black = new Rook(false);
                        white = new Rook(true);
                        break;
                    case CHAR_KNIGHT:
                        black = new Knight(false);
                        white = new Knight(true);
                        break;
                    case CHAR_BISHOP:
                        black = new Bishop(false);
                        white = new Bishop(true);
                        break;
                    case CHAR_QUEEN:
                        black = new Queen(false);
                        white = new Queen(true);
                        break;
                    case CHAR_KING:
                        black = new King(false);
                        white = new King(true);
                        break;
                    case CHAR_EMPTY:
                        break;
                    case CHAR_PAWN:
                    default:
                        black = new Pawn(false);
                        white = new Pawn(true);
                        break;
                }
                bx = i;
                while(bx >= BOARD_SIZE)
                {
                    bx -= BOARD_SIZE;
                }
                wx = bx;
                by = y;
                wy = BOARD_SIZE - y - 1;
                cwhite = Cases.Where(c => c.x == wx && c.y == wy).FirstOrDefault();
                cblack = Cases.Where(c => c.x == bx && c.y == by).FirstOrDefault();
                if (black != null && cblack != null) addPiece(black, cblack);
                if (white != null && cwhite != null) addPiece(white, cwhite);
                i++;
            }


        }
        private void addPiece(Piece piece, Case @case)
        {
            Pieces.Add(piece);
            @case.AddPiece(piece);
            piece.MouseDown += Piece_MouseDown;
        }
        private Brush GetBrush(int id)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GraphicPath.Cases.GetCase(id % 2 == 0));
            bitmap.EndInit();

            return new ImageBrush(bitmap);
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
                    Opacity = 0.4
                };
                c.Children.Add(img);
                moves.Add(img);
                img.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
                {
                    foreach(Image i in moves)
                    {
                        ((Grid)i.Parent).Children.Remove(i);
                    }
                    if(piece is Pawn && (piece.Case.y == 0 || piece.Case.y == BOARD_SIZE - 1) )
                    {
                        Promotion prm = new Promotion(PawnPromotionCallback, piece)
                        {
                            Width = Width,
                            Height = Height,
                            Background = new SolidColorBrush()
                            {
                                Color = Color.FromRgb(0,0,0),
                                Opacity = 0.5
                            }
                            
                        };
                        prm.generateUI();
                        Children.Add(prm);
                    }

                    if(piece is King && (Math.Abs(piece.Case.x - c.x) == 2))
                    {
                        int inc = (piece.Case.Id - c.Id > 0) ? -1 : 1;
                        Rook rk = null;
                        for(int i = piece.Case.Id; i >= 0 && i <= BOARD_SIZE*BOARD_SIZE; i += inc)
                        {
                            Case obj = Cases.Where(c => c.Id == i).FirstOrDefault();
                            if(obj != null && obj.Piece != null && obj.Piece is Rook)
                            {
                                rk = (Rook)obj.Piece;
                                break;
                            }
                        }
                        if (rk == null) return;
                        else
                        {
                            rk.Case.RemovePiece();
                            Case obj = Cases.Where(c => c.Id == piece.Case.Id + inc).FirstOrDefault();
                            if(obj != null)
                            {
                                obj.AddPiece(rk);
                            }
                        }
                    }
                    piece.Case.RemovePiece();
                    c.AddPiece(piece);
                };
            }
        }

        private void PawnPromotionCallback(Piece result, Piece source)
        {
            result.MouseDown += Piece_MouseDown;
            source.Case.AddPiece(result);
        }
        private void CalculKing(King sender)
        {
            var mvs = new List<int>();
            foreach (int i in new int[] { 1, BOARD_SIZE - 1, BOARD_SIZE, BOARD_SIZE + 1 })
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


            if (!sender.AlreadyMoved)
            {
                List<Piece> rooks = Pieces.Where(p => p is Rook && !p.AlreadyMoved && p.Color == sender.Color).ToList();
                foreach(Piece rook in rooks)
                {
                    int inc = (sender.Case.Id > rook.Case.Id) ? -1 : 1;
                    bool canCastle = true;
                    for(int i = sender.Case.Id + inc; i != rook.Case.Id; i += inc)
                    {
                        if (i < 0 || i > BOARD_SIZE * BOARD_SIZE) break;
                        Case obj = Cases.Where(c => c.Id == i).FirstOrDefault();
                        if(obj != null && obj.Piece != null)
                        {
                            canCastle = false;
                            break;
                        }
                    }
                    if (canCastle)
                    {
                        Case obj = Cases.Where(c => c.Id == sender.Case.Id + inc * 2).FirstOrDefault();
                        if(obj != null && obj.Piece == null)
                        {
                            res.Add(obj);
                        }
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
            int mv = sender.Color ? -1 : 1;
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
