using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Servidor
{
  public void Start()
  {
    IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
    IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11000);

    Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    try
    {
      listener.Bind(localEndPoint);
      listener.Listen(10);

      Console.WriteLine("Aguardando conexão ...");

      Socket clientSocket = listener.Accept();
      Console.WriteLine("Cliente conectado.");

      Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
      clientThread.Start(clientSocket);
    }
    catch (Exception e)
    {
      Console.WriteLine(e.ToString());
    }
  }

  private void HandleClient(object obj)
  {
    Socket clientSocket = (Socket)obj;
    string data = null;
    byte[] bytes = new byte[1024];

    try
    {
      while (true)
      {
        int numByte = clientSocket.Receive(bytes);
        data = Encoding.UTF8.GetString(bytes, 0, numByte);

        Console.WriteLine("[CLIENTE] " + data);

        Console.Write("[SERVIDOR] Digite a mensagem: ");
        string response = Console.ReadLine() + "<Final>";
        byte[] message = Encoding.UTF8.GetBytes(response);
        clientSocket.Send(message);

        if (data.ToLower().Contains("exit"))
        {
          break;
        }
      }
      clientSocket.Shutdown(SocketShutdown.Both);
      clientSocket.Close();
    }
    catch (Exception e)
    {
      Console.WriteLine(e.ToString());
    }
  }
}
