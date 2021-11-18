using chess.ChessBoardGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chess.AI
{
    public class Move
    {
        public Move(Piece piece, Case @case)
        {
            Case = @case;
            Piece = piece;
        }

        public Case Case { get; private set; }
        public Piece Piece { get; private set; }

    }
    public enum PromotionPieces
    {
        Knight,
        Rook,
        Bishop,
        Queen
    }
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
            game.onPromotion += Game_onPromotion;
        }

        private async void Game_onPromotion(object? sender, EventArgs e)
        {
            if (((PromotionArgs)e).Turn != Color) return;
            Task task = new Task(() =>
            {
                //Thread.Sleep(200);
                PromotionPieces promotion = Promotion(((PromotionArgs)e).Piece);
                UIPiece uIPiece = null;
                App.Current.Dispatcher.Invoke(() =>
                {
                    switch (promotion)
                    {
                        case PromotionPieces.Knight:
                            uIPiece = new UIKnight(Color);
                            break;
                        case PromotionPieces.Rook:
                            uIPiece = new UIRook(Color);
                            break;
                        case PromotionPieces.Bishop:
                            uIPiece = new UIBishop(Color);
                            break;
                        case PromotionPieces.Queen:
                            uIPiece = new UIQueen(Color);
                            break;
                    }
                    if (uIPiece != null) ((PromotionArgs)e).Callback(uIPiece, ((PromotionArgs)e).Piece);

                });

            });
            task.Start();

            await task;
        }

        private async void Game_onTurnChanged(object? sender, EventArgs e)
        {
            if (((ChangeTurn)e).Turn != Color) return;
            Task task = new Task(() =>
            {
                Thread.Sleep(50);
                Move mv = Play();
                if (mv != null)MovePieces(mv);
            });
            task.Start();

            await task;
        }
        protected List<Case> GetMoves(Piece piece)
        {
            if (piece.Color != Color) return new List<Case>();
            return Game.GetMoves(piece);
        }
        private bool MovePieces(Move move)
        {
            if (move.Piece.Color != Color) return false;
            return Game.played(move.Piece, move.Case);
        }
        protected abstract Move Play();
        protected abstract PromotionPieces Promotion(Piece piece);
    }
}
