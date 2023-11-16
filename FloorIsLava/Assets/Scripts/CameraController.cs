using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        StartCoroutine(FindCinemachineFreeLook());

    }


    private void OnGameStatusChanged(GameStatus newStatus)
    {
        switch (newStatus)
        {
            case GameStatus.GameOver:
                StartCoroutine("SetGameOverCamera");
                break;
            case GameStatus.Paused:
                LockCamera(true);
                break;
            case GameStatus.Playing:
                LockCamera(false); 
                break;
        }
    }

    IEnumerator SetGameOverCamera()
    {


        CurrentTransformCamFollow = _spectatorPov;
        CurrentTramsformCamLookAt = _spectatorTarget;
        CameraFov = 110;
        //detengo movimiento libre para no poder seguir moviendo la cámara desp de morir
        yield return new WaitForSecondsRealtime(.5f);
        LockCamera(true);
    }

    IEnumerator FindCinemachineFreeLook()
    {
        yield return new WaitForSecondsRealtime(.2f);
        GameObject activeCamera = GameObject.Find(
                        GetComponent<CinemachineBrain>()
                        .ActiveVirtualCamera.Name);
        _cineFL = activeCamera.GetComponent<CinemachineFreeLook>();
    }
    private void LockCamera(bool status)
    {
        try
        {
            _cineFL.enabled = !status;
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}
