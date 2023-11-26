using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteBehaviour : MonoBehaviour, IEnemyDamage
{
    public int damage { get; set; }

    private void Start()
    {   
        // These two lines initially disable RigidBody physics.
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        // This sets the damage the stalactite deals to the player in a collision.
        // It is not very realistic because in real life the damage depends on the mass and speed at the time of collision.
        damage = 10;

    }

    // This exists only in order to detect when stalactite is grounded.
    // I can't use a collision with a "Platform" tagged object because, in this game, stalactites begin in contact with that kind of object.
    // In my case, the trigger for the stalactite to start falling is also the one that disables the damage once the stalactite hits it near the ground.
    public void OnTriggerEnter(Collider other)
    {
        GameObject triggerObject = other.gameObject;
        // Only detection/trigger/collision between childs of same parent (StalactiteSystem) and with these tags        
        if (triggerObject.transform.parent==gameObject.transform.parent && triggerObject.tag==Tags.StalactiteGround && gameObject.tag==Tags.Rock)
        {
            // This must be de first and last time that stalactite is "disarmed", so I erase "StalactiteGround" tag
            triggerObject.tag = "Untagged";
            // But there should be a little delay before damage capability ceases (erasing "Rock" tag)
            StartCoroutine(DisarmeStallactite()); 
        }
    }

    // With this I intend to "disarm" the damage capacity of the stalactite (one second after the detection occurs).
    IEnumerator DisarmeStallactite()
    {   
        // Waits one second
        yield return new WaitForSeconds(1);
        
        // PlayerBehaviour.cs checks if tag is "Rock" in order to call PlayerTakeDmg method
        // So this line removes that tag, and then damage capability ceases
        transform.gameObject.tag = "Untagged";
        damage = 0; // Perhaps this is not so necessary and a bit redundant.
        
        // The first MeshCollider is present in the parent (initially disabled), a second meshcollider is present in the child (initially enabled).
        // The first one has the stalactite form, the second one is bigger than stalactite (it represents a "damage zone")
        // At this point, we must enable the first and disable the second ().
        MeshCollider[] meshColliderArray = GetComponentsInChildren<MeshCollider>();
        meshColliderArray[0].enabled = true;
        meshColliderArray[1].enabled = false;
        // Or...
        //foreach (MeshCollider collider in meshColliderArray)
        //    collider.enabled = !(collider.enabled);
    }

}

