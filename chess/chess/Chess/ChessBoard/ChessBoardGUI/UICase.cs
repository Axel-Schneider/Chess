﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace chess.ChessBoardGUI
{
    class UICase : Border
    {
        private Grid main = new Grid();
        public int x
        {
            get { return CaseChess.x; }
            set { CaseChess.x = value; }
        }
        public int y
        {
            get { return CaseChess.y; }
            set { CaseChess.y = value; }
        }
        public Case CaseChess { get; private set; }
        public UICase(int id) : base()
        {
            CaseChess = new Case(id);
            CaseChess.onPiecesChanged += CaseChess_onPiecesChanged;
            CaseChess.onAddChild += CaseChess_onAddChild;

            Child = main;
        }

        private void CaseChess_onAddChild(object? sender, EventArgs e)
        {
            main.Children.Add(((ChildArgs)e).Element);
        }

        private void CaseChess_onPiecesChanged(object? sender, EventArgs e)
        {
            main.Children.Clear();
            if(CaseChess.Piece != null)
            {
                main.Children.Add(CaseChess.Piece.UIPiece);
            }
        }
    }
}
