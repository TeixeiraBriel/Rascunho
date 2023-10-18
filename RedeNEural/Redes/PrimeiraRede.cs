using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RedeNEural
{
    public class PrimeiraRede
    {
        private static bool existeMelhor = false;
        private static double[,] melhorQtabel = null;

        public static void Executa()
        {
            // Definir quantidade de episodios
            int episodiosTreino = 50;

            // Definir o tamanho da grade
            int gridSize = 5;

            // Inicializar o ambiente de grade
            GridEnvironment gridEnv = new GridEnvironment(gridSize);

            List<double[,]> qtables = new List<double[,]>();
            for (int i = 0; i < 10; i++)
            {
                qtables.Add(treinarRedeNeural(gridEnv, gridSize, episodiosTreino));
            }

            List<(int, string, double[,])> values = new List<(int, string, double[,])>();
            int count = 1;

            foreach (var qtable in qtables)
            {
                string nome = "Agente" + count;
                int passos = executaRedeNeural(gridEnv, qtable, nome);
                values.Add((passos, nome, qtable));
                count++;
            }

            values.OrderBy(x => x.Item1).ToList().ForEach(x => Console.WriteLine($"{x.Item2} finalizou em {x.Item1} passos."));
            Console.WriteLine();

            #region loop
            //APRIMORANDO
            melhorQtabel = values.OrderBy(x => x.Item1).FirstOrDefault().Item3;
            existeMelhor = true;

            qtables = new List<double[,]>();
            for (int i = 0; i < 10; i++)
            {
                qtables.Add(treinarRedeNeural(gridEnv, gridSize, episodiosTreino));
            }
            values = new List<(int, string, double[,])>();
            count = 1;

            foreach (var qtable in qtables)
            {
                string nome = "Agente" + count;
                int passos = executaRedeNeural(gridEnv, qtable, nome);
                values.Add((passos, nome, qtable));
                count++;
            }

            values.OrderBy(x => x.Item1).ToList().ForEach(x => Console.WriteLine($"{x.Item2} finalizou em {x.Item1} passos."));
            Console.WriteLine();


            //APRIMORANDO
            melhorQtabel = values.OrderBy(x => x.Item1).FirstOrDefault().Item3;
            existeMelhor = true;

            qtables = new List<double[,]>();
            for (int i = 0; i < 10; i++)
            {
                qtables.Add(treinarRedeNeural(gridEnv, gridSize, episodiosTreino));
            }
            values = new List<(int, string, double[,])>();
            count = 1;

            foreach (var qtable in qtables)
            {
                string nome = "Agente" + count;
                int passos = executaRedeNeural(gridEnv, qtable, nome);
                values.Add((passos, nome, qtable));
                count++;
            }

            values.OrderBy(x => x.Item1).ToList().ForEach(x => Console.WriteLine($"{x.Item2} finalizou em {x.Item1} passos."));
            #endregion
        }

        public static double[,] treinarRedeNeural(GridEnvironment gridEnv, int gridSize, int numEpisodesPar)
        {
            Random random = new Random();

            // Inicializar a tabela Q com valores iniciais
            double[,] qTable = new double[gridSize * gridSize, 4];
            InitializeQTable(qTable);

            // Taxa de aprendizado
            double learningRate = 0.1;

            // Fator de desconto
            double discountFactor = 0.9;

            // Número de episódios de treinamento
            int numEpisodes = numEpisodesPar;


            for (int episode = 0; episode < numEpisodes; episode++)
            {
                int currentState = gridEnv.Reset();

                while (!gridEnv.IsTerminalState(currentState))
                {
                    // Escolher uma ação epsilon-greedy
                    int action = ChooseAction(qTable, currentState, random);// 0: Up -5, 1: Right +1, 2: Down +5, 3: Left -1

                    // Executar a ação e obter a recompensa
                    (int nextState, double reward) = gridEnv.TakeAction(action);

                    // Atualizar a tabela Q
                    #region calculo ResultadoQtable quebrado
                    /*
                    var valorA = (1 - learningRate);
                    var valorB = qTable[currentState, action];
                    var valorAB = valorA * valorB;

                    var valorC = learningRate;
                    var valorD = reward;
                    var valorE = discountFactor;
                    var valorF = MaxQValue(qTable, nextState);

                    var valorEF = valorE * valorF;
                    var valorDEF = valorD + valorEF;

                    var valorCDEF = valorC * valorDEF;
                    var total = valorAB + valorCDEF;
                    */
                    #endregion
                    var resultadoQtable = (1 - learningRate) * qTable[currentState, action] +
                                                   learningRate * (reward + discountFactor * MaxQValue(qTable, nextState));

                    qTable[currentState, action] = resultadoQtable;

                    currentState = nextState;
                }
            }

            return qTable;

        }
        private static void InitializeQTable(double[,] qTable)
        {
            Random rand = new Random();
            for (int state = 0; state < 4; state++)
            {
                for (int action = 0; action < 4; action++)
                {
                    if (existeMelhor)
                    {
                        string passando = melhorQtabel[state,action].ToString();
                        qTable[state,action] = double.Parse(passando);
                    }
                    else
                    {
                        qTable[state, action] = rand.NextDouble();
                        //qTable[state, action] = 0;
                    }
                }
            }
        }

        public static int executaRedeNeural(GridEnvironment gridEnv, double[,] qtable, string Nome)
        {
            Random random = new Random();

            // Agora o agente foi treinado, você pode usá-lo para tomar decisões
            int initialState = gridEnv.Reset();
            int goalState = gridEnv.GoalState;
            int passos = 0;

            //Console.WriteLine($"Caminho do agente {Nome} do estado inicial ao estado objetivo:");
            int state = initialState;

            while (state != goalState)
            {
                int action = ChooseAction(qtable, state, random);
                //string acao = action == 0 ? "Up -5" : action == 1 ? "Right +1" : action == 2 ? "Down +5" : "Left -1";
                //Console.WriteLine($"Estado: {state}, Ação: {acao}");
                (int nextState, _) = gridEnv.TakeAction(action);
                state = nextState;
                passos++;
            }
            //Console.WriteLine($"Estado: {state}, Em {passos} passos");
            //Console.WriteLine($"Agente {Nome} Em {passos} passos");
            //Console.WriteLine("");

            return passos;
        }

        public static int ChooseAction(double[,] qTable, int state, Random random)
        {
            var chanceExploracao = random.NextDouble();
            if (chanceExploracao < 0.3) // Epsilon-greedy com epsilon de 0,2
                return random.Next(4); // Existem 4 ações: Up -5, Right +1, Down +5, Left -1
            else
                return ArgMax(qTable, state);
        }

        public static int ArgMax(double[,] qTable, int state)
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

        public static double MaxQValue(double[,] qTable, int state)
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

        public class GridEnvironment //AMBIENTE
        {
            private int gridSize;
            private int currentState;
            private int goalState;
            private List<int> validActions;

            public GridEnvironment(int gridSize)
            {
                this.gridSize = gridSize;
                this.currentState = 0;
                this.goalState = gridSize * gridSize - 1;
                this.validActions = new List<int> { -gridSize, 1, gridSize, -1 }; // Up, Right, Down, Left
            }

            public int Reset()
            {
                currentState = 0; // Começa no canto superior esquerdo
                return currentState;
            }

            public (int, double) TakeAction(int action)
            {
                int nextState = currentState + validActions[action];
                if (nextState >= 0 && nextState < gridSize * gridSize)
                {
                    currentState = nextState;
                }

                double reward = IsTerminalState(currentState) ? 1.0 : 0.0;
                return (currentState, reward);
            }

            public bool IsTerminalState(int state)
            {
                return state == goalState;
            }

            public int GoalState
            {
                get { return goalState; }
            }
        }
    }
}
