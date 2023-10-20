using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace RedeNEural.Redes
{
    public class QuartaRede
    {
        List<string> log = new List<string>();
        int contadorPassos = 0;
        int contadorEpisodios = 0;
        string horarioInicioGeral = "";
        string horarioInicioEpisodio = "";

        public QuartaRede()
        {
            inicializaVariaveis();
            inicializaMatrix();
            geraObjetivo();
            InicializaAtributosQtable();
            InicializaQtable();
            inicializaAgente();
            inicializaVariaveisLogicaAprendizagem();
        }

        public static void Executa()
        {
            QuartaRede rede = new QuartaRede();
            rede.ImprimeMatrix();
            rede.horarioInicioGeral = DateTime.Now.ToString("HH:MM:ss");

            do
            {
                rede.horarioInicioEpisodio = DateTime.Now.ToString("HH:MM:ss");
                rede.executaTreino(1000);

                Console.WriteLine("Sair? y/n");
                string entrada = Console.ReadLine();

                if (entrada == "y")
                    break;
            } while (true);

            Console.Clear();
        }

        #region Definições Base Ambiente

        string[,] matrix;
        int tamanhoMatrix;
        int currentObjStateX;
        int currentObjStateY;
        Random randow;


        void inicializaVariaveis()
        {
            tamanhoMatrix = 10;
            matrix = new string[tamanhoMatrix, tamanhoMatrix];
            randow = new Random();
        }

        void inicializaMatrix()
        {
            for (int x = 0; x < tamanhoMatrix; x++)
            {
                for (int y = 0; y < tamanhoMatrix; y++)
                {
                    matrix[x, y] = " - ";
                }
            }
        }

        void geraObjetivo()
        {
            currentObjStateX = randow.Next(tamanhoMatrix);
            currentObjStateY = randow.Next(tamanhoMatrix);
            matrix[currentObjStateX, currentObjStateY] = " X ";
        }

        public void ImprimeMatrix()
        {
            Console.Clear();

            for (int i = 0; i < tamanhoMatrix; i++)
            {
                string linha = "";
                for (int j = 0; j < tamanhoMatrix; j++)
                {
                    linha += $" {matrix[i, j]}";
                }
                Console.WriteLine(linha);
            }

            int contadorImpressao = 10;
            Console.WriteLine($"\nInicio Geral:{horarioInicioGeral} Inicio Episodio:{horarioInicioEpisodio} Passos Tentados:{contadorPassos} Episodio:{contadorEpisodios}");
            Console.WriteLine($"");
            log.Reverse();
            foreach (var item in log)
            {
                Console.WriteLine(item);
                contadorImpressao--;
                if (contadorImpressao == 0)
                    break;
            }
            log.Reverse();
        }
        #endregion

        #region Definicoes Base Agente

        List<(int X, int Y, int Action, double Peso)> qTable;
        int qtdStatesX;
        int qtdStatesY;
        int qtdActions;

        int currentDistanceX;
        int currentDistanceY;
        int maxDistanceX;
        int maxDistanceY;
        int currentStateX;
        int currentStateY;


        void InicializaAtributosQtable()
        {
            qtdStatesX = tamanhoMatrix;
            qtdStatesY = tamanhoMatrix;
            maxDistanceX = tamanhoMatrix;
            maxDistanceY = tamanhoMatrix;
            qtdActions = 4;
        }

        void InicializaQtable()
        {
            qTable = new List<(int X, int Y, int Action, double Peso)>();

            for (int x = 0; x < maxDistanceX; x++)
            {
                for (int y = 0; y < maxDistanceY; y++)
                {
                    for (int action = 0; action < qtdActions; action++)
                    {
                        qTable.Add((x, y, action, new Random().NextDouble()));
                    }
                }
            }
        }

        void inicializaAgente()
        {
            currentStateX = randow.Next(tamanhoMatrix);
            currentStateY = randow.Next(tamanhoMatrix);
            matrix[currentStateX, currentStateY] = " o ";
        }
        #endregion

        #region Logica Aprendizagem por Reforço

        int qtdEpisodios;
        double taxaExploraca;
        double learningRate;
        double discountFactor;
        int timerThread;

        void inicializaVariaveisLogicaAprendizagem()
        {
            qtdEpisodios = 20;
            taxaExploraca = 0.2;
            learningRate = 0.1;
            discountFactor = 0.7;
            timerThread = 200;
        }

        public void executaTreino(int _qtdEpisodios)
        {
            qtdEpisodios = _qtdEpisodios;
            for (int ep = 0; ep < qtdEpisodios; ep++)
            {
                contadorEpisodios++;
                executaEpisodio(true);

                log.Add($"Inicio Geral:{horarioInicioGeral} Inicio Episodio:{horarioInicioEpisodio} Passos Tentados:{contadorPassos} Episodio:{contadorEpisodios}");
                horarioInicioEpisodio = DateTime.Now.ToString("HH:MM:ss");
                contadorPassos = 0;
            }
        }

        public void executaEpisodio(bool imprimir = true)
        {
            currentDistanceX = calculaDistancia(0);
            currentDistanceY = calculaDistancia(1);

            bool sucesso = false;
            do
            {
                int action = ChooseAction();
                ExecuteAction(action);
                double reward = calculateReward();
                sucesso = currentDistanceX == 0 && currentDistanceY == 0 ? true : false;
                UpdateQValue(action, reward);
                if (imprimir)
                {
                    ImprimeMatrix();
                    Thread.Sleep(timerThread);
                }
                contadorPassos++;
            } while (!sucesso);
        }

        int ChooseAction()
        {
            int bestAction = 0;

            var chanceExploracao = new Random().NextDouble();
            if (chanceExploracao < taxaExploraca)
            {
                bestAction = new Random().Next(qtdActions);
            }
            else
            {
                // Escolher ação com base nos valores Q
                double bestQValue = 0;
                bool first = true;

                List<int> actionsValidas = getValidActions();
                foreach (var action in actionsValidas)
                {
                    var QValue = obtemPesoQValue(currentDistanceX, currentDistanceY, action);

                    if (first)
                    {
                        first = false;
                        bestAction = action;
                        bestQValue = QValue;
                    }
                    else if (QValue > bestQValue)
                    {
                        bestAction = action;
                        bestQValue = QValue;
                    }
                }
            }
            return bestAction;
        }

        List<int> getValidActions()
        {
            bool negX = calculaDistancia(0, false) < 0 ? true : false;
            bool negY = calculaDistancia(1, false) < 0 ? true : false;

            List<int> actions = new List<int>() { 0, 1, 2, 3 };
            List<int> actionsRemoved = new List<int>();

            if (currentDistanceX == 0)
            {
                actions.Remove(actions.Find(x => x == 0));
                actions.Remove(actions.Find(x => x == 1));
                actionsRemoved.Add(0);
                actionsRemoved.Add(1);
            }

            if (currentDistanceY == 0)
            {
                actions.Remove(actions.Find(x => x == 2));
                actions.Remove(actions.Find(x => x == 3));
                actionsRemoved.Add(2);
                actionsRemoved.Add(3);
            }

            if (currentStateX == 0 && !actionsRemoved.Exists(x => x == 1))
            {
                actions.Remove(actions.Find(x => x == 1));
            }

            if (currentStateY == 0 && !actionsRemoved.Exists(x => x == 3))
            {
                actions.Remove(actions.Find(x => x == 3));
            }

            if ((currentStateX == maxDistanceX - 1 || !negX) && !actionsRemoved.Exists(x => x == 0))
            {
                actions.Remove(actions.Find(x => x == 0));
            }

            if ((currentStateY == maxDistanceX - 1 || !negY) && !actionsRemoved.Exists(x => x == 2))
            {
                actions.Remove(actions.Find(x => x == 2));
            }

            //0 - Baixo     1 - Cima        2 - Direita         3 - Esquerda
            return actions;
        }

        void ExecuteAction(int action)
        {
            matrix[currentStateX, currentStateY] = " - ";

            if (action == 0) // Baixo
            {
                if (currentStateX + 1 < tamanhoMatrix)
                    currentStateX += 1;
            }
            else if (action == 1) // Cima
            {
                if (currentStateX - 1 >= 0)
                    currentStateX += -1;
            }
            else if (action == 2) // Direita
            {
                if (currentStateY + 1 < tamanhoMatrix)
                    currentStateY += 1;
            }
            else if (action == 3) // Esquerda
            {
                if (currentStateY - 1 >= 0)
                    currentStateY += -1;
            }

            matrix[currentStateX, currentStateY] = " O ";
        }

        double calculateReward()
        {
            double totalReward = 0;

            int distAtualX = calculaDistancia(0);
            int distAtualY = calculaDistancia(1);

            if (distAtualX != currentDistanceX)
                totalReward = distAtualX == 0 ? 1 : distAtualX < currentDistanceX ? 0.5 : distAtualX > currentDistanceX ? -1 : 0;
            else if (distAtualY != currentDistanceY)
                totalReward += distAtualY == 0 ? 1 : distAtualY < currentDistanceY ? 0.5 : distAtualY > currentDistanceY ? -1 : 0;
            else
                totalReward += -1;

            if (distAtualX == 0 && distAtualY == 0)
            {
                geraObjetivo();
            }

            currentDistanceX = distAtualX;
            currentDistanceY = distAtualY;

            return totalReward;
        }

        void UpdateQValue(int action, double reward)
        {
            var valorAtual = obtemPesoQValue(currentDistanceX, currentDistanceY, action);
            var resultadoQtable = (1 - learningRate) * obtemPesoQValue(currentDistanceX, currentDistanceY, action) +
                                           learningRate * (reward + discountFactor * MaxQValue());

            var QvalueOut = qTable.FirstOrDefault(Qvalue => Qvalue.X == currentDistanceX && Qvalue.Y == currentDistanceY && Qvalue.Action == action);
            qTable.Remove(QvalueOut);
            QvalueOut.Peso = resultadoQtable;
            qTable.Add(QvalueOut);
        }

        double MaxQValue()
        {
            double maxQValue = obtemPesoQValue(currentDistanceX, currentDistanceY, 0);

            for (int action = 1; action < qtdActions; action++)
            {
                if (obtemPesoQValue(currentDistanceX, currentDistanceY, action) > maxQValue)
                {
                    maxQValue = obtemPesoQValue(currentDistanceX, currentDistanceY, action);
                }
            }

            return maxQValue;
        }

        int calculaDistancia(int Type, bool positivo = true)
        {
            int distancia = 0;

            switch (Type)
            {
                case 0: //Posicao X
                    distancia = currentStateX - currentObjStateX;
                    break;
                case 1: //Posicao Y
                    distancia = currentStateY - currentObjStateY;
                    break;
            }

            if (positivo)
                distancia = distancia < 0 ? distancia * -1 : distancia;
            return distancia;
        }

        double obtemPesoQValue(int x, int y, int action)
        {
            return qTable.FirstOrDefault(Qvalue => Qvalue.X == x && Qvalue.Y == y && Qvalue.Action == action).Peso;
        }
        #endregion
    }
}
