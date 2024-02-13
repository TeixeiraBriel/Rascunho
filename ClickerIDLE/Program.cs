using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClickerIDLE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int contadorLoop = 0;
            do
            {
                Console.WriteLine("VAI SAIR CLICANDO IGUAL DOIDO NA SUA TELA, CERTEZA Q VAI CONTINUAR?");
                Console.ReadLine();
                Console.WriteLine($"Processo iniciado - Loop N{contadorLoop}");
                //GeneticGame();
                IddleChamps();
                Console.WriteLine("Processo Pausado");
                for (int i = 0; i < 9; i++)
                {
                    Console.WriteLine(i + 1);
                    Thread.Sleep(1000);
                }

                contadorLoop++;
            } while (true);
            Console.WriteLine("Processo finalizado");
            Console.ReadLine();
        }

        static void IddleChamps()
        {
            //Run
            AutoIt.AutoItX.MouseMove(150, 930);
            AutoIt.AutoItX.MouseClick();

            //UPS
            AutoIt.AutoItX.MouseMove(1750, 930);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(1380, 980);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(1380, 880);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(1280, 980);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(1870, 850);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(1770, 830);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(1850, 980);
            AutoIt.AutoItX.MouseClick();
            //EndUps

            AutoIt.AutoItX.MouseMove(100, 1050);
            AutoIt.AutoItX.MouseClick();

            for (int i = 0; i < 530; i++)
            {
                AutoIt.AutoItX.MouseClick();
            }

            //Run
            AutoIt.AutoItX.MouseMove(150, 930);
            AutoIt.AutoItX.MouseClick();

            AutoIt.AutoItX.MouseMove(100, 1050);

            for (int i = 0; i < 750; i++)
            {
                AutoIt.AutoItX.MouseClick();
            }

            //Run
            AutoIt.AutoItX.MouseMove(150, 930);
            AutoIt.AutoItX.MouseClick();

            if (false)
            {
                Thread.Sleep(8500);
            }
            else
            {
                AutoIt.AutoItX.MouseMove(100, 1050);
                for (int i = 0; i < 550; i++)
                {
                    AutoIt.AutoItX.MouseClick();
                }
            }

            AutoIt.AutoItX.MouseMove(500, 500);
        }

        static void GeneticGame()
        {
            AutoIt.AutoItX.MouseMove(-800, 800);
            AutoIt.AutoItX.MouseClick();

            int num = 200;
            AutoIt.AutoItX.MouseMove(-1500, num);
            AutoIt.AutoItX.MouseClick();

            for (int i = 0; i < 8; i++)
            {
                num += 100;
                AutoIt.AutoItX.MouseMove(-1500, num);
                AutoIt.AutoItX.MouseClick();
            }

            AutoIt.AutoItX.MouseMove(-800, 800);
            for (int i = 0; i < 1000; i++)
            {
                AutoIt.AutoItX.MouseClick();
            }
        }
    }
}
