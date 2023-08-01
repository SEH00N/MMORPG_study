
namespace Server
{
    public class SessionManager
    {
        private static SessionManager instance = new SessionManager();
        public static SessionManager Instance => instance;

        private int sessionID = 0;
        private object locker = new object();
        Dictionary<int, ClientSession> sessions = new Dictionary<int, ClientSession>();

        public ClientSession Generate()
        {
            lock(locker)
            {
                int sessionID = ++this.sessionID;
                
                ClientSession session = new ClientSession();
                session.SessionID = sessionID;

                sessions.Add(sessionID, session);
                Console.WriteLine($"Connected : {sessionID}");

                return session;
            }
        }

        public ClientSession Find(int id)
        {
            lock(locker)
            {
                ClientSession session = null;
                sessions.TryGetValue(id, out session);

                return session;
            }
        }

        public void Remove(ClientSession session)
        {
            lock(locker)
            {
                sessions.Remove(session.SessionID);
            }
        }
    }
}
