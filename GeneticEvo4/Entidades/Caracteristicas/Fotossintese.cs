
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4.Entidades.Caracteristicas
{
    public class Fotossintese : Caracteristica
    {
        public Fotossintese()
        {
            Nome = "Fotossintese";
            Prioridade = 0;
            Multiplicador = 1;
            Valor1 = 5;
            Valor2 = 1;
        }

        public override Mundo Executa(Individuo individuo = null, Mundo mundo = null)
        {
            double gastoEnergia = -1 * (2 * Multiplicador);
            double consumoFome = -1 * (Valor1 * Multiplicador);
            if (individuo.Energia + gastoEnergia > 0)
            {
                individuo.Energia += gastoEnergia;
                individuo.Fome += consumoFome;
            }

            return mundo;
        }
    }
}
