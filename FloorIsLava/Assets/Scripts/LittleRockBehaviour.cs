using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleRockBehaviour : MonoBehaviour, IEnemyDamage
{
    public int damage { get; set; }

    private void Start()
    {
        damage = 10;
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
