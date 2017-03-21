using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolverLib.Techniques
{
    public class OverlapTechnique : INonogramTechnique
    {
        // Determines if any particular group of cells must overlap with itself
        // for both the min and max case
        /*public void Apply(CellLine line)
        {
            int length = line.Length;
            int minimumWidth = line.Hints.Sum() + line.Hints.Count - 1;
            int remainder = length - minimumWidth;
            
            if (remainder >= line.Hints.Max())
            {
                return;
            }
            
            int offset = 0;
            for (int group = 0; group < line.Hints.Count; group++)
            {
                int numberKnown = Math.Max(line.Hints[group] - remainder, 0);
                int numberUnknown = line.Hints[group] - numberKnown;
                offset += numberUnknown;

                for (int i = 0; i < numberKnown; i++)
                {
                    line[offset + i].State = Cell.CellState.Filled;
                }

                // Special case: hints give a complete picture of the line
                // I.E. remainder = 0, there are no unknown cells
                // We can fill in the rest of the 'unknowns' with Blanks.
                if (remainder == 0 && group + 1 != line.Hints.Count)
                {
                    line[offset + numberKnown].State = Cell.CellState.Blank;
                }

                offset += numberKnown + 1;
            }
        }*/

        public void Apply(CellLine line)
        {
            var min = line.Min();
            var max = line.Max();

            if (min == null || max == null)
            {
                // Min or Max was given conflicting information and cannot be filled.
                // Can't do anything, so return now.
                return;
            }

            for (int i = 0; i < line.Length; i++)
            {
                var minCell = min[i];
                var maxCell = max[i];

                if (minCell == maxCell)
                {
                    line[i].State = minCell.State;
                }
            }
        }
    }
}
