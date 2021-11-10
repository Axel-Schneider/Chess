using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace chess
{
    public class ChildArgs : EventArgs
    {
        public UIElement Element;

        public ChildArgs(UIElement element)
        {
            this.Element = element;
        }
    }
    public class Case : ICloneable
    {
        public event EventHandler onPiecesChanged;
        public event EventHandler onAddChild;

        public int Id { get; set; }
        public Piece? Piece { get; private set; }
        public Piece? realPiece { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Case(int id) : base()
        {
            Id = id;
        }
        public void addChild(UIElement element)
        {
            onAddChild.Invoke(this, new ChildArgs(element));
        }
        public void AddPiece(Piece piece)
        {
            RemovePiece();
            Piece = piece;
            Piece.move(this);
            onPiecesChanged.Invoke(this, null);
        }
        public void SimulateNewPiece(Piece piece)
        {
            realPiece = Piece;
            Piece = piece;
        }

        public void returnToRealPiece()
        {
            Piece = realPiece;
            realPiece = null;
        }

        public void RemovePiece()
        {
            Piece = null;
            onPiecesChanged.Invoke(this, null);
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
        public string GetCaseName()
        {
            return convertIntToString(x) + (y+1).ToString();
        }

        private static string[] alphabetArray = { string.Empty, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static IEnumerable<string> alphaList = alphabetArray.Cast<string>();

        public static string convertIntToString(int value)
        {
            while (alphaList.Count() - 1 < value)
            {
                IncreaseList();
            }

            return alphaList.ElementAt(value);
        }

        private static void IncreaseList()
        {
            alphaList = alphabetArray.Take(1).Union(
                alphaList.SelectMany(currentLetter =>
                   alphabetArray.Skip(1).Select(innerLetter => currentLetter + innerLetter)
                )
            );
        }
    }
}