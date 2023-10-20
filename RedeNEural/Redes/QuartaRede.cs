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

        List<(int XPos, int XNeg, int YPos, int YNeg, int Action, double Peso)> qTable;
        int qtdStatesX;
        int qtdStatesY;
        int qtdActions;

        int distanciaDireita;
        int distanciaEsquerda;
        int distanciaBaixo;
        int distanciaCima;
        int maxDistanceX;
        int maxDistanceY;
        int currentStateX;
        int currentStateY;


        void InicializaAtributosQtable()
        {
            qtdStatesX = tamanhoMatrix;
            qtdStatesY = tamanhoMatrix;
            distanciaDireita = tamanhoMatrix - 1;
            distanciaEsquerda = tamanhoMatrix - 1;
            distanciaBaixo = tamanhoMatrix - 1;
            distanciaCima = tamanhoMatrix - 1;
            maxDistanceX = tamanhoMatrix;
            maxDistanceY = tamanhoMatrix;
            qtdActions = 4;
        }

        void InicializaQtable()
        {
            qTable = new List<(int XPos, int XNeg, int YPos, int YNeg, int Action, double Peso)>();

            for (int direita = 0; direita < maxDistanceX; direita++)
            {
                for (int esquerda = 0; esquerda < maxDistanceY; esquerda++)
                {
                    for (int cima = 0; cima < maxDistanceX; cima++)
                    {
                        for (int baixo = 0; baixo < maxDistanceY; baixo++)
                        {
                            for (int action = 0; action < qtdActions; action++)
                            {
                                qTable.Add((direita, esquerda, baixo, cima, action, new Random().NextDouble()));
                            }
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
            timerThread = 100;
            atualizaVariaveisDistancia();
        }

        public void executaTreino(int _qtdEpisodios)
        {
            qtdEpisodios = _qtdEpisodios;
            for (int ep = 0; ep < qtdEpisodios; ep++)
            {
                contadorEpisodios++;
                executaEpisodio(true);//Assistido = true / false

                log.Add($"Inicio Geral:{horarioInicioGeral} Inicio Episodio:{horarioInicioEpisodio} Passos Tentados:{contadorPassos} Episodio:{contadorEpisodios}");
                horarioInicioEpisodio = DateTime.Now.ToString("HH:MM:ss");
                contadorPassos = 0;
            }
        }

        public void executaEpisodio(bool imprimir = true)
        {
            do
            {
                int action = ChooseAction();
                ExecuteAction(action);
                double reward = calculateReward();
                UpdateQValue(action, reward);
                if (imprimir)
                {
                    ImprimeMatrix();
                    Thread.Sleep(timerThread);
                }
                atualizaVariaveisDistancia();
                contadorPassos++;
            } while (!validaSucessoEpisodio());
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

                List<int> actionsValidas = new List<int>() { 0, 1, 2, 3 };
                foreach (var action in actionsValidas)
                {
                    var QValue = obtemPesoQValue(distanciaDireita, distanciaEsquerda, distanciaBaixo, distanciaCima, action);

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

            int distAtualCima = calculaDistancia(0);
            int distAtualBaixo = calculaDistancia(1);
            int distAtualEsquerda = calculaDistancia(2);
            int distAtualDireita = calculaDistancia(3);

            totalReward += validaRetornaValorRecompensa(distAtualDireita, distanciaDireita);
            totalReward += validaRetornaValorRecompensa(distAtualEsquerda, distanciaEsquerda);
            totalReward += validaRetornaValorRecompensa(distAtualBaixo, distanciaBaixo);
            totalReward += validaRetornaValorRecompensa(distAtualCima, distanciaCima);

            return totalReward;
        }

        void UpdateQValue(int action, double reward)
        {
            var valorAtual = obtemPesoQValue(distanciaDireita, distanciaEsquerda, distanciaBaixo, distanciaCima, action);
            var resultadoQtable = (1 - learningRate) * obtemPesoQValue(distanciaDireita, distanciaEsquerda, distanciaBaixo, distanciaCima, action) +
                                           learningRate * (reward + discountFactor * MaxQValue());

            var QvalueOut = qTable.FirstOrDefault(Qvalue =>
                    Qvalue.XPos == distanciaDireita && 
                    Qvalue.XNeg == distanciaEsquerda && 
                    Qvalue.YPos == distanciaBaixo && 
                    Qvalue.YNeg == distanciaCima && 
                    Qvalue.Action == action
                );

            qTable.Remove(QvalueOut);
            QvalueOut.Peso = resultadoQtable;
            qTable.Add(QvalueOut);
        }

        double MaxQValue()
        {
            double maxQValue = obtemPesoQValue(distanciaDireita, distanciaEsquerda, distanciaBaixo, distanciaCima, 0);

            for (int action = 1; action < qtdActions; action++)
            {
                if (obtemPesoQValue(distanciaDireita, distanciaEsquerda, distanciaBaixo, distanciaCima, action) > maxQValue)
                {
                    maxQValue = obtemPesoQValue(distanciaDireita, distanciaEsquerda, distanciaBaixo, distanciaCima, action);
                }
            }

            return maxQValue;
        }

        int calculaDistancia(int Type, bool positivo = true)
        {
            int distancia = 0;

            switch (Type)
            {
                case 0: //YCima
                    distancia = currentStateX - currentObjStateX;
                    distancia = distancia > 0 ? distancia : 0;
                    break;
                case 1: //YBaixo
                    distancia = currentStateX - currentObjStateX;
                    distancia = distancia < 0 ? distancia * -1 : 0;
                    break;
                case 2: //Esquerda
                    distancia = currentStateY - currentObjStateY;
                    distancia = distancia > 0 ? distancia : 0;
                    break;
                case 3: //Direta
                    distancia = currentStateY - currentObjStateY;
                    distancia = distancia < 0 ? distancia * -1 : 0;
                    break;
            }

            if (positivo)
                distancia = distancia < 0 ? distancia * -1 : distancia;
            return distancia;
        }

        double obtemPesoQValue(int direita, int esquerda, int baixo, int cima, int action)
        {
            return qTable.FirstOrDefault(Qvalue => Qvalue.XPos == direita && Qvalue.XNeg == esquerda && Qvalue.YPos == baixo && Qvalue.YNeg == cima && Qvalue.Action == action).Peso;
        }

        double validaRetornaValorRecompensa(int valAtual, int valAnterior)
        {
            double totalReward = 0;

            if (valAtual != valAnterior)
                totalReward = valAtual == 0 ? 1 : valAtual < valAnterior ? 0.5 : valAtual > valAnterior ? -1 : 0;

            return totalReward;
        }

        void atualizaVariaveisDistancia()
        {
            distanciaCima = calculaDistancia(0);
            distanciaBaixo = calculaDistancia(1);
            distanciaEsquerda = calculaDistancia(2);
            distanciaDireita = calculaDistancia(3);
        }

        bool validaSucessoEpisodio()
        {
            bool sucesso = false;

            int distAtualCima = calculaDistancia(0);
            int distAtualBaixo = calculaDistancia(1);
            int distAtualEsquerda = calculaDistancia(2);
            int distAtualDireita = calculaDistancia(3);
            sucesso = distAtualDireita == 0 && distAtualEsquerda == 0 && distAtualBaixo == 0 && distAtualCima == 0;

            if (sucesso)
            {
                geraObjetivo();
            }

            return sucesso;
        }
        #endregion
    }
}
