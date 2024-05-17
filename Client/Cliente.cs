using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Cliente
{
  public void Start()
  {
    try
    {
      IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
      IPEndPoint remoteEP = new IPEndPoint(ipAddr, 11000);

      Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        sender.Connect(remoteEP);
        Console.WriteLine("Socket conectado a {0}", sender.RemoteEndPoint.ToString());

        while (true)
        {
          Console.Write("[CLIENTE] Digite a mensagem: ");
          string message = Console.ReadLine() + "<Final>";

          byte[] msg = Encoding.UTF8.GetBytes(message);
          sender.Send(msg);

          byte[] messageReceived = new byte[1024];
          int byteRecv = sender.Receive(messageReceived);
          Console.WriteLine("[SERVIDOR] " + Encoding.UTF8.GetString(messageReceived, 0, byteRecv));

          if (message.ToLower().Contains("exit"))
          {
            break;
          }
        }

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
      }
      catch (Exception e)
      {
        Console.WriteLine("Erro de conexão: {0}", e.ToString());
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e.ToString());
    }
  }
}
