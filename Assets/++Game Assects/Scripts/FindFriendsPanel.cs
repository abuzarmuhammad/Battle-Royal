using System;
using System.Collections.Generic;
using UnityEngine;

public class FindFriendsPanel : MonoBehaviour
{
    public List<Panels> FriendPanels;

    public enum PanelType
    {
        SearchFriends,
        FriendsRequest,
        SelectFriends
    }

    // Define a serializable struct
    [System.Serializable]
    public struct Panels
    {
        public PanelType Type;
        public GameObject Panel;
    }

    public void OpenFriendPanel(int PanelIndex)
    {
        if (PanelIndex < 0 || PanelIndex >= FriendPanels.Count)
        {
            Debug.LogWarning("Invalid panel index!");
            return;
        }

        foreach (var panel in FriendPanels)
        {
            if (FriendPanels.IndexOf(panel) == PanelIndex)
            {
                panel.Panel.SetActive(true); // Open the selected panel
            }
            else
            {
                panel.Panel.SetActive(false); // Close all other panels
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OpenFriendPanel(0);
    }

}
