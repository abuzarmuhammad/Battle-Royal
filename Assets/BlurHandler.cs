using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurHandler : MonoBehaviour
{
    [SerializeField] private Camera blurrCamera;
    [SerializeField] private Material blurrMat;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (blurrCamera.targetTexture != null)
        {
            blurrCamera.targetTexture.Release();
        }
        
        blurrCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32,1);
        blurrMat.SetTexture("_zzZ", blurrCamera.targetTexture);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
