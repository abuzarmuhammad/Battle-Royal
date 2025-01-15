using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public AutoHostClient autoHostClient;
    
    public void LoadMainMenu()
    {
        InvokeRepeating(nameof(TryToLoadMainMenu),0.1f,0.1f);
    }
    
    private void TryToLoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        CancelInvoke(nameof(TryToLoadMainMenu));
    }
    
}
