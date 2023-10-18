using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4.Entidades.Caracteristicas
{
    public class Digestao : Caracteristica
    {
        public Digestao()
        {
            Nome = "Digestao";
            Prioridade = 9;
            Valor1= 1;
        }

        public override Mundo Executa(Individuo individuo = null, Mundo mundo = null)
        {
            if (individuo.Fome < 0)
            {
                individuo.Energia += individuo.Fome * -Valor1;
                individuo.Fome = 0;
            }

            return mundo;
        }
    }
}
