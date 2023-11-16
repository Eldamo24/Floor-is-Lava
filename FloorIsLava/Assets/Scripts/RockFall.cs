using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    [SerializeField]
    private GameObject rock;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(rock.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = rock.AddComponent<Rigidbody>();
                rb.mass = 300;
            }
        }
    }
}
