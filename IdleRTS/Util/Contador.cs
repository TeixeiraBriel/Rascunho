using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

namespace IdleRTS.Util
{
    internal class Contador
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public int contadorSegundos = 0;
        public int contadorMinutos = 0;
        public int contadorHoras = 0;
        public TimeSpan contadorData = new TimeSpan(0, 0, 0);

        public void inicializaTimer()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                contadorData += TimeSpan.FromSeconds(1);

                //TesteCampo.Text = $"{contadorData.Hours} Horas - {contadorData.Minutes} Minutos - {contadorData.Seconds} Segundos";
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
    }
}
