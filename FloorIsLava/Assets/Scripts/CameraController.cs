using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraController : MonoBehaviour
{
    private GameObject _activeCamera;
    private CinemachineFreeLook _cineFL;
    private GameObject _currentCameraFollow;
    private GameObject _currentCameraLookAt;
    private GameObject _player;
    private GameObject _spectatorPov;
    private GameObject _spectatorTarget;
    private int _cameraFov;
    public GameObject CurrentCameraFollow
    {
        get
        {
            return _currentCameraFollow;
        }
        private set
        {
            _currentCameraFollow = value;
            _cineFL.Follow = value.transform;
        }
    }
    public GameObject CurrentCameraLookAt
    {
        get
        {
            return _currentCameraLookAt;
        }
        private set
        {
            _currentCameraLookAt = value;
            _cineFL.LookAt = value.transform;
        }
    }
    public int CameraFov
    {
        get
        {
            return _cameraFov;
        }
        private set
        {
            _cameraFov = value;
            _cineFL.m_Lens.FieldOfView = value;
        }
    }


    void Start()
    {

        _player = GameObject.Find("Player");
        _spectatorPov = GameObject.Find("SpectatorPov");
        _spectatorTarget = GameObject.Find("SpectatorTarget");
        _activeCamera = GameObject.Find(
                                        GetComponent<CinemachineBrain>()
                                        .ActiveVirtualCamera.Name);
        _cineFL = _activeCamera.GetComponent<CinemachineFreeLook>();

        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);

    }


    private void OnGameStatusChanged(GameStatus newStatus)
    {
        switch (newStatus)
        {
            case GameStatus.GameOver:
                CurrentCameraFollow = _spectatorPov;
                CurrentCameraLookAt = _spectatorTarget;
                CameraFov = 100;
                break;
        }
    }


}
