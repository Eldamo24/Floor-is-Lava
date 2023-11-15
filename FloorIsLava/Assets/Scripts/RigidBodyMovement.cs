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
    private GameManager gameManager;

    private Vector2 input;
    private PlayerInput playerInput;
    public float rotationSpeed;
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float playerSpeed = 15f;

    public bool IsMovementAllowed
    {
        get
        {
            return GameManager.gameManager.CurrentGameStatus == GameStatus.Playing;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        upForce = 290f;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMovementAllowed)
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

    public void Pause()
    {
        switch (gameManager.CurrentGameStatus)
        {
            case GameStatus.Paused:
                gameManager.CurrentGameStatus = GameStatus.Playing;
                break;
            case GameStatus.Playing:
                gameManager.CurrentGameStatus = GameStatus.Paused;
                break;
        }
    }
}
