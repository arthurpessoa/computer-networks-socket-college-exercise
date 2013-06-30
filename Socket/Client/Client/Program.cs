using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        public static void Main()
        {
            string IP = "127.0.0.1";
            int Porta = 6900;

            Console.WriteLine("Conectando ao servidor...");

            IPEndPoint endereco = new IPEndPoint(IPAddress.Parse(IP), Porta);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(endereco);
                Console.WriteLine("Conectado a {0}:{1}.", IP, Porta);
                Console.WriteLine("Digite a string a ser enviada, ou sair para desconectar.");
                while (true)
                {
                    Console.Write(">");
                    string input, stringData;
                    input = Console.ReadLine();

                    //Enviar
                    server.Send(Encoding.ASCII.GetBytes(input));
                    if (input.Equals("sair")) break;
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Enviado: {0}", input);

                    //Receber
                    byte[] data = new byte[1024];
                    int bytesLidos = server.Receive(data);
                    stringData = Encoding.ASCII.GetString(data, 0, bytesLidos);
                    Console.WriteLine("Recebido: {0}", stringData);
                    Console.WriteLine("---------------------------------");
                }
                Console.WriteLine("desconectando do servidor...");
                server.Close();
                Console.ReadKey();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Erro {0}", ex.Message);
            }
            Console.ReadKey();
        }
    }
}