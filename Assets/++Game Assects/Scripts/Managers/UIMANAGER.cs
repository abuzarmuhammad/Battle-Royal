using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PanelType
{
    MainMenu,
    Settings,
    Shop,
    Inventory,
    CustomizeCharacter,
    Friends,
    Exit,
    ModeSelection,
    AddFriends,
    RemoveFriends,
    PlayerProfile
}
[Serializable]
public class Panel
{
    public PanelType _type;
    public GameObject _panelObject;
}

public class UIMANAGER : MonoBehaviour
{
    public static UIMANAGER Instance;
    
    [SerializeField] private List<Panel> allPanels;
    [SerializeField] private GameObject LoadingScreen;
    
    
    private SplashScreen _splashScreen;
    private Stack<Panel> _panelStack;
    
    private void Awake()
    {
        Instance = this;
        _splashScreen = LoadingScreen.GetComponent<SplashScreen>();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    private void OnEnable()
    {
        _panelStack = new Stack<Panel>();
        _panelStack.Push(allPanels.Find(x=>x._type==PanelType.MainMenu));
        EventManager.OnPanelOpenClicked += OpenPanel;
        EventManager.OnPanelCloseClicked += ClosePanel;
    }
    
    private void OnDisable()
    {
        EventManager.OnPanelOpenClicked -= OpenPanel;
        EventManager.OnPanelCloseClicked -= ClosePanel;
    }
    
    public void OpenPanel(PanelType _type)
    {
        Panel panel = allPanels.Find(x => x._type == _type);
        if (_panelStack.Count > 1)
        {
            Panel _poppedPanel = _panelStack.Peek();
            _poppedPanel._panelObject.SetActive(false);
        }
        panel._panelObject.SetActive(true);
        _panelStack.Push(panel);
        LeanTween.alphaCanvas(panel._panelObject.GetComponent<CanvasGroup>(), 1, 0.2f);
    }

    public void ClosePanel()
    {
        if (_panelStack.Count > 1)
        {
            Panel _poppedPanel = _panelStack.Pop();
            LeanTween.alphaCanvas(_poppedPanel._panelObject.GetComponent<CanvasGroup>(), 0, 0.2f).setOnComplete(() =>
            {
                _poppedPanel._panelObject.SetActive(false);
            });
        }
    }
    
    public void ConnectClient()
    {
        Debug.LogWarning("Connecting to Client");
        Player.localPlayer.SearchGame();
    }

    private void OnConnectedToServer()
    {
        Debug.LogWarning("Connected To Server");
    }

    public void OpenLoadingScreen()
    {
        LoadingScreen.SetActive(true);
        Player[] allplayers = FindObjectsOfType<Player>();
        for (int i = 0; i < allplayers.Length; i++)
        {
            allplayers[i].SetupForLobby();
        } 
    }
}
