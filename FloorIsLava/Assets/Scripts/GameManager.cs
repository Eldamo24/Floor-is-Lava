using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager {  get; private set; }

    public UnitHealth _playerHealth = new UnitHealth(100, 100);

    public LavaFloorBehaviour _lavaFloor;

    public bool isGameOver;

    private void Awake()
    {
        if(gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }
    }

    void NewGame()
    {

    }
}
