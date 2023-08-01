using ServerCore;
using System;

namespace Server
{
    public class GameRoom : IJobQueue
    {
        private List<ClientSession> sessions = new List<ClientSession>();
        private JobQueue jobQueue = new JobQueue();

        private List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in sessions)
                s.Send(pendingList);

            Console.WriteLine($"Flushed {pendingList.Count} Items");
            pendingList.Clear();
        }

        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new S_Chat();
            packet.playerID = session.SessionID;
            packet.chat = $"{packet.playerID} : {chat}";

            ArraySegment<byte> buffer = packet.Write();
            pendingList.Add(buffer);
        }

        public void Enter(ClientSession session)
        {
            sessions.Add(session);
            session.Room = this;
        }

        public void Leave(ClientSession session)
        {
            sessions.Remove(session);

        }
    }
}
