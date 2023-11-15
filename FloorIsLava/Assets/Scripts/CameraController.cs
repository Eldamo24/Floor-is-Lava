using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook _cineFL;
    private GameObject _spectatorPov;
    private GameObject _spectatorTarget;
    private int _cameraFov;
    public GameObject CurrentTransformCamFollow
    {
        get
        {
            return _cineFL.Follow.gameObject;
        }
        private set
        {
            _cineFL.Follow = value.transform;
        }
    }
    public GameObject CurrentTramsformCamLookAt
    {
        get
        {
            return _cineFL.LookAt.gameObject;
        }
        private set
        {
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

        _spectatorPov = GameObject.Find("SpectatorPov");
        _spectatorTarget = GameObject.Find("SpectatorTarget");

        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);

    }


    private void OnGameStatusChanged(GameStatus newStatus)
    {
        switch (newStatus)
        {
            case GameStatus.GameOver:
                StartCoroutine("SetGameOverCamera");
                break;
        }
    }

    IEnumerator SetGameOverCamera()
    {
        GameObject activeCamera = GameObject.Find(
                                GetComponent<CinemachineBrain>()
                                .ActiveVirtualCamera.Name);
        _cineFL = activeCamera.GetComponent<CinemachineFreeLook>();

        CurrentTransformCamFollow = _spectatorPov;
        CurrentTramsformCamLookAt = _spectatorTarget;
        CameraFov = 110;
        //detengo movimiento libre para no poder seguir moviendo la cámara desp de morir
        yield return new WaitForSecondsRealtime(.5f);
        _cineFL.enabled = false;
    }
}
