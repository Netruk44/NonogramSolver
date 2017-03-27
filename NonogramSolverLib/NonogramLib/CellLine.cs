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

        public CellLine DeepClone()
        {
            // Deep copy of cells
            var source = cells;
            var newCells = new Cell[source.Length];

            for (int i = 0; i < cells.Length; i++)
            {
                newCells[i] = new Cell(source[i]);
            }

            var ret = new CellLine(newCells, Hints);
            ret.ClearFlags();
            return ret;
        }

        public void ClearFlags()
        {
            foreach (var cell in this)
            {
                cell.ClearFlag();
            }
        }

        public bool Fill(Cell.CellState desiredState)
        {
            return Fill(0, Length, desiredState);
        }

        public bool Fill(int startingPosition, int length, Cell.CellState desiredState)
        {
            bool ret = true;

            for (int i = startingPosition; i < startingPosition + length; i++)
            {
                if (cells[i].State != Cell.CellState.Unknown && cells[i].State != desiredState)
                {
                    ret = false;
                }

                cells[i].State = desiredState;
            }

            return ret;
        }

        public bool Fill(int startingPosition, int length, Cell.CellState desiredState, int flag)
        {
            bool ret = true;

            for (int i = startingPosition; i < startingPosition + length; i++)
            {
                if (cells[i].State != Cell.CellState.Unknown && cells[i].State != desiredState)
                {
                    ret = false;
                }

                cells[i].State = desiredState;
                cells[i].Flag = flag;
            }

            return ret;
        }

        /// <summary>
        /// Reverses the cells in this CellLine
        /// </summary>
        public void Reverse()
        {
            for (int i = 0; i < Length / 2; i++)
            {
                var temp = cells[i];
                cells[i] = cells[Length - i - 1];
                cells[Length - i - 1] = temp;
            }
        }

        /// <summary>
        /// Gets the cell line with all cells moved as far left as possible given
        /// the hints and current state of the cell line.
        /// </summary>
        /// <returns>A new cell line with all filled in cells as far left as possible.
        /// The cell's flag variable is set to the hint that it corresponds to, or
        /// in the case of a Blank cell, the flag is the number of blanks before it
        /// Null if the line is in a state that cannot be filled in.</returns>
        public CellLine Min()
        {
            var ret = DeepClone();
            int currentPosition = 0;

            for (int hint = 0; hint < ret.Hints.Count; hint++)
            {
                int length = ret.Hints[hint];
                int? opening = ret.FindNextOpening(currentPosition, length, Cell.CellState.Filled);

                if (!opening.HasValue)
                {
                    return null;
                }

                currentPosition = opening.Value;
                if (!ret.Fill(currentPosition, length, Cell.CellState.Filled, hint))
                {
                    // Something wasn't unknown. Conflicting information.
                    return null;
                }

                if (currentPosition + length < ret.Length)
                {
                    ret.cells[currentPosition + length].State = Cell.CellState.Blank;
                }

                currentPosition += length + 1;
            }

            // Fill and number the rest as blank cells
            int currentOpening = 0;
            foreach (var cell in ret)
            {
                if (cell.State == Cell.CellState.Unknown || cell.State == Cell.CellState.Blank)
                {
                    cell.State = Cell.CellState.Blank;
                    cell.Flag = currentOpening++;
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets the cell line with all cells moved as far right as possible given
        /// the hints and current state of the cell line.
        /// </summary>
        /// <returns>A new cell line with all filled in cells as far right as possible.
        /// The cell's flag variable is set to the hint that it corresponds to, or
        /// in the case of a Blank cell, the flag is the number of blanks before it
        /// Null if the line is in a state that cannot be filled in.</returns>
        public CellLine Max()
        {
            var ret = DeepClone();
            ret.Reverse();
            int currentPosition = 0;

            for (int hint = ret.Hints.Count - 1; hint >= 0; hint--)
            {
                int length = ret.Hints[hint];
                int? opening = ret.FindNextOpening(currentPosition, length, Cell.CellState.Filled);

                if (!opening.HasValue)
                {
                    return null;
                }

                currentPosition = opening.Value;
                if (!ret.Fill(currentPosition, length, Cell.CellState.Filled, hint))
                {
                    // Something wasn't unknown. Conflicting information.
                    return null;
                }

                if (currentPosition + length < ret.Length)
                {
                    ret.cells[currentPosition + length].State = Cell.CellState.Blank;
                }

                currentPosition += length + 1;
            }
            
            ret.Reverse();
            
            // Fill and number the rest as blank cells
            int currentOpening = 0;
            foreach (var cell in ret)
            {
                if (cell.State == Cell.CellState.Unknown || cell.State == Cell.CellState.Blank)
                {
                    cell.State = Cell.CellState.Blank;
                    cell.Flag = currentOpening++;
                }
            }

            return ret;
        }

        public int DistinctGroups()
        {
            int distinctGroups = 0;
            bool currentlyInGroup = false;

            for (int i = 0; i < cells.Length; i++)
            {
                if (currentlyInGroup)
                {
                    if (cells[i].State == Cell.CellState.Blank)
                    {
                        currentlyInGroup = false;
                    }
                }
                else
                {
                    if (cells[i].State == Cell.CellState.Filled)
                    {
                        currentlyInGroup = true;
                        ++distinctGroups;
                    }
                }
            }

            return distinctGroups;
        }

        public IEnumerable<CellLine> Possibilities()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].State == Cell.CellState.Unknown)
                {
                    var unknownIsFilled = DeepClone();
                    var unknownIsBlank = DeepClone();

                    unknownIsFilled[i].State = Cell.CellState.Filled;
                    unknownIsBlank[i].State = Cell.CellState.Blank;

                    foreach (var line in unknownIsBlank.Possibilities())
                    {
                        yield return line;
                    }

                    foreach (var line in unknownIsFilled.Possibilities())
                    {
                        yield return line;
                    }

                    yield break;
                }
            }

            yield return this;
        }

        public bool IsValid()
        {
            // Validate the hints
            int minimumWidth = Hints.Sum() + Hints.Count - 1;
            if (minimumWidth > cells.Length)
            {
                return false;
            }

            // Validate the number of groups
            int numGroups = DistinctGroups();
            if (numGroups > Hints.Count)
            {
                return false;
            }

            // Validate the number of filled in cells
            if (FilledCount > Hints.Sum())
            {
                return false;
            }

            // Validate there are enough unknown cells to account
            // for the rest of the hints
            if (UnknownCount < Hints.Sum() - FilledCount)
            {
                return false;
            }

            // Validate the largest group
            int largestGroupPossible;
            int currentGroupSize = 0;
            bool inGroup = false;

            if (Hints.Count != 0)
            {
                largestGroupPossible = Hints.Max();
            }
            else
            {
                largestGroupPossible = 0;
            }

            for (int i = 0; i < cells.Length; i++)
            {
                if (inGroup)
                {
                    if (cells[i].State != Cell.CellState.Filled)
                    {
                        inGroup = false;
                        continue;
                    }

                    ++currentGroupSize;

                    if (currentGroupSize > largestGroupPossible)
                    {
                        break;
                    }
                }
                else
                {
                    if (cells[i].State == Cell.CellState.Filled)
                    {
                        inGroup = true;
                        currentGroupSize = 1;
                    }
                }
            }

            if (currentGroupSize > largestGroupPossible)
            {
                return false;
            }

            return true;
        }

        public bool IsValid_BruteForce()
        {
            // TODO: Make this non-brute force, if possible.
            // O(2^Length)
            // O(😱)
            return Possibilities()
                .AsParallel()
                .Any(x => x.Recursive_IsValid());
        }

        bool Recursive_IsValid()
        {
            // Assumes no cell is unknown.

            // Validate the hints
            int minimumWidth = Hints.Sum() + Hints.Count - 1;
            if (minimumWidth > cells.Length)
            {
                return false;
            }

            // Validate the number of groups
            int numGroups = DistinctGroups();
            if (numGroups != Hints.Count)
            {
                return false;
            }

            // Validate the number of filled in cells
            if (FilledCount > Hints.Sum())
            {
                return false;
            }

            // Validate there are enough unknown cells to account
            // for the rest of the hints
            if (UnknownCount < Hints.Sum() - FilledCount)
            {
                return false;
            }

            // Validate the largest group
            int largestGroupPossible;
            int currentGroupSize = 0;
            bool inGroup = false;

            if (Hints.Count != 0)
            {
                largestGroupPossible = Hints.Max();
            }
            else
            {
                largestGroupPossible = 0;
            }
            
            for(int i = 0; i < cells.Length; i++)
            {
                if (inGroup)
                {
                    if (cells[i].State == Cell.CellState.Blank)
                    {
                        inGroup = false;
                        continue;
                    }

                    ++currentGroupSize;

                    if (currentGroupSize > largestGroupPossible)
                    {
                        break;
                    }
                }
                else
                {
                    if (cells[i].State == Cell.CellState.Filled)
                    {
                        inGroup = true;
                        currentGroupSize = 1;
                    }
                }
            }

            if (currentGroupSize > largestGroupPossible)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns whether or not the next x cells starting from a given position
        /// can be filled with desiredState without issues occurring. For example,
        /// there is a state of a different value contained within, or the given
        /// length would run off the edge of the line.
        /// </summary>
        /// <param name="position">The position to start checking from</param>
        /// <param name="length">The number of positions to check</param>
        /// <returns>Whether or not the given positions are able to be filled</returns>
        private bool IsClear(int position, int length, Cell.CellState desiredState)
        {
            for (int i = 0; i < length; i++)
            {
                int curPos = position + i;

                // Check bounds
                if (curPos >= Length || position < 0 || curPos < 0)
                {
                    return false;
                }

                // Check for non-compatible cell.
                if (cells[curPos].State != Cell.CellState.Unknown && 
                    cells[curPos].State != desiredState)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidPosition(int position, int length, Cell.CellState desiredState)
        {
            var temp = DeepClone();

            temp.Fill(position, length, desiredState);
            return temp.IsValid();
        }

        /// <summary>
        /// Finds the next index that will fit a line of cells with the given cell state
        /// in this CellLine.
        /// </summary>
        /// <param name="startingPosition">The position to start searching from.</param>
        /// <param name="length">The number of cells to look for an opening for.</param>
        /// <param name="desiredState">The state that we're looking for an opening for.</param>
        /// <returns>The next index that will fit the given cell state. Null if no opening exists.</returns>
        private int? FindNextOpening(int startingPosition, int length, Cell.CellState desiredState)
        {
            if (length == 0)
            {
                return startingPosition;
            }

            for (int i = startingPosition; i <= Length - length; i++)
            {
                if (IsClear(i, length, desiredState) && IsValidPosition(i, length, desiredState))
                {
                    return i;
                }

                // TODO: Optimization - Find the obstruction and move forward until i is past it.
            }

            return null;
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
