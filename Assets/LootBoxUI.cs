using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LootBoxUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    
    [SerializeField] private List<LootBoxSlot> allLootBoxSlot;


    public void Activate()
    {
        panel.SetActive(true);
    }

    public void Deactivate()
    {
        panel.SetActive(false);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
