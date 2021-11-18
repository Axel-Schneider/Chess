using chess.ChessBoardGUI;
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
    public delegate void PromotionCallback(UIPiece result, Piece source);
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
                .Case((UIBishop x) => IsBishop(x))
                .Case((UIKnight x) => IsKnight(x))
                .Case((UIRook x) => IsRook(x))
                .Case((UIQueen x) => IsQueen(x));

        }
        public void generateUI()
        {
            double imgWidth = Width / 4;
            double mTop = Height / 2 - imgWidth / 2;
            UIPiece[] pieces = new UIPiece[] {
                new UIKnight(Piece.Color),
                new UIBishop(Piece.Color),
                new UIRook(Piece.Color),
                new UIQueen(Piece.Color),
            };
            int i = 0;

            foreach (UIPiece pc in pieces)
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
        private void IsQueen(UIQueen x)
        {
            Callback(new UIQueen(Piece.Color), Piece);
        }

        private void IsRook(UIRook x)
        {
            Callback(new UIRook(Piece.Color), Piece);
        }

        private void IsKnight(UIKnight x)
        {
            Callback(new UIKnight(Piece.Color), Piece);
        }

        private void IsBishop(UIBishop x)
        {
            Callback(new UIBishop(Piece.Color), Piece);
        }
    }
}
