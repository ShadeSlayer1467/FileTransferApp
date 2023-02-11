using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferHost
{
    class Computer
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }

        public Computer(string name, string ip, int port)
        {
            Name = name;
            IP = ip;
            Port = port;
        }
    }
    class ReceiveFile
    {
        public static void Receive(string[] args)
        {
            Console.WriteLine("Starting file receiver");

            Computer[] computers = {
                new Computer("RGBDesk", "172.20.10.10", 59666),
                new Computer("OmenLap", "172.20.10.6", 59666)
            };

            Console.WriteLine("Choose a computer to receive a file from:");
            for (int i = 0; i < computers.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {computers[i].Name}");
            }

            int selectedComputer = int.Parse(Console.ReadLine()) - 1;
            Computer chosenComputer = computers[selectedComputer];

            TcpListener listener = new TcpListener(IPAddress.Parse(chosenComputer.IP), chosenComputer.Port);
            listener.Start();

            Console.WriteLine("Waiting for file");

            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            Console.WriteLine("Receiving file");

            using (FileStream fileStream = new FileStream("received_file.txt", FileMode.Create, FileAccess.Write))
            {
                int bytesRead;
                byte[] buffer = new byte[1024];

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }

            Console.WriteLine("File received successfully");

            stream.Close();
            client.Close();
            listener.Stop();
        }
    }
}
    /* 
    In this example, the Main method starts a TCP listener that listens on a specific IP address 
    and port. When a connection is established, the program receives the file by reading the 
    data from the network stream and writing it to a file on disk.
    
    You'll need to replace <Listening IP Address> and <Listening Port> with the actual IP address 
    and port that the receiver should listen on.
    
    Note: This code is just an example and should be adapted to fit the specific needs of your 
    use case. You may also want to add error handling and other features to make the 
    application more robust. 
    */

