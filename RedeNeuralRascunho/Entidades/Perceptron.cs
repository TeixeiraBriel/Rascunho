using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralRascunho.Entidades
{
    public class Perceptron
    {
        public int Id { get; set; }

        public double Valor;

        public List<RelacionamentoAcao> Relacionamentos { get; set; }

        public void UpdateRelacionamento(Entrada chamador, double modificadorSomar)
        {
            chamador.Relacionamentos.FirstOrDefault(x => x.Receptor == this).Peso += modificadorSomar;
        }
    }
}
