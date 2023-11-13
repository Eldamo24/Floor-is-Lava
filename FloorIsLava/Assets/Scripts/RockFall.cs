using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField]
    private GameObject rock;

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.tag == "Player")
            {
                Rigidbody rb = rock.AddComponent<Rigidbody>();
                rb.mass = 300;
            }
        }
        catch (ArgumentException e)
        {
            Debug.Log($"Processing failed: {e.Message}");
        }
    }
}
