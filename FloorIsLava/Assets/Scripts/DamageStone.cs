using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStone : MonoBehaviour
{
    public int damage;
    public GameObject Player;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Player.GetComponent<LifeBar>().Health -= damage;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.GetComponent <LifeBar>().Health -= damage;
        }
         
    }



    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
