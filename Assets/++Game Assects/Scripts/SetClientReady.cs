using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SetClientReady : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        NetworkClient.Ready();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
