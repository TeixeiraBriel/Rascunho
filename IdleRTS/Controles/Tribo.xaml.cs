using IdleRTS.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace IdleRTS.Controles
{
    /// <summary>
    /// Interação lógica para Tribo.xam
    /// </summary>
    public partial class Tribo : Page
    {

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public int contadorSegundos = 0;
        public int contadorMinutos = 0;
        public int contadorHoras = 0;
        public TimeSpan contadorData = new TimeSpan(0, 0, 0);
        TriboDados triboDados = new TriboDados();

        public Tribo()
        {
            InitializeComponent();
            Infos.Text = $"Madeira: {triboDados.Madeira} - Comida: {triboDados.Comida} - Ouro: {triboDados.Ouro}";
            inicializaTimer();
        }

        public void inicializaTimer()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 700);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                contadorData += TimeSpan.FromSeconds(1);
                Contador.Text = $"Tempo Jogado: {contadorData.Hours} Horas - {contadorData.Minutes} Minutos - {contadorData.Seconds} Segundos";
                triboDados.ColetaPassiva();
                var textoFarmAtivoInfo = "";

                switch (cmbProd.Text)
                {
                    case "Madeira":
                        triboDados.adicionaMadeira(false);
                        textoFarmAtivoInfo = $"{(triboDados.qtdPessoasMadeira + 1) * triboDados.MelhoriaMadeira}/s";
                        break;
                    case "Comida":
                        triboDados.adicionaComida(false);
                        textoFarmAtivoInfo = $"{(triboDados.qtdPessoasMadeira + 1) * triboDados.MelhoriaComida}/s";
                        break;
                    case "Ouro":
                        triboDados.adicionaOuro(false);
                        textoFarmAtivoInfo = $"{(triboDados.qtdPessoasMadeira + 1) * triboDados.MelhoriaOuro}/s";
                        break;
                }
                FarmAtivoInfo.Text = textoFarmAtivoInfo;
                Infos.Text = $"Madeira: {triboDados.Madeira} - Comida: {triboDados.Comida} - Ouro: {triboDados.Ouro}";
            }
            catch (Exception ex)
            {
                string messageBoxText = ex.Message;
                string caption = "Erro ao executar";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon);

                dispatcherTimer.Stop();
            }
        }

        public void ZerarContadorFunc()
        {
            contadorData = new TimeSpan(0, 0, 0);
            //TesteCampo.Text = $"{contadorData.Hours} Horas - {contadorData.Minutes} Minutos - {contadorData.Seconds} Segundos";
        }

        private void ComprarMelhoria(object sender, RoutedEventArgs e)
        {
            if (/*triboDados.Ouro > 100*/true)
            {
                switch (cmbProd.Text)
                {
                    case "Madeira":
                        triboDados.MelhoriaMadeira++;
                        break;
                    case "Comida":
                        triboDados.MelhoriaComida++;
                        break;
                    case "Ouro":
                        triboDados.MelhoriaOuro++;
                        break;
                }
                triboDados.Ouro -= 100;
            }
        }
    }
}
