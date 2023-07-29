using ServerCore;
using System.Net;
using System.Text;

namespace Server
{
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

            listener.Init(endPoint, () => new ClientSession());
            Console.WriteLine("Listening...");

            while (true)
            {

            }
        }
    }
}