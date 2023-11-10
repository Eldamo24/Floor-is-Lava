using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloorBehaviour : MonoBehaviour, IEnemyDamage
{
    //fields
    Vector3 _startingPosition;
    [SerializeField]
    private float _velocityMultiplier = 1.0f;
    [SerializeField]
    private bool _frozenVelocity = false;
    [SerializeField]
    private bool _levelFilled = false;

    //Properties
    public Vector3 CurrentPosition { get { return transform.position;} set { transform.position = value; } }

    public Vector3 StartingPosition { get { return _startingPosition; } private set { _startingPosition = value; } }

    public float VelocityMultiplier { get { return _velocityMultiplier; } set { _velocityMultiplier = value; } }

    public int damage { get;set; }

    private void Awake()
    {
        StartingPosition = CurrentPosition;
        damage = 1000;
    }

    private void Update()
    {
        if (!_frozenVelocity && !_levelFilled)
        {
            IncreaseYPosition();
            if (CurrentPosition.y > 86)
            {
                _levelFilled = true;
            };
        }

        if (Input.GetKey(KeyCode.R))
        {
            RestartPosition();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            _frozenVelocity = !_frozenVelocity;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            _velocityMultiplier *= 2;
        }else if (Input.GetKeyUp(KeyCode.V))
        {
            _velocityMultiplier /= 2;
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
        IncreaseYPosition(_velocityMultiplier);
    }

}
