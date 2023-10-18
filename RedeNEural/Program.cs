using RedeNEural;
using RedeNEural.Redes;
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
            Console.WriteLine($"Run: {contadorRun}");
            //PrimeiraRede.Executa();
            //SegundaRedeClass.execute();
            //TerceiraRedeClass.execute();
            QuartaRede.Executa();

            contadorRun++;
            Console.ReadLine();
        } while (true);
    }
}

