# Floor-is-Lava
A third person game where you have to escape from an erupting volcano

GameManager y control de estados del juego
```mermaid
classDiagram

    class GameManager{
      <<singleton>>
      +UnitHealth _playerHealth
      -LavaFloorBehaviour _lavaFloor
      -GameStatus _currentGameStatus
      +UnityEvent<GameStatus> OnGameStatusChanged
      +GameStatus CurrentGameStatus : prop
      +GameManager gameManager : prop
      -Awake()
      +Update()
      +NewGame(string)
      +LoadScene(string)
      +RestartScene()
      +LoadMainMenu()
      +ExitGame()
      +Pause()
    }

    class GameStatus{
    <<enum>>
    Playing,
    Paused,
    GameOver,
    OnLevelEnd,
    OnMenu

    }

    class GameScenes{
        <<enum>>
        MainMenu,
        Level1,
        Level2
    }
	
    class UnitHealth{
        -int _currentHealth
        -int _currentMaxHealth
        +int Health:prop
        +int MaxHealth:prop
        +UnitHealth(int,int)
        +DmgUnit(int)
        +HealUnit(int)
    }

```

Gameplay basico (movimiento, camara, colliders)
```mermaid
classDiagram

    class CameraController{
        -CinemachineFreeLook _cineFL
        -GameObject _spectatorPov
        -GameObject _spectatorTarget
        -int _cameraFov
        +GameObject CurrentTransformCamFollow : prop
        +GameObject CurrentTransformCamLookAt : prop
        +int CameraFov
        -void Start()
        -void OnGameStatusChanged(GameStatus) 
        -IEnumerator SetGameOverCamera()
        -IEnumerator FindCinemachineFreeLook()
        -void LockCamera(bool)
        }
		
	class isGrounded{
        +bool grounded
        +UnityEvent<bool> OnFloorCollisionChanged
	+GameObject player
	-LayerMask layer
	+CheckGround()
        
    }

	    class PlayerBehaviour{
        -HealthBar _healthBar
        +bool IsDead:prop
        -Update()
        +OnTriggerEnter(Collider)
        +OnCollisionEnter(Collision)
        -FixedUpdate()
        -PlayerTakeDmg(int)
        -PlayerHeal(int)
    }
	
    class RigidBodyMovement{
        +Transform orientation
        +Transform player
        +Transform playerObj
        +Rigidbody rb
        +GameObject playe
        -Animator anim:serialized
        -Vector2 input
        -PlayerInput playerInput
        +float rotationSpeed
        -float upForce:serialized
        -float playerSpeed:serialized
        +bool IsMovementAllowed:prop
        -Start()
        -Update()
        +Jump(CallbackContext)
        -setJumpingAnimation(bool)
        +Pause()
    }
	
	class PlayerMovement{
        <<pending>>
    }

```

Daños del entorno
```mermaid
classDiagram

    class Tags{
        <<static>>
        +string LavaFloor
        +string Rock
        +string Bat
        +string LavaGeiser
        +string Platform
        +string Stake
        +string Healer 
    }

    class IEnemyDamage{
    <<interface>>
    +int damage:prop
    }

    class GeiserCreation{
        -ParticleSystem geiser
        -Collider box
        -Start()
        -OnGameStatusChanged(GameStatus)
        -changeGeiserState()
    }

    IEnemyDamage<|--GeiserDamageBehaviour : Implements
    class GeiserDamageBehaviour{
        +int damage:prop
        -Start()
    }

    IEnemyDamage<|--GiantRockBehaviour : Implements
    class GiantRockBehaviour{
        +int damage:prop
        -Start()
    }

    class HealthGiver{
        +int health
        -Start()
        -Update()
        -OnTriggerEnter(Collider)
    }

    IEnemyDamage<|-- LavaFloorBehaviour : Implements
    class LavaFloorBehaviour{
        -Vector3 _startingPosition
        -float _velocityMultiplier
        -float _gameOverVelocityMultiplier
        -bool _frozenVelocity
        -bool _levelFilled
        -int _levelFilledYPosition
        +Vector3 CurrentPosition:prop
        +Vector3 StartingPosition
        +float VelocityMultiplier
        +int damage
        -Start()
        -Awake()
        -Update()
        +RestartPosition()
        +SetPosition(Vector3)
        +IncreaseYPosition(float)
        +IncreaseYPosition()
        -OnGameStatusChanged(GameStatus)
    }

    IEnemyDamage<|-- LittleRockBehaviour : Implements
    class LittleRockBehaviour{
        +int damage:prop
    }

    class RockFall{
        -GameObject rock:serialized
        -OnTriggerEnter(Collider)
    }

     class FallingPlatform{
        -Animator anim
        -OnCollisionEnter(Collision)
    }

```

Mecánica de grappling
```mermaid
classDiagram

    class RotateGun{
        +Grapping grappling
        -Update()
    }

    class ray{
        -Update()
    }
	
	class Grappling{
        -LineRenderer Ir
        -Vector3 grapplePoint
        +LayerMask whatisGrappeable
        +Transform gunTip
        +Transfor camera
        +Transform _playerHealth
        -float maxDistance
        -SpringJoint joint
        -bool isGrappling
        -Rigidbody body
        -Awake()
        -Update()
        -LateUpdate()
        -StartGrapple()
        -DrawRope()
        -StopGrapple()
        +IsGrappling() : bool
        +GetGrapplePoint() : Vector3
        +Grap(CallbackContext)
    }
```

UI
```mermaid
classDiagram

    class UiManager{
        -GameObject _uiGameOver:serialized
        -GameObject _uiGameplay:serialized
        -GameObject _uiPause:serialized
        -GameObject _uiLevelEnd:serialized
        -GameObject _uiMainMenu:serialized
        -GameObject[] _uiElements
        -CursorManager _cursorManager
        -Start()
        -SetUIBasedOnGameStatus(GameStatus)
        -OnGameStatusChanged(GameStatus)
    }

    class CursorManager{
        +CursorManager()
        +SetCursorVisible()
        +SetCursorInvisible()
    }
	
	class TextBar{
        +HealthBar healthBar:serialized
        -Text text
        -Slider sliderBar
        -Image image
        -AudioSource audioSource
        -Start()
        -Update()
    }
	
	class HealthBar{
        -Slider _healthSlider
        -Start()
        +SetMaxHealth(int)
        +SetHealth(int)
    }
```
