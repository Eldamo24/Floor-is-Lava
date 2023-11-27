using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    [SerializeField]
    public float speed = -2f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * new Vector3(0, 1, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BatTrigger")
        {
            speed *= -1;
        }
    }


}
