using CobrinhaNeural.Entidades;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xaml;

namespace CobrinhaNeural
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public double pontos;
        int movimentos;
        int comidasComidas;
        int tamanhoGridX;
        int tamanhoGridY;
        private bool[,] gridComida;
        TextBlock comidaAtual;
        Cobra cobra;
        RedeNeural redeNeural;

        public MainWindow()
        {
            InitializeComponent();
            iniciaVariaveis();
            iniciaGrid();

            resetAll();
        }

        void resetAll()
        {
            GridPai.Children.Clear();
            iniciaVariaveis();
            GeraCobra(true);
            GeraCobra();
            GeraComida();
            atualizarDados();
        }

        void iniciaVariaveis()
        {
            tamanhoGridX = 10;
            tamanhoGridY = 10;
            cobra = new Cobra();
            gridComida = new bool[tamanhoGridX, tamanhoGridY];
            pontos = 0;
            movimentos = 0;
            comidasComidas = 0;
        }

        void iniciaGrid()
        {
            Grid DynamicGrid = GridPai;
            DynamicGrid.ShowGridLines = true;
            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);
            DynamicGrid.Width = 500;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Center;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Center;

            for (int x = 0; x < tamanhoGridX; x++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(50);
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }
            for (int y = 0; y < tamanhoGridY; y++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(50);
                DynamicGrid.RowDefinitions.Add(gridRow);
            }
        }

        private void GeraCobra(bool cabeca = false, bool crescer = false)
        {
            TextBlock parteCobra = new TextBlock();

            if (cabeca)
                parteCobra.FontSize = 35;
            else
                parteCobra.FontSize = 20;

            parteCobra.Text = "*";
            parteCobra.FontWeight = FontWeights.Bold;
            parteCobra.Foreground = new SolidColorBrush(Colors.Red);
            parteCobra.VerticalAlignment = VerticalAlignment.Center;
            parteCobra.HorizontalAlignment = HorizontalAlignment.Center;

            if (cabeca)
            {
                int valX = new Random().Next(tamanhoGridX - 1);
                int valY = new Random().Next(tamanhoGridY - 1);

                Grid.SetColumn(parteCobra, valX);
                Grid.SetRow(parteCobra, valY);

                cobra.posX = valX;
                cobra.posY = valY;
            }
            else if (crescer)
            {
                (int prevX, int prevY) = cobra.retornaPrevisaoPasso(true);
                int modX = prevX != cobra.posX ? prevX > cobra.posX ? 1 : -1 : 0;
                int modY = prevY != cobra.posY ? prevY > cobra.posY ? 1 : -1 : 0;

                int valX = cobra.posCorpo.LastOrDefault().X + modX;
                int valY = cobra.posCorpo.LastOrDefault().Y + modY;

                valX = cobra.posCrescer.X;
                valY = cobra.posCrescer.Y;

                Grid.SetColumn(parteCobra, valX);
                Grid.SetRow(parteCobra, valY);
                cobra.posCorpo.Add((valX, valY));
            }
            else
            {
                (int prevX, int prevY) = cobra.retornaPrevisaoPasso(true);
                Grid.SetColumn(parteCobra, prevX);
                Grid.SetRow(parteCobra, prevY);
                cobra.posCorpo.Add((prevX, prevY));
            }

            GridPai.Children.Add(parteCobra);
            cobra.textBlocks.Add(parteCobra);
        }

        private void GeraComida()
        {
            TextBlock comida = new TextBlock();

            comida.Text = "*";
            comida.FontSize = 25;
            comida.FontWeight = FontWeights.Bold;
            comida.Foreground = new SolidColorBrush(Colors.Green);
            comida.VerticalAlignment = VerticalAlignment.Center;
            comida.HorizontalAlignment = HorizontalAlignment.Center;

            var lugaresVagos = obtemLugaresVagos();
            if (lugaresVagos.Count == 0)
            {
                FimDeJogo();
                return;
            }

            (int X, int Y) lugarNascer = lugaresVagos[new Random().Next(lugaresVagos.Count)];

            Grid.SetColumn(comida, lugarNascer.X);
            Grid.SetRow(comida, lugarNascer.Y);

            GridPai.Children.Add(comida);
            gridComida[lugarNascer.X, lugarNascer.Y] = true;
            comidaAtual = comida;
        }

        void moverCobraFrente()
        {
            movimentos++;
            int direcao = cobra.frente; //0: UP(), 1: DOWN, 2:LEFT, 3:RIGTH

            switch (direcao)
            {
                case 0:
                    if (validarMovimento(cobra.posX - 1, cobra.posY))
                        cobra.moverCobra(GridPai, -1, "X");
                    break;
                case 1:
                    if (validarMovimento(cobra.posX, cobra.posY + 1))
                        cobra.moverCobra(GridPai, 1, "Y");
                    break;
                case 2:
                    if (validarMovimento(cobra.posX + 1, cobra.posY))
                        cobra.moverCobra(GridPai, 1, "X");
                    break;
                case 3:
                    if (validarMovimento(cobra.posX, cobra.posY - 1))
                        cobra.moverCobra(GridPai, -1, "Y");
                    break;
            }
        }

        bool validarMovimento(int posAlvoX, int posAlvoY)
        {
            var lugaresVagos = obtemLugaresVagos();
            bool lugarVazio = lugaresVagos.Exists(local => local.y == posAlvoY && local.x == posAlvoX);
            bool dentroLimitesX = posAlvoX >= 0 && posAlvoX < tamanhoGridX ? true : false;
            bool dentroLimitesY = posAlvoY >= 0 && posAlvoY < tamanhoGridX ? true : false;

            if (dentroLimitesX && dentroLimitesY && !lugarVazio)
            {
                FimDeJogo();
            }

            bool locaLivre = lugarVazio && dentroLimitesX && dentroLimitesY;
            return locaLivre;
        }

        void atualizarDados()
        {
            txtDados.Text = $"Pontos: {pontos} Movimentos:{movimentos} Comeu:{comidasComidas} CobraX:{cobra.posX} CobraY:{cobra.posY} Olhando:{cobra.direcoesTradutor[cobra.frente]} Filhos:{GridPai.Children.Count}";
        }

        void FimDeJogo()
        {
            resetAll();
        }

        List<(int x, int y)> obtemLugaresVagos()
        {

            bool[,] lugarOcupado = new bool[10, 10];
            cobra.posCorpo.ToList().ForEach(x => lugarOcupado[x.X, x.Y] = true);
            lugarOcupado[cobra.posX, cobra.posY] = true;

            List<(int x, int y)> lugaresVagos = new List<(int x, int y)>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (!lugarOcupado[i, j])
                    {
                        lugaresVagos.Add((i, j));
                    }
                }
            }

            return lugaresVagos;
        }


        #region ClickFunctions

        public async void executaVirarEsquerda()
        {
            cobra.virar(1);
            atualizarDados();
        }

        public async void executaVirarDireita()
        {
            cobra.virar(-1);
            atualizarDados();
        }

        public async void executaSeguirReto()
        {
            (int prevX, int prevY) = cobra.retornaPrevisaoPasso();
            if (prevX >= 0 &&
                prevY >= 0 &&
                prevX < tamanhoGridX &&
                prevY < tamanhoGridY &&
                gridComida[prevX, prevY])
            {
                GridPai.Children.Remove(comidaAtual);
                gridComida[prevX, prevY] = false;
                pontos += 10 + (movimentos * -0.1);
                comidasComidas++;
                moverCobraFrente();
                GeraCobra(false, true);
                GeraComida();
            }
            else
            {
                moverCobraFrente();
            }

            atualizarDados();
        }

        #endregion

        #region CLICKS
        private void VirarEsquerda(object sender, RoutedEventArgs e)
        {
            executaVirarEsquerda();
        }

        private void VirarDireita(object sender, RoutedEventArgs e)
        {
            executaVirarDireita();
        }

        private void SeguirReto(object sender, RoutedEventArgs e)
        {
            executaSeguirReto();
        }
        #endregion

        void inicializaRedeNeural()
        {
            redeNeural = new RedeNeural(cobra, gridComida, this);
            redeNeural.inicializa();
        }

        private bool RedeInicializada = false;

        private void ExecutaTreinamento(object sender, RoutedEventArgs e)
        {
            if (!RedeInicializada)
            {
                inicializaRedeNeural();
                RedeInicializada = true;
            }

            //redeNeural.executaEpisodio();
            redeNeural.Executa();
        }

        private void ExecutaSimulacao(object sender, RoutedEventArgs e)
        {
            if (!RedeInicializada)
            {
                inicializaRedeNeural();
                RedeInicializada = true;
            }

            redeNeural.executaEpisodio();
            //redeNeural.Executa();

        }
    }
}
