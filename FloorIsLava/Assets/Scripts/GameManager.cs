using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public UnitHealth _playerHealth = new UnitHealth(100, 100);
    [SerializeField]
    private LavaFloorBehaviour _lavaFloor;
    [SerializeField]
    private GameStatus _currentGameStatus;     //field que cambia segun el esstado del juego
    public UnityEvent<GameStatus> OnGameStatusChanged;
    private float _defaultTimeScale = 1f;

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

            switch (value)
            {
                case GameStatus.Paused:
                    TimeScale = 0f; break;
                case GameStatus.Playing:
                    TimeScale = _defaultTimeScale; break;
            }
        }
    }

    public float TimeScale
    {
        get { return Time.timeScale; }
        set { Time.timeScale = value; }
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

    public void LoadScene(string str)
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
                AudioManager.Instance.PlaySFX("pause");
                break;
        }
    }

    public void SaveGame()
    {
        Transform playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        Transform lava = GameObject.Find("Floor").GetComponent<Transform>();
        PlayerPrefs.SetInt("health", _playerHealth.Health);
        PlayerPrefs.SetFloat("posX", playerPosition.position.x);
        PlayerPrefs.SetFloat("posY", playerPosition.position.y);
        PlayerPrefs.SetFloat("posZ", playerPosition.position.z);
        PlayerPrefs.SetFloat("posLavaX", lava.position.x);
        PlayerPrefs.SetFloat("posLavaY", lava.position.y);
        PlayerPrefs.SetFloat("posLavaZ", lava.position.z);


    }

    public void LoadGame()
    {
        Transform playerPosition = GameObject.Find("Player").GetComponent<Transform>();
        Transform lava = GameObject.Find("Floor").GetComponent<Transform>();
        playerPosition.position = new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
        lava.position = new Vector3(PlayerPrefs.GetFloat("posLavaX"), PlayerPrefs.GetFloat("posLavaY"), PlayerPrefs.GetFloat("posLavaZ"));
        _playerHealth.Health = PlayerPrefs.GetInt("health");
    }

    public void TriggerEndLevel()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "level1":
                LoadScene("level2");
                break;
            case "level2":
                LoadScene("MainMenu");
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
    public const string StalactiteGround = "StalactiteGround";
    public const string Damager = "Damager";
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