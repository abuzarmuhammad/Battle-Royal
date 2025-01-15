using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mirror.Examples.MultipleMatch;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour
{
    /// <summary>
    /// Match Controllers listen for this to terminate their match and clean up
    /// </summary>
    public event Action<NetworkConnectionToClient> OnPlayerDisconnected;

    /// <summary>
    /// Cross-reference of client that created the corresponding match in openMatches below
    /// </summary>
    //internal static readonly Dictionary<NetworkConnectionToClient, Guid> playerMatches = new Dictionary<NetworkConnectionToClient, Guid>();

    /// <summary>
    /// Open matches that are available for joining
    /// </summary>
    internal static readonly Dictionary<Guid, MatchInfo> openMatches = new Dictionary<Guid, MatchInfo>();

    /// <summary>
    /// Network Connections of all players in a match
    /// </summary>
    internal static readonly Dictionary<Guid, HashSet<NetworkConnectionToClient>> matchConnections = new Dictionary<Guid, HashSet<NetworkConnectionToClient>>();

    /// <summary>
    /// Player informations by Network Connection
    /// </summary>
    internal static readonly Dictionary<NetworkConnection, PlayerInfo> playerInfos = new Dictionary<NetworkConnection, PlayerInfo>();

    /// <summary>
    /// Network Connections that have neither started nor joined a match yet
    /// </summary>
    internal static readonly List<NetworkConnectionToClient> waitingConnections = new List<NetworkConnectionToClient>();

    /// <summary>
    /// GUID of a match the local player has created
    /// </summary>
    internal Guid localPlayerMatch = Guid.Empty;

    /// <summary>
    /// GUID of a match the local player has joined
    /// </summary>
    internal Guid localJoinedMatch = Guid.Empty;

    /// <summary>
    /// GUID of a match the local player has selected in the Toggle Group match list
    /// </summary>
    internal Guid selectedMatch = Guid.Empty;

    // Used in UI for "Player #"
    int playerIndex = 1;


    List<GameObject> players;
    MatchInfo currentMatchInfo;
    bool lobbyStarted = false;
    bool everyoneReady = true;
    bool owner;

    // RuntimeInitializeOnLoadMethod -> fast playmode without domain reload
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetStatics()
    {
        //playerMatches.Clear();
        openMatches.Clear();
        matchConnections.Clear();
        playerInfos.Clear();
        waitingConnections.Clear();
    }

    #region UI Functions

    // Called from several places to ensure a clean reset
    //  - MatchNetworkManager.Awake
    //  - OnStartServer
    //  - OnStartClient
    //  - OnClientDisconnect
    //  - ResetCanvas
    internal void InitializeData()
    {
        //playerMatches.Clear();
        openMatches.Clear();
        matchConnections.Clear();
        waitingConnections.Clear();
        localPlayerMatch = Guid.Empty;
        localJoinedMatch = Guid.Empty;
        lobbyStarted = false;
        everyoneReady = true;
    }

    // Called from OnStopServer and OnStopClient when shutting down
    void Reset()
    {
        InitializeData();
        //gameObject.SetActive(false);
    }

    #endregion

    #region Button Calls

    /// <summary>
    /// Called from <see cref="MatchGUI.OnToggleClicked"/>
    /// </summary>
    /// <param name="matchId"></param>
    public void SelectMatch(MatchInfo _matchInfo)
    {
        if (!NetworkClient.active) return;

        var matchID = openMatches.Where(x => x.Value.maxPlayers == _matchInfo.maxPlayers).Select(x => x.Key).ToList();

        if(matchID.Count == 0)
        {
            CreateMatch(_matchInfo);
            Debug.Log("No Match Found");
        }
        else
        {
            Debug.Log(matchID.Count);
            selectedMatch = matchID[0];
            RequestJoinMatch();
            Debug.Log("Match Found");
        }
    }

    // public void SelectPrivateMatch(MatchInfo _matchInfo)
    // {
    //     if (!NetworkClient.active) return;
    //
    //     var matchID = openMatches.Where(x => x.Value.inviteCode == _matchInfo.inviteCode).Select(x => x.Key).ToList();
    //
    //     if (matchID.Count == 0)
    //     {
    //         Debug.Log("No Match Found");
    //         UIManager.GetInstance().ActiveMessagePanel("Please enter correct code");
    //     }
    //     else
    //     {
    //         Debug.Log(matchID.Count);
    //         currentMatchInfo = openMatches[matchID[0]];
    //         if (currentMatchInfo.entryFee <= UIManager.GetInstance().gameManager.GetPlayerProfileMetaData().user_profile_data.totalCoins)
    //         {
    //             selectedMatch = matchID[0];
    //             RequestJoinMatch();
    //         }
    //         else
    //             UIManager.GetInstance().ActiveMessagePanel("Not Enough Coins!, This Match Requires " + currentMatchInfo.entryFee + " Coins.");
    //         Debug.Log("Match Found");
    //     }
    // }

    /// <summary>
    /// Assigned in inspector to Create button
    /// </summary>
    public void RequestCreateMatch()
    {
        if (!NetworkClient.active) return;
        
        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Create , matchInfo = currentMatchInfo } );

        Debug.Log("Network Client is : " + NetworkClient.active);
    }

    /// <summary>
    /// Assigned in inspector to Join button
    /// </summary>
    public void RequestJoinMatch()
    {
        if (!NetworkClient.active || selectedMatch == Guid.Empty) return;

        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Join, matchId = selectedMatch });
    }

    /// <summary>
    /// Assigned in inspector to Leave button
    /// </summary>
    public void RequestLeaveMatch()
    {
        if (!NetworkClient.active || localJoinedMatch == Guid.Empty) return;

        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Leave, matchId = localJoinedMatch });
    }

    /// <summary>
    /// Assigned in inspector to Cancel button
    /// </summary>
    public void RequestCancelMatch()
    {
        if (!NetworkClient.active || currentMatchInfo.matchId == Guid.Empty) return;

        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Cancel, matchId = currentMatchInfo.matchId });
    }

    /// <summary>
    /// Assigned in inspector to Ready button
    /// </summary>
    public void RequestReadyChange()
    {
        if (!NetworkClient.active || (localPlayerMatch == Guid.Empty && localJoinedMatch == Guid.Empty)) return;

        Guid matchId = localPlayerMatch == Guid.Empty ? localJoinedMatch : localPlayerMatch;

        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Ready, matchId = matchId });
    }

    /// <summary>
    /// Assigned in inspector to Start button
    /// </summary>
    public void RequestStartMatch()
    {
        Debug.Log("Inside request start match");
        Debug.Log(localPlayerMatch.ToString());
        if (!NetworkClient.active) return;
        Debug.Log("Network Clientis active");
        NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Start, matchId = currentMatchInfo.matchId });
    }

    /// <summary>
    /// Called from <see cref="MatchController.RpcExitGame"/>
    /// </summary>
    public void OnMatchEnded()
    {
        if (!NetworkClient.active) return;

        localPlayerMatch = Guid.Empty;
        localJoinedMatch = Guid.Empty;
        lobbyStarted = false;
        everyoneReady = true;
        // myIndexInGame = 0;
        //UIManager.GetInstance().RestMenusToHome();
    }

    /// <summary>
    /// Sends updated match list to all waiting connections or just one if specified
    /// </summary>
    /// <param name="conn"></param>
    internal void SendMatchList(NetworkConnectionToClient conn = null)
    {
        if (!NetworkServer.active) return;

        if (conn != null)
        {
            conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.List, matchInfos = openMatches.Values.ToArray() });
        }
        else
        {
            foreach (NetworkConnectionToClient waiter in waitingConnections)
            {
                waiter.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.List, matchInfos = openMatches.Values.ToArray() });
            }
        }
    }

    #endregion

    #region Server & Client Callbacks

    // Methods in this section are called from MatchNetworkManager's corresponding methods

    internal void OnStartServer()
    {
        if (!NetworkServer.active) return;

        InitializeData();
        NetworkServer.RegisterHandler<ServerMatchMessage>(OnServerMatchMessage);
    }

    internal void OnServerReady(NetworkConnectionToClient conn)
    {
        if (!NetworkServer.active) return;

        waitingConnections.Add(conn);
        playerInfos.Add(conn, new PlayerInfo { playerIndex = this.playerIndex, ready = false });
        playerIndex++;

        SendMatchList();
    }

    internal void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (!NetworkServer.active) return;

        // Invoke OnPlayerDisconnected on all instances of MatchController
        //OnPlayerDisconnected?.Invoke(conn);

        //Guid matchId;
        //if (playerMatches.TryGetValue(conn, out matchId))
        //{
        //    playerMatches.Remove(conn);
        //    openMatches.Remove(matchId);

        //    foreach (NetworkConnectionToClient playerConn in matchConnections[matchId])
        //    {
        //        PlayerInfo _playerInfo = playerInfos[playerConn];
        //        _playerInfo.ready = false;
        //        _playerInfo.matchId = Guid.Empty;
        //        playerInfos[playerConn] = _playerInfo;
        //        playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });
        //    }
        //}

        Debug.Log("above disconnected");
        foreach (KeyValuePair<Guid, HashSet<NetworkConnectionToClient>> kvp in matchConnections)
        {
            kvp.Value.Remove(conn);
        }

        PlayerInfo playerInfo = playerInfos[conn];
      
        if (playerInfo.matchId != Guid.Empty && openMatches.ContainsKey(playerInfo.matchId))
            openMatches.Remove(playerInfo.matchId);
        HashSet<NetworkConnectionToClient> connections;
        if (matchConnections.TryGetValue(playerInfo.matchId, out connections))
        {
            Debug.Log("disconnect matchid found");


            PlayerInfo[] infos = connections.Select(playerConn => playerInfos[playerConn]).ToArray();
            Debug.Log("hello");
            bool isOwnerAvailable = false;
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].isOwner)
                {
                    isOwnerAvailable = true;
                    break;
                }
            }
			Debug.Log("hello2");
			if (!isOwnerAvailable)
            {
                if (infos.Length > 0)
                {
                    infos[0].isOwner = true;
                    matchConnections[playerInfo.matchId].First().Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateOwner, playerInfo = infos[0] });
                }
            }
			Debug.Log("hello3");
			foreach (NetworkConnectionToClient playerConn in matchConnections[playerInfo.matchId])
            {
                if (playerConn != conn)
                {
                    Debug.Log("Disconnect on clients called");
                    playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
                }
            }
			Debug.Log("hello4");
            if(infos.Length > 0)
            {
                NetworkServer.RemovePlayerForConnection(conn, true);
            }
            else
            {
                matchConnections.Remove(playerInfo.matchId);
            }
		}

        SendMatchList();
    }

    internal void OnStopServer()
    {
        Reset();
    }

    internal void OnClientConnect()
    {
        playerInfos.Add(NetworkClient.connection, new PlayerInfo { playerIndex = this.playerIndex, ready = false });
    }

    internal void OnStartClient()
    {
        if (!NetworkClient.active) return;

        InitializeData();
        NetworkClient.RegisterHandler<ClientMatchMessage>(OnClientMatchMessage);
    }

    internal void OnClientDisconnect()
    {
        if (!NetworkClient.active) return;

        InitializeData();
    }

    internal void OnStopClient()
    {
        Reset();
    }

    #endregion

    #region Server Match Message Handlers

    void OnServerMatchMessage(NetworkConnectionToClient conn, ServerMatchMessage msg)
    {
        if (!NetworkServer.active) return;

        switch (msg.serverMatchOperation)
        {
            case ServerMatchOperation.None:
                {
                    Debug.LogWarning("Missing ServerMatchOperation");
                    break;
                }
            case ServerMatchOperation.Create:
                {
                    Debug.Log("Match Create enum sent.");
                    OnServerCreateMatch(conn, msg.matchInfo);
                    break;
                }
            case ServerMatchOperation.Cancel:
                {
                    OnServerCancelMatch(conn, msg.matchId);
                    break;
                }
            case ServerMatchOperation.Start:
                {
                    OnServerStartMatch(conn, msg.matchId);
                    break;
                }
            case ServerMatchOperation.Join:
                {
                    OnServerJoinMatch(conn, msg.matchId);
                    break;
                }
            case ServerMatchOperation.Leave:
                {
                    OnServerLeaveMatch(conn, msg.matchId);
                    break;
                }
            case ServerMatchOperation.Ready:
                {
                    OnServerPlayerReady(conn, msg.matchId);
                    break;
                }
        }
    }

    void OnServerPlayerReady(NetworkConnectionToClient conn, Guid matchId)
    {
        if (!NetworkServer.active) return;

        PlayerInfo playerInfo = playerInfos[conn];
        playerInfo.ready = !playerInfo.ready;
        playerInfos[conn] = playerInfo;

        HashSet<NetworkConnectionToClient> connections = matchConnections[matchId];
        PlayerInfo[] infos = connections.Select(playerConn => playerInfos[playerConn]).ToArray();

        foreach (NetworkConnectionToClient playerConn in matchConnections[matchId])
        {
            playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
        }
    }

    void OnServerLeaveMatch(NetworkConnectionToClient conn, Guid matchId)
    {
        Debug.Log("above leave server");
        if (!NetworkServer.active) return;
        Debug.Log("inside leave server");
        MatchInfo matchInfo = openMatches[matchId];
        matchInfo.players--;
        openMatches[matchId] = matchInfo;

        PlayerInfo playerInfo = playerInfos[conn];
        playerInfo.ready = false;
        playerInfo.matchId = Guid.Empty;
        playerInfo.isOwner = false;
        playerInfos[conn] = playerInfo;

        foreach (KeyValuePair<Guid, HashSet<NetworkConnectionToClient>> kvp in matchConnections)
        {
            kvp.Value.Remove(conn);
        }

        HashSet<NetworkConnectionToClient> connections = matchConnections[matchId];
        PlayerInfo[] infos = connections.Select(playerConn => playerInfos[playerConn]).ToArray();

        bool isOwnerAvailable = false;
        for(int i=0; i< infos.Length;i++)
        {
            if(infos[i].isOwner)
            {
                isOwnerAvailable = true;
                break;
            }
        }
        if (!isOwnerAvailable)
        {
            infos[0].isOwner = true;
            matchConnections[matchId].First().Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateOwner, playerInfo = infos[0] });
        }
        Debug.Log(isOwnerAvailable);
        foreach (NetworkConnectionToClient playerConn in matchConnections[matchId])
        {
            playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
        }

        NetworkServer.RemovePlayerForConnection(conn, true);
        SendMatchList();

        conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });
    }

    void OnServerCreateMatch(NetworkConnectionToClient conn, MatchInfo matchInfo)
    {
        Debug.Log("Network Server is : " + NetworkServer.active);
        if (!NetworkServer.active) return;// || playerMatches.ContainsKey(conn)) return;
        currentMatchInfo = matchInfo;
        Guid newMatchId = Guid.NewGuid();
        matchConnections.Add(newMatchId, new HashSet<NetworkConnectionToClient>());
        matchConnections[newMatchId].Add(conn);
        //playerMatches.Add(conn, newMatchId);
        //openMatches.Add(newMatchId, new MatchInfo { matchId = newMatchId, maxPlayers = 2, players = 1 });
        currentMatchInfo.matchId = newMatchId;
        currentMatchInfo.players = 1;

        openMatches.Add(newMatchId, currentMatchInfo);

        PlayerInfo playerInfo = playerInfos[conn];
        playerInfo.ready = false;
        playerInfo.matchId = newMatchId;
        playerInfo.isOwner = true;
        playerInfos[conn] = playerInfo;

        PlayerInfo[] infos = matchConnections[newMatchId].Select(playerConn => playerInfos[playerConn]).ToArray();

        SendMatchList();

        conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Created, matchId = newMatchId, playerInfos = infos });
    }

    void OnServerCancelMatch(NetworkConnectionToClient conn, Guid matchId)
    {
        Debug.Log("hello above");
        if (!NetworkServer.active) return;
        Debug.Log("hello inside");
        conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Cancelled });
        conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });

        if (!openMatches.ContainsKey(matchId))
            return;
        MatchInfo matchInfo = openMatches[matchId];

        if (matchInfo.players > 1)
        {
            Debug.Log("more than 1 player");
            OnServerLeaveMatch(conn, matchId);
        }
        else
        {
            openMatches.Remove(matchId);

            foreach (NetworkConnectionToClient playerConn in matchConnections[matchId])
            {
                PlayerInfo playerInfo = playerInfos[playerConn];
                playerInfo.ready = false;
                playerInfo.matchId = Guid.Empty;
                playerInfos[playerConn] = playerInfo;
                playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Departed });
            }

            matchConnections.Remove(matchId);

            SendMatchList();
        }
        NetworkServer.RemovePlayerForConnection(conn, true);
    }

    void OnServerStartMatch(NetworkConnectionToClient conn, Guid matchId)
    {
        Debug.Log("Inside server start match");
        if (!NetworkServer.active) return;
        bool prefabsSpawned = false;
        for (int i = 0; i < NetworkManager.singleton.spawnPrefabs.Count; i++)
        {
            GameObject go = Instantiate(NetworkManager.singleton.spawnPrefabs[i]);
            go.GetComponent<NetworkMatch>().matchId = matchId;
            Debug.Log(go.GetComponent<NetworkMatch>().matchId.ToString());
            NetworkServer.Spawn(go);
            //RpcSpawnPrefabs(go.GetComponent<NetworkIdentity>().assetId);
        }
        foreach (NetworkConnectionToClient playerConn in matchConnections[matchId])
        {
            Debug.Log("Instantiating players");
            GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
            player.GetComponent<NetworkMatch>().matchId = matchId;
            Debug.Log(player.GetComponent<NetworkMatch>().matchId.ToString());

            if (playerConn.identity != null)
                NetworkServer.ReplacePlayerForConnection(playerConn, player, true);

            NetworkServer.AddPlayerForConnection(playerConn, player);

            //TODO : Set Player
            // player.GetComponent<MirrorBasics.Player>().SetMatchId(matchId);
            // player.GetComponent<MirrorBasics.Player>().RpcSetMatchId(matchId);
            if (!prefabsSpawned)
            {
                //player.GetComponent<Player>().SpawnPrefabs();
                prefabsSpawned = true;
            }

            playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Started });


            /* Reset ready state for after the match. */
            PlayerInfo playerInfo = playerInfos[playerConn];
            playerInfo.ready = false;
            playerInfos[playerConn] = playerInfo;
        }

        //playerMatches.Remove(conn);
        openMatches.Remove(matchId);
        //matchConnections.Remove(matchId);

        SendMatchList();
    }

    void OnServerJoinMatch(NetworkConnectionToClient conn, Guid matchId)
    {
        if (!NetworkServer.active || !matchConnections.ContainsKey(matchId) || !openMatches.ContainsKey(matchId)) return;

        MatchInfo matchInfo = openMatches[matchId];
        matchInfo.players++;
        openMatches[matchId] = matchInfo;
        matchConnections[matchId].Add(conn);

        PlayerInfo playerInfo = playerInfos[conn];
        playerInfo.ready = false;
        playerInfo.matchId = matchId;
        playerInfos[conn] = playerInfo;

        PlayerInfo[] infos = matchConnections[matchId].Select(playerConn => playerInfos[playerConn]).ToArray();
        SendMatchList();

        conn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Joined, matchId = matchId, playerInfos = infos });

        foreach (NetworkConnectionToClient playerConn in matchConnections[matchId])
        {
            playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.UpdateRoom, playerInfos = infos });
        }
    }

    #endregion

    #region Client Match Message Handler

    void OnClientMatchMessage(ClientMatchMessage msg)
    {
        if (!NetworkClient.active) return;

        switch (msg.clientMatchOperation)
        {
            case ClientMatchOperation.None:
                {
                    Debug.LogWarning("Missing ClientMatchOperation");
                    break;
                }
            case ClientMatchOperation.List:
                {
                    openMatches.Clear();
                    foreach (MatchInfo matchInfo in msg.matchInfos)
                    {
                        openMatches.Add(matchInfo.matchId, matchInfo);
                    }
                    break;
                }
            case ClientMatchOperation.Created:
                {
                    SetOwner(true);
                    localPlayerMatch = msg.matchId;
                    OnMatchCreated(msg.playerInfos);
                    break;
                }
            case ClientMatchOperation.Cancelled:
                {
                    Debug.Log("Cancelled");
                    localPlayerMatch = Guid.Empty;
                    BackFromMatchMaking();
                    break;
                }
            case ClientMatchOperation.Joined:
                {
                    SetOwner(false);
                    localJoinedMatch = msg.matchId;
                    OnMatchJoined(msg.playerInfos);
                    break;
                }
            case ClientMatchOperation.Departed:
                {
                    localJoinedMatch = Guid.Empty;
                    break;
                }
            case ClientMatchOperation.UpdateRoom:
                {
                    StartLobbyOrGame(msg.playerInfos);
                    break;
                }
            case ClientMatchOperation.Started:
                {
                    OnStartMatch(msg.matchId);
                    break;
                }
            case ClientMatchOperation.UpdateOwner:
                {
                    if(msg.playerInfo.isOwner)
                    {
                        SetOwner(true);
                    }
                    break;
                }
        }
    }

    #endregion

    void OnStartMatch(Guid _matchID)
    {
        // UIManager.GetInstance().gameManager.totalPlayer = currentMatchInfo.maxPlayersToStart;
        // UIManager.GetInstance().gameManager.DeductEntryFee(currentMatchInfo.entryFee);
        // UIManager.GetInstance().gameManager.SetGameType((GameType)currentMatchInfo.maxPlayersToStart);
        currentMatchInfo.matchStarted = true;
        Debug.Log("start match");
    }

    /// <summary>
    /// Just create match with matchInfo;
    /// </summary>
    /// <param name="_matchInfo"></param>
    public void CreateMatch(MatchInfo _matchInfo)
    {
        currentMatchInfo = new MatchInfo();
        currentMatchInfo = _matchInfo;
        RequestCreateMatch();
    }

    public void JoinMatch(MatchInfo _matchInfo)
    {
        if (_matchInfo.inviteCode == null)
            SelectMatch(_matchInfo);
        // else
            // SelectPrivateMatch(_matchInfo);
    }

    void OnMatchCreated(PlayerInfo[] playerInfos)
    {
        Debug.Log("Inside match is created" +  openMatches.Count);
        if (openMatches.ContainsKey(localPlayerMatch))
        {
            currentMatchInfo = openMatches[localPlayerMatch];
            
            Debug.Log("this match is created");
            // UIManager.GetInstance().gameManager.isPrivate = currentMatchInfo.isPrivate;
            // UIManager.GetInstance().gameManager.inviteCode = currentMatchInfo.inviteCode;
            // UIManager.GetInstance().SpawnNextPanel("GamePlayUI", true);
            // UIManager.GetInstance().gameManager.gamePlay.GetComponent<GamePlay>().SetActiveSelectedBoard(currentMatchInfo.environmentName);
            // UIManager.GetInstance().DisactiveLoadingPanel();
            currentPlayerCount = playerInfos.Length;
            // UIManager.GetInstance().crossFadeCallBack = SetJoinedPlayersCount;
            SendMatchList();
            RequestReadyChange();
        }
    }

    int currentPlayerCount;
    void SetJoinedPlayersCount()
    {
        // UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetJoinedPlayersCount(currentPlayerCount, currentMatchInfo.maxPlayersToStart);
        // UIManager.GetInstance().crossFadeCallBack = null;
    }
    void OnMatchJoined(PlayerInfo[] playerInfos)
    {
        currentMatchInfo = openMatches[localJoinedMatch];
        Debug.Log("this match is joined = " + currentMatchInfo.matchId);
        // UIManager.GetInstance().SpawnNextPanel("GamePlayUI", true);
        // UIManager.GetInstance().gameManager.gamePlay.GetComponent<GamePlay>().SetActiveSelectedBoard(currentMatchInfo.environmentName);
        // UIManager.GetInstance().DisactiveLoadingPanel();
        RequestReadyChange();
    }


    public void OnCancelCreateOrJoinGame()
    {
        if (localPlayerMatch != Guid.Empty)
        {
            Debug.Log("Cancel create game");
            RequestCancelMatch();
        }
        else if (localJoinedMatch != Guid.Empty)
        {
            Debug.Log("leave joined game");
            RequestLeaveMatch();
        }
    }

    public void OnCancelCreateGame()
    {
        Debug.Log("Cancel create game");
        RequestCancelMatch();
    }

    public void OnLeaveGame()
    {
        RequestLeaveMatch();
    }

    void BackFromMatchMaking()
    {
        //UIManager.GetInstance().BackButtonIsPressed();
        //UIManager.GetInstance().gameManager.gamePlay.GetComponent<GamePlay>().DisableAllBoards();
        //UIManager.GetInstance().GetComponent<Image>().enabled = true;
    }

    void StartLobbyOrGame(PlayerInfo[] playerInfos)
    {
        Debug.Log("inside start lobby");
        Debug.Log(playerInfos.Length);
        if (!currentMatchInfo.matchStarted)
        {
            currentPlayerCount = playerInfos.Length;
            // UIManager.GetInstance().gameManager.GetComponent<GameManager>().totalWinningCoins = currentMatchInfo.entryFee * currentMatchInfo.maxPlayersToStart;
            // if (UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>())
                SetJoinedPlayersCount();
            // else
                // UIManager.GetInstance().crossFadeCallBack = SetJoinedPlayersCount;
        }
        else
        {
            // UIManager.GetInstance().gameManager.totalPlayer--;
            // UIManager.GetInstance().gameManager.gamePlay.GetComponent<GamePlay>().isAnyPlayerDisconnected = true;
            // UIManager.GetInstance().gameManager.gamePlay.GetComponent<GamePlay>().WaitForDisconnect();
        }

        if (playerInfos.Length == currentMatchInfo.maxPlayersToStart)
        {
            //Debug.Log("Inside Player required completed.");
            //if (!lobbyStarted)
            //{
            //    lobbyStarted = true;
            //    Debug.Log("lobby started is true");
            //}
            //else
            //{
            Debug.Log("Everyone joined");
            everyoneReady = true;

            foreach (PlayerInfo playerInfo in playerInfos)
            {
                if (!playerInfo.ready)
                {
                    everyoneReady = false;
                }
            }

            if (everyoneReady)
            {
                if (owner)
                {
                    Debug.Log("everyoneReady = " + everyoneReady);
                    RequestStartMatch();
                }
            }
            //}
        }
        //if (everyoneReady == false)
        //    NetworkClient.connection.Send(new ServerMatchMessage { serverMatchOperation = ServerMatchOperation.Start });
    }

    public void SetOwner(bool owner)
    {
        this.owner = owner;
    }

    public bool IsWoner()
    {
        return owner;
    }

    public Dictionary<Guid, MatchInfo> GetOpenMatches()
    {
        Debug.Log("333" + openMatches.Count);
        return openMatches;
    }

    public Dictionary<NetworkConnection, PlayerInfo> GetPlayerInfos()
    {
        return playerInfos;
    }
}