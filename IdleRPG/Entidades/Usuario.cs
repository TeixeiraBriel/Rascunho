using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleRPG.Entidades
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Nivel { get; set; }
        public int Vida { get; set; }
        public int Defesa { get; set; }
        public int Dano { get; set; }
        public int VelAtaque { get; set; }

        public Usuario(int id, string user, string pass) 
        {
            this.Id = id;
            this.Username = user; 
            this.Password = pass;

            this.Nivel = 1;
            this.Vida = 10;
            this.Defesa = 2;
            this.Dano = 2;
            this.VelAtaque = 1;
        }
    }
}
