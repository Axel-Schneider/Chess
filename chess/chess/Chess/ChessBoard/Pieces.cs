using chess.ChessBoardGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chess
{
    public abstract class Piece : ICloneable
    {
        public int Id { get; protected set; }
        public static int count { get; protected set; }
        public bool Color { get; protected set; }
        public bool IsAlive { get; protected set; } = true;
        public bool AlreadyMoved
        {
            get
            {
                return moveCount > 1;
            }
        }
        public Case Case { get; protected set; }
        public Case realCase { get; set; }
        public List<Case> Moved { get; set; }
        public UIPiece UIPiece { get; protected set; }

        protected int moveCount = 0;
        public void move(Case @case)
        {
            this.Case = @case;
            moveCount++;
        }
        public Piece(bool color, UIPiece uiPiece) : base()
        {
            Color = color;
            Id = count;
            count++;
            UIPiece = uiPiece;
        }
        public void Delete()
        {
            Case = null;
            realCase = null;
            IsAlive = false;
        }
        public abstract object Clone();


        public void SimulateMove(Case @case)
        {
            realCase = Case;
            Case = @case;
        }

        public void returnToRealCase()
        {
            Case = realCase;
            realCase = null;
        }
    }

    public class Pawn : Piece
    {
        public Pawn(bool color, UIPawn uIPawn) : base(color, uIPawn) { }

        public override object Clone()
        {
            Pawn r = new Pawn(Color, null)
            {
                Case = this.Case,
                realCase = this.realCase,
                moveCount = this.moveCount,
                Id = this.Id
            };
            return r;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(bool color, UIBishop uIBishop) : base(color, uIBishop) { }
        public override object Clone()
        {
            Bishop r = new Bishop(Color, null)
            {
                Case = this.Case,
                moveCount = this.moveCount,
                realCase = this.realCase,
                Id = this.Id
            };
            return r;
        }
    }

    public class Knight : Piece
    {
        public Knight(bool color, UIKnight uIKnight) : base(color, uIKnight) { }
        public override object Clone()
        {
            Knight r = new Knight(Color, null)
            {
                Case = this.Case,
                moveCount = this.moveCount,
                realCase = this.realCase,
                Id = this.Id
            };
            return r;
        }
    }

    public class Rook : Piece
    {
        public Rook(bool color, UIRook uIRook) : base(color, uIRook) { }
        public override object Clone()
        {
            Rook r = new Rook(Color, null)
            {
                Case = this.Case,
                moveCount = this.moveCount,
                realCase = this.realCase,
                Id = this.Id
            };
            return r;
        }
    }

    public class Queen : Piece
    {
        public Queen(bool color, UIQueen uIQueen) : base(color, uIQueen) { }
        public override object Clone()
        {
            Queen r = new Queen(Color, null)
            {
                Case = this.Case,
                realCase = this.Case,
                moveCount = this.moveCount,
                Id = this.Id
            };
            return r;
        }
    }

    public class King : Piece
    {
        public King(bool color, UIKing uIKing) : base(color, uIKing) { }
        public override object Clone()
        {
            King r = new King(Color, null)
            {
                Case = this.Case,
                realCase = this.Case,
                moveCount = this.moveCount,
                Id = this.Id
            };
            return r;
        }
    }
}
