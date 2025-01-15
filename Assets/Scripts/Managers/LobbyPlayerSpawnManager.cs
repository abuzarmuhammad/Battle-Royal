using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using MirrorBasics;
using UnityEngine;

public class LobbyPlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private static List<Transform> spawnPoints;
    private int nextIndex = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        Player.localPlayer.EnablePlayerController();
        //NetworkClient.Ready();
    }
    
    public static void AddSpawnPoint(Transform transform)
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    //public override void OnStartServer() => NetworkManagerBattleRoyale.OnServerReadied += SpawnPlayer;
    
    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        /*Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        NetworkServer.Spawn(playerInstance, conn);

        nextIndex++;*/
    }
}
