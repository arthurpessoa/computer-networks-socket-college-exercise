/*Arthur Pessoa de Souza RA: 380075
 *Atividade de Redes (Socket)
 *Server
 */

using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace Server
{
    class Program
    {
        public static void Main()
        {
            new Server();
        }
    }
    class Server
    {
        private TcpListener listener;
        private Thread listenThread;
        public Server()
        {
            this.listener = new TcpListener(IPAddress.Any, 6900);
            this.listenThread = new Thread(new ThreadStart(ListenClients));
            this.listenThread.Start();
        }
        private void ListenClients()
        {
            this.listener.Start();
            while (true)
            {
                //bloqueia até um cliente solicitar conexão
                TcpClient client = this.listener.AcceptTcpClient();
                //cria uma thread pra gerenciar a conexão
                Thread clientThread = new Thread(new ParameterizedThreadStart(trataClient));
                clientThread.Start(client);
            }
        }
        private void trataClient(object client)
        {
            TcpClient Client = (TcpClient)client;
            NetworkStream Stream = Client.GetStream();
            try
                {
                    string enderecoClient = Client.Client.RemoteEndPoint.ToString();
                    string stringData;
                    byte[] data = new byte[1024];                        
                    int bytesLidos = Stream.Read(data, 0, data.Length);
                    
                    //processa
                    stringData = Encoding.ASCII.GetString(data, 0, bytesLidos);
                    Console.WriteLine("{0}->{1}",enderecoClient,stringData);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    data = encoding.GetBytes(stringData.ToUpper()); //deixa maiusculo
                    //envia
                    Stream.Write(data, 0, bytesLidos);                  
                }catch(Exception ex)
                {
                    Console.WriteLine("Erro: {0}", ex.Message);
                }
            Stream.Close();
            Client.Close();
        }
    }
}
