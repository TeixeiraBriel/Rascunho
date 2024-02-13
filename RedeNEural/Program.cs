using RedeNEural;
using RedeNEural.Redes;
using RedeNEural.Redes._2048;
using System;
using System.Collections.Generic;
using System.Linq;
using static RedeNEural.PrimeiraRede;

class Program
{
    static void Main()
    {
        int contadorRun = 0;
        do
        {
            //Console.WriteLine($"Run: {contadorRun}");
            //PrimeiraRede.Executa();
            //SegundaRedeClass.execute();
            //TerceiraRedeClass.execute();
            //QuartaRede.Executa();
            //QuintaRede.Executa();
            new SextaRede().Run(50);

            contadorRun++;
            Console.ReadLine();
        } while (true);
    }
}

