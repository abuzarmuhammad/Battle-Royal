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
    
    [SyncVar]
    [SerializeField] private GameStateEnum currentGameState = GameStateEnum.MAIN_MENU;
    [SerializeField] private  List<GameState> allGameStates = new List<GameState>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentGameState = GameStateEnum.MAIN_MENU; 
    }
    
    public void SwitchPosition(GameStateEnum _newState,Player _player)
    {
        CMD_GetPosition(_newState,_player);
    }
    
    
    private void CMD_GetPosition(GameStateEnum _newState,Player _player)
    {
        Debug.LogWarning("Getting Position from Server");
        if (!Equals(_newState, currentGameState))
        {
            currentGameState = _newState;
        }
        if (currentGameState.Equals(GameStateEnum.MAIN_MENU))
        {
            Vector3 newPosition = allGameStates.Find(x => x.gameState == currentGameState)._targetTransform[0].position;
            Vector3 newRotation = allGameStates.Find(x => x.gameState == currentGameState)._targetTransform[0].eulerAngles;
            _player.RPC_SetPosition(newPosition,newRotation);
        }
        else
        {
            List<Transform> newPositions = new List<Transform>();
            newPositions = allGameStates.Find(x => x.gameState == currentGameState)._targetTransform;
            int randomNumber = UnityEngine.Random.Range(0, newPositions.Count);
            Vector3 newPosition = newPositions[randomNumber].position;
            Vector3 newRotation = newPositions[randomNumber].eulerAngles;
            newPositions.RemoveAt(randomNumber);
            _player.RPC_SetPosition(newPosition,newRotation);
        }
    }
    
}
[Serializable]
public class GameState
{
    public GameStateEnum gameState;
    public List<Transform> _targetTransform;
}

