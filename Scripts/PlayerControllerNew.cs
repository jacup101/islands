using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerNew : MonoBehaviour
{
    [field: SerializeField] public float MoveForce {get; private set;} = 5f;
    [field: SerializeField] public PlayerState Idle {get; private set;}
    [field: SerializeField] public PlayerState Walk {get; private set;}
    [field: SerializeField] public PlayerState Act {get; private set;}
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations {get; private set;}
    [field: SerializeField] public float WalkVelocityThreshold { get; private set; } = 0.05f;
    private Harvesting _targetHarvesting;
    private BuildBridge _buildBridge;
    public PlayerState CurrentState 
    {
        get
        {
            return _currentState;
        }
        private set
        {
            if(_currentState != value){
                _currentState = value;
                ChangeClip();
                _timeToEndAnimation = _currentClip.length;
            }
            
        }
    }

    private Vector2 _axisInput = Vector2.zero;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Stamina _staminaController;
    private PlayerState _currentState;
    private AnimationClip _currentClip;
    private Vector2 _facingDir;
    private float _timeToEndAnimation = 0f;
    private float _currentStamina;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _targetHarvesting = GetComponentInChildren<Harvesting>();
        _buildBridge = GetComponentInChildren<BuildBridge>();
        _staminaController = GetComponentInChildren<Stamina>();

        CurrentState = Idle;
    }

    private void Update()
    {
        _timeToEndAnimation = Mathf.Max(_timeToEndAnimation - Time.deltaTime, 0);
        if(CurrentState.CanExitWhilePlaying || _timeToEndAnimation <= 0)
        {
            // Switch between Walk and Idle
            if (_axisInput != Vector2.zero && _rb.velocity.magnitude > WalkVelocityThreshold)
            {
                CurrentState = Walk;
            }
            else
            {
                CurrentState = Idle;
            }

            ChangeClip();
        }

    }

    private void ChangeClip()
    {
        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(_currentState, _facingDir);

        if (_currentClip == null || _currentClip != expectedClip)
        {
            _animator.Play(expectedClip.name);
            _currentClip = expectedClip;
        }
    }

    private void FixedUpdate()
    {
        if(_currentState.CanMove){
            Vector2 moveForce = _axisInput * MoveForce * Time.fixedDeltaTime;
            _rb.AddForce(moveForce);
        }
        
    }

    private void OnMove(InputValue value)
    {
        _axisInput = value.Get<Vector2>();

        if(_axisInput != Vector2.zero)
        {
            _facingDir = _axisInput;
        }
    }

    private void OnAct(InputValue value)
    {   
        if (_staminaController.canAct())
        {
            _staminaController.isActing = true;
            if(_targetHarvesting.EquippedTool.name == "Tool_Bridge_Hammer")
            {
                _buildBridge._facingDir = _facingDir;
                _buildBridge.buildMode = true;
            }
            CurrentState = Act;
        }

    }
}
