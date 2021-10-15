using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace chess
{
    public class Case : Grid, ICloneable
    {
        public int Id {  get; set; }
        public Piece? Piece {  get; private set; }
        public int x {  get; set; }
        public int y { get; set; }
        public Case(int id) : base()
        {
            Id = id;
        }
        public void AddPiece(Piece piece)
        {
            RemovePiece();
            Piece = piece;
            Piece.move(this);
            Children.Add(Piece);
        }

        public void RemovePiece()
        {
            Piece = null;
            Children.Clear();
        }
        public object Clone()
        {
            return new Case(Id)
            {
                Piece = Piece,
                x = this.x,
                y = this.y
            };
        }
    }
}
