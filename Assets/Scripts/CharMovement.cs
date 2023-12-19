using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CandyCoded.HapticFeedback;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;


public class CharMovement : MonoBehaviour
{
    private float _speed = 1.25f;
    private float _maxSpeed = 2.8f;
    private float _thrust = 20f;
    private float _gravityScale = 5f;
    private float _jumpResetTime = 0.5f;
    private float _downResetTime = 0.1f;

    private float _moveRotation = 15f;
    private float _moveDuration = 0.2f;
    private Rigidbody _rb;

    private int _deadCount = 0;
    private float _lastSpeed = 0;
    private float _increaseOfSpeed = 0.04f;
    private int _currentLine = 1; //0:left, 1:middle, 2:right
    private int _jumpCount = 0; //For double jump
    private float _lineDistance = 8f; //distance between two line


    private bool _isMoving = false;
    private bool _isJumping = false;
    private bool _isDown = false;
    private bool _isDead = false;
    private bool _isPlayerPaused = false;
    private bool _isTouch = false;
    private bool _isStarted = false;

    Animator _anim;

    int Idle;
    int Run;


    enum animationState {  Idle, Run, Jump, Air };
    animationState _animationState = animationState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Instance.OnPlayerPaused += PlayerPaused;
        PlayerManager.Instance.OnSecondChance += SecondChance;
        PlayerManager.Instance.OnGameStart += EndStartAnm;

        ConfChar(GetComponent<Transform>().name);

        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        Idle = Animator.StringToHash("Idle");
        Run = Animator.StringToHash("Run");
        _anim.speed = 2f;
    }

    private void EndStartAnm(bool _isEnd)
    {
        _isStarted = _isEnd;
        HapticFeedback.MediumFeedback();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && _isStarted)
        {
            Touch finger = Input.GetTouch(0);

            if (finger.deltaPosition.x > 25 && finger.deltaPosition.y < 20 && finger.deltaPosition.y > -20 && !_isDead && !_isDead && !_isMoving)
            {
                _currentLine++;
                if (_currentLine >= 3)
                {
                    _currentLine = 2;
                }
                else
                {
                    StartCoroutine(MoveTo(transform.position.x + _lineDistance, _moveDuration, 1));
                    _isMoving = true;
                }
            }
            else if (finger.deltaPosition.x < -25 && finger.deltaPosition.y < 20 && finger.deltaPosition.y > -20 && !_isDead && !_isMoving)
            {
                _currentLine--;
                if (_currentLine <= -1)
                {
                    _currentLine = 0;
                }
                else
                {
                    StartCoroutine(MoveTo(transform.position.x - _lineDistance, _moveDuration, -1));
                    _isMoving = true;
                }

            }

            if (finger.deltaPosition.y > 25 && finger.deltaPosition.x > -20 && finger.deltaPosition.x < 20 && !_isDead && !_isJumping && !_isTouch)
            {
                JumpAction();
                _isTouch = true;
                StartCoroutine(JumpReset());
            }

            if (finger.deltaPosition.y < -25 && finger.deltaPosition.x > -20 && finger.deltaPosition.x < 20 && !_isDead && !_isDown)
            {
                DownAction();
                _isDown = true;
                StartCoroutine(DownReset());
            }
        }

        //if (Input.GetKeyDown(KeyCode.RightArrow) && !_isDead && !_isMoving && _isStarted)
        //{
        //    _currentLine++;
        //    if (_currentLine >= 3)
        //    {
        //        _currentLine = 2;
        //    }
        //    else
        //    {
        //        StartCoroutine(MoveTo(transform.position.x + _lineDistance, _moveDuration, 1));
        //        _isMoving = true;
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow) && !_isDead && !_isMoving && _isStarted)
        //{
        //    _currentLine--;
        //    if (_currentLine <= -1)
        //    {
        //        _currentLine = 0;
        //    }
        //    else
        //    {
        //        StartCoroutine(MoveTo(transform.position.x - _lineDistance, _moveDuration, -1));
        //        _isMoving = true;
        //    }

        //}

        //if (Input.GetKeyDown(KeyCode.UpArrow) && !_isDead && !_isJumping && !_isTouch && _isStarted)
        //{
        //    JumpAction();
        //    _isTouch = true;
        //    StartCoroutine(JumpReset());
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow) && !_isDead && _isStarted)
        //{
        //    DownAction();
        //}


        if (transform.position.y <= 0.05f && !_isDead && _isStarted)
        {
            _jumpCount = 0;
            _isJumping = false;
            _animationState = animationState.Run;
        }

    }

    private IEnumerator JumpReset()
    {
        yield return new WaitForSeconds(_jumpResetTime);
        _isTouch = false;

    }

    private IEnumerator DownReset()
    {
        yield return new WaitForSeconds(_downResetTime);
        _isDown = false;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration);

        if (!_isDead && !_isPlayerPaused && _isStarted)
        {
            transform.position += Vector3.forward * _speed;

            //zamanla hýzý ve hareket kabiliyetini arttýr
            if(_speed < _maxSpeed)
            {
                _speed += _increaseOfSpeed * Time.deltaTime;
                _moveDuration = 0.25f / _speed;
            }
            //Debug.Log(Speed);
        }

        switch (_animationState)
        {
            case animationState.Idle:
                _anim.SetBool(Idle, true);
                _anim.SetBool(Run, false);
                break;

            case animationState.Run:
                _anim.SetBool(Idle, false);
                _anim.SetBool(Run, true);
                break;

            case animationState.Jump:
                _anim.SetBool(Idle, false);
                _anim.SetBool(Run, false);
                _anim.SetTrigger("Jump");
                _animationState = animationState.Air;
                break;

            case animationState.Air:
                _anim.SetBool(Idle, false);
                _anim.SetBool(Run, false);
                break;
        }
    }

    private void JumpAction()
    {
        _jumpCount++;

        // Zýplama hareketini belirlediðiniz süre boyunca sürdürmek için bir bekleme iþlemi ekleyin
        if (_jumpCount >= 2) 
        {
            _isJumping = true;
            StartCoroutine(EndJump(2f));
        }
        else
        {
            _rb.AddForce(transform.up * _thrust, ForceMode.Impulse);
            _animationState = animationState.Jump;
            HapticFeedback.MediumFeedback();
        }

    }

    private IEnumerator EndJump(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isJumping = false;
        _jumpCount = 0;
    }

    private void DownAction()
    {
        _rb.AddForce(transform.up * -40, ForceMode.Impulse);
        HapticFeedback.MediumFeedback();
    }

    public IEnumerator MoveTo(float position, float duration, int direction)
    {
        //.SetEase(Ease.Linear)
        transform.Rotate(0, _moveRotation * direction, 0);
        transform.DOMoveX(position, duration);

        yield return new WaitForSeconds(duration);

        _isMoving = false;
        transform.Rotate(0, _moveRotation * direction * -1, 0);

    }

    private void Dead()
    {
        _lastSpeed = _speed;
        _isDead = true;
        _speed = 0;
        _animationState = animationState.Idle;
        _deadCount++;

        if( _deadCount >= 1 )
        {
            PlayerManager.Instance.SetPlayerDead(true);
        }
        else
        {
            PlayerManager.Instance.DeadBeforeSecondChance();
        }
    }

    private void SecondChance(bool isOk)
    {
        transform.position += Vector3.forward * 20;
        if(_currentLine == 0)
        {
            transform.position = new Vector3(_lineDistance * -1, transform.position.y, transform.position.z) ;
        }
        else if(_currentLine == 1)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(_lineDistance, transform.position.y, transform.position.z);
        }
        _isDead = false;

        if(_lastSpeed <= _maxSpeed * 0.8f)
        {
            _speed = _lastSpeed * 0.9f;
        }
        else
        {
            _speed = _maxSpeed * 0.8f;
            _increaseOfSpeed *= 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obstacle")
        {
            if (!_isDead)
            {
                HapticFeedback.HeavyFeedback();
                Dead();
            }
        }
    }

    private void PlayerPaused(bool _isPaused)
    {
        _isPlayerPaused = _isPaused;
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnPlayerPaused -= PlayerPaused;
        PlayerManager.Instance.OnSecondChance -= SecondChance;
        PlayerManager.Instance.OnGameStart -= EndStartAnm;
    }

    private void ConfChar(string selectedChar)
    {
        switch (selectedChar)
        {
            case "Koala(Clone)":
                //ek özellik yok
                break;
            case "Fox(Clone)":
                //daha yükseðe zýplar, 2 kere zýplamak için aradaki süre arttýrýldý, çok yükseðe zýplamamasý için
                _thrust = 35f;
                _jumpResetTime = 1f;
                break;
            case "Bear(Clone)":
                //1 kere yeniden doðma þansý var
                _deadCount = -1;
                break;
            case "Badger(Clone)":
                //etrafýndaki altýnlarý da toplar
                break;
            case "Porky(Clone)":
                
                break;
            case "Fish(Clone)":
                //score deðeri daha yüksek olur
                break;

        }
    }
}
