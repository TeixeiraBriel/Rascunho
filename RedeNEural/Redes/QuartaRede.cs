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
                rede.executaTreino(100);

                Console.WriteLine("Sair? y/n");
                string entrada = Console.ReadLine();

                if(entrada == "y")
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
            tamanhoMatrix = 5;
            matrix = new string[tamanhoMatrix,tamanhoMatrix];
            randow = new Random();
        }

        void inicializaMatrix()
        {
            for (int x = 0; x < tamanhoMatrix; x++)
            {
                for (int y = 0; y < tamanhoMatrix; y++)
                {
                    matrix[x,y] = " - ";
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
            Console.WriteLine($"\nInicio Geral:{horarioInicioGeral} Inicio Episodio:{horarioInicioEpisodio} Passos Tentados:{contadorPassos} Episodio:{contadorEpisodios}");
        }
        #endregion

        #region Definicoes Base Agente

        double[,,,] qTable;
        int qtdStatesX;
        int qtdStatesY;
        int distMax;
        int qtdActions;

        int currentStateX;
        int currentStateY;
        int currentdist;


        void InicializaAtributosQtable()
        {
            qtdStatesX = tamanhoMatrix;
            qtdStatesY = tamanhoMatrix;
            distMax = (tamanhoMatrix * 2) - 2;
            qtdActions = 4;
        }

        void InicializaQtable()
        {
            qTable = new double[qtdStatesX, qtdStatesY,distMax,qtdActions];

            for (int x = 0; x < qtdStatesX; x++)
            {
                for (int y = 0; y < qtdStatesY; y++)
                {
                    for (int dist = 0; dist < distMax; dist++)
                    {
                        for (int action = 0; action < qtdActions; action++)
                        {
                            qTable[x,y,dist,action] = new Random().NextDouble();
                        }
                    }
                }
            }
        }

        void inicializaAgente()
        {
            currentStateX = randow.Next(tamanhoMatrix);
            currentStateY = randow.Next(tamanhoMatrix);
            matrix[currentStateX, currentStateY] = " o ";
            currentdist = calculaDistancia();
        }

        int calculaDistancia()
        {
            int distancia = 0;

            int distanciaX = currentStateX - currentObjStateX;
            int distanciaY = currentStateY - currentObjStateY;

            distanciaY = distanciaY < 0 ? distanciaY * -1 : distanciaY;
            distanciaX = distanciaX < 0 ? distanciaX * -1 : distanciaX;

            distancia = distanciaY + distanciaX;

            return distancia;
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
            qtdEpisodios = 10;
            taxaExploraca = 0.3;
            learningRate = 0.1;
            discountFactor = 0.9;
            timerThread = 10;
        }

        public void executaTreino(int _qtdEpisodios)
        {
            qtdEpisodios = _qtdEpisodios;
            for (int ep = 0; ep < qtdEpisodios; ep++)
            {
                contadorEpisodios++;
                executaEpisodio();
            }
        }

        public void executaEpisodio()
        {
            bool sucesso = false;
            do
            {
                int action = ChooseAction();
                ExecuteAction(action);
                double reward = calculateReward();
                sucesso = reward == 1 ? true : false;
                UpdateQValue(action, reward);
                ImprimeMatrix();
                contadorPassos++;
                Thread.Sleep(timerThread);
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
                double bestQValue = qTable[currentStateX, currentStateY, currentdist, 0];
                for (int action = 1; action < qtdActions; action++)
                {
                    var QValue = qTable[currentStateX, currentStateY, currentdist, action];
                    if (QValue > bestQValue)
                    {
                        bestAction = action;
                        bestQValue = QValue;
                    }
                }
            }
            return bestAction;
        }

        void ExecuteAction(int action)
        {
            matrix[currentStateX, currentStateY] = " - ";

            if (action == 0) // Baixo
            {
                if(currentStateX + 1 < tamanhoMatrix)
                    currentStateX += 1;
            }
            else if (action == 1) // Cima
            {
                if(currentStateX - 1 >= 0)
                    currentStateX += -1;
            }
            else if (action == 2) // Direita
            {
                if(currentStateY + 1 < tamanhoMatrix)
                    currentStateY += 1;
            }
            else if (action == 3) // Esquerda
            {
                if(currentStateY - 1 >= 0) 
                    currentStateY += -1;   
            }

            matrix[currentStateX, currentStateY] = " O ";
        }

        double calculateReward()
        {
            double totalReward = 0;
            int distAtual = calculaDistancia();
            totalReward = distAtual == 0 ? 1 : distAtual < currentdist ? 0.2 : distAtual > currentdist ? -0.3 : -0.1;

            if (distAtual == 0)
            {
                geraObjetivo();
            }

            currentdist = distAtual;

            return totalReward;
        }

        void UpdateQValue(int action, double reward)
        {
            var valorAtual = qTable[currentStateX, currentStateY, currentdist, action];
            var resultadoQtable = (1 - learningRate) * qTable[currentStateX, currentStateY, currentdist, action] +
                                           learningRate * (reward + discountFactor * MaxQValue());
            qTable[currentStateX, currentStateY, currentdist, action] = resultadoQtable;
        }

         double MaxQValue()
        {
            double maxQValue = qTable[currentStateX, currentStateY, currentdist,0];

            for (int action = 1; action < qtdActions; action++)
            {
                if (qTable[currentStateX, currentStateY, currentdist, action] > maxQValue)
                {
                    maxQValue = qTable[currentStateX, currentStateY, currentdist, action];
                }
            }

            return maxQValue;
        }
        #endregion
    }
}
