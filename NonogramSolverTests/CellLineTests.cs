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
        public void CellLineClonesReferToTheSameCells()
        {
            const int width = 7;
            const int height = 3;

            Nonogram n = new Nonogram(width, height);

            var first = n.Row(0);
            var second = first.Clone();

            Assert.AreSame(first[0], second[0]);
        }

        [TestMethod]
        public void CellLineDeepClonesReferToDifferentCells()
        {
            const int width = 7;
            const int height = 3;

            Nonogram n = new Nonogram(width, height);

            var first = n.Row(0);
            var second = first.DeepClone();

            Assert.AreNotSame(first[0], second[0]);
        }

        [TestMethod]
        public void ModifyingACloneColumnCellLineModifiesTheNonogramCell()
        {
            const int width = 5;
            const int column = 3;

            const int height = 5;
            const int row = 3;

            Nonogram n = new Nonogram(width, height);

            var original = n.Column(column);
            var clone = original.Clone();

            clone[row].State = Cell.CellState.Filled;

            Assert.AreEqual(original[row].State, clone[row].State);
        }

        [TestMethod]
        public void ModifyingADeepCloneColumnCellLineDoesNotModifyTheNonogramCell()
        {
            const int width = 5;
            const int column = 3;

            const int height = 5;
            const int row = 3;

            Nonogram n = new Nonogram(width, height);

            var original = n.Column(column);
            var clone = original.DeepClone();

            clone[row].State = Cell.CellState.Filled;

            Assert.AreNotEqual(original[row].State, clone[row].State);
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

        [TestMethod]
        public void CellLineIsCompleteIsFalseWhenAllUnknown()
        {
            Nonogram n = new Nonogram(5, 5);

            Assert.IsFalse(n.Row(0).isComplete);
        }

        [TestMethod]
        public void CellLineIsCompleteIsTrueWhenAllFilled()
        {
            Nonogram n = new Nonogram(5, 5);
            CellLine l = n.Row(0);

            foreach (var cell in l)
            {
                cell.State = Cell.CellState.Filled;
            }

            Assert.IsTrue(n.Row(0).isComplete);
        }

        [TestMethod]
        public void CellLineIsCompleteIsTrueWhenAllBlank()
        {
            Nonogram n = new Nonogram(5, 5);
            CellLine l = n.Row(0);

            foreach (var cell in l)
            {
                cell.State = Cell.CellState.Blank;
            }

            Assert.IsTrue(n.Row(0).isComplete);
        }

        [TestMethod]
        public void CellLineIsCompleteIsTrueWhenAllNotUnknown()
        {
            Nonogram n = new Nonogram(5, 5);
            CellLine l = n.Row(0);
            bool setToFilled = false;

            foreach (var cell in l)
            {
                cell.State = setToFilled ? Cell.CellState.Filled : Cell.CellState.Blank;
                setToFilled = !setToFilled;
            }

            Assert.IsTrue(n.Row(0).isComplete);
        }

        [TestMethod]
        public void CellLineIsCompleteIsFalseWithOneUnknown()
        {
            Nonogram n = new Nonogram(5, 5);
            CellLine l = n.Row(0);
            bool setToUnknown = false;

            foreach (var cell in l)
            {
                cell.State = setToUnknown ? Cell.CellState.Unknown : Cell.CellState.Filled;
                setToUnknown = true;
            }

            Assert.IsFalse(n.Row(0).isComplete);
        }

        [TestMethod]
        public void ReversingACellLineReversesItsContents()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            l[0].State = Cell.CellState.Blank;
            l.Reverse();

            Assert.AreEqual(Cell.CellState.Blank, l[4].State);
        }

        [TestMethod]
        public void FillingALineWithStateFillsTheEntireLineWithThatStateAndReturnsTrue()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            Assert.IsTrue(l.Fill(Cell.CellState.Filled));

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(Cell.CellState.Filled, l[i].State);
            }
        }

        [TestMethod]
        public void FillingALineThatHasStateWithStateFillsTheEntireLineWithThatStateAndReturnsFalse()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            l[0].State = Cell.CellState.Blank;

            Assert.IsFalse(l.Fill(Cell.CellState.Filled));

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(Cell.CellState.Filled, l[i].State);
            }
        }

        [TestMethod]
        public void FillingPartOfALineWithStateFillsTheSectionWithThatStateAndReturnsTrue()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            Assert.IsTrue(l.Fill(1, 2, Cell.CellState.Filled));

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expectedStates[i], l[i].State);
            }
        }

        [TestMethod]
        public void FillingPartOfALineThatHasStateInThatPartWithStateFillsTheSectionWithThatStateAndReturnsFalse()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            l[2].State = Cell.CellState.Blank;

            Assert.IsFalse(l.Fill(1, 2, Cell.CellState.Filled));

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expectedStates[i], l[i].State);
            }
        }

        [TestMethod]
        public void FillingPartOfALineThatHasStateOutsideThatPartWithStateFillsTheSectionWithThatStateAndReturnsTrue()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            l[0].State = Cell.CellState.Blank;

            Assert.IsTrue(l.Fill(1, 2, Cell.CellState.Filled));

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expectedStates[i], l[i].State);
            }
        }

        [TestMethod]
        public void FillingPartOfALineWithStateAndFlagFillsTheSectionWithThatStateAndFlagAndReturnsTrue()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            Assert.IsTrue(l.Fill(1, 2, Cell.CellState.Filled, 42));

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            var expectedFlags = new int[5] {
                0,
                42,
                42,
                0,
                0
            };

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expectedStates[i], l[i].State);
                Assert.AreEqual(expectedFlags[i], l[i].Flag);
            }
        }

        [TestMethod]
        public void FillingPartOfALineThatHasStateInThatPartWithStateAndFlagFillsTheSectionWithThatStateAndFlagAndReturnsFalse()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            l[2].State = Cell.CellState.Blank;

            Assert.IsFalse(l.Fill(1, 2, Cell.CellState.Filled, 42));

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Unknown,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            var expectedFlags = new int[5] {
                0,
                42,
                42,
                0,
                0
            };

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expectedStates[i], l[i].State);
                Assert.AreEqual(expectedFlags[i], l[i].Flag);
            }
        }

        [TestMethod]
        public void FillingPartOfALineThatHasStateOutsideThatPartWithStateAndFlagFillsTheSectionWithThatStateAndFlagAndReturnsFalse()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            l[0].State = Cell.CellState.Blank;

            Assert.IsTrue(l.Fill(1, 2, Cell.CellState.Filled, 42));

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Unknown,
                Cell.CellState.Unknown
            };

            var expectedFlags = new int[5] {
                0,
                42,
                42,
                0,
                0
            };

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(expectedStates[i], l[i].State);
                Assert.AreEqual(expectedFlags[i], l[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinDoesNotModifyOriginalLine()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 1 });

            var min = l.Min();

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreNotSame(l[i], min[i]);
                Assert.AreEqual(Cell.CellState.Unknown, l[i].State);
                Assert.AreEqual(0, l[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxDoesNotModifyOriginalLine()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 1 });

            var max = l.Max();

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreNotSame(l[i], max[i]);
                Assert.AreEqual(Cell.CellState.Unknown, l[i].State);
                Assert.AreEqual(0, l[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinFunctionsWithAllUnknownCells_5_1_1()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 1 });

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Blank
            };

            var expectedFlags = new int[5] {
                0,
                0,
                1,
                1,
                2
            };

            var min = l.Min();

            Assert.IsNotNull(min);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], min[i].State);
                Assert.AreEqual(expectedFlags[i], min[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinFunctionsWithAllUnknownCells_5_1_2()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 2 });

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank
            };

            var expectedFlags = new int[5] {
                0,
                0,
                1,
                1,
                1
            };

            var min = l.Min();

            Assert.IsNotNull(min);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], min[i].State);
                Assert.AreEqual(expectedFlags[i], min[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinFunctionsWithAllUnknownCells_5_2_1()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 2, 1 });

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank
            };

            var expectedFlags = new int[5] {
                0,
                0,
                0,
                1,
                1
            };

            var min = l.Min();

            Assert.IsNotNull(min);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], min[i].State);
                Assert.AreEqual(expectedFlags[i], min[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinFunctionsWithSomeBlockedCells()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 2 });
            l[2].State = Cell.CellState.Blank;

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Blank, // forced blank
                Cell.CellState.Filled,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                0,
                1,
                1,
                1
            };

            var min = l.Min();

            Assert.IsNotNull(min);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], min[i].State);
                Assert.AreEqual(expectedFlags[i], min[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinReturnsNullWhenNotPossibleToFill()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 2, 2 });
            l[1].State = Cell.CellState.Blank;

            var min = l.Min();

            Assert.IsNull(min);
        }

        [TestMethod]
        public void CellLineMinReturnsNullWhenGivenConflictingInformation()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3 });
            l[0].State = Cell.CellState.Filled;
            l[4].State = Cell.CellState.Filled;

            var min = l.Min();

            Assert.IsNull(min);
        }

        [TestMethod]
        public void CellLineMinFunctionsWithSomeKnownCells_5_3_first()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3 });
            l[0].State = Cell.CellState.Filled;

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Blank
            };

            var expectedFlags = new int[5] {
                0,
                0,
                0,
                0,
                1
            };

            var min = l.Min();

            Assert.IsNotNull(min);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], min[i].State);
                Assert.AreEqual(expectedFlags[i], min[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMinFunctionsWithSomeKnownCells_5_3_last()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3 });
            l[4].State = Cell.CellState.Filled;

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                1,
                0,
                0,
                0
            };

            var min = l.Min();

            Assert.IsNotNull(min);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], min[i].State);
                Assert.AreEqual(expectedFlags[i], min[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxFunctionsWithAllUnknownCells_5_1_1()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 1 });

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                1,
                0,
                2,
                1
            };

            var max = l.Max();

            Assert.IsNotNull(max);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], max[i].State);
                Assert.AreEqual(expectedFlags[i], max[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxFunctionsWithAllUnknownCells_5_1_2()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 1, 2 });

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                0,
                1,
                1,
                1
            };

            var max = l.Max();

            Assert.IsNotNull(max);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], max[i].State);
                Assert.AreEqual(expectedFlags[i], max[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxFunctionsWithAllUnknownCells_5_2_1()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 2, 1 });

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                0,
                0,
                1,
                1
            };

            var max = l.Max();

            Assert.IsNotNull(max);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], max[i].State);
                Assert.AreEqual(expectedFlags[i], max[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxFunctionsWithSomeBlockedCells()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 2, 1 });
            l[2].State = Cell.CellState.Blank;

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank, // forced blank
                Cell.CellState.Blank,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                0,
                0,
                1,
                1
            };

            var max = l.Max();

            Assert.IsNotNull(max);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], max[i].State);
                Assert.AreEqual(expectedFlags[i], max[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxReturnsNullWhenNotPossibleToFill()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 2, 2 });
            l[1].State = Cell.CellState.Blank;

            var max = l.Max();

            Assert.IsNull(max);
        }

        [TestMethod]
        public void CellLineMaxReturnsNullWhenGivenConflictingInformation()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3 });
            l[0].State = Cell.CellState.Filled;
            l[4].State = Cell.CellState.Filled;

            var max = l.Max();

            Assert.IsNull(max);
        }

        [TestMethod]
        public void CellLineMaxFunctionsWithSomeKnownCells_5_3_first()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3 });
            l[0].State = Cell.CellState.Filled;

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Blank,
                Cell.CellState.Blank
            };

            var expectedFlags = new int[5] {
                0,
                0,
                0,
                0,
                1
            };

            var max = l.Max();

            Assert.IsNotNull(max);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], max[i].State);
                Assert.AreEqual(expectedFlags[i], max[i].Flag);
            }
        }

        [TestMethod]
        public void CellLineMaxFunctionsWithSomeKnownCells_5_3_last()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3 });
            l[4].State = Cell.CellState.Filled;

            var expectedStates = new Cell.CellState[5] {
                Cell.CellState.Blank,
                Cell.CellState.Blank,
                Cell.CellState.Filled,
                Cell.CellState.Filled,
                Cell.CellState.Filled
            };

            var expectedFlags = new int[5] {
                0,
                1,
                0,
                0,
                0
            };

            var max = l.Max();

            Assert.IsNotNull(max);

            for (int i = 0; i < 5; ++i)
            {
                Assert.AreEqual(expectedStates[i], max[i].State);
                Assert.AreEqual(expectedFlags[i], max[i].Flag);
            }
        }

        [TestMethod]
        public void DistinctGroupsReturnsZeroWithAllUnknowns()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            Assert.AreEqual(0, l.DistinctGroups());
        }

        [TestMethod]
        public void DistinctGroupsReturnsZeroWithAllBlank()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(Cell.CellState.Blank);

            Assert.AreEqual(0, l.DistinctGroups());
        }

        [TestMethod]
        public void DistinctGroupsReturnsOneWithAllFilled()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(Cell.CellState.Filled);

            Assert.AreEqual(1, l.DistinctGroups());
        }

        [TestMethod]
        public void DistinctGroupsReturnsActualNumberOfGroupsWithTwoGroupsOfTwo()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(Cell.CellState.Filled);
            l[2].State = Cell.CellState.Blank;

            Assert.AreEqual(2, l.DistinctGroups());
        }

        [TestMethod]
        public void DistinctGroupsReturnsActualNumberOfGroupsWithThreeGroupsOfOne()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l[0].State = l[2].State = l[4].State = Cell.CellState.Filled;
            l[1].State = l[3].State = Cell.CellState.Blank;

            Assert.AreEqual(3, l.DistinctGroups());
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenAllUnknown()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);

            Assert.AreEqual(true, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenHintMatchesContents_Hint5_Contents5()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(Cell.CellState.Filled);
            l.Hints.Add(5);

            Assert.AreEqual(true, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenHintMatchesContents_Hint0_Contents0()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(Cell.CellState.Blank);

            Assert.AreEqual(true, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenHintMatchesContents_Hint111_Contents111()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l[0].State = l[2].State = l[4].State = Cell.CellState.Filled;
            l[1].State = l[3].State = Cell.CellState.Blank;
            l.Hints.AddRange(new int[] { 1, 1, 1 });

            Assert.AreEqual(true, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenTooManyGroups_Hint11_Contents111()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l[0].State = l[2].State = l[4].State = Cell.CellState.Filled;
            l[1].State = l[3].State = Cell.CellState.Blank;
            l.Hints.AddRange(new int[] { 1, 1 });

            Assert.AreEqual(false, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNotEnoughGroups_Hint111_Contents11()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l[0].State = l[2].State = Cell.CellState.Filled;
            l[1].State = l[3].State = Cell.CellState.Blank;
            l.Hints.AddRange(new int[] { 1, 1, 1 });

            Assert.AreEqual(true, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenGroupIsTooLarge_Hint3_Contents4_left()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(0, 4, Cell.CellState.Filled);
            l[4].State = Cell.CellState.Blank;
            l.Hints.AddRange(new int[] { 3 });

            Assert.AreEqual(false, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenGroupIsTooLarge_Hint3_Contents4_right()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l[0].State = Cell.CellState.Blank;
            l.Fill(1, 4, Cell.CellState.Filled);
            l.Hints.AddRange(new int[] { 3 });

            Assert.AreEqual(false, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenHintIsNotPossible_Hint32()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Hints.AddRange(new int[] { 3, 2 });

            Assert.AreEqual(false, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenNoPossibleSolutionsExist()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(0, 3, Cell.CellState.Filled);
            l[4].State = Cell.CellState.Filled;
            l.Hints.AddRange(new int[] { 3 });

            Assert.AreEqual(false, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenPossibleSolutionsExist()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(0, 3, Cell.CellState.Blank);
            l[4].State = Cell.CellState.Blank;
            l.Hints.AddRange(new int[] { 1 });

            Assert.AreEqual(true, l.IsValid());
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenHintsExistButAllBlank()
        {
            Nonogram n = new Nonogram(5, 1);
            CellLine l = n.Row(0);
            l.Fill(Cell.CellState.Blank);
            l.Hints.AddRange(new int[] { 1 });

            Assert.AreEqual(false, l.IsValid());
        }
    }
}
