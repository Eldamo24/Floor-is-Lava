using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class Grappling : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 _grapplePoint;
    public LayerMask whatIsGrappeable;
    public Transform gunTip, camera, player;
    private float maxDistance = 3000f;
    private SpringJoint joint;
    public bool isGrappling = false;
    private float _speed = 5f;
    [SerializeField]
    private Rigidbody body;

    private float Spring
    {
        set
        {
            if (joint != null)
            {
                joint.spring = value;
            }
        }
    }

    private Vector3 PlayerPosition
    {
        get => Player.GetComponent<Transform>().position;
        set => GameObject.Find("Player").GetComponent<Transform>().position = value;
    }

    private GameObject Player => GameObject.Find("Player");

    private float JointLenght => joint != null ? Vector3.Distance(GrapplePoint, joint.gameObject.transform.position) : 0;

    public Vector3 GrapplePoint
    {
        get { return _grapplePoint; }
        set { _grapplePoint = value; }
    }


    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawLine(ray.origin, ray.direction);
    }

    private void Update()
    {
        if (isGrappling)
        {
            float step = _speed * Time.deltaTime;
            PlayerPosition = Vector3.MoveTowards(
                                                PlayerPosition
                                                , GrapplePoint
                                                , step);
        }

        if (Player.GetComponent<Rigidbody>().velocity.y > 0)
        {
            Debug.Log($"Longitud de joint{JointLenght.ToString()} \n Velocidad en Y {Player.GetComponent<Rigidbody>().velocity.y.ToString()}");

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
            GrapplePoint = jointImpact.transform.position;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = GrapplePoint;

            joint.maxDistance = JointLenght * 0.8f;
            joint.minDistance = JointLenght * 0.25f;

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
        lr.SetPosition(1, _grapplePoint);

    }

    void StopGrapple()
    {
        //Debug.Log("Deje de graplear");
        lr.positionCount = 0;
        Destroy(joint);
    }

    public bool IsGrappling()
    {
        return joint != null;
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
        float endingGrappleOffsetModifier = 1.2f;

        try
        {

            //le doy un impulso hacia arriba

            PlayerPosition = Vector3.MoveTowards(
                                                PlayerPosition
                                                , new Vector3(GrapplePoint.x, (GrapplePoint.y + endingGrappleOffsetModifier ), GrapplePoint.z) 
                                                , 7 * Time.deltaTime);

            Vector3 velocity = rb.velocity;
            velocity.y = Mathf.Clamp(velocity.y, -1000, 6);


            rb.velocity = velocity;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }


        if(JointLenght < (endingGrappleOffsetModifier + 0.3))
        {
            //necesito que siga estando isGrappling = true por unos frames más sino acumula poco addForce así que lo hago async
            StartCoroutine(AsyncStopGrapple(.1f));
        }

    }

    IEnumerator AsyncStopGrapple(float waitTime)
    {


        yield return new WaitForSecondsRealtime(waitTime);
        if (isGrappling)
        {
            StopGrapple();
        }
        isGrappling = false;
    }
}
