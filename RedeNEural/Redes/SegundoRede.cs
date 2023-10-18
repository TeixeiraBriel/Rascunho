using System;
using System.Collections.Generic;

class SegundaRedeClass
{
    public static void execute()
    {
        int numStates = 10;
        int numActions = 4;
        SegundaRedeClass qLearning = new SegundaRedeClass(numStates, numActions);

        // Treinar o agente
        qLearning.Train(1000);

        // Obter valores Q após o treinamento
        for (int state = 0; state < numStates; state++)
        {
            for (int action = 0; action < numActions; action++)
            {
                double qValue = qLearning.GetQValue(state, action);
                Console.WriteLine($"Q({state}, {action}) = {qValue}");
            }
        }
    }

    // Número de estados e ações
    private int numStates;
    private int numActions;

    // Parâmetros do Q-learning
    private double learningRate = 0.1;
    private double discountFactor = 0.9;
    private double explorationProbability = 0.2;

    // Tabela Q
    private double[,] qTable;

    private SegundaRedeClass(int numStates, int numActions)
    {
        this.numStates = numStates;
        this.numActions = numActions;
        this.qTable = new double[numStates, numActions];
        InitializeQTable();
    }

    private void InitializeQTable()
    {
        Random rand = new Random();
        for (int state = 0; state < numStates; state++)
        {
            for (int action = 0; action < numActions; action++)
            {
                qTable[state, action] = rand.NextDouble();
                //qTable[state, action] = 0;
            }
        }
    }

    public int ChooseAction(int state)
    {
        Random rand = new Random();
        if (rand.NextDouble() < explorationProbability)
        {
            // Ação aleatória para exploração
            return rand.Next(numActions);
        }
        else
        {
            // Escolher ação com base nos valores Q
            int bestAction = 0;
            double bestQValue = qTable[state, 0];
            for (int action = 1; action < numActions; action++)
            {
                if (qTable[state, action] > bestQValue)
                {
                    bestAction = action;
                    bestQValue = qTable[state, action];
                }
            }
            return bestAction;
        }
    }

    public void UpdateQValue(int state, int action, double reward, int nextState)
    {
        double maxNextQValue = MaxQValue(nextState);
        double currentQValue = qTable[state, action];
        double updatedQValue = currentQValue + learningRate * (reward + discountFactor * maxNextQValue - currentQValue);
        qTable[state, action] = updatedQValue;
    }

    private double MaxQValue(int state)
    {
        double maxQValue = qTable[state, 0];
        for (int action = 1; action < numActions; action++)
        {
            if (qTable[state, action] > maxQValue)
            {
                maxQValue = qTable[state, action];
            }
        }
        return maxQValue;
    }

    public void Train(int numEpisodes)
    {
        for (int episode = 0; episode < numEpisodes; episode++)
        {
            int currentState = 0; // Estado inicial
            while (currentState != numStates - 1) // Enquanto não alcançar o estado objetivo
            {
                int action = ChooseAction(currentState);
                // Simular o ambiente e obter recompensa e próximo estado
                int nextState = SimulateEnvironment(currentState, action);
                double reward = CalculateReward(currentState, nextState);
                // Atualizar o valor Q
                UpdateQValue(currentState, action, reward, nextState);
                currentState = nextState;
            }
        }
    }

    private int SimulateEnvironment(int state, int action)
    {
        // Simulação do ambiente (pode ser personalizada)
        // Retornar o próximo estado com base no estado atual e ação escolhida
        int saida = state + action > 9 ? state : state + action;
        return saida;
    }

    private double CalculateReward(int currentState, int nextState)
    {
        if (nextState == numStates - 1)
        {
            return 1.0; // Recompensa máxima quando alcança o objetivo
        }
        else if (nextState <= currentState)
        {
            return -0.5; // Recompensa Negativa por se afastar ou n mover
        }

        return 0;
    }

    public double GetQValue(int state, int action)
    {
        return qTable[state, action];
    }
}
