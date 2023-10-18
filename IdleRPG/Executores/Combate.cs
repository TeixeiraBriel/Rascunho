using IdleRPG.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XpandoLibrary;

namespace IdleRPG.Executores
{
    public static class Combate
    {
        public static void Inicia(int id)
        {
            Usuario usuario;
            Inimigos inimigosAptos;
            Inimigo inimigo;

            if (id == 0)
                Console.WriteLine("Você não esta logado.");
            else
            {
                usuario = InicializaUsuario(id);
                inimigosAptos = InicializaInimigos(usuario.Nivel);
                inimigo = inimigosAptos.ListaInimigos[0];
                bool sair = false;
                bool combateReset = true;
                do
                {
                    if (combateReset)
                    {
                        inimigo.Vida = InicializaInimigos(usuario.Nivel).ListaInimigos[0].Vida;
                        usuario.Vida = InicializaUsuario(id).Vida;
                        combateReset = false;
                        Console.Clear();
                    }

                    Thread.Sleep(500);
                    int usuarioDano = (usuario.Dano * usuario.VelAtaque) - inimigo.Defesa;
                    Console.WriteLine($"{usuario.Username} desfere {usuario.VelAtaque} golpe(s) que causa total de {usuarioDano} de dano.");
                    inimigo.Vida -= usuarioDano;
                    if (inimigo.Vida <= 0)
                    {
                        Console.WriteLine($"Vida {usuario.Username}:{usuario.Vida} || Vida {inimigo.Nome}:{inimigo.Vida} \n");
                        Console.WriteLine("Voce Venceu combate \n");
                        sair = validaContinuar();
                        combateReset = true;
                        continue;
                    }

                    Thread.Sleep(500);
                    int inimigoDano = (inimigo.Dano * inimigo.VelAtaque) - usuario.Defesa;
                    Console.WriteLine($"{inimigo.Nome} desfere {inimigo.VelAtaque} golpe(s) que causa total de {inimigoDano} de dano.");
                    usuario.Vida -= inimigoDano;
                    if (usuario.Vida <= 0)
                    {
                        Console.WriteLine($"Vida {usuario.Username}:{usuario.Vida} || Vida {inimigo.Nome}:{inimigo.Vida} \n");
                        Console.WriteLine("Voce perdeu o combate \n");
                        sair = validaContinuar();
                        combateReset = true;
                        continue;
                    }

                    Console.WriteLine($"Vida {usuario.Username}:{usuario.Vida} || Vida {inimigo.Nome}:{inimigo.Vida} \n");
                } while (!sair);
            }
        }

        static Usuario InicializaUsuario(int id)
        {
            var pathListaJogadores = @"Dados\ListaJogadores.json";
            var listaJogadores = JsonConvert.DeserializeObject<Jogadores>(File.ReadAllText(pathListaJogadores, Encoding.UTF8));
            var LoginTentado = listaJogadores.ListaJogadores.FirstOrDefault(x => x.Item1 == id);

            var pathUsuario = $@"Dados\{LoginTentado.Item2}.json";
            var usuarioEncontrado = JsonConvert.DeserializeObject<Usuario>(File.ReadAllText(pathUsuario, Encoding.UTF8));

            return usuarioEncontrado;
        }

        static Inimigos InicializaInimigos(int nivelMonstro)
        {
            var pathInimigos = @"Dados\Inimigos.json";
            var listaInimigos = JsonConvert.DeserializeObject<Inimigos>(File.ReadAllText(pathInimigos, Encoding.UTF8));
            var inimigosAptos = new Inimigos() { ListaInimigos = listaInimigos.ListaInimigos.FindAll(x => x.Nivel == nivelMonstro).ToList() };

            return inimigosAptos;
        }

        static bool validaContinuar()
        {
            int decisao = 10;
            do
            {
                Console.WriteLine("" +
                    "Deseja continuar?:\r" +
                    "\n========================================\r" +
                    //"\n1 - Novo monstro\r" +
                    "\n1 - Repetir monstro\r" +
                    "\n0 - Sair\r" +
                    "\n");

                try
                {
                    decisao = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Favor, digitar apenas o numero.");
                    continue;
                }

                switch (decisao)
                {
                    case 0:
                        return true;
                        break;
                    case 1:
                        return false;
                        break;
                    default:
                        Console.WriteLine("Opção não encontrada.");
                        break;
                }

            } while (true);
        }
    }
}
