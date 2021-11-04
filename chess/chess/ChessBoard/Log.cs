using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess
{
    public class MoveLog
    {
        private static int count = 1;

        public int Id { get; private set; }
        public Piece Piece { get; private set; }
        public Case From { get; private set; }
        public Case To { get; private set; }
        public bool WasCheck { get; private set; }
        public bool Kill { get; private set; }

        public MoveLog(Piece piece, Case from, Case to, bool wasCheck = false, bool kill = false)
        {
            Id = count;
            Piece = piece;
            From = from;
            To = to;
            WasCheck = wasCheck;
            Kill = kill;
            count++;
        }
    }
}
