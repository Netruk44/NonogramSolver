using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NonogramSolverLib;

namespace NonogramVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = ReadNonogramFromFile(args[0]);
            var s = new NonogramSolver();

            Console.WriteLine("Starting position:");
            PrintNonogram(n);
            Console.WriteLine();
            Console.WriteLine();

            int currentStep = 0;

            while (s.SolveStep(n))
            {
                ++currentStep;
                Console.WriteLine($"Step {currentStep}:");
                PrintNonogram(n);
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("Can not make any more solve steps.");
            Console.WriteLine($"Solved: {n.IsComplete()}");
        }

        static Nonogram ReadNonogramFromFile(string filename)
        {
            var hintFile = File.ReadAllLines(filename);
            var strHorizontalHints = hintFile[0].Split(',');
            var strVerticalHints = hintFile[1].Split(',');

            var horizontalHints = new List<int>[strHorizontalHints.Length];
            var verticalHints = new List<int>[strVerticalHints.Length];

            for (int i = 0; i < horizontalHints.Length; i++)
            {
                horizontalHints[i] = new List<int>(
                    strHorizontalHints[i].Split(' ')
                        .Select(singleHint => int.Parse(singleHint))
                );
            }

            for (int i = 0; i < verticalHints.Length; i++)
            {
                verticalHints[i] = new List<int>(
                    strVerticalHints[i].Split(' ')
                        .Select(singleHint => int.Parse(singleHint))
                );
            }

            return new Nonogram(horizontalHints.Length, verticalHints.Length, horizontalHints, verticalHints);
        }

        static void PrintNonogram(Nonogram n)
        {
            var charMapping = new Dictionary<Cell.CellState, char>();
            charMapping[Cell.CellState.Blank] = ' ';
            charMapping[Cell.CellState.Filled] = 'X';
            charMapping[Cell.CellState.Unknown] = '?';

            var maxVertHintCount = n.VerticalHints.Max(x => x.Count);
            var maxHorizHintCount = n.HorizontalHints.Max(x => x.Count);

            // Print vertical hints
            for (int y = 0; y < maxHorizHintCount; y++)
            {
                for (int i = 0; i < maxVertHintCount; i++)
                {
                    Console.Write(" ");
                }

                for (int x = 0; x < n.width; x++)
                {
                    var curCol = n.HorizontalHints[x];

                    var index = curCol.Count - maxHorizHintCount + y;

                    if (index < 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        // mod 10 since it needs to fit into a 1x1 box.
                        Console.Write(curCol[index] % 10);
                    }
                }

                Console.WriteLine();
            }

            for (int y = 0; y < n.height; y++)
            {
                // Print horizontal hints
                for (int x = 0; x < maxVertHintCount; x++)
                {
                    var curCol = n.VerticalHints[y];
                    var index = curCol.Count - maxVertHintCount + x;

                    if (index < 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(curCol[index] % 10);
                    }
                }

                for (int x = 0; x < n.width; x++)
                {
                    Console.Write(charMapping[n[x, y].State]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
