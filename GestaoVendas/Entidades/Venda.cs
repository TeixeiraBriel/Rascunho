using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoVendas.Entidades
{
    internal class Venda
    {
        public int CodigoProduto { get; set; }
        public int IdFuncionario { get; set; }
        public int Quantidade { get; set; }
        public double Valor { get; set; }

    }
}
