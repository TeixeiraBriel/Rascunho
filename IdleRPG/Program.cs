using BrielinaIdleRpg.Entidades;
using BrielinaIdleRpg.Executores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool sair = false;
            int idLogado = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("" +
                    "Menu:\r" +
                    "\n========================================\r" +
                    "\n1 - Logar\r" +
                    "\n2 - Informações Jogador\r" +
                    "\n3 - Craft\r" +
                    "\n4 - Combate\r" +
                    "\n0 - Sair\r" +
                    "\n");

                int decisao = 10;
                try
                {
                    decisao = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Favor, digitar apenas o numero.");
                    Continuar();
                    continue;
                }
                switch (decisao)
                {
                    case 0:
                        sair = true;
                        break;
                    case 1:
                        idLogado = Login.Logar();
                        Continuar();
                        break;
                    case 2:
                        InfoJogador.Buscar(idLogado);
                        Continuar();
                        break;
                    case 3:
                        Console.WriteLine("Craft");
                        Continuar();
                        break;
                    case 4:
                        Console.WriteLine("Combate");
                        Combate.Inicia(idLogado);
                        Continuar();
                        break;
                    default:
                        Console.WriteLine("Opção não encontrada.");
                        Continuar();
                        break;
                }

            } while (!sair);
        }



        static void Continuar()
        {
            Console.Write("\nClique qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
