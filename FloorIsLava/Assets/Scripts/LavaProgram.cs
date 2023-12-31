// Note by Leo:
//
// There has been no way to establish a unified criteria about how the game should be.
// Consequently, some code contributions were developed considering only the artistic vision of level 1.
// Since there is a general fear against to modify code that works for level 1, 
// LavaFloorBehaviour_DIRTY.cs is a quick way to reuse LavaFloorBehaviour.cs without modifying it.
// I know it's not the best programming practice, it's what I can do given the circumstances.
// I know it's a very dirty bypass, I know it breaks the encapsulation, I know...
// My other two options would be to submit to the vision of the other contributors, 
// or to redo the code from scratch taking responsibility for making it work at both levels
// (without support from anyone and, now, with nothing of time).
//
// LavaFloorBehaviour.cs has the UpwardsSpeed serialized field
// (it is only read in the class construction and in the awake callback)
// Actually that script considers a speed called "VelocityMultiplier"
// "VelocityMultiplier" can take 3 possible values: Normal=0.15*UpwardsSpeed, Fast=1.4*UpwardsSpeed, GameOverFast=15*UpwardsSpeed
// (I think this is very confusing, because UpwardsSpeed is not the actual speed)
// LavaFloorBehaviour_DIRTY.cs overwrites "Normal" speed with "publicSpeed" (if publicSpeedOn=true)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Needed in order to use .ToList() method

public class LavaProgram : MonoBehaviour
{
    [SerializeField]
    private GameObject lava;
    [SerializeField]
    private LavaFloorBehaviour_DIRTY lavaClass; // A script component of lava

    private float initialLavaHeight;

    // Each child of this script must have a LavaTriggerBehaviour.cs
    // Each child of this script must have a name equal to an positive integer written with numeric characters.
    // These numbers must have an order relationship associated with the order in which the player is supposed to find them.
    private Dictionary<string,LavaTriggerBehaviour> childsDictionary = new Dictionary<string,LavaTriggerBehaviour>();

    private int maxTrigger;
    private int currentTrigger;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childTransform in transform)
        {
            childsDictionary.Add(childTransform.name,childTransform.GetComponent<LavaTriggerBehaviour>());
        }
        initialLavaHeight = lava.transform.position.y; 
        lavaClass.publicSpeedOn = true;

        maxTrigger=0;
        currentTrigger=0;        
        StartCoroutine(Trigger00());
    }

    private void FixedUpdate() 
    {
        lava.transform.Rotate(new Vector3(0f, 7.2f * lavaClass.publicSpeed / 0.01f, 0f) * Time.deltaTime);   
    }
    private void OnTriggerEnter(Collider other) // In order to detect child's triggers, Parent GameObject (this one) need to have a rigidbody (could be kinematic)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach(KeyValuePair<string,LavaTriggerBehaviour> child in childsDictionary.ToList()) // without ToList conversion/copy it can't remove dictionary entries inside foreach loop
            {
                if (child.Value.Triggered())
                {
                    maxTrigger=Mathf.Max(maxTrigger,int.Parse(child.Key)); // It's assumed order relationship associated with the order in which the player is supposed to find them
                    childsDictionary.Remove(child.Key); // Once triggers occurs it deletes dictionary entry
                }
            }

            switch (maxTrigger)
            {
                case 1:
                    StartCoroutine(SpeedUpLavaFixedHeight(0.1025f, 0.02f, 6));
                    break;
                case 2:
                    StartCoroutine(SpeedUpLavaFixedHeight(0.15f, 0.01f, 7.5f));
                    break;
                case 3:
                    StartCoroutine(SpeedUpLavaFixedHeight(0.37f, 0.00001f, 27f));
                    break;                   
                default:
                    break;
            }
            
        }
    }

      private IEnumerator Trigger00()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(SpeedUpLavaFixedHeight(0.1f, 0.01f, 0.75f));
    }
    private IEnumerator SpeedUpLavaFixedHeight(float newSpeed, float endSpeed, float maxLavaHeight)
    {
        currentTrigger = maxTrigger;
        lavaClass.publicSpeed = newSpeed;        
        while (lava.transform.position.y<initialLavaHeight+maxLavaHeight) // maxLavaHeight is referred to initialLavaHeight
        {
           yield return new WaitForSeconds(0.1f); 
        }
        // The "IF" is just as a precaution in case another trigger shoot another speed change
        if (currentTrigger == maxTrigger) lavaClass.publicSpeed = endSpeed; 
    }

}