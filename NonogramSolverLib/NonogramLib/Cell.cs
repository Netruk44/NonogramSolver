using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NonogramSolverLib
{
    [DebuggerDisplay("{State} ({Flag})")]
    public class Cell
    {
        public enum CellState
        {
            Unknown,  // Currently unknown
            Filled,   // Definitely filled
            Blank     // Definitely blank
        }

        public CellState State;
        public int Flag; // Used for temporary storage for techniques.

        public Cell()
        {
            State = CellState.Unknown;
            ClearFlag();
        }

        public Cell(Cell c)
        {
            State = c.State;
            ClearFlag();
        }

        public void ClearFlag()
        {
            Flag = 0;
        }

        public static bool operator ==(Cell c1, Cell c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Cell c1, Cell c2)
        {
            return !c1.Equals(c2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Cell))
            {
                return false;
            }

            return Equals((Cell)obj);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Compares two cells, using both State and Flag.
        /// </summary>
        /// <param name="other">The cell to compare this cell with</param>
        /// <returns>Whether or not the two cells are the same</returns>
        public bool Equals(Cell other)
        {
            return State == other.State && Flag == other.Flag;
        }
    }
}
