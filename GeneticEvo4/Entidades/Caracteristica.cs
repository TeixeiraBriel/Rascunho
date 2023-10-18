using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4.Entidades
{
    public abstract class Caracteristica
    {
        public string  Nome { get; set; }
        public int Prioridade { get; set; }
        public double Multiplicador { get; set; }
        public string DescValor0 { get; set; }
        public string DescValor1 { get; set; }
        public string DescValor2 { get; set; }
        public string DescValor3 { get; set; }
        public string DescValor4 { get; set; }
        public string DescValor5 { get; set; }
        public string DescValor6 { get; set; }
        public string DescValor7 { get; set; }
        public string DescValor8 { get; set; }
        public string DescValor9 { get; set; }
        public double Valor0 { get; set; }
        public double Valor1 { get; set; }
        public double Valor2 { get; set; }
        public double Valor3 { get; set; }
        public double Valor4 { get; set; }
        public double Valor5 { get; set; }
        public double Valor6 { get; set; }
        public double Valor7 { get; set; }
        public double Valor8 { get; set; }
        public double Valor9 { get; set; }

        public abstract Mundo Executa(Individuo individuo = null, Mundo mundo = null);
    }
}
