using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private ParticleSystem _dashParticle;
    private Rigidbody2D _rb;
    private PlayerData _data;
    private GroundDetector _gd;
    private PlayerAbilities _abilities;
    private PlayerText _playerText;

    public ShadowController _shadowController;
    public bool lockShadow = false;
    public Vector3 latestPosition;
    private PlayerLife _playerLife;

    private bool _isMovable = true;
    private bool Movable => _isMovable && _playerLife.isAlive;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody2D>();
        _data = GetComponent<PlayerData>();
        _gd = GetComponent<GroundDetector>();
        _playerLife = GetComponent<PlayerLife>();
        _abilities = GetComponent<PlayerAbilities>();
        _playerText = GetComponent<PlayerText>();
    }

    private void Start()
    {
        latestPosition = transform.position;
    }

    private float _time;
    private void Update()
    {
        _time += Time.deltaTime;
        /*if (!Movable)
        {
            Time.timeScale = 0;
            return;
        }
        else
        {
            Time.timeScale = 1;
        }*/
        ProcessInput();
    }

    private bool _jumpToConsume;
    private float _jumpPressedTime;

    private void ProcessInput()
    {
        if (_input.frameInput.jump)
        {
            _jumpToConsume = true;
            _jumpPressedTime = _time;
        }

        if (_input.frameInput.lockShadow)
        {
            lockShadow = !lockShadow;
        }

        if (_input.frameInput.swap)
        {
            _swapToConsume = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _isMovable = false;
            _shadowController.transform.DOMove(transform.position + _playerLife.shadowOffset, 0.5f).OnComplete(()=>_isMovable = true);
            latestPosition = transform.position;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        Move();
        ProcessJump();
        ApplyGravity();
        UpdateShadowPosition();
        ProcessSwap();
    }

    private void UpdateShadowPosition()
    {
        if (!Movable) return;
        if (!lockShadow) _shadowController.MoveAlong(transform.position - latestPosition);
        latestPosition = transform.position;
    }

    private bool _swapToConsume;
    private bool _canSwap = true;
    private void ProcessSwap()
    {
        if (!_canSwap) return;
        if (!_swapToConsume) return;
        _canSwap = false;
        if (!Movable) return;
        StartCoroutine(SwapCo());
    }

    private IEnumerator SwapCo()
    {
        _swapToConsume = false;
        _gravityCanChange = false;
        _isMovable = false;
        
        (transform.position, _shadowController.transform.position) = (_shadowController.transform.position, transform.position);
        
        yield return new WaitForSecondsRealtime(0f);
        
        _gravityCanChange = true;
        _isMovable = true;
        _canSwap = true;
        latestPosition = transform.position;
    }
    private bool _gravityCanChange = true;

    private float _moveTextTime;
    private void Move()
    {
        if (!Movable) return;
        float speed = _grounded ? _data.GroundSpeed : _data.AirSpeed;
        float acceleration = _grounded ? _data.GroundAcceleration : _data.AirAccerlation;
        if (!_abilities.canGoLeft && _input.frameInput.move.x < 0)
        {
            if (_time - _moveTextTime > 1f)
            {
                _playerText.FloatText("Cannot go left");
                _moveTextTime = _time;
            }
            return;
        }

        if (!_abilities.canGoRight && _input.frameInput.move.x > 0)
        {
            if (_time - _moveTextTime > 1f)
            {
                _playerText.FloatText("Cannot go right");
                _moveTextTime = _time;
            }
            return;
        }
        _rb.linearVelocity = new Vector2(Mathf.MoveTowards(_rb.linearVelocity.x,_input.frameInput.move.x * speed,acceleration), _rb.linearVelocity.y);
        FaceDirection();
    }

    private void FaceDirection()
    {
        if (_rb.linearVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
        else if (_rb.linearVelocity.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void ApplyGravity()
    {
        if (!_gravityCanChange) return;
        _rb.gravityScale = _rb.linearVelocity.y < 0 ?  _data.GravityScale * _data.FallMultiplier : _data.GravityScale;
        if (_rb.linearVelocity.y < -_data.maxFallSpeed)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, -_data.maxFallSpeed);
        }
    }

    private bool _grounded;
    private float _leftGroundTime;
    private bool _canJump; // turns false when jump, turns true when landing ground
    private void CheckCollisions()
    {
        // Land to ground
        if (!_grounded && _gd.isGrounded)
        {
            _grounded = true;
            _canJump = true;
        }

        // Leave ground
        else if (_grounded && !_gd.isGrounded)
        {
            _grounded = false;
            _leftGroundTime = _time;
        }

    }

    private void ProcessJump()
    {
        if (!_jumpToConsume)
            return;
        // out of jump buffer time
        if (_grounded && _time - _jumpPressedTime > _data.JumpBuffer)
            return;
        // out of coyote time
        if (!_grounded && _time - _leftGroundTime > _data.CoyoteTime)
            return;
        // prevent double jump during coyote time
        if (!_grounded && !_canJump)
            return;
        ExecuteJump();

    }

    private void ExecuteJump()
    {
        _jumpToConsume = false;
        if (!_abilities.canJump)
        {
            _playerText.FloatText("Cannot jump");
            return;
        }
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _data.JumpPower);
        _canJump = false;
    }
}

