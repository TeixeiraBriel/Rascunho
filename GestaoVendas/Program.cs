using GestaoVendas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoVendas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Gestao _Gestao = new Gestao();
            _Gestao.Inicia();
            bool sair = false;
            bool primeiraRotacao = true;
            do
            {
                Console.Clear();
                if (!primeiraRotacao)
                    imprimirEstoque(_Gestao);

                Console.WriteLine("" +
                    "\nMenu:\r" +
                    "\n========================================\r" +
                    "\n1 - Cadastrar produtos\r" +
                    "\n2 - Realizar uma venda\r" +
                    "\n3 - Relatório de vendas\r" +
                    "\n4 - Relatório de vendas por funcionários\r" +
                    "\n0 - Sair\r" +
                    "\n");

                int decisao = int.Parse(Console.ReadLine());
                switch (decisao)
                {
                    case 0:
                        sair = true;
                        break;
                    case 1:
                        CadastrarProduto(_Gestao);
                        Continuar();
                        break;
                    case 2:
                        RealizarVenda(_Gestao);
                        Continuar();
                        break;
                    case 3:
                        RelatorioVendas(_Gestao);
                        Continuar();
                        break;
                    case 4:
                        RelatorioVendaFuncionario(_Gestao);
                        Continuar();
                        break;
                }
                primeiraRotacao = false;
            } while (!sair);
        }

        static void imprimirEstoque(Gestao _Gestao)
        {
            Console.WriteLine("Estoque Atual:");
            foreach (var item in _Gestao.Estoque)
            {
                if (item.Quantidade > 0)
                {
                    Console.WriteLine($"{item.Id} | {item.Descricao} | {item.Quantidade} | {item.Valor}");
                }
            }
        }
        static void CadastrarProduto(Gestao _Gestao)
        {
            Console.WriteLine("\nCadastrar produtos\n");
            Produto produto = new Produto();
            produto.Id = _Gestao.Estoque.Count() + 1;
            Console.Write("Descricao: ");
            produto.Descricao = Console.ReadLine();
            Console.Write("Quantidade: ");
            produto.Quantidade = int.Parse(Console.ReadLine());
            Console.Write("Valor: ");
            produto.Valor = double.Parse(Console.ReadLine());

            _Gestao.Estoque.Add(produto);
            Console.WriteLine($"\nItem {produto.Descricao} cadastrado com sucesso!\n");
        }
        static void RealizarVenda(Gestao _Gestao)
        {
            Console.WriteLine("\nRealizar uma venda\n");

            Venda venda = new Venda();
            Console.Write("Codigo do produto: ");
            venda.CodigoProduto = int.Parse(Console.ReadLine());
            Console.Write("Quantidade: ");
            venda.Quantidade = int.Parse(Console.ReadLine());
            Console.Write("Codigo do Funcionario: ");
            venda.IdFuncionario = int.Parse(Console.ReadLine());

            var produtoNoEstoque = _Gestao.Estoque.FirstOrDefault(x => x.Id == venda.CodigoProduto);

            if (produtoNoEstoque != null && produtoNoEstoque.Quantidade - venda.Quantidade >= 0)
            {
                venda.Valor = produtoNoEstoque.Valor * venda.Quantidade;
                _Gestao.Vendas.Add(venda);
                _Gestao.Estoque.FirstOrDefault(x => x.Id == venda.CodigoProduto).Quantidade = produtoNoEstoque.Quantidade - venda.Quantidade;
                Console.WriteLine($"\nItem {produtoNoEstoque.Descricao}x{venda.Quantidade} vendido com sucesso no valro total de R${venda.Valor}!\n");
            }
            else
            {
                Console.WriteLine("Não foi possivel realizar a venda");
            }
        }
        static void RelatorioVendas(Gestao _Gestao)
        {
            Console.WriteLine("\nRelatório de vendas\n");
            double valorTotal = 0;
            foreach (var item in _Gestao.Vendas)
            {
                Console.WriteLine($"{item.CodigoProduto} | {item.IdFuncionario} | {item.Valor}");
                valorTotal += item.Valor;
            }
            Console.WriteLine($"Total = {valorTotal}");
        }
        static void RelatorioVendaFuncionario(Gestao _Gestao)
        {
            Console.WriteLine("\nRelatório de vendas por funcionários\n");
            Console.Write("Id Funcionario: ");
            int idFuncionario = int.Parse(Console.ReadLine());
            double valorTotal = 0;
            foreach (var item in _Gestao.Vendas)
            {
                if (item.IdFuncionario == idFuncionario)
                {
                    Console.WriteLine($"{item.CodigoProduto} | {item.IdFuncionario} | {item.Valor}");
                    valorTotal += item.Valor;
                }
            }
            Console.WriteLine($"Total = {valorTotal}");
        }
        static void Continuar()
        {
            Console.Write("Clique qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
