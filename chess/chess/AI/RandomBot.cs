using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess.AI
{
    public class RandomBot : ArtificalInteligence
    {
        public RandomBot(Board game, bool color) : base(game, color)
        {

        }
        protected override Move Play()
        {
            Random rdm = new Random();
            List<Piece> myPieces = MyPieces;
        cases:
            if (myPieces.Count <= 0) return null;
            Piece piece = myPieces[rdm.Next(myPieces.Count - 1)];
            List<Case> cases = GetMoves(piece);
            if (cases.Count <= 0)
            {
                myPieces.Remove(piece);
                goto cases;
            }
            Case @case = cases[rdm.Next(cases.Count - 1)];

            return new Move(piece, @case);
        }

        protected override PromotionPieces Promotion(Piece piece)
        {
            Random _R = new Random();
            var v = Enum.GetValues(typeof(PromotionPieces));
            return (PromotionPieces)v.GetValue(_R.Next(v.Length));
        }
    }
}
