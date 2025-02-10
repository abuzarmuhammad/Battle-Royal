using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MirrorBasics {
    public class AutoHostClient : MonoBehaviour {

        [SerializeField] NetworkManager networkManager;
        [SerializeField] private bool serverBuild;
        [SerializeField] private bool Host;
        
        public bool isConnectedToServer;
        
        void Start () {
            // if (!Application.isBatchMode) { //Headless build
            if (serverBuild) { //Headless build
                Debug.Log ($"=== Server Build ===");
                networkManager.StartServer();
            }
            else
            {
                if (Host)
                {
                    networkManager.StartHost();
                }
                else
                {
                    networkManager.StartClient();
                }
            }
        }
    }
}