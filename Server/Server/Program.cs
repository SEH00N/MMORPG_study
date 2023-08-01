using ServerCore;
using System.Net;
using System.Text;

namespace Server
{
    public class Program
    {
        static Listener listener = new Listener();
        public static GameRoom Room = new GameRoom();

        static void FlushRoom()
        {
            Room.Push(() => Room.Flush());
            JobTimer.Instance.PusH(FlushRoom, 250);
        }

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            listener.Init(endPoint, () => SessionManager.Instance.Generate());
            Console.WriteLine("Listening...");

            //FlushRoom();
            JobTimer.Instance.PusH(FlushRoom);

            while (true)
            {
                JobTimer.Instance.Flush();
            }
        }
    }
}