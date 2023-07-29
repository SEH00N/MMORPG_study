using ServerCore;
using System.Net;

namespace Server
{
    public class Packet
    {
        public ushort size;
        public ushort packetID;
    }

    public class PlayerInfoReq : Packet
    {
        public long playerID;
    }

    public class PlayerInfoOK : Packet
    {
        public int hp;
        public int attack;
    }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOK = 2,
    }

    public class ClientSession : PacketSession
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
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;
            
            switch((PacketID)id)
            {
                case PacketID.PlayerInfoReq:
                    long playerID = BitConverter.ToInt64(buffer.Array, buffer.Offset + count);
                    count += 8;
                    Console.WriteLine($"Player Info Req : {playerID}");
                    break;
            }

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
}
