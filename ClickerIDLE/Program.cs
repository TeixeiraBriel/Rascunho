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
                Console.WriteLine($"Processo iniciado - Loop N{contadorLoop}");
                GeneticGame();
                //IddleChamps();
                Console.WriteLine("Processo Pausado");
                for (int i = 0; i < 9; i++)
                {
                    Console.WriteLine(i+1);
                    Thread.Sleep(1000);
                }

                contadorLoop++;
            } while (true);
            Console.WriteLine("Processo finalizado");
            Console.ReadLine();
        }

        static void IddleChamps()
        {
            AutoIt.AutoItX.MouseMove(-70, 220);
            AutoIt.AutoItX.MouseClick();

            Thread.Sleep(1500);

            var num = -1735;
            AutoIt.AutoItX.MouseMove(num, 1050);
            AutoIt.AutoItX.MouseClick();

            num = -1600;
            AutoIt.AutoItX.MouseMove(num, 1050);
            AutoIt.AutoItX.MouseClick();

            for (int i = 0; i < 8; i++)
            {
                num += 170;
                AutoIt.AutoItX.MouseMove(num, 1050);
                AutoIt.AutoItX.MouseClick();
            }

            AutoIt.AutoItX.MouseMove(-300, 500);
            for (int i = 0; i < 1000; i++)
            {
                AutoIt.AutoItX.MouseClick();
            }
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
