using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = -6f;
    private float rotationSpeed = 5f;
    private float jump = 600f;

    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical);
        dir.Normalize();
        transform.position = transform.position + dir * speed * Time.deltaTime;
        if (dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(0f, jump, 0f);
        }

    }
}
