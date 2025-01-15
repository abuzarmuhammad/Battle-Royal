using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConnectionHandler : MonoBehaviour
{
    NetworkManager manager;

    //public string networkAddress;
    public bool isServerOnly;
    public bool isClientPlusServer;
    public bool isClientWithAdress;

    public int offsetX;
    public int offsetY;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    private void Start()
    {
        StartCoroutine(CheckNetworkType());
    }

    IEnumerator CheckNetworkType() 
    {
        yield return new WaitForSeconds(2.0f);
        if (isServerOnly)
        {
            ServerOnly();
        }
        else if (isClientPlusServer)
        {
            SeverAndClient();
        }
        else if (isClientWithAdress)
        {
            Debug.Log("Hello");
            ClientWithIPAdress();
        }
    }
    void SeverAndClient() 
    {
        manager.StartHost();
    }

    void ClientWithIPAdress() 
    {
        manager.StartClient();
    }

    void ServerOnly() 
    {
        manager.StartServer();
    }

    //void OnGUI()
    //{
    //    //GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
    //    if (!NetworkClient.isConnected && !NetworkServer.active)
    //    {
    //    //    StartButtons();
    //    }
    //    else
    //    {
    //        StatusLabels();
    //    }

    //    //// client ready
    //    if (NetworkClient.isConnected && !NetworkClient.ready)
    //    {
    //        Debug.Log("client ready 1");

    //        if (GUILayout.Button("Client Ready"))
    //        {
    //            NetworkClient.Ready();
    //            if (NetworkClient.localPlayer == null)
    //            {
    //                Debug.Log("AddPlayer");
    //                NetworkClient.AddPlayer();
    //            }
    //        }
    //    }

    //    //StopButtons();

    //    //GUILayout.EndArea();
    //}

    //void StartButtons()
    //{
    //    if (!NetworkClient.active)
    //    {
    //        // Server + Client
    //        if (Application.platform != RuntimePlatform.WebGLPlayer)
    //        {
    //            if (GUILayout.Button("Host (Server + Client)"))
    //            {
    //                manager.StartHost();
    //            }
    //        }

    //        // Client + IP
    //        GUILayout.BeginHorizontal();
    //        if (GUILayout.Button("Client"))
    //        {
    //            manager.StartClient();
    //        }
    //        // This updates networkAddress every frame from the TextField
    //       // manager.networkAddress = networkAddress;
    //        GUILayout.EndHorizontal();

    //        // Server Only
    //        if (Application.platform == RuntimePlatform.WebGLPlayer)
    //        {
    //            // cant be a server in webgl build
    //            GUILayout.Box("(  WebGL cannot be server  )");
    //        }
    //        else
    //        {
    //            if (GUILayout.Button("Server Only")) manager.StartServer();
    //        }
    //    }
    //    else
    //    {
    //        // Connecting
    //        GUILayout.Label($"Connecting to {manager.networkAddress}..");
    //        if (GUILayout.Button("Cancel Connection Attempt"))
    //        {
    //            manager.StopClient();
    //        }
    //    }
    //}

    //void StatusLabels()
    //{
    //    // host mode
    //    // display separately because this always confused people:
    //    //   Server: ...
    //    //   Client: ...
    //    if (NetworkServer.active && NetworkClient.active)
    //    {
    //        GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
    //    }
    //    // server only
    //    else if (NetworkServer.active)
    //    {
    //        GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
    //    }
    //    // client only
    //    else if (NetworkClient.isConnected)
    //    {
    //        GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
    //    }
    //}

    //void StopButtons()
    //{
    //    // stop host if host mode
    //    if (NetworkServer.active && NetworkClient.isConnected)
    //    {
    //       // if (GUILayout.Button("Stop Host"))
    //       // {
    //       //     manager.StopHost();
    //       // }
    //    }
    //    // stop client if client-only
    //    else if (NetworkClient.isConnected)
    //    {
    //       // if (GUILayout.Button("Stop Client"))
    //      //  {
    //      //      manager.StopClient();
    //      //  }
    //    }
    //    // stop server if server-only
    //    else if (NetworkServer.active)
    //    {
    //       // if (GUILayout.Button("Stop Server"))
    //     //   {
    //      //      manager.StopServer();
    //      //  }
    //    }
    //}
}
