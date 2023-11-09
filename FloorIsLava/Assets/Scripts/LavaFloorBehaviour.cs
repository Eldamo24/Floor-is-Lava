using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloorBehaviour : MonoBehaviour
{
    //fields
    Vector3 _startingPosition;
    [SerializeField]
    private float velocityMultiplier = 1.0f;
    [SerializeField]
    private bool frozenVelocity = false;

    //Properties
    public Vector3 CurrentPosition { get { return transform.position;} set { transform.position = value; } }

    public Vector3 StartingPosition { get { return _startingPosition; } private set { _startingPosition = value; } }


    private void Awake()
    {
        StartingPosition = CurrentPosition;
        Debug.Log(StartingPosition.y);
        Debug.Log("START");
    }

    private void Update()
    {
        if (!frozenVelocity)
        {
            IncreaseYPosition();
        }

        if (Input.GetKey(KeyCode.R))
        {
            RestartPosition();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            frozenVelocity = !frozenVelocity;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            velocityMultiplier *= 2;
        }else if (Input.GetKeyUp(KeyCode.V))
        {
            velocityMultiplier /= 2;
        }
    }

    public void RestartPosition()
    {
        if(StartingPosition != null)
        {
            CurrentPosition = StartingPosition;
        }
        else
        {
            Debug.Log("RestartPosition: no se pudo reiniciar posición de la lava");
        }
    }

    public void SetPosition(Vector3 position)
    {
        CurrentPosition = position;
    }

    public void IncreaseYPosition(float multiplier)
    {
        transform.Translate((Vector3.up * multiplier) * Time.deltaTime, Space.World);
    }

    public void IncreaseYPosition()
    {
        IncreaseYPosition(velocityMultiplier);
    }

}
