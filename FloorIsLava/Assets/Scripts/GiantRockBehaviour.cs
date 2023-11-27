using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRockBehaviour : MonoBehaviour, IEnemyDamage
{
    public int damage { get ; set ; }

    // Start is called before the first frame update
    void Start()
    {
        damage = 20;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            gameObject.tag = "Untagged";
            Destroy(gameObject, 5);
        }
    }
}
