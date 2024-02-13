using System;
using System.Linq;
using System.Threading;

public class QuintaRede
{
    public static void Executa()
    {
        // Define o labirinto: 0 - espaço vazio, 1 - obstáculo, 2 - objetivo
        int[,] maze = new int[,] { { 0, 0, 0, 0, 1 }, { 0, 1, 0, 0, 1 }, { 0, 1, 1, 1, 1 }, { 0, 0, 0, 0, 2 } };

        MazeEnvironment environment = new MazeEnvironment(maze);

        // Hiperparâmetros do agente Q-learning
        int stateSize = 2;          // Tamanho do vetor de estado (posição x, posição y)
        int actionSize = 4;         // Número de ações possíveis (cima, baixo, esquerda, direita)
        double explorationRate = 1.0; // Taxa inicial de exploração
        double learningRate = 0.1;    // Taxa de aprendizado
        double discountFactor = 0.9;  // Fator de desconto para recompensas futuras

        // Criação do agente Q-learning
        QLearningAgent agent = new QLearningAgent(stateSize, actionSize, explorationRate, learningRate, discountFactor);

        // Treina o agente
        agent.Train(environment, maxEpisodes: 100);

        // Testa o agente treinado no ambiente
        agent.Test(environment);
    }
}

class QLearningNetwork
{
    private double[,] weights;
    private int stateSize;
    private int actionSize;

    public QLearningNetwork(int stateSize, int actionSize)
    {
        this.stateSize = stateSize;
        this.actionSize = actionSize;

        // Inicialização aleatória dos pesos
        weights = InitializeWeights(stateSize, actionSize);
    }

    private double[,] InitializeWeights(int rows, int cols)
    {
        Random rand = new Random();
        double[,] weights = new double[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                weights[i, j] = rand.NextDouble();
            }
        }

        return weights;
    }

    public int ChooseAction(double[] state, double explorationRate)
    {
        // Escolhe uma ação com base nos valores Q ou realiza uma ação aleatória (exploração)
        if (explorationRate > 0 && new Random().NextDouble() < explorationRate)
        {
            return new Random().Next(actionSize); // Exploração aleatória
        }
        else
        {
            // Exploitation: Escolhe a ação com o maior valor Q
            double[] qValues = GetQValues(state);
            return Array.IndexOf(qValues, qValues.Max());
        }
    }

    public double[] GetQValues(double[] state)
    {
        // Propagação para frente
        double[] qValues = new double[actionSize];

        for (int i = 0; i < actionSize; i++)
        {
            for (int j = 0; j < stateSize; j++)
            {
                qValues[i] += state[j] * weights[j, i];
            }
        }

        return qValues;
    }

    public void Train(double[] state, int action, double reward, double[] nextState, double learningRate, double discountFactor)
    {
        // Propagação para frente
        double[] qValues = GetQValues(state);
        double predictedQ = qValues[action];

        // Calcula o valor alvo Q usando a equação de Bellman
        double targetQ = reward + discountFactor * GetQValues(nextState).Max();

        // Calcula o erro
        double error = targetQ - predictedQ;

        // Atualiza os pesos usando o erro
        for (int i = 0; i < stateSize; i++)
        {
            weights[i, action] += learningRate * state[i] * error;
        }
    }
}

class MazeEnvironment
{
    private int[,] maze;
    private int agentPositionX;
    private int agentPositionY;
    private int[,] possibleActions;

    public MazeEnvironment(int[,] maze)
    {
        this.maze = maze;
        agentPositionX = 0;
        agentPositionY = 0;
        InitializePossibleActions();
    }

    public int[,] GetMaze()
    {
        return maze;
    }

    public int GetAgentPositionX()
    {
        return agentPositionX;
    }

    public int GetAgentPositionY()
    {
        return agentPositionY;
    }
    private void InitializePossibleActions()
    {
        int rows = maze.GetLength(0);
        int cols = maze.GetLength(1);
        possibleActions = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {                
                // Inicialmente, consideramos todas as ações possíveis
                possibleActions[i, j] = maze[i, j] == 1 ? 0 : 1;
            }
        }
    }

    public void MoveAgent(int action)
    {
        // Ações: 0 - cima, 1 - baixo, 2 - esquerda, 3 - direita
        int nextX = agentPositionX;
        int nextY = agentPositionY;

        switch (action)
        {
            case 0:
                nextX--;
                break;
            case 1:
                nextX++;
                break;
            case 2:
                nextY--;
                break;
            case 3:
                nextY++;
                break;
            default:
                break;
        }

        // Verifica se a próxima posição é válida e não é um obstáculo
        if (IsValidPosition(nextX, nextY) && possibleActions[nextX, nextY] == 1)
        {
            agentPositionX = nextX;
            agentPositionY = nextY;
        }
    }
    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < maze.GetLength(0) && y >= 0 && y < maze.GetLength(1);
    }

    public bool IsGoalReached()
    {
        return maze[agentPositionX, agentPositionY] == 2; // O objetivo é representado pelo valor 2 no labirinto
    }

    public double GetReward()
    {
        return IsGoalReached() ? 1.0 : -0.1; // Recompensa positiva ao atingir o objetivo, penalidade por cada movimento
    }

    public double[] GetState()
    {
        // Retorna o estado atual do ambiente (posição do agente)
        return new double[] { agentPositionX, agentPositionY };
    }
}

class QLearningAgent
{
    private QLearningNetwork qNetwork;
    private double explorationRate;
    private double learningRate;
    private double discountFactor;

    public QLearningAgent(int stateSize, int actionSize, double explorationRate, double learningRate, double discountFactor)
    {
        qNetwork = new QLearningNetwork(stateSize, actionSize);
        this.explorationRate = explorationRate;
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
    }

    public void Train(MazeEnvironment environment, int maxEpisodes)
    {
        for (int episode = 0; episode < maxEpisodes; episode++)
        {
            // Reinicia o ambiente para um novo episódio
            environment = new MazeEnvironment(new int[,] { { 0, 0, 0, 0, 1 }, { 0, 1, 0, 0, 1 }, { 0, 1, 1, 1, 1 }, { 0, 0, 0, 0, 2 } });
            Console.WriteLine($"Run:{episode}");

            while (!environment.IsGoalReached())
            {
                // Obtém o estado atual do ambiente
                double[] currentState = environment.GetState();

                // Escolhe uma ação com base na política epsilon-greedy
                int action = qNetwork.ChooseAction(currentState, explorationRate);

                // Executa a ação no ambiente
                environment.MoveAgent(action);

                // Obtém a recompensa do ambiente
                double reward = environment.GetReward();

                // Obtém o próximo estado
                double[] nextState = environment.GetState();

                // Treina a Q-Network com base na experiência atual
                qNetwork.Train(currentState, action, reward, nextState, learningRate, discountFactor);

                Console.Clear();
                Console.WriteLine($"Run:{episode}");
                PrintMazeState(environment);
                Thread.Sleep(10);
            }

            // Reduz a taxa de exploração ao longo do tempo
            explorationRate *= 0.99;
        }
    }

    public void Test(MazeEnvironment environment)
    {
        // Testa o agente treinado em um ambiente
        Console.WriteLine("Testando o agente treinado...");

        while (!environment.IsGoalReached())
        {
            double[] currentState = environment.GetState();
            int action = qNetwork.ChooseAction(currentState, 0); // Sem exploração durante o teste
            environment.MoveAgent(action);

            // Exibe o estado atual do ambiente
            PrintMazeState(environment);
        }

        Console.WriteLine("Objetivo alcançado!");
    }

    private void PrintMazeState(MazeEnvironment environment)
    {
        // Função para exibir o estado atual do labirinto
        for (int i = 0; i < environment.GetMaze().GetLength(0); i++)
        {
            for (int j = 0; j < environment.GetMaze().GetLength(1); j++)
            {
                if (i == environment.GetAgentPositionX() && j == environment.GetAgentPositionY())
                    Console.Write("A "); // Representação do agente
                else
                    Console.Write(environment.GetMaze()[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}