using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleRTS.Entidades
{
    internal class TriboDados
    {
        public double Madeira { get; set; }
        public double Comida { get; set; }
        public double Ouro { get; set; }

        public double MelhoriaMadeira { get; set; }
        public double MelhoriaComida { get; set; }
        public double MelhoriaOuro { get; set; }

        public int qtdPessoasMadeira { get; set; }
        public int qtdPessoasComida { get; set; }
        public int qtdPessoasOuro { get; set; }

        public int qtdPessoasMadeiraMax { get; set; }
        public int qtdPessoasComidaMax { get; set; }
        public int qtdPessoasOuroMax { get; set; }


        public TriboDados() 
        {
            qtdPessoasMadeiraMax = 1;
            qtdPessoasComidaMax = 1;
            qtdPessoasOuroMax = 1;
            MelhoriaMadeira = 1;
            MelhoriaComida = 1;
            MelhoriaOuro = 1;
        }

        public void ColetaPassiva()
        {
            adicionaMadeira();
            adicionaComida();
            adicionaOuro();
        }

        public void adicionaMadeira(bool passivo = true)
        {
            int qtd = passivo ? qtdPessoasMadeira : (qtdPessoasMadeira + 1);
            Madeira += qtd * MelhoriaMadeira;
        }
        public void adicionaComida(bool passivo = true)
        {
            int qtd = passivo ? qtdPessoasComida : (qtdPessoasComida + 1);
            Comida += qtd * MelhoriaComida;
        }
        public void adicionaOuro(bool passivo = true)
        {
            int qtd = passivo ? qtdPessoasOuro : (qtdPessoasOuro + 1);
            Ouro += qtd * MelhoriaOuro;
        }
    }
}
