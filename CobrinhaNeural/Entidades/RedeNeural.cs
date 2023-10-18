using System;
using System.Threading;
using System.Threading.Tasks;

namespace CobrinhaNeural.Entidades
{
    public class RedeNeural
    {
        MainWindow context;

        //Qtable qtable;
        Cobra cobra;
        bool[,] gridComida;
        double[,,] qTable;
        int numStatesX;
        int numStatesY;
        int numActions;
        double learningRate;
        double discountFactor;
        int numEpisodes;
        int loops = 0;
        double taxaExploraca;

        (int posX, int posY) objetivo;
        int[,] estado;

        public RedeNeural(Cobra _cobra, bool[,] _gridComida, MainWindow _context)
        {
            cobra = _cobra;
            gridComida = _gridComida;
            context = _context;
        }

        public void inicializa()
        {
            learningRate = 0.1;
            discountFactor = 0.9;
            numEpisodes = 100;
            numStatesX = 10;
            numStatesY = 10;
            numActions = 3;
            taxaExploraca = 0.5;
            InitializeQTable();
        }

        private void InitializeQTable()
        {
            this.qTable = new double[numStatesX, numStatesY, numActions];
            Random rand = new Random();
            for (int x = 0; x < numStatesX; x++)
            {
                for (int y = 0; y < numStatesY; y++)
                {
                    for (int action = 0; action < numActions; action++)
                    {
                        qTable[x, y, action] = rand.NextDouble();
                        //qTable[x, y, action] = 0;
                    }
                }
            }
        }

        public void Executa()
        {
            for (int ep = 0; ep < numEpisodes; ep++)
            {
                //EXECUTA EPSODIOS
                executaEpisodio();
            }
        }

        public async Task executaEpisodio()
        {
            int posX = cobra.posX;
            int posY = cobra.posY;
            double pontos = context.pontos;

            do
            {
                int action = escolherAcao(posX, posY);
                executaAcao(action);
                double reward = pontos > context.pontos ? -0.5 : pontos == context.pontos ? 0.0 : 1.0;
                AtualizarQValue(posX, posY, action, reward, cobra.posX, cobra.posY);
                if (context.pontos == 0)
                    pontos = 0;
                loops++;
            }
            while (pontos >= context.pontos);
            pontos = context.pontos;
        }

        public int escolherAcao(int posX, int posY)
        {
            int bestAction = 0;

            var chanceExploracao = new Random().NextDouble();
            if (chanceExploracao < taxaExploraca)
            {
                bestAction = new Random().Next(3);
            }
            else
            {
                // Escolher ação com base nos valores Q
                double bestQValue = qTable[posX, posY, 0];
                for (int action = 1; action < numActions; action++)
                {
                    if (qTable[posX, posY, action] > bestQValue)
                    {
                        bestAction = action;
                        bestQValue = qTable[posX, posY, action];
                    }
                }
            }
            return bestAction;
        }

        bool validaEncontrouComida(int posX, int posY)
        {
            return gridComida[posX, posY];
        }

        private async Task executaAcao(int action)
        {
            if (action == 0) // Norte
            {
                context.executaVirarDireita();
            }
            else if (action == 1) // Sul
            {
                context.executaVirarEsquerda();
            }
            else if (action == 2) // Leste
            {
                context.executaSeguirReto();
            }  
        }

        public void AtualizarQValue(int x, int y, int action, double reward, int nextX, int nextY)
        {
            double maxNextQValue = MaxQValue(nextX, nextY);
            double currentQValue = qTable[x, y, action];
            double updatedQValue = currentQValue + learningRate * (reward + discountFactor * maxNextQValue - currentQValue);
            qTable[x, y, action] = updatedQValue;
        }

        private double MaxQValue(int x, int y)
        {
            double maxQValue = qTable[x, y, 0];
            for (int action = 1; action < numActions; action++)
            {
                if (qTable[x, y, action] > maxQValue)
                {
                    maxQValue = qTable[x, y, action];
                }
            }
            return maxQValue;
        }
    }
}
