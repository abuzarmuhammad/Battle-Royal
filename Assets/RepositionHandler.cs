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

public class RepositionHandler : MonoBehaviour
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
        Quaternion newRotation = allGameStates.Find(x => x.gameState == currentGameState)._targetTransform.rotation;
        Debug.LogWarning("setting position of " + _transform.name + " to " + newPosition + " and rotation to " + newRotation);
        _transform.position = newPosition;
        _transform.rotation = newRotation;
    }
    
}
[Serializable]
public class GameState
{
    public GameStateEnum gameState;
    public Transform _targetTransform;
}

