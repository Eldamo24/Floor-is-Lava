using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
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
        playerInput = gameObject.GetComponent<PlayerInput>();
        gravity = 30f;
        jumpForce = 15f;
    }

 
    void Update()
    {
        input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            moveDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
           
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        setGravity();
        controller.Move(speed * Time.deltaTime * moveDirection.normalized);

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



    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && controller.isGrounded)
        {
            fallVelocity = jumpForce;
            moveDirection.y = fallVelocity;
            controller.Move(new Vector3(0f, moveDirection.y, 0f) * Time.deltaTime);
        }
        
    }


}
