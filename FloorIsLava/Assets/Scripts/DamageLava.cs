using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLava : MonoBehaviour
{
    public int damageLava;
    public GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.GetComponent<LifeBar>().Health -= damageLava;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Lava")
    //    {
    //        Player.GetComponent<LifeBar>().Health -= damageLava;
    //    }
    //}



    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
