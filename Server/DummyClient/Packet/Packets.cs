using System;
using System.Collections.Generic;
using System.Text;
using ServerCore;

public enum PacketID
{
    S_BroadcastEnterGame = 1,
    C_LeaveGame = 2,
    S_BroadcastLeaveGame = 3,
    S_PlayerList = 4,
    C_Move = 5,
    S_BroadcastMove = 6,
    
}

public interface IPacket
{
    public ushort Protocol { get; }
    
    public void Read(ArraySegment<byte> buffer);
	public ArraySegment<byte> Write();
}

public class S_BroadcastEnterGame : IPacket
{
    public ushort Protocol => (ushort)PacketID.S_BroadcastEnterGame;

    public int playerID;
	public float posX;
	public float posY;
	public float posZ;

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerID = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
		count += sizeof(int);
		this.posX = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.playerID), 0, buffer.Array, buffer.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.posX), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posY), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posZ), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_LeaveGame : IPacket
{
    public ushort Protocol => (ushort)PacketID.C_LeaveGame;

    

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_LeaveGame), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_BroadcastLeaveGame : IPacket
{
    public ushort Protocol => (ushort)PacketID.S_BroadcastLeaveGame;

    public int playerID;

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerID = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
		count += sizeof(int);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastLeaveGame), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.playerID), 0, buffer.Array, buffer.Offset + count, sizeof(int));
		count += sizeof(int);

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_PlayerList : IPacket
{
    public ushort Protocol => (ushort)PacketID.S_PlayerList;

    public class Player
	{
	    public bool isSelf;
		public int playerID;
		public float posX;
		public float posY;
		public float posZ;
	
	    public void Read(ArraySegment<byte> buffer, ref ushort count)
	    {
	        this.isSelf = BitConverter.ToBoolean(buffer.Array, buffer.Offset + count);
			count += sizeof(bool);
			this.playerID = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
			count += sizeof(int);
			this.posX = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
			count += sizeof(float);
			this.posY = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
			count += sizeof(float);
			this.posZ = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
			count += sizeof(float);
	    } 
	
	    public bool Write(ArraySegment<byte> buffer, ref ushort count)
	    {
	        bool success = true;
	
	        Array.Copy(BitConverter.GetBytes(this.isSelf), 0, buffer.Array, buffer.Offset + count, sizeof(bool));
			count += sizeof(bool);
			Array.Copy(BitConverter.GetBytes(this.playerID), 0, buffer.Array, buffer.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(this.posX), 0, buffer.Array, buffer.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(this.posY), 0, buffer.Array, buffer.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(this.posZ), 0, buffer.Array, buffer.Offset + count, sizeof(float));
			count += sizeof(float);
	
	        return success;
	    }
	}
	public List<Player> players = new List<Player>();
	

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        this.players.Clear();
		ushort playerLen = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += sizeof(ushort);
		
		for(int i = 0; i < playerLen; i++)
		{
		    Player player = new Player();
		    player.Read(buffer, ref count);
		    players.Add(player);
		}
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_PlayerList), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)this.players.Count), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach(Player player in this.players)
		    player.Write(buffer, ref count);

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class C_Move : IPacket
{
    public ushort Protocol => (ushort)PacketID.C_Move;

    public float posX;
	public float posY;
	public float posZ;

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        this.posX = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.C_Move), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.posX), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posY), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posZ), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class S_BroadcastMove : IPacket
{
    public ushort Protocol => (ushort)PacketID.S_BroadcastMove;

    public int playerID;
	public float posX;
	public float posY;
	public float posZ;

    public void Read(ArraySegment<byte> buffer)
    {
        ushort count = 0;

        count += sizeof(ushort);
        count += sizeof(ushort);

        this.playerID = BitConverter.ToInt32(buffer.Array, buffer.Offset + count);
		count += sizeof(int);
		this.posX = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
		this.posY = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
		this.posZ = BitConverter.ToSingle(buffer.Array, buffer.Offset + count);
		count += sizeof(float);
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> buffer = SendBufferHelper.Open(4096);

        ushort count = 0;

        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastMove), 0, buffer.Array, buffer.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(this.playerID), 0, buffer.Array, buffer.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.posX), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posY), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.posZ), 0, buffer.Array, buffer.Offset + count, sizeof(float));
		count += sizeof(float);

        Array.Copy(BitConverter.GetBytes(count), 0, buffer.Array, buffer.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

