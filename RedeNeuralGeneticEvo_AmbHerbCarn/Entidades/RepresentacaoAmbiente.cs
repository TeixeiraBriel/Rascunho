using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralGeneticEvo_AmbHerbCarn.Entidades
{
    public class RepresentacaoAmbiente
    {
        public List<(int,int)> grid;
        public int gridSize;

        public RepresentacaoAmbiente(int gridSize)
        {
            this.gridSize = gridSize;
            reset();
        }

        public void reset()
        {
            grid = new List<(int,int)> ();

            for (int i = 0; i < gridSize; i++)
            {
                grid.Add((i, 0));
            }
        }
    }
}
