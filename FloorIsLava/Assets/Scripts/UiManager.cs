using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject[] _uiElements;
    private  CursorManager _cursorManager;

    private void Start()
    {
        _uiElements = new GameObject[] { _uiGameOver, _uiGameplay, _uiPause, _uiLevelEnd };
        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);
        _uiGameplay.SetActive(true);
        _cursorManager = new CursorManager();
    }

    private void OnGameStatusChanged(GameStatus newStatus)
    {
        foreach (GameObject uiElement in _uiElements)
        {
            uiElement.SetActive(false);
        }

        switch (newStatus)
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
            case GameStatus.LevelEnded: 
                _uiLevelEnd.SetActive(true);
                _cursorManager.SetCursorVisible();
                break;
        }
    }

}

public class CursorManager
{
    public CursorManager()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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