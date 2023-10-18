using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument($@"user-data-dir={Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%")}\Google\Chrome\User Data\");
            chromeOptions.AddArgument("profile-directory=Default");
            //chromeOptions.AddAdditionalOption("useAutomationExtension", false);

            IWebDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);
            var asd = driver.PageSource;
        }
    }
}
