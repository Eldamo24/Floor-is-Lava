using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserCreation : MonoBehaviour
{

    private ParticleSystem geiser;
    private Collider box;
    private float time = 5f;
    void Start()
    {
        geiser = GetComponent<ParticleSystem>();
        box = GetComponent<Collider>();
        InvokeRepeating("changeGeiserState", 0f, time);
    }

    private void changeGeiserState()
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
