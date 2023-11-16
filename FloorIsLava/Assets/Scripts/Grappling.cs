using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappeable;
    public Transform gunTip, camera, player;
    private float maxDistance = 600f;
    private SpringJoint joint;
    private bool isGrappling = false;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        
        
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        Debug.Log("Intente Graplear");
        if(Physics.Raycast(gunTip.position, camera.forward, out hit, maxDistance, whatIsGrappeable))
        {
            Debug.Log("estoy grapleando con el stake");
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);

    }

    void StopGrapple()
    {
        Debug.Log("Deje de graplear");
        lr.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    public void Grap(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed) 
        {
            if (isGrappling)
            {
                isGrappling = false;
                StopGrapple();
            }
            else
            {
                isGrappling = true;
                StartGrapple();
            }
        }
            
    }
        

}
