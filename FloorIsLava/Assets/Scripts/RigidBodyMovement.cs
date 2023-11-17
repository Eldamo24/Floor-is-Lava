using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class RigidBodyMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public GameObject play;
    private GameManager gameManager;
    [SerializeField]
    private Animator anim;

    private Vector2 input;
    private PlayerInput playerInput;
    public float rotationSpeed = 7f;
    [SerializeField]
    private float upForce = 290f;
    [SerializeField]
    private float playerSpeed = 4f;

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
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        //upForce = 290f;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMovementAllowed)
        {
            if (play.GetComponent<isGrounded>().isOnFloor && anim.GetInteger("Jumping") == 1)
            {
                anim.SetInteger("Jumping", 0);
            }
            if (anim.GetBool("IsRunning"))
            {
                anim.SetBool("IsRunning", false);

            }
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            input = playerInput.actions["Movement"].ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * input.y + orientation.right * input.x;
            if (inputDir != Vector3.zero)
            {
                anim.SetBool("IsRunning", true);
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                rb.MovePosition(player.position + inputDir * Time.deltaTime * playerSpeed);
            }
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        try
        {
            if (callbackContext.performed)
            {
                if (play.GetComponent<isGrounded>().isOnFloor)
                {
                    rb.AddForce(Vector3.up * upForce);
                    anim.SetInteger("Jumping", 1);
                }

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public void Pause()
    {
        GameManager.gameManager.Pause();
    }


}
