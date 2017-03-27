using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolverLib
{
    public class NonogramSolver
    {
        private List<INonogramTechnique> techniques;

        public NonogramSolver()
        {
            techniques = new List<INonogramTechnique>();

            techniques.Add(new Techniques.OverlapTechnique());
        }

        public bool Solve(Nonogram n)
        {
            while (SolveStep(n)) ;

            return n.IsComplete();
        }

        public bool SolveStep(Nonogram n)
        {
            bool madeChanges = false;

            for (int y = 0; y < n.height; y++)
            {
                var row = n.Row(y);

                foreach (var t in techniques)
                {
                    madeChanges |= t.Apply(row);
                }
            }

            for (int x = 0; x < n.width; x++)
            {
                var col = n.Column(x);

                foreach (var t in techniques)
                {
                    madeChanges |= t.Apply(col);
                }
            }

            return madeChanges;
        }
    }
}
