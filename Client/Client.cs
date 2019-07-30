using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    static class Client
    {
        public static void ConnectToServer(IPAddress ipAddress, int portNumber)
        {
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var remoteEndPointConnection = new IPEndPoint(ipAddress, portNumber);
            try
            {
                clientSocket.Connect(remoteEndPointConnection);
                Console.WriteLine("Connected to {0}", clientSocket.RemoteEndPoint.ToString());
                //while(true)
                //{


                //    Task.Factory.StartNew(() => RecieveMessage(clientSocket));
                //    Task.Factory.StartNew(() => SendMessage(clientSocket));
                //}
                var readMessage = new Thread(new ThreadStart(() => RecieveMessage(clientSocket)));
                var sendMessage = new Thread(new ThreadStart(() => SendMessage(clientSocket))); ;
                readMessage.Start();
                sendMessage.Start();

                //clientSocket.Shutdown(SocketShutdown.Both);
                //clientSocket.Close();
            }
            catch (SocketException se)
            {
                Console.WriteLine("Socket Exception :{0}", se.ToString());
            }
        }

        private static void SendMessage(Socket clientSocket)
        {
            while(true)
            {
                var message = Console.ReadLine();
                var messageToBeSent = Encoding.ASCII.GetBytes(message);
                clientSocket.Send(messageToBeSent);
            }

        }

        private static void RecieveMessage(Socket clientSocket)
        {
            var messageRecieved = new byte[1024];
            while (true)
            {
                var byteRecieved = clientSocket.Receive(messageRecieved);
                Console.WriteLine(Encoding.ASCII.GetString(messageRecieved, 0, byteRecieved));
            }
        }
    }
}
