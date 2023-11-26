using System.Collections;
using Data;
using ObserverPattern;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Subject, IObserver
{
    [SerializeField] private Subject uiSubject;
    [SerializeField] private PlayerAttak playerAttack;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator anim;
    [SerializeField]private float playerSpeed = 2.0f;
    [SerializeField] private float dodgeSpeed = 2.0f;
    [SerializeField] private float distanceCoveredDelay = 1.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float swipeDistance = 200.0f;

    [Header("Touch Logs Texts")]
    [SerializeField] private TextMeshProUGUI touchText;
    [SerializeField] private TextMeshProUGUI xValueText;
    [SerializeField] private TextMeshProUGUI yValueText;
    [SerializeField] private TextMeshProUGUI swipeStatusText;
    [SerializeField] private TextMeshProUGUI RollJumpStatus;

    private float _currentCoveredTime;
    private bool _isPlayerDied = true;
    private bool _isGameOver;

    //Touch Variables for swipe
    private Vector3 _startTouchPosition;
    private Vector3 _endTouchPosition;

    public void Notify(GameData data)
    {
        if (data.IsStartNewGame)
        {
            Debug.Log("--------------- Start New Game ------------");
            RunnerLog.Log("....Start New Game.........");
            controller.enabled = true;
        }
        else if (data.IsGameOver)
        {
            Debug.Log("--------------- GameOver ------------");
            RunnerLog.Log("......GameOver.......");
        }

        _isGameOver = data.IsGameOver;

        if (!_isGameOver || data.IsStartNewGame)
        {
            StartCoroutine(Restart());
        }
        else
        {
            controller.enabled = false;
            //gameObject.SetActive(false);
        }
    }

    private float _colHeight;
    private float _colCenterY;

    private void Start()
    {
        playerAttack.PlayerSpeed = playerSpeed;
        _colCenterY = controller.center.y;
        _colHeight = controller.height;
    }

    private enum Side { Left, Mid, Right}

    private Side _side = Side.Mid;
    private bool _swipeLeft, _swipeRight, _swipeUp, _swipeDown;
    private bool InJump;
    private bool InRoll;
    public float xValue = 2;
    private float newXPos = 0;
    private float moveX = 0;
    private float moveY = 0;
    private float _currentTouchDistance = 0;
    private bool _isMouseInteraction;

    private Vector3 moveVector = Vector3.zero;
    void Update()
    {
        //Debug.Log("_isPlayerDied : " + _isPlayerDied+ " : _isGameOver : "+ _isGameOver);
        if (_isGameOver || _isPlayerDied) return;


        //Uncomment this line to work on editor with mouse
       /*if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
            xValueText.text = "Start X : " + Input.mousePosition.x.ToString();
            yValueText.text = "Start Y : " + Input.mousePosition.y.ToString();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isMouseInteraction = true;
            _endTouchPosition = Input.mousePosition;
            xValueText.text = "End X : " + Input.mousePosition.x.ToString();
            yValueText.text = "End Y : " + Input.mousePosition.y.ToString();
        }*/



        touchText.text = "Tocuh Count : " + Input.touchCount.ToString();
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RunnerLog.Log("Touch began.............");
            xValueText.text = "Start X : " + Input.GetTouch(0).position.x.ToString();
            yValueText.text = "Start Y : " + Input.GetTouch(0).position.y.ToString();
            _startTouchPosition = Input.GetTouch(0).position;
            
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            RunnerLog.Log("Touch End.............");
            xValueText.text = "End X : " + Input.GetTouch(0).position.x.ToString();
            yValueText.text = "End Y : " + Input.GetTouch(0).position.y.ToString();
            _endTouchPosition = Input.GetTouch(0).position;
            _isMouseInteraction = true;

            RunnerLog.Log("Touch End.....Swipe left......"+_swipeLeft +" : Right"+ _swipeRight);
            
        }

        if(_isMouseInteraction)
        {
            _isMouseInteraction = false;
            
            if (_endTouchPosition.x < _startTouchPosition.x)
            {
                _swipeLeft = false;
                _swipeRight = false;
                _currentTouchDistance = _startTouchPosition.x - _endTouchPosition.x;
                if (_currentTouchDistance > swipeDistance)
                {
                    _swipeLeft = true;
                    _swipeRight = false;
                }
            }
            else if (_endTouchPosition.x > _startTouchPosition.x)
            {
                _swipeLeft = false;
                _swipeRight = false;
                _currentTouchDistance = _endTouchPosition.x - _startTouchPosition.x;
                if (_currentTouchDistance > swipeDistance)
                {
                    _swipeLeft = false;
                    _swipeRight = true;
                }
            }

            if (_endTouchPosition.y > _startTouchPosition.y && !_swipeLeft && !_swipeRight)
            {
                _swipeUp = false;
                _swipeDown = false;
                _currentTouchDistance = _endTouchPosition.y - _startTouchPosition.y;
                if (_currentTouchDistance > swipeDistance)
                {
                    
                    _swipeUp = true;
                    _swipeDown = false;
                }
            }
            if (_endTouchPosition.y < _startTouchPosition.y && !_swipeLeft && !_swipeRight)
            {
                _swipeUp = false;
                _swipeDown = false;
                _currentTouchDistance = _startTouchPosition.y - _endTouchPosition.y;
                if (_currentTouchDistance > swipeDistance)
                {
                    _swipeUp = false;
                    _swipeDown = true;
                }
            }
        }

        if (_swipeDown || _swipeLeft || _swipeRight || _swipeUp)
            swipeStatusText.text = $"Left : {_swipeLeft} : Right : {_swipeRight} : Up : {_swipeUp} : Down : {_swipeDown}";

        /* if(Input.GetMouseButton(0))
            {
                float center = Screen.width / 2;
                float xPos = (Input.mousePosition.x - center) / center;
                moveX = Mathf.Clamp(xPos * dodgeSpeed, -dodgeSpeed, dodgeSpeed);
            }*/

        //Uncomment this 4 lines to run with arrow keys.
        //_swipeLeft = Input.GetKeyDown(KeyCode.LeftArrow);
        //_swipeRight = Input.GetKeyDown(KeyCode.RightArrow);
        //_swipeUp = Input.GetKeyDown(KeyCode.UpArrow);
        //_swipeDown = Input.GetKeyDown(KeyCode.DownArrow);

        if (_swipeDown || _swipeLeft || _swipeRight || _swipeUp)
            swipeStatusText.text = $"Left : {_swipeLeft} : Right : {_swipeRight} : Up : {_swipeUp} : Down : {_swipeDown}";

        if (InRoll || InJump)
            RollJumpStatus.text = $"Jump : {InJump} : Roll : {InRoll}";

        if (_swipeLeft && !InRoll)
        {
            RunnerLog.Log("Left Arrow");
            if(_side == Side.Mid)
            {
                newXPos = -xValue;
                _side = Side.Left;
            }
            else if(_side == Side.Right)
            {
                newXPos = 0;
                _side = Side.Mid;
                _swipeLeft = false;
            }
        }
        else if(_swipeRight && !InRoll)
        {
            RunnerLog.Log("Right Arrow");
            if (_side == Side.Mid)
            {
                newXPos = xValue;
                _side = Side.Right;
            }
            else if(_side == Side.Left)
            {
                newXPos = 0;
                _side = Side.Mid;
                _swipeRight = false;
            }
        }
        moveVector = new Vector3(moveX - transform.position.x, moveY * Time.deltaTime, playerSpeed * Time.deltaTime);
        moveX = Mathf.Lerp(moveX, newXPos, dodgeSpeed * Time.deltaTime) ;

        controller.Move(moveVector);
        Jump();
        Roll();
        
        _currentCoveredTime += Time.deltaTime;

        if (_currentCoveredTime >= distanceCoveredDelay && !_isPlayerDied)
        {
            _currentCoveredTime = 0;
            float distance = (startingPoint.position - transform.position).magnitude;
            NotifyObserver(new GameData() { DistanceCovered =  (int) distance});
        }

        if (transform.position.y < -2)
        {
            _isPlayerDied = true;
           StartCoroutine(Restart());
        }
    }

    private void Jump()
    {
        if(controller.isGrounded)
        {
            if (_swipeUp)
            {
                moveY = jumpHeight;
                anim.Play("Jump");
                InJump = true;
            }
        }
        else
        {
            moveY -= jumpHeight * 2 * Time.deltaTime;
            _swipeUp = false;
            if (controller.velocity.y < 0.1f)
                InJump = false;
        }
        
    }

    internal float _rollCounter;
    private void Roll()
    {
        _rollCounter -= Time.deltaTime;
        if(_rollCounter <= 0.0f)
        {
            _rollCounter = 0;
            controller.center = new Vector3(0,_colCenterY,0);
            controller.height = _colHeight;
            InRoll = false;
        }
        if (_swipeDown)
        {
            _rollCounter = 0.2f;
            moveY -= 10f;
            _swipeDown = false;
            controller.center = new Vector3(0, _colCenterY/2, 0);
            controller.height = _colHeight/2;
            anim.Play("Roll");
            InRoll = true;
            InJump = false;
        }
    }

    private void PlayerDied()
    {
        _isPlayerDied = true;
        anim.Play("Death");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_isPlayerDied || _isGameOver) return;

        if (!_isPlayerDied)
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
//                Debug.Log("--------- Obstacle Hit --------------");
                PlayerDied();
                NotifyObserver(new GameData() { IsDamagePlayer = true });
            }
        }
    }


    private IEnumerator Restart()
    {
        _currentCoveredTime = 0;
        yield return new WaitForSeconds(0.3f);
        anim.Play("Idle");
        transform.position = startingPoint.position;
        yield return new WaitForSeconds(2.0f);
        _isPlayerDied = _isGameOver;
    }


    private void OnEnable()
    {
        uiSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        uiSubject.RemoveObserver(this);
    }

}
