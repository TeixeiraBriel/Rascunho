using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeuralRascunho.Entidades
{
    public class Agente
    {
        public List<Entrada> Entradas { get; set; }
        public List<Perceptron> perceptrons { get; set; }

        public Agente()
        {
            InicializaEntradas();
            perceptrons = new List<Perceptron>();
            Entradas = new List<Entrada>();
        }

        public void Executa()
        {

        }

        void CriaNovaEntrada(string Nome, int Valor, List<Perceptron> _perceptronsRelacionar = null)
        {
            Entrada novaEntrada = new Entrada()
            {
                Nome = "posEixoX",
                Valor = Valor
            };

            Entradas.Add(novaEntrada);
            if (_perceptronsRelacionar != null)
            {
                criaRelacionamentoEntradaPercepton(novaEntrada, _perceptronsRelacionar);
            }
        }

        void InicializaEntradas()
        {
            CriaNovaEntrada("posEixoX", 0);
            CriaNovaEntrada("posEixoY", 0);
            CriaNovaEntrada("distanciaDireita", 0, perceptrons);
            CriaNovaEntrada("distanciaEsquerda", 0, perceptrons);
            CriaNovaEntrada("distanciaBaixo", 0, perceptrons);
            CriaNovaEntrada("distanciaCima", 0, perceptrons);
        }

        void InicializaPerceptrons()
        {
            perceptrons.Add(new Perceptron() { Id = perceptrons.Count - 1 });
            perceptrons.Add(new Perceptron() { Id = perceptrons.Count - 1 });
            perceptrons.Add(new Perceptron() { Id = perceptrons.Count - 1 });
            perceptrons.Add(new Perceptron() { Id = perceptrons.Count - 1 });
        }

        void criaRelacionamentoEntradaPercepton(Entrada _entrada, List<Perceptron> _perceptrons)
        {
            foreach (var item in _perceptrons)
            {
                _entrada.Relacionamentos.Add(new RelacionamentoPerceptron() { Peso = new Random().NextDouble(), Receptor = item });
            }
        }
    }
}
