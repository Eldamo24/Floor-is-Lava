using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class RigidBodyMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public GameObject play;

    private Vector2 input;
    private PlayerInput playerInput;
    public float rotationSpeed;
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float playerSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        upForce = 290f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameManager.CurrentGameStatus == GameStatus.Playing)
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            input = playerInput.actions["Movement"].ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * input.y + orientation.right * input.x;
            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                rb.MovePosition(player.position + inputDir * Time.deltaTime * playerSpeed);
            }
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (play.GetComponent<isGrounded>().isOnFloor)
            {
                rb.AddForce(Vector3.up * upForce);
            }

        }
    }
}
