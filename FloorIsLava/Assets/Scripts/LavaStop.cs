using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaStop : MonoBehaviour
{
    private LavaFloorBehaviour lava;

    private void Start()
    {
        lava = GameObject.Find("Floor").GetComponent<LavaFloorBehaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            lava.SetFrozenVelocity(true);
            Destroy(gameObject);
        }
    }
}
