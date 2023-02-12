using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferHost
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Do you want to send or receive a file?");
                Console.WriteLine("1: Send");
                Console.WriteLine("2: Receive");
                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    SendFile.Send(null);
                }
                else if (choice == 2)
                {
                    ReceiveFile.Receive(59666, null);

                    Console.WriteLine("ESC to stop, Enter to continue");
                    ConsoleKeyInfo cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                }
            }
        }
    }
}
