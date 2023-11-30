using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class Grappling : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappeable;
    public Transform gunTip, camera, player;
    private float maxDistance = 3000f;
    private SpringJoint joint;
    public bool isGrappling = false;
    private float speed = 5f;
    [SerializeField]
    private Rigidbody body;

    private float Spring
    {
        set
        {
            if(joint != null)
            {
                joint.spring = value;
            }
        }
    }

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawLine(ray.origin, ray.direction);
    }

    private void FixedUpdate()
    {
        if (isGrappling)
        {
            float step = speed * Time.deltaTime;
            GameObject.Find("Player").GetComponent<Transform>().position = Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().position, grapplePoint, step);
        }
        
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        Debug.Log("Intente Graplear");
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        //if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, whatIsGrappeable))
        //{
        //    isGrappling = true;
        //    Debug.Log("estoy grapleando con el stake");
        //    grapplePoint = hit.point;
        //    joint = player.gameObject.AddComponent<SpringJoint>();
        //    joint.autoConfigureConnectedAnchor = false;
        //    joint.connectedAnchor = grapplePoint;

        //    float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
        //    joint.maxDistance = distanceFromPoint * 0.8f;
        //    joint.minDistance = distanceFromPoint * 0.25f;

        //    joint.spring = 4.5f;
        //    joint.damper = 7f;
        //    joint.massScale = 4.5f;

        //    lr.positionCount = 2;
        //    body.AddForce(new Vector3(0f, 10f, 10f), ForceMode.Impulse);
        //}
        RaycastHit hit;
        if (Physics.Raycast(gunTip.position, camera.forward * maxDistance, out hit, maxDistance, whatIsGrappeable))
        {
            GameObject hitObject = hit.collider.gameObject;
            GameObject jointImpact = hitObject.transform.GetChild(0).gameObject;

            isGrappling = true;
            Debug.Log("estoy grapleando con el stake");
            grapplePoint = jointImpact.transform.position;
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
          //  
            
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

    public Vector3 GetGrapplePoint
    {
        get { return grapplePoint; }
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
                
                StartGrapple();
            }
        }
            
    }
        
    
    public void OnEndingGrappleAction(Rigidbody rb)
    {
        //impido que el joint me entorpezca cualquier intento de movimiento siguiente
        Spring = 0f;

        try
        {
            //le doy un impulso hacia arriba

            rb.AddForce(Vector3.up * 10, ForceMode.Force);
            //rb.AddRelativeForce(new Vector3(GetGrapplePoint.x, GetGrapplePoint.y + 2, GetGrapplePoint.z), ForceMode.Force);
            //rb.AddRelativeForce(new Vector3(GetGrapplePoint.x, GetGrapplePoint.y+6, GetGrapplePoint.z), ForceMode.Acceleration)
            //rb.AddExplosionForce(10, Vector3.down, 10);
            Debug.Log("AddForce desde OnEndingGrappleAction");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
        //necesito que siga estando isGrappling = true por unos frames más sino acumula poco addForce así que lo hago async
        StartCoroutine(AsyncStopGrapple(.9f));

    }

    IEnumerator AsyncStopGrapple(float waitTime)
    {
        StopGrapple();
        yield return new WaitForSecondsRealtime(waitTime);
        isGrappling = false;
    }
}
