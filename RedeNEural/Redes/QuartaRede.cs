using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNEural.Redes
{
    public class QuartaRede
    {

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

            for (int i = 0; i < rede.tamanhoMatrix; i++)
            {
                string linha = "";
                for (int j = 0; j < rede.tamanhoMatrix; j++)
                {
                    linha += $" {rede.matrix[i, j]}";
                }
                Console.WriteLine(linha);
            }
        }

        #region Definições Base Ambiente

        string[,] matrix;
        int tamanhoMatrix;
        int currentObjStateX;
        int currentObjStateY;
        Random randow;


        void inicializaVariaveis()
        {
            tamanhoMatrix = 20;
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

        void inicializaVariaveisLogicaAprendizagem()
        {
            qtdEpisodios = 10;
            taxaExploraca = 0.2;
        }

        void executaTreino()
        {
            for (int ep = 0; ep < qtdEpisodios; ep++)
            {
                executaEpisodio();
            }
        }

        void executaEpisodio()
        {
            //Ultilizar
            //currentStateX
            //currentStateY
            //currentdist

            do
            {
                int action = ChooseAction();

            } while (GoalState());

        }

        bool GoalState()
        {
            bool posX = currentStateX == currentObjStateX;
            bool posY = currentStateY == currentObjStateY;

            bool saida = posY && posX;
            return saida;
        }

        public int ChooseAction()
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
                    if (qTable[currentStateX, currentStateY, currentdist, action] > bestQValue)
                    {
                        bestAction = action;
                        bestQValue = qTable[currentStateX, currentStateY, currentdist, action];
                    }
                }
            }
            return bestAction;
        }
        #endregion
    }
}
