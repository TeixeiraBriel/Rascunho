using IdleRPG.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpandoLibrary;

namespace IdleRPG.Executores
{
    public static class InfoJogador
    {
        public static void Buscar(int id)
        {
            if (id == 0)
                Console.WriteLine("Você não esta logado.");
            else
                imprimirInfo(id);
        }

        static void imprimirInfo(int id)
        {
            var pathListaJogadores = @"Dados\ListaJogadores.json";
            var listaJogadores = JsonConvert.DeserializeObject<Jogadores>(File.ReadAllText(pathListaJogadores, Encoding.UTF8));
            var LoginTentado = listaJogadores.ListaJogadores.FirstOrDefault(x => x.Item1 == id);

            var pathUsuario = $@"Dados\{LoginTentado.Item2}.json";
            var usuarioEncontrado = JsonConvert.DeserializeObject<Usuario>(File.ReadAllText(pathUsuario, Encoding.UTF8));

            foreach (var item in usuarioEncontrado.ToExpando())
            {
                if (item.Key != "Password")
                {
                    Console.WriteLine($"{item.Key}:{item.Value}");
                }
            }
        }
    }
}
