using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferHost
{
    class ReceiveFile
    {
        static int Port { get; set; }
        public static void Receive(int port, string[] args)
        {
            Port = port;
            Console.WriteLine("Starting file receiver\n");

            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            Console.WriteLine("Waiting for file");

            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            IPEndPoint sender = (IPEndPoint)client.Client.RemoteEndPoint;

            string senderIP = sender.Address.ToString();
            int senderPort = sender.Port;

            Console.WriteLine("Received file from: " + senderIP + ":" + senderPort);


            Console.WriteLine("Receiving file: ");

            byte[] filenameBuffer = new byte[1024];
            int bytesRead = stream.Read(filenameBuffer, 0, filenameBuffer.Length);
            Console.WriteLine("First Stuff: -----------\n" + Encoding.ASCII.GetString(filenameBuffer, 0, bytesRead) + "\n---------------");
            string fileName = Encoding.ASCII.GetString(filenameBuffer, 0, bytesRead).TrimEnd('\0');
            fileName = (fileName.Split('\r'))[0];

            //Console.WriteLine("\"" + fileName + "\"");

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                    {
                        bytesRead = 0;
                        byte[] buffer = new byte[1024];

                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                        }
                    }
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

