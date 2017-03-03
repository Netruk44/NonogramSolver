using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramSolverLib;

namespace NonogramSolverTests
{
    [TestClass]
    public class NonogramTests
    {
        [TestMethod]
        public void NonogramSizeIsEqualToConstructedSize_Square()
        {
            const int width = 5;
            const int height = width;
            Assert.AreEqual(width, height);

            Nonogram n = new Nonogram(width, height);
            
            Assert.AreEqual(width, n.width);
            Assert.AreEqual(height, n.height);
        }

        [TestMethod]
        public void NonogramSizeIsEqualToConstructedSize_Rectangle()
        {
            const int width = 3;
            const int height = 7;
            Assert.AreNotEqual(width, height);

            Nonogram n = new Nonogram(width, height);
            
            Assert.AreEqual(width, n.width);
            Assert.AreEqual(height, n.height);
        }

        [TestMethod]
        public void AllCellsStartAsUnknown_Square()
        {
            const int width = 5;
            const int height = width;
            Assert.AreEqual(width, height);

            Nonogram n = new Nonogram(width, height);

            for (int y = 0; y < height; y++)
            {
                var row = n.Row(y);

                Assert.AreEqual(width, row.Length);
                for (int x = 0; x < width; x++)
                {
                    Assert.AreEqual(Cell.CellState.Unknown, row[x].State);
                }
            }
        }

        [TestMethod]
        public void AllCellsStartAsUnknown_Rectangle()
        {
            const int width = 3;
            const int height = 7;
            Assert.AreNotEqual(width, height);

            Nonogram n = new Nonogram(width, height);

            for (int y = 0; y < height; y++)
            {
                var row = n.Row(y);

                Assert.AreEqual(width, row.Length);
                for (int x = 0; x < width; x++)
                {
                    Assert.AreEqual(Cell.CellState.Unknown, row[x].State);
                }
            }
        }

        [TestMethod]
        public void AccessingOutOfBoundsCellHorizontallyThrowsOutOfRangeException()
        {
            Nonogram n = new Nonogram(5, 5);

            try
            {
                var c = n[7, 2];
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AccessingOutOfBoundsCellVerticallyThrowsOutOfRangeException()
        {
            Nonogram n = new Nonogram(5, 5);

            try
            {
                var c = n[2, 7];
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AccessingFarOutOfBoundsRowThrowsOutOfRangeException()
        {
            Nonogram n = new Nonogram(5, 5);

            try
            {
                var r = n.Row(7);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AccessingOnePastOutOfBoundsRowThrowsOutOfRangeException()
        {
            Nonogram n = new Nonogram(5, 5);

            try
            {
                var r = n.Row(5);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AccessingFarOutOfBoundsColumnThrowsOutOfRangeException()
        {
            Nonogram n = new Nonogram(5, 5);

            try
            {
                var r = n.Column(7);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AccessingOnePastOutOfBoundsColumnThrowsOutOfRangeException()
        {
            Nonogram n = new Nonogram(5, 5);

            try
            {
                var r = n.Column(5);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
