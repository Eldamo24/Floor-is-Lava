using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeiserDamageBehaviour : MonoBehaviour, IEnemyDamage
{
    public int damage { get ; set ; }

    // Start is called before the first frame update
    void Start()
    {
        damage = 20;
    }

}
