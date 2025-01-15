using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MatchSystem : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HostMatch()
    {
        Debug.Log("HostMatch");
        CMDHostMatch();
        
    }

    [Command]
    private void CMDHostMatch()
    {
        Debug.Log("Making Match");
    }

    [TargetRpc]
    private void TargetRpcHostMatch()
    {
        Debug.Log("TargetRpcHostMatch");
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
