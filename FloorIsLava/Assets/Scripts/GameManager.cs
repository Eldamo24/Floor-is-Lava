using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor.SceneManagement;

public class GameManager : MonoBehaviour
{

    public UnitHealth _playerHealth = new UnitHealth(100, 100);
    private PlayerBehaviour _player;
    [SerializeField]
    private UiManager _uiManager;
    [SerializeField]
    private LavaFloorBehaviour _lavaFloor;
    //field para saber en que estado está el juego
    [SerializeField]
    private GameStatus _currentGameStatus;
    public UnityEvent<GameStatus> OnGameStatusChanged;


    public static GameManager gameManager { get; private set; }

    public GameStatus CurrentGameStatus
    {
        get
        {
            return _currentGameStatus;
        }
        set
        {
            _currentGameStatus = value;
            OnGameStatusChanged.Invoke(_currentGameStatus);
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

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
            case "Level2":
                _player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
                CurrentGameStatus = GameStatus.Playing;
                break;
            case "MainMenu":
                CurrentGameStatus = GameStatus.OnMenu;
                break;
        }

        //if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2")
        //{
        //    _player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        //    CurrentGameStatus = GameStatus.Playing;
        //}

    }

    private void Start()
    {
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
        
    }



    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    switch(CurrentGameStatus)
        //    {
        //        case GameStatus.Paused:
        //            CurrentGameStatus = GameStatus.Playing;
        //            break;
        //        case GameStatus.Playing:
        //            CurrentGameStatus = GameStatus.Paused;
        //            break;
        //        case GameStatus.GameOver:
        //            NewGame();
        //            break;

        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CurrentGameStatus = GameStatus.GameOver;
        //}

        if (_playerHealth.Health <= 0 && CurrentGameStatus != GameStatus.GameOver)
        {

            CurrentGameStatus = GameStatus.GameOver;
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

public enum GameStatus
{
    Playing,
    Paused,
    GameOver,
    OnLevelEnd,
    OnMenu
}

