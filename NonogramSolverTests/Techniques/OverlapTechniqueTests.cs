using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NonogramSolverLib;
using NonogramSolverLib.Techniques;

namespace NonogramSolverTests
{
    [TestClass]
    public class OverlapTechniqueTests
    {
        [TestMethod]
        public void OverlapTechniqueGivesSolutionWhenHintGivenCompleteInformation_1_1()
        {
            // Testing: Finds solution for simple problem
            // Expected Results:
            // Width: 1
            // Hint: 1
            //
            // Solution:
            // X

            const int width = 1;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 1 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Filled
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesSolutionWhenHintGivenCompleteInformation_5_2_2()
        {
            // Testing: Finds solution for odd-numbered width
            // Expected Results:
            // Width: 5
            // Hint: 2, 2
            //
            // Solution:
            // XX XX

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[]{ 2, 2 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesSolutionWhenHintGivenCompleteInformation_10_1_2_3_1()
        {
            // Testing: Finds solution for > 2 hints and larger width
            // Expected Results:
            // Width: 10
            // Hint: 1, 2, 3, 1
            //
            // Solution:
            // X XX XXX X

            const int width = 10;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 1, 2, 3, 1 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesInformationWhenHintGivenIncompleteInformation_5_1_2()
        {
            // Testing: Finds information when given incomplete hints, variation 1
            // Expected Results:
            // Width: 5
            // Hint: 1, 2
            //
            // Min/Max:
            // X XX 
            //  X XX
            //
            // Overlap:
            // ???X?

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 1, 2 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Unknown
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesInformationWhenHintGivenIncompleteInformation_5_2_1()
        {
            // Testing: Finds information when given incomplete hints, variation 2
            // Expected Results:
            // Width: 5
            // Hint: 2, 1
            //
            // Min/Max:
            // XX X?
            // ?XX X
            //
            // Overlap:
            // ?X???

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 2, 1 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesInformationWhenHintGivenIncompleteInformation_10_2_2_3()
        {
            // Testing: Odd number of hints with even width
            // Expected Results:
            // Width: 10
            // Hint: 2, 2, 3
            //
            // Min/Max:
            // XX XX XXX?
            // ?XX XX XXX
            //
            // Overlap:
            // ?X??X??XX?

            const int width = 10;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 2, 2, 3 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesInformationWhenHintGivenIncompleteInformation_15_8()
        {
            // Testing: Edge case, odd width with single filled block
            // Expected Results:
            // Width: 15
            // Hint: 8
            //
            // Min/Max:
            // XXXXXXXX???????
            // ???????XXXXXXXX
            //
            // Overlap:
            // ???????X???????

            const int width = 15;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 8 });

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueGivesNoInformationWhenHintGivenIncompleteInformation_5_1_1()
        {
            // Testing: Returns no information when given hints that don't overlap
            // Expected Results:
            // Width: 5
            // Hint: 1, 1
            //
            // Min/Max:
            // X X??
            // ??X X
            //
            // Overlap:
            // ?????

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 1, 1 });

            var t = new OverlapTechnique();
            t.Apply(line);

            // All are unknown
            Assert.IsTrue(line.All(x => x.State == Cell.CellState.Unknown));
        }

        [TestMethod]
        public void OverlapTechniqueGivesInformationWhenHintGivenIncompleteInformationAndPartialSolution_10_1_1_1_1_1()
        {
            // Testing: Finds information when a partial solution is in place
            // Expected Results:
            // Width: 10
            // Hint: 1, 1, 1, 1, 1
            // Pre-given information: ??????? ??
            //
            // Min/Max:
            // ??????? ??
            // X X X X X 
            // X X X X  X
            //
            // Overlap:
            // X X X X ??

            const int width = 10;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 1, 1, 1, 1, 1 });

            line[7].State = Cell.CellState.Blank;

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueDoesNothingWhenGivenConflictingInformation()
        {
            // Testing: Finds information when a partial solution is in place
            // Expected Results:
            // Width: 5
            // Hint: 3
            // Pre-given information: X???X

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 3 });

            line[0].State = Cell.CellState.Filled;
            line[4].State = Cell.CellState.Filled;

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Filled
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueFindsSolutionWhenGivenPartialInformation_5_3_first()
        {
            // Testing: Finds information when a partial solution is in place
            // Expected Results:
            // Width: 5
            // Hint: 3
            // Pre-given information: X????
            // Solution: XXX__

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 3 });

            line[0].State = Cell.CellState.Filled;

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Blank
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueFindsSolutionWhenGivenPartialInformation_5_3_last()
        {
            // Testing: Finds information when a partial solution is in place
            // Expected Results:
            // Width: 5
            // Hint: 3
            // Pre-given information: ????X
            // Solution: __XXX

            const int width = 5;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 3 });

            line[4].State = Cell.CellState.Filled;

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Blank,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }

        [TestMethod]
        public void OverlapTechniqueFindsSolutionForGreedyPitfall()
        {
            // Hint: 2 3 4 2
            // Given:    ???????XX???XXXX????
            // Expected: ???????XX??_XXXX_?X?
            const int width = 20;
            var n = new Nonogram(width, 1);
            var line = n.Row(0);
            line.Hints.AddRange(new int[] { 2, 3, 4, 2 });
            line[7].State =
                line[8].State =
                line[12].State =
                line[13].State =
                line[14].State =
                line[15].State =
                Cell.CellState.Filled;

            Cell.CellState[] expected = new Cell.CellState[width] {
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown,

                Cell.CellState.Unknown,
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,

                Cell.CellState.Unknown,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled,

                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Unknown
            };

            var t = new OverlapTechnique();
            t.Apply(line);

            for (int i = 0; i < width; ++i)
            {
                Assert.AreEqual(expected[i], line[i].State);
            }
        }
    }
}
