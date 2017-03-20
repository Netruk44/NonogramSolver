using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolverLib
{
    public class Nonogram
    {
        private Cell[][] cells;
        public int width { get; }
        public int height { get; }

        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "An out of range exception is expected when accessing an array-like object")]
        public Cell this[int x, int y]
        {
            get
            {
                if (x >= width)
                {
                    throw new IndexOutOfRangeException($"x: {x} is greater than width of nonogram ({width})");
                }
                if (y >= height)
                {
                    throw new IndexOutOfRangeException($"x: {y} is greater than height of nonogram ({height})");
                }
                return cells[y][x];
            }
        }

        public Nonogram(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new Cell[height][];

            for (int y = 0; y < height; y++)
            {
                cells[y] = new Cell[width];

                for (int x = 0; x < width; x++)
                {
                    cells[y][x] = new Cell();
                }
            }
        }

        public CellLine Row(int index)
        {
            return new CellLine(getRow(index));
        }

        public CellLine Column(int index)
        {
            return new CellLine(getColumn(index));
        }

        private IEnumerable<Cell> getRow(int index)
        {
            if (index > height)
            {
                throw new IndexOutOfRangeException($"Row {index} is greater than height of nonogram ({height})");
            }

            for (int x = 0; x < width; x++)
            {
                yield return cells[index][x];
            }
        }

        private IEnumerable<Cell> getColumn(int index)
        {
            if (index > width)
            {
                throw new IndexOutOfRangeException($"Column {index} is greater than width of nonogram ({height})");
            }

            for (int y = 0; y < height; y++)
            {
                yield return cells[y][index];
            }
        }
    }
}
