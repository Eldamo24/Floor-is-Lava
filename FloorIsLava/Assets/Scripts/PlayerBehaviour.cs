using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Grappling _grapplingScript;
    private isGrounded _isGrounded;
    private Rigidbody _rb;
    [SerializeField] HealthBar _healthBar;
    public bool IsDead
    {
        get
        {
            return GameManager.gameManager._playerHealth.Health < 1;
        }
    }

    private void Start()
    {

        _grapplingScript = Component.FindAnyObjectByType<Grappling>();
        _isGrounded = Component.FindAnyObjectByType<isGrounded>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.LeftAlt))
        //{
           
        //    _rb.AddExplosionForce(1000, Vector3.down, 10);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //{
        //    _grapplingScript.OnEndingGrappleAction(_rb);
        //}


    }


    public void OnTriggerEnter(Collider other)
    {
        GameObject collisionedObject = other.gameObject;

        switch (collisionedObject.tag)
        {
            case Tags.LavaFloor:
                PlayerTakeDmg(collisionedObject.GetComponent<IEnemyDamage>().damage);
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                break;
            case Tags.Damager:
                PlayerTakeDmg(collisionedObject.GetComponent<IEnemyDamage>().damage);
                break;
            case Tags.Healer:
                PlayerHeal(collisionedObject.GetComponent<HealthGiver>().health); 
                break;
            case Tags.Stake:
                if (_grapplingScript.isGrappling && !_isGrounded.grounded)
                {
                    //Debug.Log("tgrEnter de platform atascado");
                    //_rb.AddForce(Vector3.up * 300, ForceMode.VelocityChange);
                    _rb.AddExplosionForce(100, Vector3.up, 10);
                }
                break;

        }
    }

    public void OnTriggerStay(Collider other)
    {
        GameObject collisionedObject = other.gameObject;

        switch (collisionedObject.tag)
        {
            case Tags.Stake:
                if (_grapplingScript.isGrappling && !_isGrounded.grounded)
                {
                    //Debug.Log("tgrStay de platform atascado");

                    _grapplingScript.OnEndingGrappleAction(_rb);
                }
                break;

        }
    }

    public void OnCollisionEnter(Collision other)
    {
        GameObject collisionedObject = other.gameObject;

        switch (collisionedObject.tag)
        {
            case Tags.Damager:
                PlayerTakeDmg(collisionedObject.GetComponent<IEnemyDamage>().damage);
                break;
        }
    }
    private void FixedUpdate()
    {
        //si colisiono con un gameobject && tiene tag damageGiver

        //entonces obtener collision.damage
    }
    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
        AudioManager.Instance.PlaySFX("damage");
    }

    private void PlayerHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        _healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);

    }
}

