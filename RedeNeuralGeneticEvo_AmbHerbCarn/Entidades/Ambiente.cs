using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralGeneticEvo_AmbHerbCarn.Entidades
{
    public class Ambiente
    {
        public int tamanhoMatrix { get; set; }
        public int[,] matrix { get; set; }

        public Ambiente()
        {

        }

        void inicializaAmbiente()
        {
            matrix = new int[tamanhoMatrix,tamanhoMatrix];
        }
    }
}
