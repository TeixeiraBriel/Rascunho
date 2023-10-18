using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleRPG.Entidades
{
    internal class Inimigos
    {
        public List<Inimigo> ListaInimigos { get; set; }

        public Inimigos() 
        {
            ListaInimigos = new List<Inimigo>();
        }
    }
    internal class Inimigo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }
        public int Vida { get; set; }
        public int Defesa { get; set; }
        public int Dano { get; set; }
        public int VelAtaque { get; set; }
    }
}
