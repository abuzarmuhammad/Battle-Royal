using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayModePanel : MonoBehaviour
{
    [SerializeField] private Image toggleButton;
    [SerializeField] private Image toggleBg;
    [SerializeField] private Transform toggleOffTransform;
    [SerializeField] private Transform toggleOnTransform;
    

    private bool autoMatching = true;

    public void ToggleAutoMatching()
    {
        autoMatching = !autoMatching;
        LeanTween.moveLocalX(toggleButton.gameObject,autoMatching ? toggleOnTransform.localPosition.x : toggleOffTransform.localPosition.x,0.1f);
        if (autoMatching)
        {
            //rgba(137, 204, 140, 1)
            Color greenColor = new Color(137.0f / 255.0f, 204.0f / 255.0f, 140.0f / 255.0f);
            LeanTween.imageColor(toggleButton.GetComponent<RectTransform>(), greenColor, 0.2f);
            LeanTween.imageColor(toggleBg.GetComponent<RectTransform>(), greenColor, 0.2f);
        }
        else
        {
            Color grey = new Color(.70f, .70f, .70f);
            LeanTween.imageColor(toggleButton.GetComponent<RectTransform>(), grey, 0.2f);
            LeanTween.imageColor(toggleBg.GetComponent<RectTransform>(), grey, 0.2f);
        }
    }
    
}
