using Microsoft.VisualBasic;
using ServerCore;
using System.Net;
using System.Text;

namespace DummyClient
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

    public class ServerSession : Session
    {
        private static unsafe void ToBytes(byte[] array, int offset, ulong value)
        {
            fixed (byte* ptr = &array[offset])
                *(ulong*)ptr = value;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            PlayerInfoReq packet = new PlayerInfoReq() { size = 12, packetID = (ushort)PacketID.PlayerInfoReq, playerID = 1001 };

            // 보낸다
            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> b = SendBufferHelper.Open(4096);

                ushort count = 0;
                bool success = true;

                //success &= BitConverter.TryWriteBytes(new Span<byte>(b.Array, b.Offset + count, b.Count - count), packet.size);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(b.Array, b.Offset + count, b.Count - count), packet.packetID);
                count += 2;
                success &= BitConverter.TryWriteBytes(new Span<byte>(b.Array, b.Offset + count, b.Count - count), packet.playerID);
                count += 8;

                success &= BitConverter.TryWriteBytes(new Span<byte>(b.Array, b.Offset, b.Count), count);

                ArraySegment<byte> sendBuffer = SendBufferHelper.Close(count);

                if(success)
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
}
