using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private List<LeftNavigationSlot> allRightSlots;
    [SerializeField] private List<GameObject> allPanels;

    [Header("--------Featued--------")]
    public List<GameObject> FeatureButton;
   // [Header("--------Coins--------")]
    [Header("--------Outfits--------")]
    public List<UIButtonVisuals> OutfitsRightScroll;
    [Header("--------Weapons--------")]
    public List<UIButtonVisuals> WeaponsRightScroll;

    public GameObject WeaponPurchasePopupObject;
    public GameObject WeaponPurchasePopupPanel;
    public RectTransform fromPosition;
    public RectTransform toPosition;


    public GameObject PurchaseCompletdPanel;
    // [Header("--------Vechicles--------")]

    [Header("--------Other--------")]
    public Sprite RightScrollUnSelect_Sprite;
    public Sprite RightScrollSelect_Sprite;


    private void OnEnable()
    {
        OpenPanel(0);
    }
    public void OpenPanel(int index)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(i == index);
            allRightSlots[i].Disable();
        }
        allRightSlots[index].Enable();
    }


    public void OpenFeature(int index)
    {
        for (int i = 0; i < FeatureButton.Count; i++)
        {
            FeatureButton[i].transform.GetChild(0).gameObject.SetActive(i == index);    
        }
    }

    public void OpenOutfitsRightScroll(int index)
    {
        for (int i = 0; i < OutfitsRightScroll.Count; i++)
        {
            if (i == index)
            {
                // Set selected sprite
                OutfitsRightScroll[i].Bg.sprite = RightScrollSelect_Sprite;
                OutfitsRightScroll[i].icon.color = Color.black; 
            }
            else
            {
                // Set unselected sprite
                OutfitsRightScroll[i].Bg.sprite = RightScrollUnSelect_Sprite;
                OutfitsRightScroll[i].icon.color = Color.white;
            }
        }
    }

    public void OpenWeaponsRightScroll(int index)
    {
        for (int i = 0; i < WeaponsRightScroll.Count; i++)
        {
            if (i == index)
            {
                // Set selected sprite
                WeaponsRightScroll[i].Bg.sprite = RightScrollSelect_Sprite;
                WeaponsRightScroll[i].icon.color = Color.black;
            }
            else
            {
                // Set unselected sprite
                WeaponsRightScroll[i].Bg.sprite = RightScrollUnSelect_Sprite;
                WeaponsRightScroll[i].icon.color = Color.white;
            }
        }
    }

    public void OpenWeaponPurchasePopup()
    {
        WeaponPurchasePopupObject.SetActive(true);

        // Animate only the Y position from `fromPosition` to `toPosition`
        LeanTween.value(gameObject, fromPosition.position.y, toPosition.position.y, 0.2f)
                 .setEase(LeanTweenType.easeOutQuad) // Set ease-out effect
                 .setOnUpdate((float newY) =>
                 {
                     MovePanel(WeaponPurchasePopupPanel, newY);
                 });
    }

    public void CloseWeaponPurchasePopup()
    {
        // Animate only the Y position from `fromPosition` to `toPosition`
        LeanTween.value(gameObject, toPosition.position.y, fromPosition.position.y, 0.2f)
                 .setEase(LeanTweenType.easeOutQuad) // Set ease-out effect
                .setOnUpdate((float newY) =>
                {
                    MovePanel(WeaponPurchasePopupPanel, newY);
                })
                .setOnComplete(() =>
                {
                    WeaponPurchasePopupObject.SetActive(false); // Call ClosePanel to close the popup
                });
    }

    public void PurchaseWeaponCompleted()
    {
        CloseWeaponPurchasePopup();
        PurchaseCompletdPanel.SetActive(true);
    }

    public void PurchaseCompletedContine()
    {
        PurchaseCompletdPanel.SetActive(false);
    }

    void MovePanel(GameObject target ,float newY)
    {
        // Update the target's position
        Vector3 newPosition = target.transform.position;
        newPosition.y = newY;
        target.transform.position = newPosition;
    }



    private void OnDisable()
    {
        PurchaseCompletdPanel.SetActive(false);
        WeaponPurchasePopupObject.SetActive(false);
    }
}
[System.Serializable]
public class UIButtonVisuals
{
    public Image Bg;
    public Image icon;
}
