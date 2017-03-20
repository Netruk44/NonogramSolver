using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolverLib
{
    class NonogramSolver
    {
        private List<INonogramTechnique> techniques;

        public NonogramSolver()
        {
            this.techniques = GetTechniques().ToList();
        }

        private IEnumerable<INonogramTechnique> GetTechniques()
        {
            // TODO: Determine that this actually works.
            // TODO: Test that this actually works?
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.GetCustomAttributes(typeof(NonogramTechniqueAttribute), true).Length > 0)
                {
                    yield return (INonogramTechnique)Activator.CreateInstance(type);
                }
            }
        }
    }
}
