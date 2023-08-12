using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private static PlayerManager instance = new PlayerManager();
    public static PlayerManager Instance => instance;

	private MyPlayer myPlayer;
    private Dictionary<int, Player> players = new Dictionary<int, Player>();

    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Player");

        foreach(S_PlayerList.Player p in packet.players)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if(p.isSelf)
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerID = p.playerID;
                myPlayer.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                this.myPlayer = myPlayer;
            }
            else
            {
                Player player = go.AddComponent<Player>();
                player.PlayerID = p.playerID;
                player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                players.Add(p.playerID, player);
            }
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        if(myPlayer.PlayerID == packet.playerID)
        {
            myPlayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        }
        else
        {
            Player player = null;
            if (players.TryGetValue(packet.playerID, out player))
                player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        }
    }
    
    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if(myPlayer.PlayerID == packet.playerID)
            return;

        Object obj = Resources.Load("Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);

        players.Add(packet.playerID, player);
    }
    
    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if(myPlayer.PlayerID == packet.playerID)
        {
            GameObject.Destroy(myPlayer.gameObject);
            myPlayer = null;
        }
        else
        {
            Player player = null;
            if(players.TryGetValue(packet.playerID, out player))
            {
                GameObject.Destroy(player.gameObject);
                players.Remove(packet.playerID);
            }
        }
    }
}
