using System.Collections.Generic;

public class PacketQueue
{
    public static PacketQueue Instance { get; } = new PacketQueue();
    
    private Queue<IPacket> packetQueue = new Queue<IPacket>();

    private object locker = new object();

    public void Push(IPacket packet)
    {
        lock(locker)
        {
            packetQueue.Enqueue(packet);
        }
    }

    public IPacket Pop()
    {
        lock(locker)
        {
            if (packetQueue.Count == 0)
                return null;

            return packetQueue.Dequeue();
        }
    }

    public List<IPacket> PopAll()
    {
        List<IPacket> list = new List<IPacket>();

        lock(locker)
        {
            while(packetQueue.Count > 0)
                list.Add(packetQueue.Dequeue());
        }

        return list;
    }
}
