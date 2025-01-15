using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeftNavigationSlot : MonoBehaviour
{
    [SerializeField] private GameObject redBar;
    [SerializeField] private TMP_Text barText;
    [SerializeField] private Color barColor;
    
    public void Enable()
    {
        redBar.SetActive(true);
        barText.color = barColor;
    }

    public void Disable()
    {
        redBar.SetActive(false);
        barText.color = Color.white;
    }
    
}
