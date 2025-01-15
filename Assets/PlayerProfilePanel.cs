using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerProfilePanel : MonoBehaviour
{
    [SerializeField] private List<PlayerAttributes> playerAttributes = new List<PlayerAttributes>();
    [SerializeField] private  List<GameObject> allPanels = new List<GameObject>();
    [SerializeField] private List<LeftNavigationSlot> allLeftSlots;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePlayerAttributes();
    }

    public void OpenPanel(int index)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(false);
            allLeftSlots[i].Disable();
        }
        
        allPanels[index]?.SetActive(true);
        allLeftSlots[index].Enable();
        
    }
    
    private void UpdatePlayerAttributes()
    {
        for (int i = 0; i < playerAttributes.Count; i++)
        {
            string _value;
            DataHandler.Instance.IngameData.PlayerAttributesData.TryGetValue(playerAttributes[i].key, out _value);
            if (string.IsNullOrEmpty(_value))
            {
                _value = "0";
                DataHandler.Instance.IngameData.PlayerAttributesData.Add(playerAttributes[i].key, _value);
            }
            
            playerAttributes[i]._valueText.text = _value;
            
        }
    }
    
}
[Serializable]
public class PlayerAttributes
{
    public string key;
    public TMP_Text _valueText;
}

