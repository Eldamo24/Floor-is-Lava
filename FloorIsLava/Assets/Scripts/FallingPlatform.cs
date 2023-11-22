using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Aqui iria el comienzo de la animacion de destruccion
            //Para luego destruir la plataforma
            anim.SetBool("IsDestroyed", true);
            Destroy(this.gameObject, 5);
        }
    }
}
