using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManagerBattleRoyale : NetworkManager
{
    [SerializeField] private GameObject playerSpawnSystem;
    public static event Action<NetworkConnection> OnServerReadied;

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Lobby"))
        {
            //GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            //NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn) 
    {
        base.OnServerReady(conn);

        //OnServerReadied?.Invoke(conn);
    }
    
    
}
