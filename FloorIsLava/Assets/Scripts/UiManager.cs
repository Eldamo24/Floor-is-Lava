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


    void Update()
    {
    }

    public void SetActive(UiObject uiObject)
    {
        SetAllInactive();
        switch (uiObject)
        {
            case UiObject.UiGameOver:
                _uiGameOver.SetActive(true);
                break;
            case UiObject.UiGameplay:
                _uiGameplay.SetActive(true);
                break;
            case UiObject.UiPause:
                _uiPause.SetActive(true);
                break;
            case UiObject.UiLevelEnd:
                _uiLevelEnd.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid UiObject: " + uiObject);
                break;
        }
    }


    public void SetAllInactive()
    {
        _uiGameOver.SetActive(false);
        _uiGameplay.SetActive(false);
        _uiPause.SetActive(false);
        _uiLevelEnd.SetActive(false);
    }
}
public enum UiObject
{
    UiGameOver,
    UiGameplay,
    UiPause,
    UiLevelEnd
}