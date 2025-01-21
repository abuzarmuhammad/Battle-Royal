using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameStateEnum
{
    MAIN_MENU,
    LOBBY,
    GAME_PLAY,
}

public class RepositionHandler : NetworkBehaviour
{
    public static RepositionHandler Instance;
    
    [SerializeField] private GameStateEnum currentGameState = GameStateEnum.MAIN_MENU;
    [SerializeField] private  List<GameState> allGameStates = new List<GameState>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentGameState = GameStateEnum.MAIN_MENU; 
        SwitchState(GameStateEnum.MAIN_MENU);
        SwitchPosition(Player.localPlayer.transform);
    }

    public void SwitchState(GameStateEnum _newState)
    {
        if (!Equals(_newState, currentGameState))
        {
            currentGameState = _newState;
        }
    }
    
    public void SwitchPosition(Transform _transform)
    {
        Vector3 newPosition = allGameStates.Find(x => x.gameState == currentGameState)._targetTransform.position;
        _transform.position = newPosition;
    }
    
}
[Serializable]
public class GameState
{
    public GameStateEnum gameState;
    public Transform _targetTransform;
}

