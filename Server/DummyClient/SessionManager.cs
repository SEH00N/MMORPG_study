namespace DummyClient
{
    public class SessionManager
    {
        private static SessionManager instance = new SessionManager();
        public static SessionManager Instance => instance;

        private List<ServerSession> sessions = new List<ServerSession>();
        private object locker = new object();
        private Random random = new Random();

        public void SendForEach()
        {
            lock(locker)
            {
                foreach (ServerSession session in sessions)
                {
                    C_Move movePacket = new C_Move();
                    movePacket.posX = random.Next(-50, 50);
                    movePacket.posY = 0;
                    movePacket.posZ = random.Next(-50, 50);

                    ArraySegment<byte> buffer = movePacket.Write();

                    session.Send(buffer);
                }
            }
        }

        public ServerSession Generate()
        {
            lock(locker)
            {
                ServerSession session = new ServerSession();
                sessions.Add(session);

                return session;
            }
        }
    }
}
