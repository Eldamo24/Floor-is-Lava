using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entro alguien al trigger");
        if(other.gameObject.tag == "Player")
        {
            GameManager.gameManager.TriggerEndLevel();
        }
    }
}
