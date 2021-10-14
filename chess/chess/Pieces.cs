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
    public abstract class Piece : Image
    {
        public bool Color { get; protected set; }
        public bool AlreadyMoved
        {
            get
            {
                return moveCount > 1;
            }
        }
        public Case Case { get; private set; }
        public List<Case> Moved { get; set; }

        private int moveCount = 0;
        public void move(Case @case)
        {
            if (Parent != null) ((Grid)Parent).Children.Clear();
            this.Case = @case;
            moveCount++;
        }
        public Piece(bool color) : base()
        {
            Color = color;
        }
        protected void setImage(string ImageUri)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(ImageUri);
            bitmap.EndInit();

            Source = bitmap;
        }
    }

    public class Pawn : Piece
    {
        public Pawn(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Pawn(color));
        }

    }

    public class Bishop : Piece
    {
        public Bishop(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Bishop(color));
        }
    }

    public class Knight : Piece
    {
        public Knight(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Knight(color));
        }
    }

    public class Rook : Piece
    {
        public Rook(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Rook(color));
        }
    }

    public class Queen : Piece
    {
        public Queen(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Queen(color));
        }
    }

    public class King : Piece
    {
        public King(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.King(color));
        }
    }
}
