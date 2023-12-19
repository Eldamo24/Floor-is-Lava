using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTriggerBehaviour : MonoBehaviour
{
    private bool triggered = false; // It assumes initially is false
    private BoxCollider thisObjectBoxCollider;

    private void Start() {
        thisObjectBoxCollider=gameObject.GetComponent<BoxCollider>();
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            triggered = true;
            thisObjectBoxCollider.enabled = false; // It disables triggering
        }
            
    }

    public bool Triggered()
    {
        return triggered;
    }

}
