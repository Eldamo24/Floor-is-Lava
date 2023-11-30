using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class isGrounded : MonoBehaviour
{
    [SerializeField]
    public bool grounded;
    [SerializeField]
    private LayerMask layer;
    [SerializeField]
    private GameObject player;
    public UnityEvent<bool> OnFloorCollisionChanged;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Platform")
    //    {
    //        this.isOnFloor = true;
    //        OnFloorCollisionChanged.Invoke(true);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Platform")
    //    {
    //        this.isOnFloor = false;
    //        OnFloorCollisionChanged.Invoke(false);
    //    }

    //}

    public void LateUpdate()
    {
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(player.transform.position, Vector3.down * 0.2f, Color.green);
        if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 0.2f, layer))
        {
            if (!grounded)
            {
                grounded =! grounded;
                OnFloorCollisionChanged.Invoke(true);
            }

        }else
        {
            if (grounded)
            {
                grounded = !grounded;
                OnFloorCollisionChanged.Invoke(false);
            }
        }
    }

}
