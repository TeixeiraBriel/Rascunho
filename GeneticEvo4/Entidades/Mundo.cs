using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4.Entidades
{
    public class Mundo
    {
        public string Nome { get; set; }
        public List<Individuo> EspecieList { get; set; }
        public int Geracao { get; set; }
    }
}
