using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetectorBehaviour : MonoBehaviour
{
    // Caching variables (more performance?)
    private GameObject collisionedObject;
    // A SER BORRADO private Transform playerModel;
    private Transform player;
    public bool wallInContact = false;

    private void Start() 
    {
        // A SER BORRADO find child of same parent by name
        // A SER BORRADO playerModel = transform.parent.Find("PlayerObj"); // The character 3d model 
        player = transform.parent; // WallDetector must be child of Player
    }

    private void FixedUpdate() {
        //A SER BORRADO gameObject.transform.forward = playerModel.forward; // It takes same orientation that 3d model
        transform.forward = player.forward;
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
