using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramSolverLib;

namespace NonogramSolverTests
{
    [TestClass]
    public class NonogramSolverTests
    {
        [TestMethod]
        public void OneByOneNonogramWithNoHintsIsSolvable()
        {
            var n = new Nonogram(1, 1);
            var s = new NonogramSolver();

            Assert.IsTrue(s.Solve(n));

            Assert.AreEqual(Cell.CellState.Blank, n[0, 0].State);
        }

        [TestMethod]
        public void OneByOneNonogramWithHintsIsSolvable()
        {
            var hHints = new List<int>[] { new List<int>(new int[] { 1 }) };
            var vHints = new List<int>[] { new List<int>(new int[] { 1 }) };
            var n = new Nonogram(1, 1, hHints, vHints);
            var s = new NonogramSolver();

            Assert.IsTrue(s.Solve(n));

            Assert.AreEqual(Cell.CellState.Filled, n[0, 0].State);
        }

        [TestMethod]
        public void TwoByOneNonogramWithHintsIsSolvable()
        {
            var hHints = new List<int>[] { new List<int>(new int[] { 1 }) };
            var vHints = new List<int>[] {
                new List<int>(new int[] { 1 }),
                new List<int>()
            };

            var n = new Nonogram(1, 2, hHints, vHints);
            var s = new NonogramSolver();

            Assert.IsTrue(s.Solve(n));

            Assert.AreEqual(Cell.CellState.Filled, n[0, 0].State);
            Assert.AreEqual(Cell.CellState.Blank, n[0, 1].State);
        }

        [TestMethod]
        public void TwoByTwoNonogramWithNotEnoughHintsIsNotSolvable()
        {
            var hHints = new List<int>[] {
                new List<int>(new int[] { 1 }),
                new List<int>(new int[] { 1 })
            };
            var vHints = new List<int>[] {
                new List<int>(new int[] { 1 }),
                new List<int>(new int[] { 1 })
            };

            var n = new Nonogram(2, 2, hHints, vHints);
            var s = new NonogramSolver();

            Assert.IsFalse(s.Solve(n));
        }

        [TestMethod]
        public void SimpleNonogramWithHintsIsSolvable()
        {
            var hHints = new List<int>[] {
                new List<int>(new int[] { 2 }),
                new List<int>(new int[] { 4 }),
                new List<int>(new int[] { 5 }),
                new List<int>(new int[] { 4 }),
                new List<int>(new int[] { 2 }),
            };
            var vHints = new List<int>[] {
                new List<int>(new int[] { 3 }),
                new List<int>(new int[] { 5 }),
                new List<int>(new int[] { 5 }),
                new List<int>(new int[] { 3 }),
                new List<int>(new int[] { 1 }),
            };

            var n = new Nonogram(5, 5, hHints, vHints);
            var s = new NonogramSolver();

            Assert.IsTrue(s.Solve(n));

            Assert.AreEqual(n[0, 0].State, Cell.CellState.Blank);
            Assert.AreEqual(n[1, 0].State, Cell.CellState.Filled);
            Assert.AreEqual(n[2, 0].State, Cell.CellState.Filled);
            Assert.AreEqual(n[3, 0].State, Cell.CellState.Filled);
            Assert.AreEqual(n[4, 0].State, Cell.CellState.Blank);

            Assert.AreEqual(n[0, 1].State, Cell.CellState.Filled);
            Assert.AreEqual(n[1, 1].State, Cell.CellState.Filled);
            Assert.AreEqual(n[2, 1].State, Cell.CellState.Filled);
            Assert.AreEqual(n[3, 1].State, Cell.CellState.Filled);
            Assert.AreEqual(n[4, 1].State, Cell.CellState.Filled);

            Assert.AreEqual(n[0, 2].State, Cell.CellState.Filled);
            Assert.AreEqual(n[1, 2].State, Cell.CellState.Filled);
            Assert.AreEqual(n[2, 2].State, Cell.CellState.Filled);
            Assert.AreEqual(n[3, 2].State, Cell.CellState.Filled);
            Assert.AreEqual(n[4, 2].State, Cell.CellState.Filled);

            Assert.AreEqual(n[0, 3].State, Cell.CellState.Blank);
            Assert.AreEqual(n[1, 3].State, Cell.CellState.Filled);
            Assert.AreEqual(n[2, 3].State, Cell.CellState.Filled);
            Assert.AreEqual(n[3, 3].State, Cell.CellState.Filled);
            Assert.AreEqual(n[4, 3].State, Cell.CellState.Blank);

            Assert.AreEqual(n[0, 4].State, Cell.CellState.Blank);
            Assert.AreEqual(n[1, 4].State, Cell.CellState.Blank);
            Assert.AreEqual(n[2, 4].State, Cell.CellState.Filled);
            Assert.AreEqual(n[3, 4].State, Cell.CellState.Blank);
            Assert.AreEqual(n[4, 4].State, Cell.CellState.Blank);
        }

        [TestMethod]
        public void SimpleNonogramWithHintsIsSolvable_NFP()
        {
            string picture = @"
_XXX_
XXXXX
XXXXX
_XXX_
__X__
";
            var n = NonogramFromPicture(picture);
            var s = new NonogramSolver();

            Assert.IsTrue(s.Solve(n));

            EnsureNonogramIsSameAsPicture(n, picture);
        }

        [TestMethod]
        public void HardestNonogramWithHintsIsSolvable()
        {
            var horizontalHints = new List<int>[]
            {
                new int[] { 10 }.ToList(),
                new int[] { 11, 1 }.ToList(),
                new int[] { 13, 9 }.ToList(),
                new int[] { 33 }.ToList(),
                new int[] { 35 }.ToList(),
                new int[] { 7, 27 }.ToList(),
                new int[] { 5, 6, 8 }.ToList(),
                new int[] { 5, 2, 1, 7 }.ToList(),
                new int[] { 4, 2, 2 ,7 }.ToList(),
                new int[] { 5, 3, 2, 2, 3 }.ToList(),
                new int[] { 6, 2, 3, 2, 3 }.ToList(),
                new int[] { 7, 1, 3, 2, 2, 2, 2 }.ToList(),
                new int[] { 7, 1, 3, 1, 2, 1, 2, 1 }.ToList(),
                new int[] { 8, 1, 3, 1, 2, 1 }.ToList(),
                new int[] { 8, 1, 2, 1, 1, 1 }.ToList(),
                new int[] { 8, 1, 11, 1, 1, 1 }.ToList(),
                new int[] { 8, 1, 3, 1, 2, 1 }.ToList(),
                new int[] { 8, 3, 1, 3, 1, 2, 1 }.ToList(),
                new int[] { 7, 4, 2, 2, 2, 1 }.ToList(),
                new int[] { 3, 1, 3, 3, 2, 2 }.ToList(),
                new int[] { 3, 3, 2, 1, 2 }.ToList(),
                new int[] { 3, 2, 2, 2 }.ToList(),
                new int[] { 3, 2, 1, 2 }.ToList(),
                new int[] { 3, 2 }.ToList(),
                new int[] { 4, 3 }.ToList(),
                new int[] { 3, 4, 7 }.ToList(),
                new int[] { 4, 26 }.ToList(),
                new int[] { 12, 9 }.ToList(),
                new int[] { 10, 1 }.ToList(),
                new int[] { 10 }.ToList(),
            };

            var verticalHints = new List<int>[]
            {
                new int[] { 13 }.ToList(),
                new int[] { 21 }.ToList(),
                new int[] { 24 }.ToList(),
                new int[] { 17, 6 }.ToList(),
                new int[] { 17, 4 }.ToList(),
                new int[] { 7, 10, 3 }.ToList(),
                new int[] { 5, 8, 2 }.ToList(),
                new int[] { 5, 5, 2 }.ToList(),
                new int[] { 4, 2 }.ToList(),
                new int[] { 4, 3 }.ToList(),
                new int[] { 5, 3 }.ToList(),
                new int[] { 5, 3 }.ToList(),
                new int[] { 6, 8, 4 }.ToList(),
                new int[] { 6, 2, 4 }.ToList(),
                new int[] { 6, 2, 4, 4 }.ToList(),
                new int[] { 7, 4, 5, 2, 1 }.ToList(),
                new int[] { 1, 10, 4, 1, 1 }.ToList(),
                new int[] { 1, 7, 1, 1 }.ToList(),
                new int[] { 1, 3, 3, 1, 3, 1, 1 }.ToList(),
                new int[] { 1, 3, 4, 1, 4, 1, 1 }.ToList(),
                new int[] { 1, 3, 4, 1, 4, 1, 1 }.ToList(),
                new int[] { 1, 4, 1, 2, 1 }.ToList(),
                new int[] { 1, 4, 1, 2, 1 }.ToList(),
                new int[] { 1, 4, 1, 2, 1 }.ToList(),
                new int[] { 6, 1, 4 }.ToList(),
                new int[] { 4, 1, 2 }.ToList(),
                new int[] { 4, 3, 3, 2 }.ToList(),
                new int[] { 4, 8, 2 }.ToList(),
                new int[] { 4, 7, 2 }.ToList(),
                new int[] { 5, 2, 3 }.ToList(),
                new int[] { 5, 1, 2 }.ToList(),
                new int[] { 16, 2 }.ToList(),
                new int[] { 9, 2 }.ToList(),
                new int[] { 6, 6, 2 }.ToList(),
                new int[] { 6, 2, 2, 3 }.ToList(),
                new int[] { 5, 4 }.ToList(),
                new int[] { 5, 4 }.ToList(),
                new int[] { 3, 2 }.ToList(),
                new int[] { 3, 2 }.ToList(),
                new int[] { 10 }.ToList(),
            };

            Assert.AreEqual(horizontalHints.Length, 30);
            Assert.AreEqual(verticalHints.Length, 40);

            var n = new Nonogram(30, 40, horizontalHints, verticalHints);
            var s = new NonogramSolver();

            Assert.IsTrue(s.Solve(n));
        }

        /*public void Test()
        {
            var horizontalHints = new int[][]
            {
                new int[] { },
            };
        }*/

        private Nonogram NonogramFromPicture(string picture)
        {
            // X = filled
            // _ = Blank
            picture = picture.Trim();
            var lines = picture.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Generate hints
            var horizontalHints = new List<int>[lines[0].Length];
            var verticalHints = new List<int>[lines.Length];

            // Vertical Hints
            for(int y = 0; y < lines.Length; y++)
            {
                bool inGroup = false;
                int curGroupSize = 0;
                verticalHints[y] = new List<int>();

                foreach (var c in lines[y])
                {
                    if (inGroup)
                    {
                        if (c == '_')
                        {
                            inGroup = false;
                            verticalHints[y].Add(curGroupSize);
                            continue;
                        }

                        ++curGroupSize;
                    }
                    else
                    {
                        if (c == 'X')
                        {
                            inGroup = true;
                            curGroupSize = 1;
                        }
                    }
                }

                if (inGroup)
                {
                    verticalHints[y].Add(curGroupSize);
                }
            }

            // Horizontal Hints
            for (int x = 0; x < lines[0].Length; x++)
            {
                bool inGroup = false;
                int curGroupSize = 0;
                horizontalHints[x] = new List<int>();

                for(int y = 0; y < lines.Length; y++)
                {
                    if (inGroup)
                    {
                        if (lines[y][x] == '_')
                        {
                            inGroup = false;
                            horizontalHints[x].Add(curGroupSize);
                            continue;
                        }

                        ++curGroupSize;
                    }
                    else
                    {
                        if (lines[y][x] == 'X')
                        {
                            inGroup = true;
                            curGroupSize = 1;
                        }
                    }
                }

                if (inGroup)
                {
                    horizontalHints[x].Add(curGroupSize);
                }
            }

            return new Nonogram(lines[0].Length, lines.Length, horizontalHints, verticalHints);
        }

        private void EnsureNonogramIsSameAsPicture(Nonogram n, string picture)
        {
            picture = picture.Trim();
            var lines = picture.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            Assert.AreEqual(lines.Length, n.height);
            Assert.AreEqual(lines[0].Length, n.width);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    Cell.CellState expected = lines[y][x] == 'X' ? Cell.CellState.Filled : Cell.CellState.Blank;

                    Assert.AreEqual(expected, n[x, y].State);
                }
            }
        }
    }
}
