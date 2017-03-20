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
            // Returns no information when given hints that don't overlap
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
    }
}
