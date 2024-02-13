using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RedeNEural.Redes
{
    public class TerceiraRedeClass
    {

        // Definição do labirinto
        private int gridSize = 3; // Distancia de Visão
        private int numStatesX = 5; // Dimensão X
        private int numStatesY = 5; // Dimensão Y
        private int numActionsVision = 0;//Numero de ações visão
        private int numActions = 4; // Norte, Sul, Leste, Oeste
        private int visionSize = 3; // Tamanho da visão local
        private int[,] stateValues;

        // Outros parâmetros
        private double learningRate = 0.1;
        private double discountFactor = 0.9;
        private double explorationProbability = 0.2;

        // Tabela Q
        private double[,,] qTable;
        private double[,] qTableVisao;


        public TerceiraRedeClass(int numStatesX, int numStatesY, int numActions, int visionSize)
        {
            this.numStatesX = numStatesX;
            this.numStatesY = numStatesY;
            this.numActions = numActions;
            this.visionSize = visionSize;
            this.qTable = new double[numStatesX, numStatesY, numActions];
            InitializeQTable();
            InitializeStateValues();
        }

        public static void execute()
        {
            int numStatesX = 5;
            int numStatesY = 5;
            int numActions = 4;
            int visionSize = 3; // Tamanho da visão local
            TerceiraRedeClass qLearning = new TerceiraRedeClass(numStatesX, numStatesY, numActions, visionSize);

            // Treinar o agente
            qLearning.Train(100);
            qLearning.obterValoresQ();
        }

        private void InitializeQTable()
        {
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

            numActionsVision = (gridSize * gridSize) - 1;
            qTableVisao = new double[gridSize * gridSize,  numActionsVision];
        }

        private void InitializeStateValues()
        {
            stateValues = new int[numStatesX, numStatesY];
            for (int x = 0; x < numStatesX; x++)
            {
                for (int y = 0; y < numStatesY; y++)
                {
                    stateValues[x, y] = 0;
                }
            }
        }

        public int ChooseAction(int x, int y)
        {
            Random rand = new Random();
            bool visao = false;

            if (rand.NextDouble() < explorationProbability)
            {
                // Ação aleatória para exploração
                return rand.Next(numActions);
            }
            else
            {
                int bestAction = 0;

                //BUSCAR BASEADO NO QUE VÊ
                int[] localView = GetLocalView(x, y);
                if (validaCampoVisao(localView))
                {
                    bestAction = ChooseBestActionFromLocalView(localView);
                    visao = true;
                }
                else
                {
                    // Escolher ação com base nos valores Q
                    double bestQValue = qTable[x, y, 0];
                    for (int action = 1; action < numActions; action++)
                    {
                        if (qTable[x, y, action] > bestQValue)
                        {
                            bestAction = action;
                            bestQValue = qTable[x, y, action];
                        }
                    }
                }



                return bestAction;
            }
        }

        public void UpdateQValue(int x, int y, int action, double reward, int nextX, int nextY)
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

        public static int MaxQValueVision(double[,] qTable, int state)
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

        public void Train(int numEpisodes)
        {
            for (int episode = 0; episode < numEpisodes; episode++)
            {
                Tuple<int, int> currentGoal = GenerateRandomGoal();
                int x = 0;
                int y = 0; // Estado inicial
                while (x != currentGoal.Item1 || y != currentGoal.Item2)
                {
                    int action = ChooseAction(x, y);
                    int nextX, nextY;
                    SimulateEnvironment(x, y, action, out nextX, out nextY);
                    double reward = CalculateReward(nextX, nextY, currentGoal);
                    UpdateQValue(x, y, action, reward, nextX, nextY);
                    x = nextX;
                    y = nextY;
                }
            }
        }

        private void SimulateEnvironment(int x, int y, int action, out int nextX, out int nextY)
        {
            // Simulação do ambiente (pode ser personalizada)
            nextX = x;
            nextY = y;
            if (action == 0) // Norte
            {
                if (y > 0)
                    nextY = y - 1;
            }
            else if (action == 1) // Sul
            {
                if (y < numStatesY - 1)
                    nextY = y + 1;
            }
            else if (action == 2) // Leste
            {
                if (x < numStatesX - 1)
                    nextX = x + 1;
            }
            else if (action == 3) // Oeste
            {
                if (x > 0)
                    nextX = x - 1;
            }
        }

        private double CalculateReward(int x, int y, Tuple<int, int> goal)
        {
            if (x == goal.Item1 && y == goal.Item2)
            {
                return 1.0; // Recompensa máxima quando alcança o objetivo
            }
            return 0.0;
        }

        public double GetQValue(int x, int y, int action)
        {
            return qTable[x, y, action];
        }

        private Tuple<int, int> GenerateRandomGoal()
        {
            Random rand = new Random();
            int x = rand.Next(numStatesX);
            int y = rand.Next(numStatesY);
            stateValues[x, y] = 10;
            return Tuple.Create(x, y);
        }

        private int[] GetLocalView(int x, int y)
        {
            int[] localView = new int[visionSize * visionSize];
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int localX = x + i;
                    int localY = y + j;
                    // Certifique-se de que as coordenadas estejam dentro dos limites
                    if (localX >= 0 && localX < numStatesX && localY >= 0 && localY < numStatesY)
                    {
                        int index = (i + 1) * visionSize + (j + 1);
                        localView[index] = GetStateValue(localX, localY);
                    }
                }
            }
            return localView;
        }

        private int ChooseBestActionFromLocalView(int[] localView)
        {
            // Implemente a lógica para escolher a melhor ação com base na visão local aqui
            // Pode ser uma abordagem de Q-learning padrão considerando a visão local
            // ou outra técnica de aprendizado de máquina.
            // A escolha dependerá de como você deseja que o agente tome decisões.
            // Esta é uma parte crítica do código que requer personalização para o seu problema específico.
            // Você pode usar uma rede neural ou outras técnicas para aprender a partir da visão local.
            // No entanto, isso pode tornar o código consideravelmente mais complexo.

            //qTableVisao
            int currentState = localView.Count() / 2;
            return MaxQValueVision(qTableVisao, currentState);


            //3x3
            //0 1 2
            //3 4 5
            //6 7 8

            //5x5
            //0  1  2  3  4
            //5  6  7  8  9
            //10 11 12 13 14
            //15 16 17 18 19
            //20 21 22 23 24

            //PROVISORIO
            return new Random().Next(numActions);
        }

        private bool validaCampoVisao(int[] localView)
        {
            return localView.ToList().Any(x => x > 0);
        }

        private int GetStateValue(int x, int y)
        {
            // Suponhamos que stateValues seja um array que representa o estado do ambiente.
            // stateValues[x, y] representa o valor do estado na posição (x, y).
            return stateValues[x, y];
        }

        private void obterValoresQ()
        {
            // Obter valores Q após o treinamento
            for (int x = 0; x < numStatesX; x++)
            {
                for (int y = 0; y < numStatesY; y++)
                {
                    for (int action = 0; action < numActions; action++)
                    {
                        double qValue = this.GetQValue(x, y, action);
                        Console.WriteLine($"Q({x}, {y}, Ação {action}) = {qValue}");
                    }
                }
            }
        }
    }
}
