using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionButton : MonoBehaviour
{
    [SerializeField] private GameObject greenOutline;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text playerText;


    public void Select()
    {
        Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
        greenOutline.SetActive(true);
        icon.color = greenColor;
        buttonText.color = greenColor;
        playerText.color = Color.white;
    }

    public void UnSelect()
    {
        Color grey = new Color(.70f, .70f, .70f);
        greenOutline.SetActive(false);
        icon.color = Color.white;
        buttonText.color = Color.white;
        playerText.color = grey;
    }
    
    
}
