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
    }
}
