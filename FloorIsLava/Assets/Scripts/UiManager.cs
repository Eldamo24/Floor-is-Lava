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

    private void Start()
    {
        _uiElements = new GameObject[] { _uiGameOver, _uiGameplay, _uiPause, _uiLevelEnd };
        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);
        _uiGameplay.SetActive(true);
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
                break;
            case GameStatus.Paused:
                _uiPause.SetActive(true);
                break;
            case GameStatus.GameOver: 
                _uiGameOver.SetActive(true);
                break;
            case GameStatus.LevelEnded: 
                _uiLevelEnd.SetActive(true); 
                break;
        }
    }

}
