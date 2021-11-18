using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace chess.ChessBoardGUI
{
    public abstract class UIPiece : Grid
    {
        public Piece PieceChess { get; protected set; }
        public UIPiece(bool color) : base()
        { }
        public SvgViewbox Image { get; private set; }
        protected void setImage(string ImageUri)
        {
            Image = new SvgViewbox()
            {
                Margin = new Thickness(10, 10, 10, 10),
                Source = new Uri(ImageUri),
            };

            Children.Add(Image);
        }
    }
    public class UIPawn : UIPiece
    {
        public UIPawn(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Pawn(color));
            PieceChess = new Pawn(color, this);
        }
    }
    public class UIBishop : UIPiece
    {
        public UIBishop(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Bishop(color));
            PieceChess = new Bishop(color, this);
        }
    }
    public class UIKnight : UIPiece
    {
        public UIKnight(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Knight(color));
            PieceChess = new Knight(color, this);
        }
    }
    public class UIRook : UIPiece
    {
        public UIRook(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Rook(color));
            PieceChess = new Rook(color, this);
        }
    }
    public class UIQueen : UIPiece
    {
        public UIQueen(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.Queen(color));
            PieceChess = new Queen(color, this);
        }
    }
    public class UIKing : UIPiece
    {
        public UIKing(bool color) : base(color)
        {
            setImage(GraphicPath.Pieces.King(color));
            PieceChess = new King(color, this);
        }
    }
}
