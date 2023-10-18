using GeneticEvo4.Entidades;
using GeneticEvo4.Entidades.Caracteristicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticEvo4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool sair = false;

            #region Criando Mundo Base
            Mundo mundo = new Mundo()
            {
                Nome = "Mundo Base",
                EspecieList = new List<Individuo>(),
                Geracao = 1
            };

            Individuo Alga = new Individuo()
            {
                Nome = "Alga1",
                Especie = "Alga",
                Geracao = 1,
                DataOrigem = 0,
                Vida = 100,
                Energia = 100,
                Caracteristicas = new List<Caracteristica> { new Fotossintese(), new Meiose(), new Digestao() }
            };
            mundo.EspecieList.Add(Alga);
            #endregion

            do
            {
                Console.WriteLine($"Geração {mundo.Geracao}");
                List<Individuo> EspecieListAtual = atribuiLista(mundo.EspecieList);

                foreach (Individuo especie in EspecieListAtual)
                {
                    especie.Imprime();
                    mundo = especie.ExecutaCaracteristicas(mundo);
                }

                Console.WriteLine("Deseja Sair? y/n");
                sair = Console.ReadLine() == "y" ? true : false;
                Console.Clear();
                mundo.Geracao++;
            } while (!sair);
        }

        static List<Individuo> atribuiLista(List<Individuo> lista)
        {
            List<Individuo> listSaida = new List<Individuo>();
            foreach (var item in lista)
            {
                listSaida.Add(item);
            }
            return listSaida;
        }
    }
}
