using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StalactiteBehaviour : MonoBehaviour, IEnemyDamage
{
    public int damage { get; set; }
    private Rigidbody rigidBody;

    private void Start()
    {   
        rigidBody = gameObject.GetComponent<Rigidbody>();
        // These two lines initially disable RigidBody physics.
        rigidBody.isKinematic = true;
        rigidBody.detectCollisions = false;
        rigidBody.freezeRotation = true;
        // This sets the damage the stalactite deals to the player in a collision.
        // It is not very realistic because in real life the damage depends on the mass and speed at the time of collision.
        damage = 10;

    }


    public void OnTriggerEnter(Collider other)
    {
        GameObject triggerObject = other.gameObject;

        switch(triggerObject.tag)
        {
            case Tags.StalactiteGround: // to detect when stalactite is grounded.                
                // I can't use a collision with a "Platform" tagged object because, in this game, stalactites begin in contact with that kind of object.
                // In my case, the trigger for the stalactite to start falling is also the one that disables the damage once the stalactite hits it near the ground.
                //
                // I'm assuming the stalactite will fall in a straight line over the trigger.
                // This is true as long as there is no force or collision with a massive body to deflect it.
                //
                // Only detection/trigger/collision between childs of same parent (StalactiteSystem) and when stalactite has damage capability      
                if (triggerObject.transform.parent==gameObject.transform.parent && gameObject.tag==Tags.Damager)
                {
                    // This must be de first and last time that stalactite is "disarmed", so... 
                    triggerObject.tag = "Untagged"; // I erase "StalactiteGround" tag
                    triggerObject.GetComponent<MeshCollider>().enabled=false; // For more performance, triggering detection ends
                    Destroy(triggerObject); // why not? I don't need it anymore (and maybe the two previous lines are not necessary)

                    // But there should be a little delay before damage capability ceases (erasing "Damager" tag)
                    StartCoroutine(DisarmeStallactite()); 
                }
                break;

            case Tags.LavaFloor: // to detect when the stalactite falls in lava
                StartCoroutine(DistroyStallactite());
                break;
        }
    }

    // With this I intend to "disarm" the damage capacity of the stalactite (one second after the detection occurs).
    IEnumerator DisarmeStallactite()
    {   
        // Disables constrain
        rigidBody.freezeRotation = false;

        // Waits one second
        yield return new WaitForSeconds(1);
        
        // PlayerBehaviour.cs checks if tag is "Damager" in order to call PlayerTakeDmg method
        // So this line removes that tag, and then damage capability ceases
        transform.gameObject.tag = "Untagged";
        damage = 0; // Perhaps this is not so necessary and a bit redundant.
        
        // The first MeshCollider is present in the parent (initially disabled), a second meshcollider is present in the child (initially enabled).
        // The first one has the stalactite form, the second one is bigger than stalactite (it represents a "damage zone")
        // At this point, we must enable the first and disable the second ().
        MeshCollider[] meshColliderArray = GetComponentsInChildren<MeshCollider>();
        meshColliderArray[0].enabled = true;
        meshColliderArray[1].enabled = false;
        Destroy(meshColliderArray[1]); // why not?
        // Or...
        //foreach (MeshCollider collider in meshColliderArray)
        //    collider.enabled = !(collider.enabled);
    }

    IEnumerator DistroyStallactite()
    {   
        // Waits one second
        yield return new WaitForSeconds(1);
        // Distroy entire StalactiteSystem game object (which contains trigger and stalactite)
        Destroy(gameObject.transform.parent.gameObject);
    }

}

