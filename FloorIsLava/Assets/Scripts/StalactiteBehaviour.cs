using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteBehaviour : MonoBehaviour, IEnemyDamage
{
    public int damage { get; set; }

    private void Start()
    {
        damage = 25;
    }

}
