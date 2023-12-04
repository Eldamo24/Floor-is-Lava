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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsMovementAllowed)
        {
            anim.SetBool("IsRunning", false);
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
            input = playerInput.actions["Movement"].ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * input.y + orientation.right * input.x;
            if (inputDir != Vector3.zero)
            {
                anim.SetBool("IsRunning", true);
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                rb.MovePosition(player.position + inputDir * Time.deltaTime * PlayerSpeed); 
            }
        }
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {

        if (callbackContext.performed)
        {
            if (play.GetComponent<isGrounded>().grounded)
            {
                rb.AddForce(Vector3.up * upForce);
                anim.SetBool("Jumping", true);
            }

        }

    }

    void Update()
    {
        descend();
    }

    void descend()
    {
        if(Input.GetKeyDown(KeyCode.U)) // It detects when the "U" key is pressed (but not if it remains pressed)
        {
            GameObject PlayerGO = GameObject.Find("/Player"); // '/' means that the scene is parent of Player
            // EdgeDetector must be player's grandchild, PlayerObj's child, because it must rigid move/rotate with the 3d model
            // It checks if player is close to an edge, if so proceed to descend
            if (GameObject.Find("/Player/PlayerObj/EdgeDetector").GetComponent<EdgeDetectorBehaviour>().detectEdge())
                StartCoroutine(descentSequence(PlayerGO));                
            else // DebugLog could be removed
                Debug.Log("No está en un borde");                 
        }
    }

    private IEnumerator descentSequence (GameObject PlayerGO)
    {
        // It disables input
        PlayerGO.GetComponent<PlayerInput>().enabled=false;
        // These two lines disable RigidBody physics.
        PlayerGO.GetComponent<Rigidbody>().isKinematic = true;
        PlayerGO.GetComponent<Rigidbody>().detectCollisions = false;

        yield return StartCoroutine(rotatePlayer(PlayerGO, new Vector3(0, 180, 0), 1 ) );
        yield return StartCoroutine(horizontalDisplacement(PlayerGO, -PlayerGO.transform.Find("PlayerObj").transform.forward, 1));
        yield return StartCoroutine(verticalDisplacement(PlayerGO, 0.75f));

        // These two lines enable RigidBody physics.
        PlayerGO.GetComponent<Rigidbody>().isKinematic = false;
        PlayerGO.GetComponent<Rigidbody>().detectCollisions = true;
        // It enables input
        PlayerGO.GetComponent<PlayerInput>().enabled=true;
    }

    private IEnumerator rotatePlayer(GameObject PlayerGO, Vector3 angles, float duration )
    {        
        Quaternion startRotation = PlayerGO.transform.rotation;
        Quaternion endRotation = Quaternion.Euler( angles ) * startRotation;
        for( float t = 0 ; t < duration ; t+= Time.deltaTime )
        {
            PlayerGO.transform.rotation = Quaternion.Lerp( startRotation, endRotation, t / duration );
            yield return null;
        }
        PlayerGO.transform.rotation = endRotation;        
    }

    private IEnumerator horizontalDisplacement(GameObject PlayerGO, Vector3 displacement, float duration )
    {
        Vector3 startPosition = PlayerGO.transform.position;
        for( float t = 0 ; t < duration ; t+= Time.deltaTime )
        {
            PlayerGO.transform.position+=displacement*Time.deltaTime/duration;
            yield return null;
        }
    }
    private IEnumerator verticalDisplacement(GameObject PlayerGO, float speed )
    {
        while (!Physics.Raycast(PlayerGO.transform.position, Vector3.down, 0.5f))
        {
            PlayerGO.transform.position+=Vector3.down*Time.deltaTime*speed;
            yield return null;
        }
        yield return new WaitForSeconds(1f); //1 second delay to release the rope
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
