using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunIdeiaTres();
        }

        #region IdeiaTres
        static void RunIdeiaTres()
        {
            int regiaoA = 0;
            int regiaoB = 0;
            int regiaoC = 0;
            int regiaoD = 0;


        }

        class Animal_I1
        {
            public int AdpA { get; set; }
            public int AdpB { get; set; }
            public int AdpC { get; set; }
            public int AdpD { get; set; }
        }
        #endregion

        #region IdeiaDois

        public void RunIdeiaDois()
        {
            List<Planta> Plantas = new List<Planta>();
            int Ano = 1;

            Plantas.Add(new Planta() { Estagio = 0, TempoVida = 0 });
            Plantas.Add(new Planta() { Estagio = 0, TempoVida = 0 });
            Plantas.Add(new Planta() { Estagio = 0, TempoVida = 0 });

            do
            {
                for (int i = 0; i < Plantas.Count; i++)
                {
                    bool mult = Plantas[i].Multiplica();
                    if (mult)
                        Plantas.Add(new Planta());
                }

                Console.WriteLine(Plantas[0].TempoVida);
                Console.WriteLine($"Ano {Ano} - Plantas Adultas:{Plantas.Where(x => x.Estagio == 1).Count()} Plantas Jovens:{Plantas.Where(x => x.Estagio == 0).Count()}");
                Console.ReadLine();
                Ano++;
            } while (true);
        }

        public class Planta
        {
            public int Estagio { get; set; }
            public int TempoVida { get; set; }

            public bool Multiplica()
            {
                bool Saida = false;

                if (Estagio == 0 && TempoVida >= 5)
                {
                    Estagio = 1;
                }
                else if (TempoVida >= 10 && TempoVida % 5 == 0)
                {
                    Saida = true;
                }

                this.TempoVida++;
                return Saida;
            }
        }

        #endregion

        #region IdeiaInicial
        static void IdeiaInicial()
        {
            //INICIALIZA VARIAVEIS
            int Vitalidade = 98;
            List<int> Herbivoros = new List<int>();
            List<int> Carnivoros = new List<int>();
            Herbivoros.Add(1);
            Carnivoros.Add(1);

            //REALIZA ACOES
            do
            {
                acaoHerb(Herbivoros, Vitalidade);
                acaoCarn(Herbivoros, Carnivoros, Vitalidade);

                Console.WriteLine($"Vitalidade:{Vitalidade}, Carnivoros:{Carnivoros.Count}, Herbivoros:{Herbivoros.Count}");
                Console.ReadLine();
            } while (Vitalidade > 0);


            //Imprime Resultados
            Console.WriteLine(Vitalidade);
            foreach (int i in Herbivoros) { Console.WriteLine(Herbivoros); };
            Console.ReadLine();
        }
        static void acaoHerb(List<int> Herbivoros, int Vitalidade)
        {
            for (int i = 0; i < Herbivoros.Count; i++)
            {
                int acao = new Random().Next(2);

                switch (acao)
                {
                    case 0:
                        if (Herbivoros[i] > 2)
                        {
                            Herbivoros[i] -= 2;
                            Herbivoros.Add(2);
                        }
                        break;
                    case 1:
                        Herbivoros[i]++;
                        Vitalidade--;
                        break;
                }
            }
        }
        static void acaoCarn(List<int> Herbivoros, List<int> Carnivoros, int Vitalidade)
        {
            for (int i = 0; i < Carnivoros.Count; i++)
            {
                int acao = new Random().Next(4);

                switch (acao)
                {
                    case 0:
                        if (Carnivoros[i] > 1)
                        {
                            Carnivoros[i] -= 1;
                            Vitalidade++;
                        }
                        else
                        {
                            Vitalidade += Carnivoros[i];
                            Carnivoros.RemoveAt(i);
                        }
                        break;
                    case 1:
                        if (Carnivoros[i] > 3)
                        {
                            Carnivoros[i] -= 3;
                            Carnivoros.Add(3);
                        }
                        break;
                    case 2:
                    case 3:
                        if (Herbivoros.Count > 0)
                        {
                            Carnivoros[i] += Herbivoros[0];
                            Herbivoros.RemoveAt(0);
                        }
                        else
                            goto case 0;
                        break;
                    case 4:
                        Vitalidade += Carnivoros[i];
                        Herbivoros.RemoveAt(i);
                        break;
                }
            }
        }
        #endregion
    }
}
