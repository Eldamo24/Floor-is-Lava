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
    [SerializeField]
    private Animator anim;

    private Vector2 input;
    private PlayerInput playerInput;
    public float rotationSpeed = 7f;
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float _playerSpeed;

    // ADDED BY LEO - It is needed to avoid going through walls (class definition in WallDetectorBehaviour)
    private WallDetectorBehaviour _WallDetectorBehaviour; // ADDED BY LEO
    private Vector3 initialPosition; // ADDED BY LEO
    private bool movementChecked; // ADDED BY LEO

    public bool IsDescending
    {
        get
        {
            return rb.velocity.y < 0;
        }
    }
    public float PlayerSpeed
    {
        get
        {
            if (IsDescending) return _playerSpeed / 2;
            return _playerSpeed;
        }
        private set { _playerSpeed = value; }
    }

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
        play.GetComponent<isGrounded>().OnFloorCollisionChanged.AddListener(setJumpingAnimation);
        
        // ADDED BY LEO - (WallDetector must be child of Player)
        _WallDetectorBehaviour = player.Find("WallDetector").GetComponent<WallDetectorBehaviour>(); // ADDED BY LEO 
        movementChecked = true; // ADDED BY LEO

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (IsMovementAllowed)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().LoadGame();
            }
            anim.SetBool("IsRunning", false);
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            input = playerInput.actions["Movement"].ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * input.y + orientation.right * input.x;
            if (inputDir != Vector3.zero)
            {
                anim.SetBool("IsRunning", true);
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
      
                if (movementChecked) // ADDED BY LEO
                {
                    initialPosition = player.position; // ADDED BY LEO
                    
                    rb.MovePosition(player.position + inputDir * Time.deltaTime * PlayerSpeed);

                    movementChecked = false; // ADDED BY LEO
                    StartCoroutine(LittleDelay()); // ADDED BY LEO - FixedTime is 0.02s by default, this delay is longer to ensure several OnTrigger evaluations
                    if (_WallDetectorBehaviour.wallInContact) // IF CONDITIONAL ADDED BY LEO - It is needed to avoid going through walls
                        rb.MovePosition(initialPosition); // it reverts change of position
                    movementChecked = true; // ADDED BY LEO
                }
            }
        }
    }

    private IEnumerator LittleDelay() // ADDED BY LEO
    {
        yield return new WaitForSeconds(0.04f); // At least two default FixedTime
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {

        if (callbackContext.performed)
        {
            if (play.GetComponent<isGrounded>().grounded && IsMovementAllowed)
            {
                rb.AddForce(Vector3.up * upForce);
                anim.SetBool("Jumping", true);
                AudioManager.Instance.PlaySFX("jump");
            }

        }

    }

    private void setJumpingAnimation(bool isOnFloor)
    {
        if(isOnFloor && anim.GetBool("Jumping") == true)
        {
            anim.SetBool("Jumping", !isOnFloor);
        }
    }

    public void Pause()
    {
        GameManager.gameManager.Pause();
    }


}
