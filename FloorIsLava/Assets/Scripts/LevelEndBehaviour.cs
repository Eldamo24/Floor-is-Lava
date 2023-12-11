using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            GameManager.gameManager.TriggerEndLevel();
        }
    }
}
