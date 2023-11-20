using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class isGrounded : MonoBehaviour
{
    public bool isOnFloor;
    public UnityEvent<bool> OnFloorCollisionChanged;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            this.isOnFloor = true;
            OnFloorCollisionChanged.Invoke(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            this.isOnFloor = false;
            OnFloorCollisionChanged.Invoke(false);
        }

    }
}
