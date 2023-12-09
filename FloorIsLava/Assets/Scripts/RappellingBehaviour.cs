using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Unity.VisualScripting;
using UnityEngine.InputSystem;
//using UnityEngine.Windows;
//using Input = UnityEngine.Input;


public class RappellingBehaviour : MonoBehaviour
{
    // Caching variables (more performance?)
    private Transform player;
    private Transform stake;
    private bool rappellingInProcess = false;
    private Vector3 stakePosition;
    private Vector3 stakeForward;
    private Transform playerModel;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Vector3 newPosition;
    private Vector3 radioVector;
    private Vector3 referencePoint;
    private float T = 0;
    private float deltaT = 0;
    private float y = 0;
    private float deltaY = 0;
    const float newPositionDistance = 1.5f; // Distance forward where the character will be lacated
    const int cantidadDePuntos = 8; // amount of point (around new position) to be cheked
    const int angularSeparation = 360/cantidadDePuntos;
    const float rotationSpeed = 360; // degrees per second 
    const float positioningSpeed = 2; // meters per second
    const float descendingSpeed = 0.75f; // meters per second
    const float delayToReleaseRope = 1; // in seconds
    

    // Start is called before the first frame update
    private void Start()
    {
        // The parent of "RappellingFeature" must be "Player"
        player = transform.parent; 
        playerInput = player.GetComponent<PlayerInput>();
        playerRigidbody =  player.GetComponent<Rigidbody>();
        // "PlayerObj" must be child of "Player"
        // Due to historical development reasons, this object is the one that contains the character's orientation.
        playerModel = player.Find("PlayerObj"); 
        // "Stake" must be child of "RappellingFeature"
        stake = transform.Find("Stake");
        stake.transform.gameObject.SetActive(false); // Initially no-visible
    }

    private void FixedUpdate()
    {
        if (rappellingInProcess)
        {
            stake.transform.position = stakePosition;
            stake.transform.forward = stakeForward;
        }        
    }

    public void Rappelling(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && !rappellingInProcess) // the action (of press "R" key) has just been performed and player is not yet rappeling
        {
            // It checks if player is close to an edge, if so proceed to descend
            if (DetectEdge())
                StartCoroutine(RappellingSequence());                
            else // DebugLog could be removed
                Debug.Log("No tiene sentido intentar descender (o no es borde, o hay lava, o suelo cerca, o está con un pie en el aire y no puede maniobrar)");                 
        }
    }

    // It tells if there is not any contact with nothing just in front (in the newPosition)
    public bool DetectEdge()
    {   
        // This first line is a patch
        // It checks that just below player there is a platform, because player could be falling and in contact with a platform (a vertical wall)
        if (!Physics.Raycast(player.transform.position, Vector3.down, 0.05f)) // Only 5cm of tolerance 
            return false;        
        
        // It checks if there will be any obstacles in the new position.
        newPosition = player.transform.position+newPositionDistance*playerModel.transform.forward;
        if (Physics.Raycast(newPosition, Vector3.down, 2) || Physics.Raycast(newPosition, Vector3.up, 1.85f))
            return false;

        // It checks the same but in some points around new position
        radioVector = newPositionDistance/2*playerModel.transform.forward;
        for (int i = 0; i < cantidadDePuntos; i++)
        {
            referencePoint=newPosition+radioVector;            
            if (Physics.Raycast(referencePoint, Vector3.down, 2) || Physics.Raycast(referencePoint, Vector3.up, 1.85f))
                return false;
            radioVector = Quaternion.Euler(0, angularSeparation, 0) * radioVector;            
        }
        return true;
    }

    private IEnumerator RappellingSequence()
    {
        rappellingInProcess = true;
        
        // It disables input (player movement)
        playerInput.enabled=false;
        // These two lines disable RigidBody physics.
        playerRigidbody.isKinematic = true;
        playerRigidbody.detectCollisions = false;

        stakePosition = player.transform.position;
        stakeForward = playerModel.transform.forward;
        yield return StartCoroutine(Rotation(new Vector3(0, 180, 0), 180/rotationSpeed) );
        yield return StartCoroutine(HorizontalDisplacement(-newPositionDistance*playerModel.transform.forward, newPositionDistance/positioningSpeed));
        stake.transform.gameObject.SetActive(true);
        yield return StartCoroutine(VerticalDisplacement(descendingSpeed));
        yield return new WaitForSeconds(delayToReleaseRope); // delay to release the rope
        stake.transform.gameObject.SetActive(false);
        
        // These two lines enable RigidBody physics.
        playerRigidbody.isKinematic = false;
        playerRigidbody.detectCollisions = true;
        // It enables input (player movement)
        playerInput.enabled=true;

        rappellingInProcess = false;
    }

    private IEnumerator Rotation(Vector3 angle, float duration )
    {        
        Quaternion startRotation = player.transform.rotation;
        Quaternion endRotation = Quaternion.Euler( angle ) * startRotation;
        for( float t = 0 ; t < duration ; t+= Time.deltaTime )
        {
            player.transform.rotation = Quaternion.Lerp( startRotation, endRotation, t / duration );
            yield return null;
        }
        player.transform.rotation = endRotation;        
    }

    private IEnumerator HorizontalDisplacement(Vector3 displacement, float duration )
    {
        for( float t = 0 ; t < duration ; t+= Time.deltaTime )
        {
            player.transform.position+=displacement*Time.deltaTime/duration;
            yield return null;
        }
    }
    private IEnumerator VerticalDisplacement(float speed )
    {
        // I think it looks better with a faster initial descent of 1.5 meters
        // But rigidbody physics are disabled, so I have to implement a "free fall".
        // In free fall y=g/2*T^2 -> dy = g*T*dT
        T = 0;
        deltaY = 0;
        y = 0;
        do
        {            
            deltaT = Time.deltaTime;
            T += deltaT;
            deltaY = 9.8f*T*deltaT;
            y += deltaY;
            player.transform.position += Vector3.down*deltaY;
            yield return null;
        } while (y<1.5);

        // A slower constant descending speed        
        while (!Physics.Raycast(player.transform.position, Vector3.down, 0.5f)) // it stops descescending 0.5 meters over ground
        {
            player.transform.position+=Vector3.down*Time.deltaTime*speed;
            yield return null;
        }
    }

}
