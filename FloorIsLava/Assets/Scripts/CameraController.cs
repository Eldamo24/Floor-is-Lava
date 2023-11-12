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
    private GameObject _player;
    private GameObject _spectatorPov;
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
        CinemachineBrain cinemachineBrain = GetComponent<CinemachineBrain>();
        _activeCamera = GameObject.Find(cinemachineBrain.ActiveVirtualCamera.Name);
        _cineFL = _activeCamera.GetComponent<CinemachineFreeLook>();
        _player = GameObject.Find("Player");
        _spectatorPov = GameObject.Find("SpectatorPov");

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.IsGameOver)
        {
            CurrentCameraFollow = _spectatorPov;
            CameraFov = 100;
        }

    }


}
