using System.Collections;
using Data;
using ObserverPattern;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Subject, IObserver
{
    [SerializeField] private Subject uiSubject;
    [SerializeField] private PlayerAttak playerAttack;
    [SerializeField] private Transform startingPoint;
    [SerializeField] private CharacterController controller;
    [SerializeField]private float playerSpeed = 2.0f;
    [SerializeField] private float dodgeSpeed = 2.0f;
    [SerializeField] private float distanceCoveredDelay = 1.0f;
    [SerializeField] private float jumpHeight = 1.0f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    private float gravityValue = -9.81f;
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
            controller.enabled = true;
        }
        else if (data.IsGameOver)
        {
            Debug.Log("--------------- GameOver ------------");
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

    private void Start()
    {
        playerAttack.PlayerSpeed = playerSpeed;
    }

    void Update()
    {
        //Debug.Log("_isPlayerDied : " + _isPlayerDied+ " : _isGameOver : "+ _isGameOver);
        if (_isGameOver || _isPlayerDied) return;

        groundedPlayer = controller.isGrounded;
        //Debug.Log("grundedLayer : " + groundedPlayer);
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        float moveX = 0;
        if (groundedPlayer)
        {
            moveX = Input.GetAxis("Horizontal") * dodgeSpeed;
            //moveX = Mathf.Clamp(Input.GetAxis("Horizontal") * dodgeSpeed, -maxXValue, maxXValue);

            Debug.Log("Move Input : "+moveX);
            //Debug.Log("Touch COunt : "+Input.touchCount);
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _startTouchPosition = Input.GetTouch(0).position;
            }

            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                _endTouchPosition = Input.GetTouch(0).position;
                if (_endTouchPosition.x < _startTouchPosition.x)
                {
                    moveX = -1 * dodgeSpeed;
                }
                else if (_endTouchPosition.x > _startTouchPosition.x)
                {
                    moveX = 1 * dodgeSpeed;
                }
            }
        }

        Vector3 move = new Vector3(moveX, 0, playerSpeed);
        controller.Move(move * Time.deltaTime);
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        
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

    private void PlayerDied()
    {
        _isPlayerDied = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_isPlayerDied || _isGameOver) return;

        if (!_isPlayerDied)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

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
