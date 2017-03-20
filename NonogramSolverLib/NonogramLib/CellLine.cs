using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace NonogramSolverLib
{
    public class CellLine : IEnumerable<Cell>
    {
        public int Length { get; }
        private Cell[] cells;
        public List<int> Hints { get; }

        public Cell this[int i]
        {
            get
            {
                return cells[i];
            }

            // Read only. If you want to modify a cell, use line[0].State instead.
        }

        public int UnknownCount
        {
            get
            {
                return cells.Count(x => x.State == Cell.CellState.Unknown);
            }
        }

        public int FilledCount
        {
            get
            {
                return cells.Count(x => x.State == Cell.CellState.Filled);
            }
        }

        public int BlankCount
        {
            get
            {
                return cells.Count(x => x.State == Cell.CellState.Blank);
            }
        }

        public bool isComplete
        {
            get
            {
                return !cells.Any(x => x.State == Cell.CellState.Unknown);
            }
        }

        public CellLine(IEnumerable<Cell> cells, List<int> hints)
        {
            this.cells = cells.ToArray();
            Length = this.cells.Length;
            Hints = hints;
        }

        public CellLine Clone()
        {
            return new CellLine(cells, Hints);
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return ((IEnumerable<Cell>)cells).GetEnumerator();
        }

        [ExcludeFromCodeCoverage]
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
