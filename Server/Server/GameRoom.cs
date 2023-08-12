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

        public void Broadcast(ArraySegment<byte> buffer)
        {
            pendingList.Add(buffer);
        }

        public void Enter(ClientSession session)
        {
            // 플레이어 추가
            sessions.Add(session);
            session.Room = this;

            // 방금 들어온 세션한테 플레이어 리스트 전송
            S_PlayerList players = new S_PlayerList();
            foreach(ClientSession s in sessions)
            {
                players.players.Add(new S_PlayerList.Player() {
                    isSelf = (s == session),
                    playerID = s.SessionID,
                    posX = s.PosX,
                    posY = s.PosY,
                    posZ = s.PosZ,
                });
            }

            session.Send(players.Write());

            // 기존 세션들에게 세션이 입장함을 알림
            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerID = session.SessionID;
            enter.posX = 0;
            enter.posY = 0;
            enter.posZ = 0;
            Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            // 세션 제거
            sessions.Remove(session);

            // 세선이 나간 걸 알리기
            S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            leave.playerID = session.SessionID;
            Broadcast(leave.Write());
        }

        public void Move(ClientSession session, C_Move packet)
        {
            // 좌표 바꾸기
            session.PosX = packet.posX;
            session.PosY = packet.posY;
            session.PosZ = packet.posZ;

            // 브로드 캐스팅
            S_BroadcastMove movePacket = new S_BroadcastMove();
            movePacket.playerID = session.SessionID;
            movePacket.posX = packet.posX;
            movePacket.posY = packet.posY;
            movePacket.posZ = packet.posZ;

            Broadcast(movePacket.Write());
        }
    }
}
