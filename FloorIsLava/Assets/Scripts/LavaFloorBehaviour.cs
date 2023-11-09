using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloorBehaviour : MonoBehaviour
{
    //fields
    Vector3 _startingPosition;

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
        if (Input.GetKey(KeyCode.Space))
        {
            IncreaseYPosition();
            Debug.Log("espaace");
            Debug.Log(StartingPosition.y.ToString());
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RestartPosition();
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

    public void IncreaseYPosition(int multiplier)
    {
        Vector3 currentPosition = CurrentPosition;
        SetPosition(new Vector3(
                                currentPosition.x,
                                (currentPosition.y + (1 * multiplier)),
                                currentPosition.z));
    }

    public void IncreaseYPosition()
    {
        IncreaseYPosition(1);
    }

}
