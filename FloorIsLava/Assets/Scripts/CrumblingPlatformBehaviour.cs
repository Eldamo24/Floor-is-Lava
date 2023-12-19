using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingPlatformBehaviour : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rigidBody;
    private bool neverTouched;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        neverTouched = true;
        rigidBody = gameObject.GetComponent<Rigidbody>();
        // Initially disable RigidBody physics.
        rigidBody.isKinematic = true;
    }

    private void Start()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && neverTouched)
        {
            neverTouched = false;
            AudioManager.Instance.PlaySFX("rockfall1");
            anim.SetBool("IsDestroyed", true);
            StartCoroutine(DelayedFall());            
        }
    }

    private IEnumerator DelayedFall()
    {        
        yield return new WaitForSeconds(1.95f);
        anim.SetBool("IsDestroyed", false);
        anim.enabled = false;
        rigidBody.isKinematic = false;
        Destroy(this.gameObject, 3);
    }
}