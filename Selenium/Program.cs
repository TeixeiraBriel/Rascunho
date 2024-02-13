using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    internal class Program
    {
        static void Main(string[] args)
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
            IWebDriver driver = new ChromeDriver(service, options);

            // Exemplo de uso: carregar uma página e exibir o título
            string url = "https://www.youtube.com/@baltaio";
            driver.Navigate().GoToUrl(url);
            Console.WriteLine("Título da página: " + driver.Title);

            // Obtenha a URL da página
            string currentUrl = driver.Url;

            driver.Navigate().GoToUrl(url);

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                // Imprima os cabeçalhos
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
            }
        }
    }
}
