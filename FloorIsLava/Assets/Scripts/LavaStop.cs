using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaStop : MonoBehaviour
{
    private LavaFloorBehaviour lava;
    private float speed = 40f;

    private void Start()
    {
        lava = GameObject.Find("Floor").GetComponent<LavaFloorBehaviour>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));
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
