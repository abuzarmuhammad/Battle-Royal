using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMANAGER : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playModePanel;
    [SerializeField] private GameObject findFriendsPanel;
    [SerializeField] private GameObject inventoryPanel;


    public void OpenInventoryPanel()
    {
        inventoryPanel.SetActive(true);
    }

    public void CloseInventoryPanel()
    {
        inventoryPanel.SetActive(false);
    }

    public void OpenFindFriendsPanel()
    {
        findFriendsPanel.SetActive(true);
    }

    public void CloseFindFriendsPanel()
    {
        findFriendsPanel.SetActive(false);
    }
    
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
