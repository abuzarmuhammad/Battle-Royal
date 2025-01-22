using System;
using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public Action OnLoadingDone;
    public void LoadMainMenu()
    {
        gameObject.SetActive(false);
        OnLoadingDone?.Invoke();
    }
}
