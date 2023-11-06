using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;

    private float upForce = 250f;
    private float movementForce = 5f;
    private Vector2 input;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Movement"].ReadValue<Vector2>();
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
            rb.AddForce(Vector3.up * upForce);
        }
        
    }

}
