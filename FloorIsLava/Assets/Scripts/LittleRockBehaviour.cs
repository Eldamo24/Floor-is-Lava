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
}
