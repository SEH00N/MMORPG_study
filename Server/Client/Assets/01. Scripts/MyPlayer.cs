using System.Collections;
using UnityEngine;

public class MyPlayer : Player
{
    private NetworkManager networkManager = null;

    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartCoroutine(SendPacketCoroutine());
    }

	private IEnumerator SendPacketCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.25f);
            
            C_Move movePacket = new C_Move();
            movePacket.posX = Random.Range(-50f, 50f);
            movePacket.posY = 0;
            movePacket.posZ = Random.Range(-50f, 50f);

            networkManager.Send(movePacket.Write());
        }
    }
}
