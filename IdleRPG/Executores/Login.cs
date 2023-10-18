using IdleRPG.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleRPG.Executores
{
    public static class Login
    {
        public static int Logar()
        {
            Console.Write("Usuario: ");
            string user = Console.ReadLine().ToLower().Replace(" ","");
            Console.Write("Senha: ");
            string pass = Console.ReadLine();
            var usuario = metodoLogin(user, pass);

            if (usuario == null)
            {
                Console.WriteLine("Não foi possivel logar.");
                return 0;
            }
            else
            { 
                Console.WriteLine("Usuario Logado com sucesso.");
                return usuario.Id;
            }
        }

        static Usuario metodoLogin(string username, string pass)
        {
            var pathListaJogadores = @"Dados\ListaJogadores.json";
            var pathUsuario = $@"Dados\{username}.json";

            var listaJogadores = JsonConvert.DeserializeObject<Jogadores>(File.ReadAllText(pathListaJogadores, Encoding.UTF8));
            var LoginTentado = listaJogadores.ListaJogadores.FirstOrDefault(x => x.Item2 == username);
            if (LoginTentado == null)
            {
                Usuario novoUsuario = new Usuario(listaJogadores.ListaJogadores.LastOrDefault().Item1 + 1, username, pass);
                listaJogadores.ListaJogadores.Add(new Tuple<int, string>(novoUsuario.Id, novoUsuario.Username));

                using (var sw = new StreamWriter(pathUsuario, true))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(novoUsuario));
                    sw.Close();
                }

                File.Delete(pathListaJogadores);
                using (var sw = new StreamWriter(pathListaJogadores, true))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(listaJogadores));
                    sw.Close();
                }
            }

            var usuarioEncontrado = JsonConvert.DeserializeObject<Usuario>(File.ReadAllText(pathUsuario, Encoding.UTF8));
            if (usuarioEncontrado.Password == pass)
            {
                return usuarioEncontrado;
            }
            else
            {
                return null;
            }
        }
    }
}
