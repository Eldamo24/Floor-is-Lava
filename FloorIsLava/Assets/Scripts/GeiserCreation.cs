using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserCreation : MonoBehaviour
{

    private ParticleSystem geiser;
    private Collider box;
    void Start()
    {
        geiser = GetComponent<ParticleSystem>();
        box = GetComponent<Collider>();
        //le saco un time fijo y le hago uno dinámico para que no sean todos los geisers clones
        //que se activan y desactivan al mismo tiempo
        InvokeRepeating("changeGeiserState", 0f, UnityEngine.Random.Range(3, 5));
        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);



    }

    private void OnGameStatusChanged(GameStatus newStatus)
    {
        switch (newStatus)
        {
            case GameStatus.Paused: geiser.Pause(); break;
            case GameStatus.Playing: geiser.Play(); break;
        }
    }



    private void changeGeiserState()
    {
        if(GameManager.gameManager.CurrentGameStatus == GameStatus.Playing)
        {
            if (geiser.isEmitting)
            {
                geiser.Stop();
                box.enabled = false;
            }
            else
            {
                geiser.Play();
                box.enabled = true;
            }
        }
    }

}
