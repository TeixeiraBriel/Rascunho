using RedeNeuralGeneticEvo_AmbHerbCarn.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralGeneticEvo_AmbHerbCarn
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Inicia Ambiente
            RepresentacaoAmbiente ambiente = new RepresentacaoAmbiente(25);
            Individuo ind = new Individuo(ambiente.gridSize);

            imprimeAmbiente(ambiente);
            Console.ReadLine();
        }

        static void imprimeAmbiente(RepresentacaoAmbiente ambiente)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"0/{ambiente.grid[1 * i].Item2}  0/{ambiente.grid[2 * i].Item2}  0/{ambiente.grid[3 * i].Item2}  0/{ambiente.grid[4 * i].Item2}  0/{ambiente.grid[5 * i].Item2}");
            }
        }


        static int ChooseAction(double[,] qTable, int state, Random random)
        {
            var chanceExploracao = random.NextDouble();
            if (chanceExploracao < 0.2) // Epsilon-greedy com epsilon de 0,2
                return random.Next(4); // Existem 4 ações: Up -5, Right +1, Down +5, Left -1
            else
                return ArgMax(qTable, state);
        }

        static int ArgMax(double[,] qTable, int state)
        {
            int bestAction = 0;
            double maxQValue = qTable[state, 0];

            for (int action = 1; action < 4; action++)
            {
                if (qTable[state, action] > maxQValue)
                {
                    bestAction = action;
                    maxQValue = qTable[state, action];
                }
            }

            return bestAction;
        }

        static double MaxQValue(double[,] qTable, int state)
        {
            double maxQValue = qTable[state, 0];

            for (int action = 1; action < 4; action++)
            {
                if (qTable[state, action] > maxQValue)
                {
                    maxQValue = qTable[state, action];
                }
            }

            return maxQValue;
        }
    }
}
