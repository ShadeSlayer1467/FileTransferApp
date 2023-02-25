using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTransferHost
{
    class SendFile
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
        public static void Send(string[] args)
        {
            Console.WriteLine("Starting file sender\n");

            Computer[] computers = {
            new Computer("RGBDesk", "172.20.10.10", 59666),
            new Computer("OmenLap", "172.20.10.6", 59666)
        };

            Console.WriteLine("Choose a computer to send a file to:");
            for (int i = 0; i < computers.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {computers[i].Name}");
            }

            int selectedComputer = int.Parse(Console.ReadLine()) - 1;
            Computer chosenComputer = computers[selectedComputer];

            Console.WriteLine("Enter the file path:");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);

                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(chosenComputer.IP), chosenComputer.Port);

                IPEndPoint sender = (IPEndPoint)client.Client.RemoteEndPoint;

                string senderIP = sender.Address.ToString();
                int senderPort = sender.Port;

                Console.WriteLine("Sending file to: " + senderIP + ":" + senderPort);

                Console.WriteLine("Sending file");

                NetworkStream stream = client.GetStream();

                // First, send the name of the file
                byte[] fileNameBytes = Encoding.ASCII.GetBytes(fileName + Environment.NewLine);
                stream.Write(fileNameBytes, 0, fileNameBytes.Length);

                // Then, send the contents of the file
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead;
                    byte[] buffer = new byte[1024];

                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine("File sent successfully\n\n");

                stream.Close();
                client.Close();
            }
            else
            {
                Console.WriteLine("File selection cancelled.");
            }
        }
    }
}
/*
In this example, there are two buttons on the form: "Select File" and "Send File". The 
selectFileButton_Click method is called when the "Select File" button is clicked, and it 
opens a file dialog to allow the user to select a file to send. The sendFileButton_Click 
method is called when the "Send File" button is clicked, and it sends the selected file to the 
remote computer using the TCP protocol.

You'll need to replace <Remote Computer IP Address> and <Remote Computer Port> with 
the actual IP address and port of the remote computer that you want to send the files to.
*/

