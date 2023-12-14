using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFall : MonoBehaviour
{
    [SerializeField]
    private GameObject fallingBody;

    private Rigidbody fallingRigidBody;
        

    private void Start() 
    {
        fallingRigidBody = fallingBody.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(fallingRigidBody == null) // RigidBody component doesn't exist
            {   
                // This is the mass value used in OLD RockFall script (it could be changed)
                fallingRigidBody.mass = 300;
            }
            else // RigidBody component exists (but RigidBody physics should be initially disabled in XXXXXBehaviour.cs).
            {
                // Enable RigidBody physics 
                fallingRigidBody.isKinematic = false;
                fallingRigidBody.detectCollisions = true;
                if(!AudioManager.Instance._sfxSource.isPlaying ) //reproducir sonido de piedra cayendo si no esta en uso el source (ABS)
                {
                    AudioManager.Instance.PlaySFX("rockfall0"); 
                }
                // Shoot Stalactite with a inicial no-null speed
                fallingRigidBody.velocity=Random.Range(5,15)*Vector3.down;

            }
        }
    }


}

