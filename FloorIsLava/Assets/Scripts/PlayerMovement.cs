using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    //private Rigidbody rb;
    //private Transform camera;

    //private float upForce = 250f;
    //private float movementForce = 5f;
    //private Vector2 input;
    //private float turnSmoothTime = 0.1f;
    //private float turnSmoothVelocity;
    //private bool isGrounded;
    private PlayerInput playerInput;
    private Vector2 input;
    private float turnSmoothVelocity;
    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float speed = 30f;
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    private float gravity;
    private float fallVelocity;
    private float jumpForce;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        gravity = 40f;
        jumpForce = 15f;
        //camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0.0f, input.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            setGravity();
            controller.Move(speed * Time.deltaTime * moveDirection.normalized);
        }
        //input = playerInput.actions["Movement"].ReadValue<Vector2>();
        //Vector3 direction = new Vector3(input.x, 0f, input.y);
        //if (direction.magnitude > 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //}
    }

    private void setGravity()
    {
        if (controller.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
        }
        moveDirection.y = fallVelocity;
    }

    //private void FixedUpdate()
    //{
    //    //  rb.AddForce(new Vector3(input.x, 0f, input.y) * movementForce);
    //    //rb.MovePosition(transform.position + new Vector3(input.x, 0f, input.y) * Time.deltaTime * movementForce);
    //}

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && controller.isGrounded)
        {
 
            fallVelocity = jumpForce;
            moveDirection.y = fallVelocity;
            controller.Move(new Vector3(0f, moveDirection.y, 0f) * Time.deltaTime);
        }
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if(collision.gameObject.tag == "Platform")
    //    //{
    //    //    this.isGrounded = true;
    //    //}
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    //if(collision.gameObject.tag == "Platform")
    //    //{
    //    //    this.isGrounded = false;
    //    //}
       
    //}

}
