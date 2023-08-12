using System;
using System.Collections.Generic;
using System.Net;
using DummyClient;
using ServerCore;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    // private static NetworkManager instance = null;
    // public static 

    private ServerSession session = new ServerSession();

    public void Send(ArraySegment<byte> buffer)
    {
        session.Send(buffer);
    }

    private void Start()
    {
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddress = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => session, 1);

    }

    private void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach(IPacket packet in list)
            PacketManager.Instance.HandlePacket(session, packet);
    }
}
