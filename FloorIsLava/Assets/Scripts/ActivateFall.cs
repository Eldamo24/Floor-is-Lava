using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFall : MonoBehaviour
{
    [SerializeField]
    private GameObject fallingBody;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(fallingBody.GetComponent<Rigidbody>() == null) // RigidBody component doesn't exist
            {   
                // This is the mass value used in OLD RockFall script (it could be changed)
                fallingBody.AddComponent<Rigidbody>().mass = 300; 
            }
            else // RigidBody component exists (but RigidBody physics should be initially disabled in XXXXXBehaviour.cs).
            {
                // Enable RigidBody physics 
                fallingBody.GetComponent<Rigidbody>().isKinematic = false;
                fallingBody.GetComponent<Rigidbody>().detectCollisions = true;
            }
        }
    }


}

