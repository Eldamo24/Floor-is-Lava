using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiGameOver;
    [SerializeField]
    private GameObject _uiGameplay;
    [SerializeField]
    private GameObject _uiPause;
    [SerializeField]
    private GameObject _uiLevelEnd;
    [SerializeField]
    private GameObject _uiMainMenu;
    private GameObject[] _uiElements;
    private  CursorManager _cursorManager;


    private void Start()
    {
        _uiElements = new GameObject[] { _uiGameOver, _uiGameplay, _uiPause, _uiLevelEnd, _uiMainMenu };
        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);
        _cursorManager = new CursorManager();

        //cuando inicio una nueva escena arranca Start así que evalúo si es level1 o 2 
        //en caso de serlo, se activa la ui de playing
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
            case "Level2":
                SetUIBasedOnGameStatus(GameStatus.Playing); 
                break;
        }
    }




    private void SetUIBasedOnGameStatus(GameStatus status)
    {
        foreach (GameObject uiElement in _uiElements)
        {
            uiElement.SetActive(false);
        }

        switch (status)
        {
            case GameStatus.Playing:
                _uiGameplay.SetActive(true);
                _cursorManager.SetCursorInvisible();
                break;
            case GameStatus.Paused:
                _uiPause.SetActive(true);
                _cursorManager.SetCursorVisible();
                break;
            case GameStatus.GameOver:
                _uiGameOver.SetActive(true);
                _cursorManager.SetCursorVisible();
                break;
            case GameStatus.OnLevelEnd:
                _uiLevelEnd.SetActive(true);
                _cursorManager.SetCursorVisible();
                break;
            case GameStatus.OnMenu:
                _uiMainMenu.SetActive(true);
                _cursorManager.SetCursorVisible();
                break;
        }
    }

    private void OnGameStatusChanged(GameStatus newStatus)
    {
        SetUIBasedOnGameStatus(newStatus);
    }

}

public class CursorManager
{
    public CursorManager()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetCursorVisible()
    {

        if (!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void SetCursorInvisible()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}