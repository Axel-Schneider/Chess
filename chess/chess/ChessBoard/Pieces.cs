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
    public abstract class Piece : Image, ICloneable
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
        public Case Simulation { get; set; }
        public List<Case> Moved { get; set; }

        protected int moveCount = 0;
        public void move(Case @case)
        {
            if (Parent != null) ((Grid)Parent).Children.Clear();
            this.Case = @case;
            moveCount++;
        }
        public void simulation(Case @case)
        {
            Simulation = Case;
            Case = @case;
        }
        public Piece(bool color) : base()
        {
            Color = color;
            Id = count;
            count++;
        }
        protected void setImage(string ImageUri)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(ImageUri);
            bitmap.EndInit();

            Source = bitmap;
        }
        public void Delete()
        {
            Case = null;
            Simulation = null;
            IsAlive = false;
        }
        public abstract object Clone();
    }

    public class Pawn : Piece
    {
        public Pawn(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Pawn(color));
        }

        public override object Clone()
        {
            Pawn r = new Pawn(Color)
            {
                Case = this.Case,
                Simulation = this.Case,
                moveCount = this.moveCount,
                Id = this.Id
            };
            Case.Simulation = r;
            return r;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Bishop(color));
        }
        public override object Clone()
        {
            Bishop r = new Bishop(Color)
            {
                Case = this.Case,
                moveCount = this.moveCount,
                Simulation = this.Case,
                Id = this.Id
            };
            Case.Simulation = r;
            return r;
        }
    }

    public class Knight : Piece
    {
        public Knight(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Knight(color));
        }
        public override object Clone()
        {
            Knight r = new Knight(Color)
            {
                Case = this.Case,
                moveCount = this.moveCount,
                Simulation = this.Case,
                Id = this.Id
            };
            Case.Simulation = r;
            return r;
        }
    }

    public class Rook : Piece
    {
        public Rook(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Rook(color));
        }
        public override object Clone()
        {
            Rook r = new Rook(Color)
            {
                Case = this.Case,
                moveCount = this.moveCount,
                Simulation = this.Case,
                Id = this.Id
            };
            Case.Simulation = r;
            return r;
        }
    }

    public class Queen : Piece
    {
        public Queen(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Queen(color));
        }
        public override object Clone()
        {
            Queen r = new Queen(Color)
            {
                Case = this.Case,
                Simulation = this.Case,
                moveCount = this.moveCount,
                Id = this.Id
            };
            Case.Simulation = r;
            return r;
        }
    }

    public class King : Piece
    {
        public King(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.King(color));
        }
        public override object Clone()
        {
            King r = new King(Color)
            {
                Case = this.Case,
                Simulation = this.Case,
                moveCount = this.moveCount,
                Id = this.Id
            };
            Case.Simulation = r;
            return r;
        }
    }
}
