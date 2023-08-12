using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

public enum PacketID
{
    C_Chat = 1,
    S_Chat = 2,
    
}

public interface IPacket
{
    public ushort Protocol { get; }
    
    public void Read(ArraySegment<byte> buffer);
	public ArraySegment<byte> Write();
}

public class C_Chat : IPacket
{
    public ushort Protocol => (ushort)PacketID.C_Chat;

    public string chat;

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        ushort chatLen = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += sizeof(ushort);
		
		this.chat = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + count, chatLen);
		count += chatLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Chat), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, this.chat.Length, buffer.Array, buffer.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(chatLen), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += chatLen;

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_Chat : IPacket
{
    public ushort Protocol => (ushort)PacketID.S_Chat;

    public int playerID;
	public string chat;

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerID = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
		count += sizeof(int);
		ushort chatLen = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += sizeof(ushort);
		
		this.chat = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + count, chatLen);
		count += chatLen;
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_Chat), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.playerID), 0, buffer.Array, buffer.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort chatLen = (ushort)Encoding.Unicode.GetBytes(this.chat, 0, this.chat.Length, buffer.Array, buffer.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(chatLen), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += chatLen;

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

