using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using SeleniumExtras.WaitHelpers;

namespace RedeNEural.Redes._2048
{
    public class SextaRede
    {
        private IWebDriver _driver;
        private Qtable _Qtable;
        private int _GridGameSize = 4;
        private int _Episodios;
        private double _TaxaExploracao = 0.3;
        private double _learningRate = 0.1;
        private double _discountFactor = 0.7;
        private Random _Random = new Random();

        public void Run(int Episodios)
        {
            _Episodios = Episodios;

            Console.WriteLine("Abrindo Chrome");
            InicializaDriver();
            Console.WriteLine("Inicializando Qtable");
            InicializaQtable();

            _Qtable = JsonConvert.DeserializeObject<Qtable>(File.ReadAllText("Saves.json", Encoding.UTF8));
            int jogadas = 0;
            int record = 0;
            int recordJogadas = 0;
            do
            {
                //Busca Estado Atual
                List<Tile> EstadoAtual = ObtemEstadoAtualAmbiente();
                var scorePreAcao = _driver.FindElement(By.ClassName("score-container")).Text;

                //ValidaOpção existe
                ValidaCriaAcoesEmEstado(EstadoAtual);

                //Encontra melhor ação
                int action = EscolherAcao(EstadoAtual);

                //Executa Acao
                EnviarComandoDriver(action);
                //Thread.Sleep(2000);
                //var scorePosAcao = _driver.FindElement(By.ClassName("score-container")).Text;

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("game-container")));

                //wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("score-container")));
                var scorePosAcao = _driver.FindElement(By.ClassName("score-container")).Text;
                if (scorePosAcao.Contains('\r'))
                {
                    scorePosAcao = scorePosAcao.Split('\r')[0];
                }
                //var scorePosAcao = scoreContainer.Text;

                double recompensa = int.Parse(scorePosAcao) > int.Parse(scorePreAcao) ? 0.01: -0.01;
                int multp = int.Parse(scorePosAcao) > int.Parse(scorePreAcao) ? int.Parse(scorePosAcao) - int.Parse(scorePreAcao) : 1;
                recompensa *= multp;


                if ((int.Parse(scorePosAcao) > record || int.Parse(scorePosAcao) == record && jogadas < recordJogadas) || _driver.FindElements(By.ClassName("retry-button")).Count > 0 && _driver.FindElements(By.ClassName("retry-button"))[0].Displayed)
                {
                    recordJogadas = int.Parse(scorePosAcao) > record || int.Parse(scorePosAcao) == record && jogadas < recordJogadas ? jogadas : recordJogadas;
                    record = int.Parse(scorePosAcao) > record ? int.Parse(scorePosAcao) : record;
                    jogadas = -1;
                    resetGame();
                    _Episodios--;
                    if (_driver.FindElements(By.ClassName("retry-button")).Count > 0 && _driver.FindElements(By.ClassName("retry-button"))[0].Displayed)
                        recompensa = -1;

                    string output = JsonConvert.SerializeObject(_Qtable);
                    File.WriteAllText(@"Saves.json", output);
                }

                AtualizarQValue(EstadoAtual, action, recompensa);

                List<Tile> EstadoPosAction = ObtemEstadoAtualAmbiente();

                jogadas++;
                Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} - Episodio {Episodios - _Episodios} - Record {record} em {recordJogadas} - Laço executado, jogada {jogadas}, ação {action}, recompensa {recompensa}");
            } while (_Episodios > 0);
        }

        void InicializaDriver()
        {
            // Path para o chromedriver.exe no root do projeto
            string chromeDriverPath = "chromedriver.exe";

            // Configuração do ChromeDriver
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true; // Esconder a janela de prompt de comando do ChromeDriver

            // Configurações do ChromeOptions
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized"); // Abrir o navegador maximizado

            // Inicialização do ChromeDriver
            _driver = new ChromeDriver(service, options);

            // Exemplo de uso: carregar uma página e exibir o título
            string url = "https://play2048.co/";
            _driver.Navigate().GoToUrl(url);
        }

        void InicializaQtable()
        {
            _Qtable = new Qtable();
        }

        List<Tile> ObtemEstadoAtualAmbiente()
        {
            List<Tile> Mapeamento = new List<Tile>();

            int tentativas = 5;
            do
            {
                try
                {
                    var tilesOcupados = _driver.FindElements(By.XPath("//*[contains(@class, 'tile-position-')]"));
                    foreach (var tilesOcupado in tilesOcupados)
                    {
                        var value = tilesOcupado.Text;
                        var classValue = tilesOcupado.GetDomAttribute("class");
                        var posX = tilesOcupado.GetDomAttribute("class").Split(new string[] { "tile-position-" }, StringSplitOptions.None)[1].Substring(0, 1);
                        var posY = tilesOcupado.GetDomAttribute("class").Split(new string[] { "tile-position-" }, StringSplitOptions.None)[1].Substring(2, 1);

                        Mapeamento.Add(new Tile() { PosX = int.Parse(posX), PosY = int.Parse(posY), Valor = int.Parse(value) });
                    }
                    break;
                }
                catch
                {
                    tentativas--;
                    Thread.Sleep(1000);
                }
            } while (tentativas > 0);

            return Mapeamento;
        }

        int EscolherAcao(List<Tile> EstadoAtual)
        {
            int bestAction = 0;

            var chanceExploracao = new Random().NextDouble();
            if (chanceExploracao < _TaxaExploracao)
            {
                bestAction = new Random().Next(4);
            }
            else
            {
                double bestQValue = 0;
                bool first = true;

                List<int> actionsValidas = new List<int>() { 0, 1, 2, 3 };
                foreach (var action in actionsValidas)
                {
                    var estadoEscolhido = _Qtable.Estados.FirstOrDefault(x => x.Acao == action && JsonConvert.SerializeObject(x.Mapeamento) == JsonConvert.SerializeObject(EstadoAtual));
                    double QValue = estadoEscolhido.Peso;

                    if (first)
                    {
                        first = false;
                        bestAction = action;
                        bestQValue = QValue;
                    }
                    else if (QValue > bestQValue)
                    {
                        bestAction = action;
                        bestQValue = QValue;
                    }
                }
            }

            return bestAction;
        }

        void ValidaCriaAcoesEmEstado(List<Tile> EstadoAtual)
        {
            var estadoEscolhido = _Qtable.Estados.FirstOrDefault(x => JsonConvert.SerializeObject(x.Mapeamento) == JsonConvert.SerializeObject(EstadoAtual));

            if (estadoEscolhido != null)
                return;

            for (int i = 0; i < 4; i++)
            {
                _Qtable.Estados.Add(new Estado() { Mapeamento = EstadoAtual, Acao = i, Peso = _Random.NextDouble() });
            }
        }

        void EnviarComandoDriver(int action)
        {
            var actions = new Actions(_driver);

            switch (action)
            {
                //Cima
                case 0:
                    actions.SendKeys(Keys.ArrowUp);
                    break;
                //Baixo
                case 1:
                    actions.SendKeys(Keys.ArrowDown);
                    break;
                //Esquerda
                case 2:
                    actions.SendKeys(Keys.ArrowLeft);
                    break;
                //Direita
                case 3:
                    actions.SendKeys(Keys.ArrowRight);
                    break;
            }

            // Executar as ações
            actions.Perform();
        }

        private void AtualizarQValue(List<Tile> estadoAtual, int action, double recompensa)
        {
            try
            {
                var estadoEscolhido = _Qtable.Estados.FirstOrDefault(x => x.Acao == action && JsonConvert.SerializeObject(x.Mapeamento) == JsonConvert.SerializeObject(estadoAtual));
                var maxQ = MaxQValue(estadoAtual);
                var novoPeso = (1 - _learningRate) * estadoEscolhido.Peso + _learningRate * (recompensa + _discountFactor * maxQ);

                estadoEscolhido.Peso = novoPeso;
            }
            catch
            {
                Console.WriteLine("ERROR ATUALIZAR QVALUE");
            }
        }

        double MaxQValue(List<Tile> estadoAtual)
        {
            var estadoEscolhido = _Qtable.Estados.Where(x => JsonConvert.SerializeObject(x.Mapeamento) == JsonConvert.SerializeObject(estadoAtual)).ToList();
            double maxQValue = estadoEscolhido[0].Peso;

            foreach (var est in estadoEscolhido)
            {
                double peso = est.Peso;
                if (peso > maxQValue)
                {
                    maxQValue = peso;
                }
            }

            return maxQValue;

            for (int action = 1; action < 4; action++)
            {
                double peso = _Qtable.Estados.FirstOrDefault(x => x.Mapeamento == estadoAtual && x.Acao == action).Peso;
                if (peso > maxQValue)
                {
                    maxQValue = peso;
                }
            }

            return maxQValue;
        }

        void resetGame()
        {
            _driver.FindElement(By.ClassName("restart-button")).Click();
            try
            {
                IAlert alert = _driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch
            {
                Console.WriteLine("Alerta não solicitado");
            }
        }
    }


    public class Qtable
    {
        public List<Estado> Estados { get; set; } = new List<Estado>();
    }

    public class Estado
    {
        public List<Tile> Mapeamento { get; set; }
        public int Score { get; set; }
        public int Acao { get; set; }
        public double Peso { get; set; }
    }

    public class Tile
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Valor { get; set; }
    }


}
