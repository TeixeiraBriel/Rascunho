using System;

namespace RedeNeuralReforço
{
    public interface IReinforcementLearningEnvironment
    {
        int StateSize { get; }
        int ActionSize { get; }

        double GetState();
        double TakeAction(double action);
        void Update(double state, double action, double reward, double nextState);
        double Reset();
        bool IsDone { get; }
    }

    public class SimpleEnvironment : IReinforcementLearningEnvironment
    {
        public int StateSize => 1;
        public int ActionSize => 2;

        private double state;
        private bool done;

        public double GetState()
        {
            return state;
        }

        public void Update(double state, double action, double reward, double nextState)
        {
            // Implemente a lógica de atualização aqui
        }

        public double Reset()
        {
            state = 0.0;
            done = false;
            return state;
        }

        public double TakeAction(double action)
        {
            // Implemente a lógica do ambiente aqui
            // Neste exemplo, recompensamos o agente por manter o estado próximo a 0
            double reward = -Math.Pow(action - state, 2);

            // Atualize o estado
            state += action;

            return reward;
        }

        public bool IsDone => done;
    }

    // Você pode usar a classe SimpleEnvironment como seu ambiente de aprendizado por reforço personalizado
}
