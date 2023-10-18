using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLikeRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool sair = false;
            bonecao personagem = new bonecao() { Nome = "Inimigo", Dano = 1, Vida = 1, Defesa = 0 };
            bonecao inimigo = new bonecao() { Nome = "Eu", Dano = 1, Vida = 1, Defesa = 0 };
            do
            {
                Console.WriteLine
                    (
                    $"Olá, esses são seus status:" +
                    $"\nVida:{personagem.Vida}" +
                    $"\nDano:{personagem.Dano}" +
                    $"\nDefesa:{personagem.Defesa}" +
                    $"\n" +
                    $"\n" +
                    "\nSelecione um up:" +
                    "\n0 - Vida" +
                    "\n1 - Dano" +
                    "\n2 - Defesa"
                    );

                string escolha = Console.ReadLine();
                personagem = subirNivel(personagem, escolha, 1);


                Console.WriteLine
                    (
                    $"\n" +
                    $"\nCombate:" +
                    $"\nSua Vida:{personagem.Vida}  |   Vida Inimigo:{inimigo.Vida}" +
                    $"\nSua Dano:{personagem.Dano}  |   Dano Inimigo:{inimigo.Dano}" +
                    $"\nSua Defesa:{inimigo.Defesa}  |   Defesa Inimigo:{inimigo.Defesa}"
                    );

                int[] vidas = new int[2] { personagem.Vida, inimigo.Vida };
                Console.WriteLine($"Vidas: Sua{vidas[0]}|Inimigo{vidas[1]}");
                do
                {
                    System.Threading.Thread.Sleep(800);
                    vidas[0] -= inimigo.Dano - personagem.Defesa;
                    Console.WriteLine($"Você recebe um ataque, sua vida {vidas[0]}");
                    if (inimigo.Dano <= 0)
                        continue;

                    System.Threading.Thread.Sleep(800);
                    vidas[1] -= personagem.Dano - inimigo.Defesa;
                    Console.WriteLine($"Você ataca, vida inimiga {vidas[1]}");
                } while (vidas[0] > 0 && vidas[1] > 0);

                if (vidas[0] <= 0)
                    Console.WriteLine($"Você morreu!");
                else
                    Console.WriteLine($"Você venceu!");

                Console.ReadKey();
                Console.Clear();
            } while (!sair);
        }

        static bonecao subirNivel(bonecao mudar, string tipo, int qtd)
        {
            switch (tipo)
            {
                case "0":
                    mudar.Vida += qtd;
                    break;
                case "1":
                    mudar.Dano += qtd;
                    break;
                case "2":
                    mudar.Defesa += qtd;
                    break;
            }

            return mudar;
        }
    }
}
