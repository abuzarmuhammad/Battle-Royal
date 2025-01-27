using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start() => Invoke(nameof(AddSpawnPoint), .5f);
    private void OnDestroy() => LobbyPlayerSpawnManager.RemoveSpawnPoint(transform);

    private void AddSpawnPoint() => LobbyPlayerSpawnManager.AddSpawnPoint(transform);
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
