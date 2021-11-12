using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using chess.ChessBoardGUI;

namespace chess
{
    public enum NullReason
    {
        ACCORD,
        TIMER,
    }
    public class Board
    {
        #region Events
        public event EventHandler onTurnChanged;
        public event EventHandler onGameEnded;
        public event EventHandler onShowMoves;
        public event EventHandler onUnshowMoves;
        public event EventHandler onPromotion;
        public event EventHandler onPromotionCallback;
        #endregion

        #region Constantes
        public const int BOARD_SIZE = 8;
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
        #endregion

        private void EndGame(string message)
        {
            onGameEnded.Invoke(this, new EndGame(message));
        }
        #region Propertys
        public string Pattern { get; private set; }
        public bool Turn { get; private set; } = true;

        public List<Case> Cases { get; private set; }
        public List<Piece> Pieces { get; private set; }
        public List<MoveLog> LastMovesLight { get; private set; }
        public List<MoveLog> LastMovesDark { get; private set; }
        #endregion

        #region Object
        private TypeSwitch ts;
        #endregion

        #region Constructor
        public Board(string pattern = DEFAULT_PATTERN) : base()
        {
            Pattern = pattern;
            Pieces = new List<Piece>();
            ts = new TypeSwitch()
                .Case((Pawn x) => ShowPawn(x))
                .Case((Bishop x) => ShowBishop(x))
                .Case((Knight x) => ShowKnight(x))
                .Case((Rook x) => ShowRook(x))
                .Case((Queen x) => ShowQueen(x))
                .Case((King x) => ShowKing(x));
            LastMovesLight = new();
            LastMovesDark = new();
        }
        #endregion
         
        #region Public Function

        public void Nulle(NullReason reason)
        {
            switch (reason)
            {
                case NullReason.ACCORD:
                    EndGame("ACCORD");
                    break;
                case NullReason.TIMER:
                    EndGame("NO MORE TIME");
                    break;
            }
        }
        #endregion

        #region Private Function

        #region Moves calcul
        private List<Case> CalculKing(King sender)
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
                    if (obj.Piece == null || !obj.Piece.IsAlive || obj.Piece.Color != sender.Color)
                    {
                        res.Add(obj);
                    }
                }
            }


            if (!sender.AlreadyMoved && sender.Color == Turn)
            {
                res.AddRange(GetCastleMove(sender));
            }

            return res;
        }

        private List<Case> CalculQueen(Queen sender)
        {
            List<Case> res = new List<Case>();
            res.AddRange(CalculDiagonal(sender));
            res.AddRange(CalculLine(sender));

            return res;
        }

        private List<Case> CalculRook(Rook sender)
        {
            List<Case> res = CalculLine(sender);

            return res;
        }

        private List<Case> CalculBishop(Bishop sender)
        {
            List<Case> res = CalculDiagonal(sender);

            return res;
        }

        private List<Case> CalculKnight(Knight sender)
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
                    if (obj.Piece == null || !obj.Piece.IsAlive || obj.Piece.Color != sender.Color) res.Add(obj);
                }
            }
            return res;
        }

        private List<Case> CalculPawn(Pawn sender)
        {
            int mv = sender.Color ? -1 : 1;
            List<Case> res = new List<Case>();
            Case obj = null;

            obj = Cases.Where(c => c.Id == sender.Case.Id + BOARD_SIZE * mv).FirstOrDefault();
            if (obj != null && (obj.Piece == null || !obj.Piece.IsAlive))
            {
                res.Add(obj);

                if (!sender.AlreadyMoved)
                {
                    obj = Cases.Where(c => c.Id == sender.Case.Id + BOARD_SIZE * (mv * 2)).FirstOrDefault();
                    if (obj != null && (obj.Piece == null || !obj.Piece.IsAlive)) res.Add(obj);
                }
            }

            foreach (int i in new int[] { 1, -1 })
            {
                obj = Cases.Where(c => c.Id == sender.Case.Id + BOARD_SIZE * mv + i && c.y == sender.Case.y + mv).FirstOrDefault();
                if (obj != null && (obj.Piece != null && obj.Piece.Color != sender.Color))
                    res.Add(obj);
            }

            return res;
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
                        if (obj.Piece == null || !obj.Piece.IsAlive || obj.Piece.Color != sender.Color) res.Add(obj);
                        if (obj.Piece != null && obj.Piece.IsAlive) break;
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
                            if (obj.Piece == null || !obj.Piece.IsAlive || obj.Piece.Color != sender.Color)
                            {
                                res.Add(obj);
                                last = obj;
                            }
                        }
                        if (obj.Piece != null && obj.Piece.IsAlive) break;
                    }
                }
            }
            return res;
        }

        private List<Case> CalculMoves(Piece sender)
        {
            List<Case> cases = null;
            if (sender is Pawn) cases = CalculPawn((Pawn)sender);
            else if (sender is Knight) cases = CalculKnight((Knight)sender);
            else if (sender is Bishop) cases = CalculBishop((Bishop)sender);
            else if (sender is Rook) cases = CalculRook((Rook)sender);
            else if (sender is Queen) cases = CalculQueen((Queen)sender);
            else if (sender is King) cases = CalculKing((King)sender);
            return cases;
        }

        private void CalculPosition(Piece sender)
        {
            ts.Switch(sender);
        }
        #endregion

        #region Moves show

        public void addPiece(Piece piece, Case @case)
        {
            Pieces.Add(piece);
            @case.AddPiece(piece);
        }
        public void initCases()
        {
            Cases = new List<Case>();
        }
        public void addCase(Case @case)
        {
            Cases.Add(@case);
        }

        private void showMoves(Piece piece, List<Case> cases)
        {
            if (cases.Count <= 0) return;
            List<Case> real = new List<Case>();
            foreach (Case c in cases)
            {
                if (piece.IsAlive && CanMoveTo(piece, c))
                {
                    real.Add(c);
                }
            }
            onShowMoves.Invoke(this, new ShowMoves(piece, real));
        }

        public void played(Piece piece, Case c)
        {
            King king = (King)Pieces.Where(pc => pc is King && pc.Color == Turn).FirstOrDefault();
            List<Piece> enemy = Pieces.Where(pc => pc.Color != Turn).ToList();

            bool isCheck = KingIsInCheck(king, enemy);

            if (piece is Pawn && (c.y == 0 || c.y == BOARD_SIZE - 1))
            {
                onPromotion.Invoke(this, new PromotionArgs(PawnPromotionCallback, piece));
            }

            if (piece is King && (Math.Abs(piece.Case.x - c.x) == 2))
            {
                int inc = (piece.Case.Id - c.Id > 0) ? -1 : 1;
                Rook rk = null;
                for (int i = piece.Case.Id; i >= 0 && i <= BOARD_SIZE * BOARD_SIZE; i += inc)
                {
                    Case obj = Cases.Where(c => c.Id == i).FirstOrDefault();
                    if (obj != null && obj.Piece != null && obj.Piece is Rook)
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
                    if (obj != null)
                    {
                        obj.AddPiece(rk);
                    }
                }
            }
            bool kill = false;
            Piece killed = null;
            if (c.Piece != null)
            {
                kill = true;
                killed = c.Piece;
                Pieces.Remove(c.Piece);
                c.Piece.Delete();
            }
            LogMove(piece, piece.Case, c, isCheck, kill, killed);
            piece.Case.RemovePiece();
            c.AddPiece(piece);
            ChangeTurn();
            if (IsCheckMate(Turn))
            {
                EndGame((!Turn)
                    ? $"LIGHT WIN"
                    : $"DARK WIN"
                    );
            }
            else if (stroke50())
            {
                EndGame("50 stroke rule");
            }
            else if (!AsMove(Turn))
            {
                EndGame("Stalemate");
            }
            else if (isCheck && Perpetual(!Turn))
            {
                EndGame("Three times repetition");
            }
            else if (mater())
            {
                EndGame("Mater");
            }
        }

        private void ShowKing(King sender)
        {
            List<Case> res = CalculKing(sender);
            showMoves(sender, res);
        }
        private void ShowQueen(Queen sender)
        {
            List<Case> res = CalculQueen(sender);
            showMoves(sender, res);
        }
        private void ShowRook(Rook sender)
        {
            List<Case> res = CalculRook(sender);
            showMoves(sender, res);
        }
        private void ShowBishop(Bishop sender)
        {
            List<Case> res = CalculBishop(sender);
            showMoves(sender, res);
        }
        private void ShowKnight(Knight sender)
        {
            List<Case> res = CalculKnight(sender);
            showMoves(sender, res);
        }
        private void ShowPawn(Pawn sender)
        {
            List<Case> res = CalculPawn(sender);
            showMoves(sender, res);
        }

        #endregion

        #region Simulation
        public bool CanMoveTo(Piece piece, Case GoTo)
        {
            List<Piece> smPieces = Clone.CloneList<Piece>(Pieces).Where(p => p != null && p.IsAlive).ToList();
            List<Piece> smenemy = smPieces.Where(p => p.Color != piece.Color).ToList();
            King smKing = (piece is King)
                ? (King)piece
                : (King)smPieces.Where(p => p.Color == piece.Color && p is King).FirstOrDefault();
            bool r = false;
            bool kill = false;
            if (piece == null || smKing == null) return false; 

            if (GoTo.Piece != null)
            {
                kill = true;
                smenemy.Remove(smenemy.Where(p => p.Id == GoTo.Piece.Id).FirstOrDefault());
            }
            GoTo.SimulateNewPiece(piece);
            piece.Case.SimulateNewPiece(null);
            piece.SimulateMove(GoTo);

            r = !KingIsInCheck(smKing, smenemy);

            GoTo.returnToRealPiece();
            piece.returnToRealCase();
            piece.Case.returnToRealPiece();

            return r;
        }

        #endregion

        #region Rules
        private List<Case> GetCastleMove(King sender)
        {
            List<Case> res = new List<Case>();
            List<Piece> rooks = Pieces.Where(p => p is Rook && !p.AlreadyMoved && p.Color == sender.Color && p.IsAlive).ToList();
            List<Piece> enemys = Pieces.Where(p => p.Color != sender.Color && p.IsAlive).ToList();

            foreach (Piece rook in rooks)
            {
                int inc = (sender.Case.Id > rook.Case.Id) ? -1 : 1;
                bool canCastle = true;
                for (int i = sender.Case.Id + inc; i != rook.Case.Id; i += inc)
                {
                    if (i < 0 || i > BOARD_SIZE * BOARD_SIZE) break;
                    Case obj = Cases.Where(c => c.Id == i).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.Piece != null && obj.Piece != rook)
                        {
                            canCastle = false;
                            break;
                        }
                        else
                        {
                            bool canmv = false;
                            foreach (Piece enemy in enemys)
                            {
                                List<Case> cases = CalculMoves(enemy);

                                if (cases.Contains(obj))
                                {
                                    canmv = true;
                                    break;
                                }
                                else
                                {
                                    int iddd = 0;
                                }
                            }
                            if (canmv)
                            {
                                canCastle = false;
                                break;
                            }
                        }
                    }
                }
                if (canCastle)
                {
                    Case obj = Cases.Where(c => c.Id == sender.Case.Id + inc * 2).FirstOrDefault();
                    if (obj != null && obj.Piece == null)
                    {
                        res.Add(obj);
                    }
                }
            }
            return res;
        }
        private void PawnPromotionCallback(UIPiece result, Piece source)
        {
            Pieces.Remove(source);
            Pieces.Add(result.PieceChess);
            source.Case.AddPiece(result.PieceChess);
            onPromotionCallback.Invoke(this, new PromotionCallbackArgs(result));
        }

        private bool KingIsInCheck(King king, List<Piece> enemy)
        {
            foreach (Piece piece in enemy)
            {
                if (CanKill(piece, king))
                    return true;
            }
            return false;
        }

        private bool CanKill(Piece killer, Piece victim)
        {
            List<Case> cases = CalculMoves(killer);
            List<int> csid = cases.Select(c => c.Id).ToList();

            if (cases != null && csid.Contains(victim.Case.Id))
                return true;
            return false;

        }
        private bool IsCheckMate(bool color)
        {
            List<Piece> smPieces = Clone.CloneList<Piece>(Pieces).Where(p => p != null && p.IsAlive).ToList();
            List<Piece> enemy = smPieces.Where(p => p.Color != color).ToList();
            List<Piece> Ally = smPieces.Where(p => p.Color == color).ToList();

            King smKing = (King)smPieces.Where(p => p.Color == color && p is King).FirstOrDefault();

            if (smKing == null) return false;
            if (!KingIsInCheck(smKing, enemy)) return false;

            foreach(Piece pc in Ally)
            {
                foreach(Case cs in CalculMoves(pc))
                {
                    if (CanMoveTo(pc, cs)) return false;
                }
            }

            return true;
        }

        private bool AsMove(bool turn)
        {
            foreach(Piece pc in Pieces.Where(pc => pc.Color == turn))
            {
                List<Case> cs = CalculMoves(pc).ToList();
                foreach(Case c in cs)
                {
                    if (CanMoveTo(pc, c)) return true;
                }
            }

            return false;
        }

        private bool Perpetual(bool color)
        {
            List<MoveLog> log = color ? LastMovesLight : LastMovesDark;
            if (log.Count < 4) return false;
            for(int i = 0; i < 3; i++)
            {
                if (!MoveSimilar(log[log.Count - i - 1], log[log.Count - i - 2]))
                    return false;
            }

            return true;
        }
        private bool MoveSimilar(MoveLog first, MoveLog second)
        {
            if (first.Piece.Id != second.Piece.Id) return false;
            if (first.To.Id == second.From.Id && first.From.Id == second.To.Id) return true;
            return false;
        }
        private bool stroke50()
        {
            if (LastMovesDark.Count + LastMovesLight.Count < 50) return false;
            List<MoveLog> log = LastMovesLight.Concat(LastMovesDark).ToList();
            log = log.Where(mv => mv.Id > log.Count - 50).ToList();
            foreach(MoveLog mv in log)
            {
                if (mv.Kill) return false;
                if (mv.Piece is Pawn) return false;
            }
            return true;

        }
        private bool mater()
        {
            bool res = false;
            List<Piece> light = Pieces.Where(p => p.Color == true && p is not King).ToList();
            List<Piece> dark = Pieces.Where(p => p.Color == false && p is not King).ToList();

            if (light.Count > 1 || dark.Count > 1) return false;

            if(testMater(light, dark) || testMater(dark, light))
            {
                return true;
            }

            return false;
        }
        private bool testMater(List<Piece> player1, List<Piece> player2)
        {
            if (player1.Count > 1 || player2.Count > 1) return false;

            if (player1.Count == 0)
            {
                if (player2.Count == 0) return true;
                Piece last = player2.FirstOrDefault();
                foreach(string s in Enum.GetNames(typeof(MaterPieces))){
                    string t = last.GetType().ToString();
                    if ("chess." + s == last.GetType().ToString())
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        #endregion

        #region Events
        public void Click(UIPiece sender)
        {
            if (((UIPiece)sender).PieceChess.Color == Turn)
            {
                onUnshowMoves.Invoke(this, null);
                CalculPosition(((UIPiece)sender).PieceChess);

            }
        }

        private void ChangeTurn()
        {
            Turn = !Turn;
            onTurnChanged.Invoke(this, new ChangeTurn(Turn));
        }
        #endregion

        #region Log
        private void LogMove(Piece piece, Case from, Case to, bool wasCheck, bool kill, Piece killed)
        {
            MoveLog mv = new(piece, from, to, wasCheck, kill, killed);

            if (piece.Color)
            {
                LastMovesLight.Add(mv);
            }
            else
            {
                LastMovesDark.Add(mv);
            }
        }
        #endregion

        #endregion
    }


    public class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }
    }
    static class Clone
    {
        public static List<T> CloneList<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
    public class ChangeTurn : EventArgs
    {
        public bool Turn { get; private set; }
        public ChangeTurn(bool turn) : base()
        {
            Turn = turn;
        }
    }
    public class EndGame : EventArgs
    {
        public string Message { get; private set; }
        public EndGame(string message)
        {
            Message = message;
        }
    }
    public class ShowMoves : EventArgs
    {
        public ShowMoves(Piece piece, List<Case> cases)
        {
            Piece = piece;
            Cases = cases;
        }
        public Piece Piece { get; private set; }
        public List<Case> Cases { get; private set; }
    }
    public class PromotionArgs : EventArgs
    {
        public PromotionArgs(PromotionCallback PawnPromotionCallback, Piece piece)
        {
            Piece = piece;
            Callback = PawnPromotionCallback;
        }
        public Piece Piece { get; private set; }
        public PromotionCallback Callback { get; private set; }

    }
    public class PromotionCallbackArgs : EventArgs
    {
        public PromotionCallbackArgs(UIPiece piece)
        {
            Piece = piece;
        }
        public UIPiece Piece { get; private set; }

    }
    public enum MaterPieces
    {
        Knight,
        Bishop,
    }
}
