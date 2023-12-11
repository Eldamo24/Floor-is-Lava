using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LavaFloorBehaviour : MonoBehaviour, IEnemyDamage
{
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


    //Properties
    public Vector3 CurrentPosition { get { return transform.position;} set { transform.position = value; } }

    public Vector3 StartingPosition { get { return _startingPosition; } private set { _startingPosition = value; } }

    public Vector3 PlayerPosition { get { return GameObject.Find("Player").GetComponent<Transform>().position; } }  

    public Vector3 SelfPosition { get { return gameObject.GetComponent<Transform>().position; } }

    public bool IsPlayerWithinRange { get { return Mathf.Abs(PlayerPosition.y - SelfPosition.y) > 10 ; } }
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
                return lavaVelocity.Fast;
            }
            else
            {
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
            Debug.Log("RestartPosition: no se pudo reiniciar posición de la lava");
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
