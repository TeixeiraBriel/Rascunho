using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralRascunho.Entidades
{
    public class Entrada
    {
        public string Nome { get; set; }
        public int Valor { get; set; }
        public List<RelacionamentoPerceptron> Relacionamentos { get; set; }

        public Entrada()
        {
            Relacionamentos = new List<RelacionamentoPerceptron> { };
        }
    }
}
