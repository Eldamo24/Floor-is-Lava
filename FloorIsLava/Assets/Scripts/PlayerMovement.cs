using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Transform camera;
    [SerializeField]
    private float upForce = 250f;
    [SerializeField]
    private float movementForce = 5f;
    private Vector2 input;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool isGrounded;
 

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void FixedUpdate()
    {
        //  rb.AddForce(new Vector3(input.x, 0f, input.y) * movementForce);
        rb.MovePosition(transform.position + new Vector3(input.x, 0f, input.y) * Time.deltaTime * movementForce);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if(this.isGrounded)
            {
                rb.AddForce(Vector3.up * upForce);
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            this.isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Platform")
        {
            this.isGrounded = false;
        }
       
    }

}
