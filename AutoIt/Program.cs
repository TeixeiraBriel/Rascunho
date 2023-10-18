using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AutoIt.AutoItX;

namespace AutoIt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {

                if (true)
                {
                    Console.WriteLine("3...");
                    Thread.Sleep(1000);
                    Console.WriteLine("2...");
                    Thread.Sleep(1000);
                    Console.WriteLine("1...");

                    AutoItSetOption("SendKeyDownDelay", 3000);
                    Send("w");
                    AutoItSetOption("SendKeyDownDelay", 0);

                    Send("{ENTER}");
                    Send("FIM");
                    Send("{ENTER}");
                    Console.WriteLine("Press any to start...");
                }
                else
                {
                }
            } while (true);
        }
    }
}
