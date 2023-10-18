using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4.Entidades.Caracteristicas
{
    public class Meiose : Caracteristica
    {
        public Meiose()
        {
            Nome = "Meiose";
            Prioridade = 1;
            Valor1 = -80;
            Valor2 = 2;
        }

        public override Mundo Executa(Individuo individuo = null, Mundo mundo = null)
        {
            if (individuo.Energia + Valor1 > 0)
            {
                individuo.Energia += Valor1;
                for (int i = 0; i < Valor2; i++)
                {
                    Individuo filhote = new Individuo()
                    {
                        Nome = $"{individuo.Especie}{individuo.Geracao + 1}",
                        Filiacao = individuo.Nome,
                        Especie = individuo.Especie,
                        Caracteristicas = individuo.Caracteristicas,
                        Energia = individuo.Energia / 2,
                        Fome = individuo.Fome,
                        PosicaoX = individuo.PosicaoX,
                        PosicaoY = individuo.PosicaoY,
                        DataOrigem = mundo.Geracao + 1,
                        Vida = individuo.Vida / 2,
                        Geracao = individuo.Geracao + 1
                    };
                    mundo.EspecieList.Add(filhote);
                }
                mundo.EspecieList.Remove(individuo);
            }
            return mundo;
        }
    }
}
