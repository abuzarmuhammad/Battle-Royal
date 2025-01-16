//using Mirror;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class PlayerController : NetworkBehaviour
//{

//    [SyncVar(hook = nameof(SetMyShipsPosition))]
//    string myShipsPositionJson;

//    [SyncVar(hook = nameof(ChangeTurnOnNetwork))]
//    NetworkIdentity playerWithTrun;

//    [SyncVar(hook = nameof(UpdateMyHealthOnNetwork))]
//    public int myhealth;

//    [SyncVar(hook = nameof(UpdateMyPowerOnNetwork))]
//    int myPower;

//    [SyncVar(hook = nameof(AttackOnEnemy))]
//    int damage;

//    [SyncVar(hook = nameof(UpdateMyGridCells))]
//    string myGridMetaDataJson;


//    [SyncVar(hook = nameof(UpdateEnemyGridCells))]
//    string enemyGridMetaDataJson;


//    ShipsPositionMetaData shipsPositionMetaData;
   
//    GridCellsMetaData myGridCellsMetaData; // this is my data;
//    GridCellsMetaData myGridCellsMetaDataToReturn;

//    GridCellsMetaData enemyGridCellsMetaData;
//    GridCellsMetaData enemyGridCellsMetaDataToReturn;

//    string positions;
//    string myGridCells;
//    string enemyGridCells;

//    int powerOnChangeTrun = 2;
//    int powerUpPerHit = 1;
//    int heathToBe;
//    int powerToBe;


//    // Start is called before the first frame update
//    void Start()
//    {
//        if (isLocalPlayer)
//        {
//            this.transform.name = "localPlayer";
//            this.tag = "localPlayer";
//            SetMyHelth(17);
//            SetShipPositions();

         
//            GridCells(UIManager.GetInstance().myGridCellMetaData);
//        }
//        else
//        {
//            this.transform.name = "enemyPlayer";
//            this.tag = "enemyPlayer";
//        }
//    }

//    //it should be improved.
//    #region Do Attack
//    public void DoAttack(int _damage)
//    {
//        DamageValue(_damage);
//    }

//    [Command]
//    void DamageValue(int _damage)
//    {
//        damage = _damage;
//    }
//    public void AttackOnEnemy(int _, int _newDamage)
//    {
//        if (!isServer)
//        {
//            if (_newDamage > 0)
//            {
//                if (!isLocalPlayer)
//                {
//                    // here we can attack on enemy with _newDamange
//                    GameObject.FindGameObjectWithTag("localPlayer").GetComponent<PlayerController>().TakeDamage(_newDamage);
//                }
//                else
//                {
//                    DoAttack(0);
//                }
//            }

//        }
//    }

//    #endregion

//    #region ship Positions

//    void SetShipPositions()
//    {
//        shipsPositionMetaData = new ShipsPositionMetaData();
//        shipsPositionMetaData.shipsPositionMetaDeta = new List<ShipPositionMetaDeta>();

//        shipsPositionMetaData.shipsPositionMetaDeta = UIManager.GetInstance().myShipsPositionMetaDeta;

//        positions = JsonUtility.ToJson(shipsPositionMetaData);

//        UpdateMyShipPositionMetaData(positions);
//    }

//    [Command]
//    void UpdateMyShipPositionMetaData(string _positions)
//    {
//        myShipsPositionJson = _positions;
//    }

//    public void SetMyShipsPosition(string _, string newJson)
//    {
//    }

//    public List<ShipPositionMetaDeta> GetShipPositionsMetaDataJson()
//    {
//        shipsPositionMetaData = new ShipsPositionMetaData();
//        shipsPositionMetaData.shipsPositionMetaDeta = new List<ShipPositionMetaDeta>();

//        shipsPositionMetaData = JsonUtility.FromJson<ShipsPositionMetaData>(myShipsPositionJson);

//        return shipsPositionMetaData.shipsPositionMetaDeta;
//    }
//    #endregion

//    #region Power
//    void SetMyPower(int _power)
//    {
//        powerToBe = myPower + _power;
//        UpdateMyPower(powerToBe);
//    }

//    [Command]
//    void UpdateMyPower(int _updatedPower)
//    {
//        myPower = _updatedPower;
//    }

//    public void UpdateMyPowerOnNetwork(int _, int _updatedPower)
//    {
//        if (!isServer)
//        {
//            if (UIManager.GetInstance().isGamePlayUI)
//            {
//                if (isLocalPlayer)
//                {
//                    UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetUIForMyPower(_updatedPower);
//                }
//                else
//                {
//                    UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetUIForEnemyPower(_updatedPower);
//                }
//            }
//        }
//    }


//    #endregion

//    #region Trun  
//    void MakeMyTurn()
//    {
//        // for local player from the enemy clone
//        UpdateTrun(this.GetComponent<NetworkIdentity>());
//    }

//    //called on local player
//    public void ChangeTurn()
//    {
//        if (isLocalPlayer)
//        {
//            // if current player is local
//            if (this.gameObject.tag == UIManager.GetInstance().TurnPlayer.tag)
//            {
//                // make enemy turn.
//                UpdateTrun(GameObject.FindGameObjectWithTag("enemyPlayer").GetComponent<NetworkIdentity>());
//            }
//            else
//            {
//                // make my trun.
//                UpdateTrun(this.GetComponent<NetworkIdentity>());
//            }
//        }
//    }

//    [Command]
//    void UpdateTrun(NetworkIdentity _networkIdentity)
//    {
//        playerWithTrun = _networkIdentity;
//    }

//    public void ChangeTurnOnNetwork(NetworkIdentity _, NetworkIdentity newPlayerTurn)
//    {
//        if (!isServer)
//        {
//            if (isLocalPlayer)
//            {
//                UIManager.GetInstance().TurnPlayer = newPlayerTurn.gameObject;
//                UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().OnTrunChange();

//                // setting power on change turn.
//                if (UIManager.GetInstance().TurnPlayer.tag == this.gameObject.tag)
//                {
//                    SetMyPower(powerOnChangeTrun);
//                }
//            }
//            else
//            {
//                if (UIManager.GetInstance().TurnPlayer.tag != newPlayerTurn.gameObject.tag)
//                {
//                    GameObject.FindGameObjectWithTag("localPlayer").GetComponent<PlayerController>().MakeMyTurn();
//                }
//            }
//        }
//    }

//    #endregion

//    #region Health

//    public void TakeDamage(int _damage) 
//    {
//        heathToBe = myhealth - _damage;
//        SetMyHelth(heathToBe);
//    }

//    void SetMyHelth(int _health)
//    {
//      //  UIManager.GetInstance().testing.text = this.name;
//        UpdateMyHealth(_health);
//    }

//    [Command]
//    void UpdateMyHealth(int _health)
//    {
//        myhealth = _health;
//    }

//    public void UpdateMyHealthOnNetwork(int _oldHealth, int _newHeath)
//    {
//        if (!isServer)
//        {
//            if (UIManager.GetInstance().isGamePlayUI)
//            {
//                if (isLocalPlayer)
//                {
//                    UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetUIForMyHealth(_newHeath);
//                    SetMyPower((_oldHealth - _newHeath) * powerUpPerHit);
//                }
//                else
//                {
//                    UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetUIForEnemyHealth(_newHeath);
//                }
//            }
//        }
//    }

//    public int GetMyHealth()
//    {
//        return myhealth;
//    }
//    #endregion

//    #region My Grid Info
//    public void GridCells(List<GridCellMetaData> _gridCells)
//    {
//        myGridCellsMetaData = new GridCellsMetaData();
//        myGridCellsMetaData.GridCellMetaDeta = new List<GridCellMetaData>();

//        myGridCellsMetaData.GridCellMetaDeta = _gridCells;

//        myGridCells = JsonUtility.ToJson(myGridCellsMetaData);

//        SetupMyGridCellsMetaData(myGridCells);
//    }

//    [Command]
//    void SetupMyGridCellsMetaData(string _gridCells)
//    {
//         myGridMetaDataJson = _gridCells;
//    }

//    public void UpdateMyGridCells(string _, string newJson)
//    {
//        if (!isServer)
//        {
//            if (isLocalPlayer)
//            {
//                if (UIManager.GetInstance().isGamePlayUI)
//                {
//                    UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetupMyGrids();
//                }
//            }
//        }
//    }

//    public List<GridCellMetaData> GetGridCellsMetaData()
//    {
//        myGridCellsMetaDataToReturn = new GridCellsMetaData();
//        myGridCellsMetaDataToReturn.GridCellMetaDeta = new List<GridCellMetaData>();

//        myGridCellsMetaDataToReturn = JsonUtility.FromJson<GridCellsMetaData>(myGridMetaDataJson);
//        return myGridCellsMetaDataToReturn.GridCellMetaDeta;
//    }
//    #endregion

//    #region Enemy grid on attack
//    public void EnemyGridCells(List<GridCellMetaData> _gridCells)
//    {
//        enemyGridCellsMetaData = new GridCellsMetaData();
//        enemyGridCellsMetaData.GridCellMetaDeta = new List<GridCellMetaData>();

//        enemyGridCellsMetaData.GridCellMetaDeta = _gridCells;

//        enemyGridCells = JsonUtility.ToJson(enemyGridCellsMetaData);

//        SetupEnemyGridCellsMetaData(enemyGridCells);
//    }

//    [Command]
//    void SetupEnemyGridCellsMetaData(string _gridCells)
//    {
//         enemyGridMetaDataJson = _gridCells;
//    }

//    public void UpdateEnemyGridCells(string _, string newJson)
//    {
//        if (!isServer)
//        {
//            if (!isLocalPlayer)
//            {
//                enemyGridCellsMetaDataToReturn = new GridCellsMetaData();
//                enemyGridCellsMetaDataToReturn.GridCellMetaDeta = new List<GridCellMetaData>();
//                enemyGridCellsMetaDataToReturn = JsonUtility.FromJson<GridCellsMetaData>(newJson);
//                GameObject.FindGameObjectWithTag("localPlayer").GetComponent<PlayerController>().GridCells(enemyGridCellsMetaDataToReturn.GridCellMetaDeta);
//            }
//            else
//            {
//                UIManager.GetInstance().GetCurrentPanel().GetComponent<GamePlayUI>().SetupEnemyGrids();
//            }
//        }
//    }
//    #endregion
//}
