using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4.Entidades
{
    public class Individuo
    {
        public string Nome { get; set; }
        public int Geracao { get; set; }
        public string Filiacao { get; set; }
        public string Especie { get; set; }
        public int DataOrigem { get; set; }
        public double Vida { get; set; }
        public int PosicaoX { get; set; }
        public int PosicaoY { get; set; }
        public double Fome { get; set; }
        public double Energia { get; set; }

        public List<Caracteristica> Caracteristicas { get; set; }

        public Mundo ExecutaCaracteristicas(Mundo mundo)
        {
            for (int i = 0; i < 10; i++)
            {
                List<Caracteristica> _caracteristicasAtual = Caracteristicas.FindAll(x => x.Prioridade == i);
                foreach (Caracteristica caracteristicas in _caracteristicasAtual)
                {
                    mundo = caracteristicas.Executa(this, mundo);
                }
            }           

            return mundo;
        }

        public void Imprime()
        {
            Console.WriteLine(
                $"\n    Nome:{Nome}" +
                $"\n    Especie:{Especie}" +
                $"\n    Filiacao:{Filiacao}" +
                $"\n    Vida:{Vida}" +
                $"\n    PosicaoX:{PosicaoX}" +
                $"\n    PosicaoY:{PosicaoY}" +
                $"\n    Fome:{Fome}" +
                $"\n    Energia:{Energia}" +
                $"\n    Origem:{DataOrigem}");
        }
    }
}
