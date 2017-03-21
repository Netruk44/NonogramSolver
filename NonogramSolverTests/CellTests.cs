using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramSolverLib;

namespace NonogramSolverTests
{
    [TestClass]
    public class CellTests
    {
        [TestMethod]
        public void DefaultCellStateIsUnknown()
        {
            Cell c = new Cell();
            Assert.AreEqual(Cell.CellState.Unknown, c.State);
        }

        [TestMethod]
        public void DefaultCellFlagIsZero()
        {
            Cell c = new Cell();
            Assert.AreEqual(0, c.Flag);
        }

        [TestMethod]
        public void CellsWithSameStateAndFlagAreEqualUsingAssertAreEqual()
        {
            Cell c1 = new Cell();
            Cell c2 = new Cell();

            c1.State = c2.State = Cell.CellState.Filled;
            c1.Flag = c2.Flag = 42;

            Assert.AreEqual(c1, c2);
        }

        [TestMethod]
        public void CellsWithSameStateAndFlagAreEqualUsingAssertObjectEquals()
        {
            Cell c1 = new Cell();
            Cell c2 = new Cell();

            c1.State = c2.State = Cell.CellState.Filled;
            c1.Flag = c2.Flag = 42;

            Assert.IsTrue(c1.Equals(c2));
        }

        [TestMethod]
        public void CellsWithSameStateAndFlagAreEqualUsingEqualOperator()
        {
            Cell c1 = new Cell();
            Cell c2 = new Cell();

            c1.State = c2.State = Cell.CellState.Filled;
            c1.Flag = c2.Flag = 42;

            Assert.IsTrue(c1 == c2);
        }

        [TestMethod]
        public void CellsWithDifferentStateAndFlagAreNotEqualUsingAssertAreNotEqual()
        {
            Cell c1 = new Cell();
            Cell c2 = new Cell();

            c1.State = Cell.CellState.Blank;
            c2.State = Cell.CellState.Filled;

            c1.Flag = 4;
            c2.Flag = 42;

            Assert.AreNotEqual(c1, c2);
        }

        [TestMethod]
        public void CellsWithDifferentStateAndFlagAreNotEqualUsingAssertObjectEquals()
        {
            Cell c1 = new Cell();
            Cell c2 = new Cell();

            c1.State = Cell.CellState.Blank;
            c2.State = Cell.CellState.Filled;

            c1.Flag = 4;
            c2.Flag = 42;

            Assert.IsFalse(c1.Equals(c2));
        }

        [TestMethod]
        public void CellsWithDifferentStateAndFlagAreNotEqualUsingNotEqualOperator()
        {
            Cell c1 = new Cell();
            Cell c2 = new Cell();

            c1.State = Cell.CellState.Blank;
            c2.State = Cell.CellState.Filled;

            c1.Flag = 4;
            c2.Flag = 42;

            Assert.IsTrue(c1 != c2);
        }

        [TestMethod]
        public void CellComparedToNonCellIsNotEqualUsingObjectEquals()
        {
            Cell c = new Cell();
            int i = 0;

            Assert.IsFalse(c.Equals(i));
        }

        [TestMethod]
        public void NonCellComparedToCellIsNotEqualUsingObjectEquals()
        {
            Cell c = new Cell();
            int i = 0;

            Assert.IsFalse(i.Equals(c));
        }
    }
}
