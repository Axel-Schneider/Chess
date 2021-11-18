using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public partial class MainWindow
    {
        private bool AI = true;
        private bool DoubleAI = false;

        private void InitAI()
        {
            if (AI)
            {
                ArtificalInteligence ai = new RandomBot(board.BoardChess, false);
                if (DoubleAI)
                {
                    ArtificalInteligence ai2 = new RandomBot(board.BoardChess, true);
                }
            }
        }
    }
}
