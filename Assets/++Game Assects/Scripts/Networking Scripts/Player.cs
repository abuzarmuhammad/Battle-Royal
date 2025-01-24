using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.AutoLANClientController;
using UnityEngine;
using UnityEngine.SceneManagement;

    [RequireComponent (typeof (NetworkMatch))]
    public class Player : NetworkBehaviour {

        public static Player localPlayer;
        public GameObject cameraController;
        [SerializeField] private List<GameObject> meshes;
        
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        NetworkMatch networkMatch;

        [SyncVar] public Match currentMatch;


        
        
        
        Guid netIDGuid;

        void Awake () {
            networkMatch = GetComponent<NetworkMatch> ();
        }

        public override void OnStartServer () {
            netIDGuid = netId.ToString ().ToGuid ();
            networkMatch.matchId = netIDGuid;
        }

        public override void OnStartClient () {
            if (isLocalPlayer) {
                localPlayer = this;
                DontDestroyOnLoad(gameObject);
                Debug.LogWarning("Client Started");
                for (int i = 0; i < meshes.Count; i++)
                {
                    meshes[i].SetActive(true);
                }
                if (localPlayer.Equals(this))
                {
                    SwitchPosition(GameStateEnum.MAIN_MENU);
                }
                //Invoke(nameof(SearchGame),4);
            } else {
                Debug.Log ($"Spawning other player UI Prefab");
                gameObject.layer = LayerMask.NameToLayer ("OtherPlayer");
                for (int i = 0; i < meshes.Count; i++)
                {
                    meshes[i].SetActive(false);
                }
                // playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab (this);
            }
        }

        public override void OnStopClient () {
            Debug.Log ($"Client Stopped");
            ClientDisconnect ();
        }

        public override void OnStopServer () {
            Debug.Log ($"Client Stopped on Server");
            ServerDisconnect ();
        }

        /* 
            HOST MATCH
        */

        public void HostGame (bool publicMatch) {
            string matchID = MatchMaker.GetRandomMatchID ();
            CmdHostGame (matchID, publicMatch, DataHandler.Instance.maxPlayers);
        }

        [Command]
        void CmdHostGame (string _matchID, bool publicMatch, int maximumPlayers) {
            matchID = _matchID;
            if (MatchMaker.instance.HostGame (_matchID, this, publicMatch, out playerIndex,maximumPlayers)) {
                Debug.Log ($"<color=green>Game hosted successfully</color>");
                networkMatch.matchId = _matchID.ToGuid ();
                TargetHostGame (true, _matchID, playerIndex);
            } else {
                Debug.Log ($"<color=red>Game hosted failed</color>");
                TargetHostGame (false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame (bool success, string _matchID, int _playerIndex) {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log ($"MatchID: {matchID} == {_matchID}");
           // UILobby.instance.HostSuccess (success, _matchID);
        }

        /* 
            JOIN MATCH
        */

        public void JoinGame (string _inputID) {
            CmdJoinGame (_inputID);
        }

        [Command]
        void CmdJoinGame (string _matchID) {
            matchID = _matchID;
            if (MatchMaker.instance.JoinGame (_matchID, this, out playerIndex)) {
                Debug.Log ($"<color=green>Game Joined successfully</color>");
                networkMatch.matchId = _matchID.ToGuid ();
                TargetJoinGame (true, _matchID, playerIndex);

                //Host
                // if (isServer && playerLobbyUI != null) {
                //     playerLobbyUI.SetActive (true);
                // }
            } else {
                Debug.Log ($"<color=red>Game Joined failed</color>");
                TargetJoinGame (false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame (bool success, string _matchID, int _playerIndex) {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log ($"MatchID: {matchID} == {_matchID}");
            //UILobby.instance.JoinSuccess (success, _matchID);
        }
        
        /* 
            DISCONNECT
        */

        public void DisconnectGame () {
            CmdDisconnectGame ();
        }

        [Command]
        void CmdDisconnectGame () {
            ServerDisconnect ();
        }

        void ServerDisconnect () {
            MatchMaker.instance.PlayerDisconnected (this, matchID);
            RpcDisconnectGame ();
            networkMatch.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame () {
            ClientDisconnect ();
        }

        void ClientDisconnect () {
            // if (playerLobbyUI != null) {
            //     if (!isServer) {
            //         Destroy (playerLobbyUI);
            //     } else {
            //         playerLobbyUI.SetActive (false);
            //     }
            // }
        }

        /* 
            SEARCH MATCH
        */

        public void SearchGame () {
            Debug.LogWarning("trying to search for game");
            // MatchInfo _matchInfo = new MatchInfo();
            // _matchInfo.maxPlayers = (byte)DataHandler.Instance.maxPlayers;
            // _matchInfo.maxPlayersToStart = DataHandler.Instance.maxPlayers;
            // _matchInfo.inviteCode = null;
            // NetworkManager_Custom.Instance.JoinMatch(_matchInfo);
            CmdSearchGame ();
        }

        [Command]
        void CmdSearchGame () {
            if (MatchMaker.instance.SearchGame (this, out playerIndex, out matchID)) {
                Debug.Log ($"<color=green>Game Found Successfully</color>");
                networkMatch.matchId = matchID.ToGuid ();
                TargetSearchGame (true, matchID, playerIndex);

                //Host
                // if (isServer && playerLobbyUI != null) {
                //     playerLobbyUI.SetActive (true);
                // }
            } else {
                Debug.Log ($"<color=red>Game Search Failed</color>");
                TargetSearchGame (false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetSearchGame (bool success, string _matchID, int _playerIndex) {
            if (success)
            {
                playerIndex = _playerIndex;
                matchID = _matchID;
                Debug.Log ($"MatchID: {matchID} == {_matchID} | {success}");
                //UILobby.instance.SearchGameSuccess (success, _matchID);
            }
            else
            {
                HostGame(true);
            }
        }

        /* 
            MATCH PLAYERS
        */
        
        [Server]
        public void MatchFull()
        {
            RPC_MatchFull();
        }

        [TargetRpc]
        private void RPC_MatchFull()
        {
            UIMANAGER.Instance.OpenLoadingScreen();
        }
        
        // [TargetRpc]
        // void TargetPlayerCountUpdated (int playerCount) {
        //     if (playerCount >= DataHandler.Instance.maxPlayers) {
        //         Debug.LogWarning("Room is full");
        //         // Move Player To Lobby
        //         UIMANAGER.Instance.OpenLoadingScreen();
        //         // BeginGame();
        //     } else {
        //         Debug.LogWarning("Waiting for Other players");
        //     }
        // }

        /* 
            BEGIN MATCH
        */

        public void SetupForLobby()
        {
            SwitchPosition(GameStateEnum.LOBBY);
            if (isLocalPlayer)
            {
                cameraController.SetActive(true);
                cameraController.transform.SetParent(null);
            }
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].SetActive(true);
            }
            gameObject.layer = LayerMask.NameToLayer("Character");
            GetComponent<Rigidbody>().useGravity = true;
            UIMANAGER.Instance.OpenLobbyUI();
            // CMD_SetupForLobby();
        }

        // [Command]
        // private void CMD_SetupForLobby()
        // {
        //     RPC_SetupForLobby();
        // }
        //
        // [TargetRpc]
        // private void RPC_SetupForLobby()
        // {
        //     for (int i = 0; i < meshes.Count; i++)
        //     {
        //         meshes[i].SetActive(true);
        //     }
        //     gameObject.layer = LayerMask.NameToLayer("Character");
        // }
        //
        [Command]
        private void SwitchPosition(GameStateEnum _newState)
        {
            Debug.LogWarning("Trying to switch position");
            RepositionHandler.Instance.SwitchPosition(_newState,this);
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].SetActive(true);
            }
            gameObject.layer = LayerMask.NameToLayer("Character");
            
        }        
        private void LoadLobbyScene()
        {
            SceneManager.LoadScene ("Lobby");
        }

        [TargetRpc]
        public void RPC_SetPosition(Vector3 _newPosition,Vector3 _newRotation)
        {
            transform.position = _newPosition;
            transform.eulerAngles = _newRotation;
        }
        
        public void BeginGame () {
            CmdBeginGame ();
        }

        [Command]
        void CmdBeginGame () {
            MatchMaker.instance.BeginGame (matchID);
            Debug.Log ($"<color=red>Game Beginning</color>");
        }

        public void StartGame () { //Server
            TargetBeginGame ();
        }

        [TargetRpc]
        void TargetBeginGame () {
            Debug.Log ($"MatchID: {matchID} | Beginning");
            //Additively load game scene
            SceneManager.LoadScene ("Gameplay");
        }


        // public void EnablePlayerController()
        // {
        //     playerController.SetActive(true);
        //     cameraController.SetActive(true);
        // }
        
    }