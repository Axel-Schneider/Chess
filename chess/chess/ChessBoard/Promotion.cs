using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace chess
{
    public delegate void PromotionCallback(Piece result, Piece source);
    public class Promotion : Grid
    {
        private TypeSwitch ts;
        private Piece Piece;
        private PromotionCallback Callback;
        public Promotion(PromotionCallback callback, Piece piece) : base()
        {
            Piece = piece;
            Callback = callback;
            ts = new TypeSwitch()
                .Case((Bishop x) => IsBishop(x))
                .Case((Knight x) => IsKnight(x))
                .Case((Rook x) => IsRook(x))
                .Case((Queen x) => IsQueen(x));

        }
        public void generateUI()
        {
            double imgWidth = Width / 4;
            double mTop = Height / 2 - imgWidth / 2;
            Piece[] pieces = new Piece[] {
                new Knight(Piece.Color),
                new Bishop(Piece.Color),
                new Rook(Piece.Color),
                new Queen(Piece.Color),
            };
            int i = 0;

            foreach (Piece pc in pieces)
            {
                pc.Width = imgWidth;
                pc.Height = imgWidth;
                pc.Margin = new Thickness(i * imgWidth, mTop, 0, 0);
                pc.VerticalAlignment = VerticalAlignment.Top;
                pc.HorizontalAlignment = HorizontalAlignment.Left;
                pc.MouseDown += (o, e) =>
                {
                    ts.Switch(o);
                    ((Grid)Parent).Children.Remove(this);
                };
                Children.Add(pc);
                i++;
            }
        }
        private void IsQueen(Queen x)
        {
            Callback(new Queen(Piece.Color), Piece);
        }

        private void IsRook(Rook x)
        {
            Callback(new Rook(Piece.Color), Piece);
        }

        private void IsKnight(Knight x)
        {
            Callback(new Knight(Piece.Color), Piece);
        }

        private void IsBishop(Bishop x)
        {
            Callback(new Bishop(Piece.Color), Piece);
        }
    }
}
