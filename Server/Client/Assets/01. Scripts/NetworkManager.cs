using System;
using System.Collections;
using System.Net;
using DummyClient;
using ServerCore;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private ServerSession session = new ServerSession();

    private void Start()
    {
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddress = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => session, 1);

        StartCoroutine(SendPacketCoroutine());
    }

    private void Update()
    {
        IPacket packet = PacketQueue.Instance.Pop();
        if(packet != null)
        {
            PacketManager.Instance.HandlePacket(session, packet);
        }
    }

    private IEnumerator SendPacketCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            
            C_Chat chatPacket = new C_Chat();
            chatPacket.chat = "Hello, Packet";

            ArraySegment<byte> buffer = chatPacket.Write();

            session.Send(buffer);
        }
    }
}
