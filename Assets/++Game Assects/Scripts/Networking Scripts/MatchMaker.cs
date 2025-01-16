using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


    [System.Serializable]
    public class Match {
        public string matchID;
        public bool publicMatch;
        public bool inMatch;
        public bool matchFull;
        public int maxPlayers;
        public List<Player> players = new List<Player> ();

        public Match (string matchID, Player player, bool publicMatch, int maxPlayers) {
            matchFull = false;
            inMatch = false;
            this.matchID = matchID;
            this.publicMatch = publicMatch;
            this.maxPlayers = maxPlayers;
            players.Add (player);
        }

        public Match () { }
    }

    public class MatchMaker : NetworkBehaviour {

        public static MatchMaker instance;

        public SyncList<Match> matches = new SyncList<Match> ();
        public SyncList<String> matchIDs = new SyncList<String> ();
        
        void Start () {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool HostGame (string _matchID, Player _player, bool publicMatch, out int playerIndex, int maxPlayers) {
            playerIndex = -1;

            if (!matchIDs.Contains (_matchID))
            {
                matchIDs.Add (_matchID);
                Match match = new Match (_matchID, _player, publicMatch, maxPlayers);
                matches.Add (match);
                Debug.Log ($"Match generated");
                _player.currentMatch = match;
                playerIndex = 1;
                match.players[0].PlayerCountUpdated (match.players.Count);
                // }

                if (match.players.Count == match.maxPlayers) {
                    match.matchFull = true;
                    Debug.LogWarning("Match Is FUll Loading New Scene");
                    NetworkManagerBattleRoyale.singleton.ServerChangeScene("Lobby",match.matchID);
                }
                return true;
            } else {
                Debug.Log ($"Match ID already exists");
                return false;
            }
        }

        public bool JoinGame (string _matchID, Player _player, out int playerIndex) {
            playerIndex = -1;

            if (matchIDs.Contains (_matchID)) {

                for (int i = 0; i < matches.Count; i++) {
                    if (matches[i].matchID == _matchID) {
                        if (!matches[i].inMatch && !matches[i].matchFull) {
                            matches[i].players.Add (_player);
                            _player.currentMatch = matches[i];
                            playerIndex = matches[i].players.Count;

                            // for (int j = 0; j < matches[i].players.Count; j++)
                            // {
                                matches[i].players[0].PlayerCountUpdated (matches[i].players.Count);
                            // }

                            if (matches[i].players.Count == matches[i].maxPlayers) {
                                matches[i].matchFull = true;
                                Debug.LogWarning("Match Is FUll Loading New Scene");
                                NetworkManagerBattleRoyale.singleton.ServerChangeScene("Lobby",matches[i].matchID);
                            }
                            
                            break;
                        } else {
                            return false;
                        }
                    }
                }

                Debug.Log ($"Match joined");
                return true;
            } else {
                Debug.Log ($"Match ID does not exist");
                return false;
            }
        }

        public Match GetMatch(string _matchID)
        {
            Match match = matches.Find( X=> X.matchID == _matchID);
            return match;
        }
        
        public bool SearchGame (Player _player, out int playerIndex, out string matchID) {
            playerIndex = -1;
            matchID = "";

            if (matches.Count <= 0)
            {
                // Create Match
                // Player.localPlayer.HostGame(true);
                return false;
            }
            
            for (int i = 0; i < matches.Count; i++) {
                Debug.Log ($"Checking match {matches[i].matchID} | inMatch {matches[i].inMatch} | matchFull {matches[i].matchFull} | publicMatch {matches[i].publicMatch}");
                if (!matches[i].inMatch && !matches[i].matchFull && matches[i].publicMatch) {
                    if (JoinGame (matches[i].matchID, _player, out playerIndex)) {
                        matchID = matches[i].matchID;
                        return true;
                    }
                }
                else
                {
                    // Create Match
                    // Player.localPlayer.HostGame(true);
                    return false;
                    //CreateGame();
                }
            }

            return false;
        }

        public void BeginGame (string _matchID) {
            for (int i = 0; i < matches.Count; i++) {
                if (matches[i].matchID == _matchID) {
                    matches[i].inMatch = true;
                    foreach (var player in matches[i].players) {
                        player.StartGame ();
                    }
                    break;
                }
            }
        }

        public static string GetRandomMatchID () {
            string _id = string.Empty;
            for (int i = 0; i < 5; i++) {
                int random = UnityEngine.Random.Range (0, 36);
                if (random < 26) {
                    _id += (char) (random + 65);
                } else {
                    _id += (random - 26).ToString ();
                }
            }
            Debug.Log ($"Random Match ID: {_id}");
            return _id;
        }

        public void PlayerDisconnected (Player player, string _matchID) {
            for (int i = 0; i < matches.Count; i++) {
                if (matches[i].matchID == _matchID) {
                    int playerIndex = matches[i].players.IndexOf (player);
                    if (matches[i].players.Count > playerIndex) matches[i].players.RemoveAt (playerIndex);
                    Debug.Log ($"Player disconnected from match {_matchID} | {matches[i].players.Count} players remaining");

                    if (matches[i].players.Count == 0) {
                        Debug.Log ($"No more players in Match. Terminating {_matchID}");
                        matches.RemoveAt (i);
                        matchIDs.Remove (_matchID);
                    } else {
                        matches[i].players[0].PlayerCountUpdated (matches[i].players.Count);
                    }
                    break;
                }
            }
        }

    }

    public static class MatchExtensions {
        public static Guid ToGuid (this string id) {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider ();
            byte[] inputBytes = Encoding.Default.GetBytes (id);
            byte[] hashBytes = provider.ComputeHash (inputBytes);

            return new Guid (hashBytes);
        }
    }