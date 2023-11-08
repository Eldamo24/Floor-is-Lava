using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserCreation : MonoBehaviour
{

    private GameObject geiser;
    private float time = 5f;
    void Start()
    {
        geiser = transform.GetChild(0).gameObject;
        InvokeRepeating("changeGeiserState", 0f, time);
    }

    private void changeGeiserState()
    {
        geiser.SetActive(!geiser.activeInHierarchy);
    }

}
