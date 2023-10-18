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
            //TerceiraRedeClass.execute();
            primeiraRede();
            //SegundaRedeClass.execute();

            contadorRun++;
            Console.ReadLine();
        } while (true);
    }

    static void primeiraRede()
    {
        // Definir quantidade de episodios
        int episodiosTreino = 10;

        // Definir o tamanho da grade
        int gridSize = 5;

        // Inicializar o ambiente de grade
        GridEnvironment gridEnv = new GridEnvironment(gridSize);

        List<double[,]> qtables = new List<double[,]>();
        for (int i = 0; i < 10; i++)
        {
            qtables.Add(treinarRedeNeural(gridEnv, gridSize, episodiosTreino));
        }

        List<(int, string)> values = new List<(int, string)>();
        int count = 1;

        foreach (var qtable in qtables)
        {
            string nome = "Agente" + count;
            int passos = executaRedeNeural(gridEnv, qtable, nome);
            values.Add((passos, nome));
            count++;
        }

        values.OrderBy(x => x.Item1).ToList().ForEach(x => Console.WriteLine($"{x.Item2}:{x.Item1} passos."));
    }
}

