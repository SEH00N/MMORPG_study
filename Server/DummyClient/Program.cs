using ServerCore;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    public class Packet
    {
        public ushort size;
        public ushort packetID;
    }

    public class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            Packet packet = new Packet() { size = 4, packetID = 7 };

            // 보낸다
            for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> openBuffer = SendBufferHelper.Open(1024);
                byte[] buffer = BitConverter.GetBytes(packet.size);
                byte[] buffer2 = BitConverter.GetBytes(packet.packetID);
                Array.Copy(buffer, 0, openBuffer.Array, openBuffer.Offset, buffer.Length);
                Array.Copy(buffer2, 0, openBuffer.Array, openBuffer.Offset + buffer.Length, buffer2.Length);
                ArraySegment<byte> sendBuffer = SendBufferHelper.Close(packet.size);

                Send(sendBuffer);
            }

            Thread.Sleep(1000);

            // 쫓아내기
            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnReceive(ArraySegment<byte> buffer)
        {
            string receiveData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {receiveData}");

            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Connector connector = new Connector();
            connector.Connect(endPoint, () => new GameSession());

            while(true)
            {
                // 휴대폰 설정
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(100);
            }

        }
    }
}