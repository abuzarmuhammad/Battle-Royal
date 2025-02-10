using System;
using Mirror;
using UnityEngine;

public class InteractionHandler : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!isLocalPlayer)
            return;

        if (other.transform.CompareTag("Lootbox"))
        {
            Debug.LogWarning("Entered Lootbox");
            EventManager.OnEnterTrigger_LootBox(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!isLocalPlayer)
            return;

        if (other.transform.CompareTag("Lootbox"))
        {
            Debug.LogWarning("Exit Lootbox");
            EventManager.OnEnterTrigger_LootBox(false);
        }
    }
}
