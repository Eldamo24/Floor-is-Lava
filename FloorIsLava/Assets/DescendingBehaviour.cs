using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Unity.VisualScripting;
using UnityEngine.InputSystem;
//using UnityEngine.Windows;
//using Input = UnityEngine.Input;


public class DescendingBehaviour : MonoBehaviour
{
    // Caching variables (more performance?)
    private Transform player;
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
    const float newPositionDistance = 1.5f;
    const int cantidadDePuntos = 8;
    const int separacionAngular = 360/cantidadDePuntos;
    const float rotationSpeed = 360; // degrees per second 
    const float positioningSpeed = 2; // meters per second
    const float descendingSpeed = 0.75f; // meters per second
    const float delayToReleaseRope = 1; // in seconds

    // Start is called before the first frame update
    private void Start()
    {
        // The parent of "DescendingFeature" must be "Player"
        player = transform.parent; 
        playerInput = player.GetComponent<PlayerInput>();
        playerRigidbody =  player.GetComponent<Rigidbody>();
        // "PlayerObj" must be child of "Player"
        // Due to historical development reasons, this object is the one that contains the character's orientation.
        playerModel= player.Find("PlayerObj"); 
    }

    // Update is called once per frame
    private void Update()
    {
        Descend();
    }

    // It tells if there is not any contact with nothing just in front (in the newPosition)
    public bool DetectEdge()
    {
        // It checks if there will be any obstacles in the new position.
        newPosition = player.transform.position+newPositionDistance*playerModel.transform.forward;
        if (Physics.Raycast(newPosition, Vector3.down, 2) || Physics.Raycast(newPosition, Vector3.up, 1.85f))
            return false;

        // It checks the same but in a some points around new position
        radioVector = newPositionDistance/2*playerModel.transform.forward;
        for (int i = 0; i < cantidadDePuntos; i++)
        {
            referencePoint=newPosition+radioVector;            
            if (Physics.Raycast(referencePoint, Vector3.down, 2) || Physics.Raycast(referencePoint, Vector3.up, 1.85f))
                return false;
            radioVector = Quaternion.Euler(0, separacionAngular, 0) * radioVector;            
        }
        return true;
    }

    private void Descend()
    {
        if(Input.GetKeyDown(KeyCode.U)) // It detects when the "U" key is pressed (but not if it remains pressed)
        {
            //GameObject playerGO = GameObject.Find("/player"); // '/' means that the scene is parent of player
            // EdgeDetector must be player's grandchild, playerObj's child, because it must rigid move/rotate with the 3d model
            // It checks if player is close to an edge, if so proceed to descend
            if (DetectEdge())
                StartCoroutine(DescentSequence());                
            else // DebugLog could be removed
                Debug.Log("No está en un borde");                 
        }
    }

    private IEnumerator DescentSequence()
    {
        // It disables input (player movement)
        playerInput.enabled=false;
        // These two lines disable RigidBody physics.
        playerRigidbody.isKinematic = true;
        playerRigidbody.detectCollisions = false;

        yield return StartCoroutine(Rotation(new Vector3(0, 180, 0), 180/rotationSpeed) );
        yield return StartCoroutine(HorizontalDisplacement(-newPositionDistance*playerModel.transform.forward, newPositionDistance/positioningSpeed));
        yield return StartCoroutine(VerticalDisplacement(descendingSpeed));
        yield return new WaitForSeconds(delayToReleaseRope); // delay to release the rope

        // These two lines enable RigidBody physics.
        playerRigidbody.isKinematic = false;
        playerRigidbody.detectCollisions = true;
        // It enables input (player movement)
        playerInput.enabled=true;
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
        // I think looks better with a faster initial descent of 1.5 meters
        // But rigidbody physics are disabled, so I have to implement a "free fall".
        // In free fall y=g/2*T^2 -> dy = g*T*dT
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
