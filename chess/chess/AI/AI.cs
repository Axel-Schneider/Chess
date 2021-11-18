using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chess.AI
{
    public abstract class ArtificalInteligence
    {
        private Board Game;
        public bool Color { get; private set; }
        public List<Piece> MyPieces
        {
            get { return Game.Pieces.Where(p => p.Color == Color).ToList(); }
        }
        public List<Piece> Enemys
        {
            get { return Game.Pieces.Where(p => p.Color != Color).ToList(); }
        }
        public ArtificalInteligence(Board game, bool color)
        {
            this.Game = game;
            Color = color;

            game.onTurnChanged += Game_onTurnChanged;
        }

        private async void Game_onTurnChanged(object? sender, EventArgs e)
        {
            if (((ChangeTurn)e).Turn != Color) return;
            Task task = new Task(() => Play());
            task.Start();

            await task;
        }
        protected List<Case> GetMoves(Piece piece)
        {
            if (piece.Color != Color) return new List<Case>();
            return Game.GetMoves(piece);
        }
        protected bool MovePieces(Piece piece, Case @case)
        {
            if (piece.Color != Color) return false;
            return Game.played(piece, @case);
        }
        protected abstract void Play();
    }

    public class RandomBot : ArtificalInteligence
    {
        public RandomBot(Board game, bool color) : base(game, color)
        {

        }
        protected override void Play()
        {
            Random rdm = new Random();
            List<Piece> myPieces = MyPieces;
        cases:
            if (myPieces.Count <= 0) return;
            Piece piece = myPieces[rdm.Next(myPieces.Count - 1)];
            List<Case> cases = GetMoves(piece);
            if (cases.Count <= 0)
            {
                myPieces.Remove(piece);
                goto cases;
            }
            Case @case = cases[rdm.Next(cases.Count - 1)];

            MovePieces(piece, @case);
        }
    }
}
