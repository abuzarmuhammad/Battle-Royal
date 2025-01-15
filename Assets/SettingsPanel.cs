using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private List<LeftNavigationSlot> allLeftSlots;
    [SerializeField] private List<GameObject> allPanels;
    

    public void OpenPanel(int index)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(i == index);
            allLeftSlots[i].Disable();
        }
        allLeftSlots[index].Enable();
    }
}
