using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralGeneticEvo_AmbHerbCarn.Entidades
{
    public class Individuo
    {
        private int gridSize;
        private int currentState;
        private int goalState;
        private List<int> validActions;
        public double[,] qTable;

        public Individuo(int _gridSize)
        {
            this.gridSize = _gridSize;
            this.currentState = 0;
            this.goalState = gridSize * gridSize - 1;
            this.validActions = new List<int> { -gridSize, 1, gridSize, -1 }; // Up, Right, Down, Left

            this.qTable = new double[gridSize, validActions.Count];
            InitializeQTable();
        }

        public (int, double) TakeAction(int action, RepresentacaoAmbiente ambiente)
        {
            int nextState = currentState + validActions[action];
            if (nextState >= 0 && nextState < ambiente.gridSize)
            {
                currentState = nextState;
            }

            double reward = IsTerminalState(currentState, ambiente) ? 1.0 : 0.0;
            return (currentState, reward);
        }

        public bool IsTerminalState(int state, RepresentacaoAmbiente ambiente)
        {
            if (ambiente.grid[state].Item2 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void InitializeQTable()
        {
            Random rand = new Random();
            for (int state = 0; state < gridSize; state++)
            {
                for (int action = 0; action < validActions.Count; action++)
                {
                    qTable[state, action] = 0;
                }
            }
        }
    }
}
