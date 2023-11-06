using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public new Transform camera;


    private Rigidbody rb;
    public float movementSpeed;
    public float gravity = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float v = Input.GetAxisRaw("Vertical");
        //if (h != 0 || v !=0)
        //{
        //    Vector3 direction = (transform.forward * -h + transform.right * v).normalized;
        //    rb.velocity = direction * movementSpeed;
        //}
        //else
        //{
        //    rb.velocity = Vector3.zero; 
        //}

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;


        if (h != 0 ||  v != 0)
        {
            Vector3 forward = camera.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = camera.right;
            right.y = 0;
            right.Normalize();

            Vector3 direction = forward * v + right * h;
            direction.Normalize();

            movement = direction * movementSpeed * Time.deltaTime;

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }

        movement.y += gravity * Time.deltaTime;

        characterController.Move(movement);
    }
}
