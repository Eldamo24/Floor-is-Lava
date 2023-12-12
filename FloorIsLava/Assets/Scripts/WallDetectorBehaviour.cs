using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectorBehaviour : MonoBehaviour
{
    // Caching variables (more performance?)
    private GameObject collisionedObject;
    private Transform playerModel;
    public bool wallInContact = false;

    private void Start() 
    {
        // find child of same parent by name
        playerModel = transform.parent.Find("PlayerObj"); // The character 3d model 
    }

    private void FixedUpdate() {
        gameObject.transform.forward = playerModel.forward; // It takes same orientation that 3d model
    }

    // A collider component should be attached to WallDetector (around player's waist)
    private void OnTriggerExit(Collider other)
    {
        collisionedObject = other.gameObject;
        if (collisionedObject.tag == Tags.Platform)
            wallInContact = false;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        collisionedObject = other.gameObject;
        if (collisionedObject.tag == Tags.Platform)
            wallInContact = true;
    }
}
