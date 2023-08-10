using System;
using System.Collections.Generic;
using ServerCore;

public class PacketManager
{
    private static PacketManager instance = new PacketManager();
    public static PacketManager Instance => instance;

    private Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> onReceive = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();
    private Dictionary<ushort, Action<PacketSession, IPacket>> handlers = new Dictionary<ushort, Action<PacketSession, IPacket>>();

    private PacketManager()
    {
        Register();
    }
            
    public void Register()
    {
        onReceive.Add((ushort)PacketID.S_Chat, MakePacket<S_Chat>);
        handlers.Add((ushort)PacketID.S_Chat, PacketHandler.S_ChatHandler);

    }

    public void OnReceivePacket(PacketSession session, ArraySegment<byte> buffer)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;

        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>> action = null;
        if (onReceive.TryGetValue(id, out action))
            action?.Invoke(session, buffer);
    }

    private void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T packet = new T();
        packet.Read(buffer);

        Action<PacketSession, IPacket> action = null;
        if (handlers.TryGetValue(packet.Protocol, out action))
            action?.Invoke(session, packet);
    }
}