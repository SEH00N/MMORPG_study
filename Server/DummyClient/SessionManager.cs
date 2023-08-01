namespace DummyClient
{
    public class SessionManager
    {
        private static SessionManager instance = new SessionManager();
        public static SessionManager Instance => instance;

        private object locker = new object();
        private List<ServerSession> sessions = new List<ServerSession>();

        public void SendForEach()
        {
            lock(locker)
            {
                C_Chat chatPacket = new C_Chat();
                chatPacket.chat = $"Hello, Server!";

                foreach (ServerSession session in sessions)
                {
                    ArraySegment<byte> buffer = chatPacket.Write();

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
