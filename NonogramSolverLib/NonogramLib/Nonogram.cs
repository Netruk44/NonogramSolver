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

        public List<int>[] HorizontalHints;
        public List<int>[] VerticalHints;

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
            : this(width, height, CreateEmptyHintList(width), CreateEmptyHintList(height))
        { }

        public Nonogram(int width, int height, List<int>[] horizontalHints, List<int>[] verticalHints)
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

            HorizontalHints = horizontalHints;
            VerticalHints = verticalHints;
        }

        public CellLine Row(int index)
        {
            return new CellLine(getRow(index), VerticalHints[index]);
        }

        public CellLine Column(int index)
        {
            return new CellLine(getColumn(index), HorizontalHints[index]);
        }

        public bool Validate()
        {
            // Validate there are enough hints for all rows/columns
            if (HorizontalHints.Length != width)
            {
                return false;
            }
            if (VerticalHints.Length == height)
            {
                return false;
            }

            // Validate that the hints themselves add up
            // Example:
            // width = 5
            // hint: {2, 2}
            // Solution is possible: XX_XX - width of 5
            //
            // width = 5
            // hint: {2, 1, 2}
            // Solution is not possible: XX_X_XX - width of 7
            foreach (var hint in HorizontalHints)
            {
                if (hint.Sum() + hint.Count - 1 > width)
                {
                    return false;
                }
            }

            foreach (var hint in VerticalHints)
            {
                if (hint.Sum() + hint.Count - 1 > height)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsComplete()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (cells[y][x].State == Cell.CellState.Unknown)
                    {
                        return false;
                    }
                }
            }

            return true;
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

        private static List<int>[] CreateEmptyHintList(int length)
        {
            var ret = new List<int>[length];

            for (int i = 0; i < length; i++)
            {
                ret[i] = new List<int>();
            }

            return ret;
        }
    }
}
