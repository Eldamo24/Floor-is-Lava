using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuicideBehaviour : MonoBehaviour
{
     // Caching variables (more performance?)
    [SerializeField] HealthBar _healthBar;
    private Rigidbody playerRigidbody;
    private GameObject collisionedObject;
    private float playerMaxSpeed = 0;
    private bool groundInContact = false;
    const float minDamageSpeed = 6; // I have checked that it is the speed of a 2 meters free fall (in this game)
    const float letalSpeed = 10.2f; // I have checked that it is the speed of a 5.5 meters free fall (in this game)    
    // Theoretically these speeds are for 1.85 and 5.35 meters, but here the collider (around the player's feet) must be taking these 15 cm.

    // Start is called before the first frame update
    void Start()
    {   
        // The parent of "SuicideFeature" must be "Player"
        playerRigidbody =  transform.parent.GetComponent<Rigidbody>();
    }

    private void Update() 
    {
        if (!groundInContact)
            playerMaxSpeed = Mathf.Max(playerRigidbody.velocity.magnitude, playerMaxSpeed);
    }

    // A collider component should be attached to SuicideFeature (around player's feet)
    private void OnTriggerExit(Collider other)
    {
        collisionedObject = other.gameObject;
        if (collisionedObject.tag == Tags.Platform)
            groundInContact = false;
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        collisionedObject = other.gameObject;
        if (collisionedObject.tag == Tags.Platform)
        {
            groundInContact = true;
            if (playerMaxSpeed>minDamageSpeed)
            {
                GameManager.gameManager._playerHealth.DmgUnit(LevelOfDamage());
                _healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
                Debug.Log("DAÑO POR CAIDA: " + LevelOfDamage());
            }
            playerMaxSpeed = 0;
        }
    }

    private int LevelOfDamage()
    {
        if (playerMaxSpeed >= letalSpeed)
            return 100;
        else if (playerMaxSpeed>minDamageSpeed) // I take a simple proportion normalized to 100
            return Mathf.RoundToInt(100*(playerMaxSpeed-minDamageSpeed)/(letalSpeed-minDamageSpeed));
        else
            return 0;
    }
}
