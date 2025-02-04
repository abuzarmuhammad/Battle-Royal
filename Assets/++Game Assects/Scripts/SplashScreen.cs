using System;
using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public void LoadMainMenu()
    {
        gameObject.SetActive(false);
        Debug.LogWarning("Closing Loading Screen");
    }
}
