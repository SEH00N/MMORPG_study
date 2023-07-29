using ServerCore;
using System.Net;
using System.Text;

namespace Server
{
    public class GameSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            // 답장 보내기
            //byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to MMORPG Server!");
            //Send(sendBuffer);

            Thread.Sleep(5000);

            // 쫓아내기
            Disconnect();
        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2);

            Console.WriteLine($"Receive Packet ID : {id}, Size : {size}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }

    public class Program
    {
        static Listener listener = new Listener();

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            listener.Init(endPoint, () => new GameSession());
            Console.WriteLine("Listening...");

            while (true)
            {

            }
        }
    }
}