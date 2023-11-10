using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public UnitHealth _playerHealth = new UnitHealth(100, 100);
    private PlayerBehaviour _player;
    [SerializeField]
    private UiManager _uiManager;
    [SerializeField]
    private LavaFloorBehaviour _lavaFloor;
    [SerializeField]
    private bool _isGameOver = false;
    [SerializeField]
    private bool _isGamePaused = false;
    [SerializeField]
    private bool _isLevelEnded = false;



    public static GameManager gameManager { get; private set; }

    public bool IsGameOver { 
        get 
        { 
            return _isGameOver; 
        } 
        private set 
        {
            _isGameOver = value;
            if (value)
            {
                _uiManager.SetActive(UiObject.UiGameOver);
                _lavaFloor.VelocityMultiplier = 10;

            }
        } 
    }
    public bool IsGamePaused
    {
        get
        {
            return _isGamePaused;
        }
        private set
        {
            if(!IsGameOver && !IsLevelEnded)
            {
                _isGamePaused = value;
                if (value)
                {
                    _uiManager.SetActive(UiObject.UiPause);
                }
                else
                {
                    _uiManager.SetActive(UiObject.UiGameplay);
                }
            }
        }
    }

    public bool IsLevelEnded
    {
        get
        {
            return _isLevelEnded;
        }
        private set
        {
            _isLevelEnded = value;
        }
    }

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

        _player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }

    void NewGame()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGamePaused = (!IsGamePaused);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsGameOver = (!IsGameOver);
        }

        if (_playerHealth.Health <= 0)
        {
            IsGameOver = true;
        }
    }
}

public static class Tags
{
    public const string LavaFloor = "LavaFloor";
    public const string Rock = "Rock";
    public const string Bat = "Bat";
    public const string LavaGeiser = "LavaGeiser";
    public const string Platform = "Platform";
    public const string Stake = "Stake";
}