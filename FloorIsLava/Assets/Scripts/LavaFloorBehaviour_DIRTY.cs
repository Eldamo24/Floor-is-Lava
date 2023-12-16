// Note by Leo:
//
// There has been no way to establish a unified criteria about how the game should be.
// Consequently, some code contributions were developed considering only the artistic vision of level 1.
// Since there is a general fear against to modify code that works for level 1. 
// This script is a quick way to reuse LavaFloorBehaviour.cs without modifying it.
// I know it's not the best programming practice, it's what I can do given the circumstances.
// I know it's a very dirty bypass, I know it breaks the encapsulation, I know...
// My other two options would be to submit to the vision of the other contributors, 
// or to redo the code from scratch taking responsibility for making it work at both levels
// (without support from anyone and, now, with nothing of time).
//
// LavaFloorBehaviour.cs has the UpwardsSpeed serialized field
// (it is only read in the class construction and in the awake callback)
// Actually that script considers a speed called "VelocityMultiplier"
// "VelocityMultiplier" can take 3 possible values: Normal=0.15*UpwardsSpeed, Fast=1.4*UpwardsSpeed, GameOverFast=15*UpwardsSpeed
// (I think this is very confusing, because UpwardsSpeed is not the actual speed)
// LavaFloorBehaviour_DIRTY.cs overwrites Normal speed with "publicSpeed" (if publicSpeedOn=true)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LavaFloorBehaviour_DIRTY : MonoBehaviour, IEnemyDamage
{
    public bool publicSpeedOn = false; // Patch add by Leo
    public float publicSpeed; // Patch add by Leo

    //fields
    Vector3 _startingPosition;
    [SerializeField]
    private float _upwardsSpeed;
    [SerializeField]
    private float _gameOverVelocityMultiplier;
    [SerializeField]
    private bool _frozenVelocity = false;
    [SerializeField]
    private bool _levelFilled = false;
    [SerializeField]
    private int _levelFilledYPosition;
    [SerializeField]
    private LavaVelocity lavaVelocity;
    [SerializeField]
    private int _distancePlayerOutOfRange = 10;

    //Properties
    public Vector3 CurrentPosition { get { return transform.position;} set { transform.position = value; } }

    public Vector3 StartingPosition { get { return _startingPosition; } private set { _startingPosition = value; } }

    public Vector3 PlayerPosition { get { return GameObject.Find("Player").GetComponent<Transform>().position; } }  

    public Vector3 SelfPosition { get { return gameObject.GetComponent<Transform>().position; } }

    public bool IsPlayerWithinRange { get { return Mathf.Abs(PlayerPosition.y - SelfPosition.y) > _distancePlayerOutOfRange; } }
    public float VelocityMultiplier
    {
        get
        {
            if(GameManager.gameManager.CurrentGameStatus.Equals(GameStatus.GameOver))
            {
                //juego terminado entonces rellenar de lava a las chapas
                return lavaVelocity.GameOverFast;
            }else if(IsPlayerWithinRange){
                //rubberbanding: si el player esta muy lejos, llenarse al doble de rapido
                Debug.Log("lava rapida");
                return lavaVelocity.Fast;
            }
            else
            {
                if (publicSpeedOn) return publicSpeed; // Patch add by Leo
                return lavaVelocity.Normal;
            }
        }
    }

    public int damage { get;set; }
    public class LavaVelocity
    {
        float normal;

        public LavaVelocity(float normalSpeed)
        {
            this.normal = normalSpeed;
        }

        public float Normal { get { return normal * 0.15f; } }
        public float Fast { get { return normal * 1.4f; } }
        public float GameOverFast { get { return normal * 15f; } }
    }
    
    private void Start()
    {
        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);

    }
    
    private void Awake()
    {
        StartingPosition = CurrentPosition;
        damage = 1000;
        lavaVelocity = new LavaVelocity(_upwardsSpeed);
    }

    private void Update()
    {
        if (!_frozenVelocity && !_levelFilled)
        {
            IncreaseYPosition();
            if (CurrentPosition.y > _levelFilledYPosition)
            {
                _levelFilled = true;
            };
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
            Debug.Log("RestartPosition: no se pudo reiniciar posici�n de la lava");
        }
    }

    public void SetPosition(Vector3 position)
    {
        CurrentPosition = position;
    }

    public void IncreaseYPosition()
    {
        transform.Translate(Vector3.up * VelocityMultiplier * Time.deltaTime, Space.World);
    }

    private void OnGameStatusChanged(GameStatus newStatus)
    {
        switch(newStatus)
        {
            case GameStatus.Paused:
                _frozenVelocity = true;
                break;
            case GameStatus.Playing: 
                _frozenVelocity = false; 
                break;
            case GameStatus.GameOver: 
                _upwardsSpeed = _gameOverVelocityMultiplier; 
                break;
        }
    }

    public void SetFrozenVelocity(bool isFrozen)
    {
        _frozenVelocity = isFrozen;
        Invoke("RestartVelocity", 5);
    }

    private void RestartVelocity()
    {
        _frozenVelocity = false;
    }
}
