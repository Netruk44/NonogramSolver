using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolverLib
{
    public class Cell
    {
        public enum CellState
        {
            Unknown,  // Currently unknown
            Filled,   // Definitely filled
            Blank     // Definitely blank
        }

        public CellState State;

        public Cell()
        {
            State = CellState.Unknown;
        }
    }
}
