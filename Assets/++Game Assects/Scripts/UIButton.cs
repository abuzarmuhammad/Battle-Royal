using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    [SerializeField] private PanelType _type;
    [SerializeField] private bool panelClose;
    
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        if (!panelClose)
        {
            EventManager.OnPanelOpenClicked?.Invoke(_type);
        }
        else
        {
            EventManager.OnPanelCloseClicked?.Invoke();
        }
    }
}
