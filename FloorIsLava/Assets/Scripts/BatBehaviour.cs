using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour, IEnemyDamage
{

    public int damage { get; set; }
    void Start()
    {
        damage = 10;
    }

}
