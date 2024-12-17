using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMANAGER : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playModePanel;


    public void ClosePlayModePanel()
    {
        playModePanel.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    public void OpenPlayModePanel()
    {
        playModePanel.SetActive(true);
        mainMenu.SetActive(false);
    }
    
}
