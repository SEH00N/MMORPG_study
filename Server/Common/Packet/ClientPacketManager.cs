using System;
using System.Collections.Generic;
using ServerCore;

public class PacketManager
{
    private static PacketManager instance = new PacketManager();
    public static PacketManager Instance => instance;

    private Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
    private Dictionary<ushort, Action<PacketSession, IPacket>> handlers = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    private PacketManager()
    {
        Register();
    }
            
    public void Register()
    {
        makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
        handlers.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
        makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
        handlers.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
        makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
        handlers.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
        makeFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
        handlers.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);

    }

    public void OnReceivePacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onReceiveCallback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;

        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
        if (makeFunc.TryGetValue(id, out func))
        {
            IPacket packet = func?.Invoke(session, buffer);
            if(onReceiveCallback != null)
                onReceiveCallback ?.Invoke(session, packet);
            else
                HandlePacket(session, packet);
        }
    }

    private T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T packet = new T();
        packet.Read(buffer);

        return packet;
    }

    public void HandlePacket(PacketSession session, IPacket packet)
    {
        Action<PacketSession, IPacket> action = null;
        if (handlers.TryGetValue(packet.Protocol, out action))
            action?.Invoke(session, packet);
    }
}