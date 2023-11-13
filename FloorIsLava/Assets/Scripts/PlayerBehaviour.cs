using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HealthBar _healthBar;
    public GeiserDamageBehaviour geiserDamageBehaviour;
    public LittleRockBehaviour littleRockBehaviour;
    public bool IsDead
    {
        get
        {
            return GameManager.gameManager._playerHealth.Health < 1;
        }
    }

    // Update is called once per frame
    void Start()
    {
        geiserDamageBehaviour = FindObjectOfType<GeiserDamageBehaviour>();
        littleRockBehaviour = FindObjectOfType<LittleRockBehaviour>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            PlayerTakeDmg(20);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlayerHeal(10);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }

        
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Geiser") // Hay que cambiar el tag segun corresponda
        {
            PlayerTakeDmg(geiserDamageBehaviour.damage);
        }

        if (collision.transform.tag == "Rock")
        {
            PlayerTakeDmg(littleRockBehaviour.damage);
        }

        //if (collision.transform.tag == "Bat")  // Hay habilitarlo si corresponde
        //{
        //    PlayerTakeDmg(xxxxxxxxxxxxxxx.damage);
        //}

        //if (collision.transform.tag == "Life")  // Hay habilitarlo si corresponde
        //{
        //    PlayerHeal(xxxxxxxxxxxxxxx.life);
        //}
    }



    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entramos en colision");
        GameObject collisionedObject = other.gameObject;

        switch (collisionedObject.tag)
        {
            case Tags.LavaFloor:
                PlayerTakeDmg(collisionedObject.GetComponent<IEnemyDamage>().damage);
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                break;
        }
    }

    //public void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("Entramos en colision");
    //    GameObject collisionedObject = other.gameObject;

    //    switch (collisionedObject.tag)
    //    {
    //        case Tags.Rock:
    //        case Tags.LavaGeiser:
    //                PlayerTakeDmg(collisionedObject.GetComponent<IEnemyDamage>().damage);
    //                break;
    //    }
    //}
    private void FixedUpdate()
    {
        //si colisiono con un gameobject && tiene tag damageGiver

        //entonces obtener collision.damage
    }
    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
        _healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    private void PlayerHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        _healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);

    }
}

