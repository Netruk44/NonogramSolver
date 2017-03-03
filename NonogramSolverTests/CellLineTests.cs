using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramSolverLib;

namespace NonogramSolverTests
{
    [TestClass]
    public class CellLineTests
    {
        [TestMethod]
        public void CellAccessedThroughRowAndColumnAreTheSameAsThroughTheIndexer()
        {
            const int width = 7;
            const int column = 5;

            const int height = 3;
            const int row = 1;

            Nonogram n = new Nonogram(width, height);

            Cell c = n[column, row];

            Assert.AreSame(c, n.Row(row)[column]);
            Assert.AreSame(c, n.Column(column)[row]);
        }

        [TestMethod]
        public void ModifyingARowCellLineModifiesTheNonogramCell()
        {
            const int width = 5;
            const int column = 3;

            const int height = 5;
            const int row = 3;

            Nonogram n = new Nonogram(width, height);

            Cell c = n[column, row];

            n.Row(row)[column].State = Cell.CellState.Filled;

            Assert.AreEqual(Cell.CellState.Filled, c.State);
        }

        [TestMethod]
        public void ModifyingAColumnCellLineModifiesTheNonogramCell()
        {
            const int width = 5;
            const int column = 3;

            const int height = 5;
            const int row = 3;

            Nonogram n = new Nonogram(width, height);

            Cell c = n[column, row];

            n.Column(column)[row].State = Cell.CellState.Filled;

            Assert.AreEqual(Cell.CellState.Filled, c.State);
        }

        [TestMethod]
        public void CellLineCountsAddUpToTheLength()
        {
            const int width = 5;
            const int height = 5;

            Nonogram n = new Nonogram(width, height);
            var CellStateValues = (Cell.CellState[])Enum.GetValues(typeof(Cell.CellState));
            int count = 0;

            // 'randomize' the values
            foreach (var c in n.Row(0))
            {
                c.State = CellStateValues[(count++ % CellStateValues.Length)];
            }

            int unknownCount = n.Row(0).UnknownCount;
            int filledCount = n.Row(0).FilledCount;
            int blankCount = n.Row(0).BlankCount;

            Assert.AreEqual(width, unknownCount + filledCount + blankCount);
        }
    }
}
