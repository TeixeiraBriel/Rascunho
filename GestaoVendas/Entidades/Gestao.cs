using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoVendas.Entidades
{
    internal class Gestao
    {
        public List<Produto> Estoque { get; set; }
        public List<Venda> Vendas { get; set; }

        public void Inicia()
        {
            Estoque = new List<Produto>();
            Vendas = new List<Venda>();
        }
    }
}
