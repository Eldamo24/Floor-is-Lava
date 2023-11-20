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
    [SerializeField]
    private LavaFloorBehaviour _lavaFloor;

    [SerializeField]
    private GameStatus _currentGameStatus;     //field que cambia segun el esstado del juego
    public UnityEvent<GameStatus> OnGameStatusChanged;

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

    public static GameManager gameManager { get; private set; } //para hacer singleton al GameManager
    private void Awake()
    {
        if (gameManager != null && gameManager != this)
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
                CurrentGameStatus = GameStatus.Playing;
                break;
            case "MainMenu":
                CurrentGameStatus = GameStatus.OnMenu;
                break;
        }

    }

    public void Update()
    {

        if (_playerHealth.Health <= 0 && CurrentGameStatus != GameStatus.GameOver)
        {

            CurrentGameStatus = GameStatus.GameOver;
        }
    }

    public void NewGame(string str)
    {
        SceneManager.LoadScene(str);
    }
    public void RestartScene()
    {
        try
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }

    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(GameScenes.MainMenu.ToString());
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Pause()
    {
        switch (gameManager.CurrentGameStatus)
        {
            case GameStatus.Paused:
                gameManager.CurrentGameStatus = GameStatus.Playing;
                break;
            case GameStatus.Playing:
                gameManager.CurrentGameStatus = GameStatus.Paused;
                break;
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
    public const string Healer = "Healer";
}

public enum GameStatus
{
    Playing,
    Paused,
    GameOver,
    OnLevelEnd,
    OnMenu
}

public enum GameScenes
{
    MainMenu,
    Level1,
    Level2,
}