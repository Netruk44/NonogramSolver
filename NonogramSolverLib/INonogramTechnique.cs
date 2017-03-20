using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolverLib
{
    public class NonogramTechniqueAttribute : System.Attribute
    {

    }

    [NonogramTechnique]
    interface INonogramTechnique
    {
        /// <summary>
        /// Apply the technique to the given cell line.
        /// </summary>
        /// <param name="line">The line to apply the technique to.</param>
        void Apply(CellLine line);
    }
}
