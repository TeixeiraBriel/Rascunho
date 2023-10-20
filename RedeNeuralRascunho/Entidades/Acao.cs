using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralRascunho.Entidades
{
    public class Acao
    {
        public string Nome { get; set; }
        public double Valor { get; set; }

        public void UpdateRelacionamento(Perceptron chamador, double modificadorSomar)
        {
            chamador.Relacionamentos.FirstOrDefault(x => x.Acao == this).Peso += modificadorSomar;
        }

        public void Executa()
        {

        }
    }
}
