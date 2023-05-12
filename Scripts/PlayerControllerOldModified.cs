using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerOldModified : MonoBehaviour
{
    [field: SerializeField] public float MoveForce {get; private set;} = 5f;
    [field: SerializeField] public PlayerState Idle {get; private set;}
    [field: SerializeField] public PlayerState Walk {get; private set;}
    [field: SerializeField] public PlayerState Act {get; private set;}
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations {get; private set;}
    [field: SerializeField] public float WalkVelocityThreshold { get; private set; } = 0.05f;
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
    private PlayerState _currentState;
    private AnimationClip _currentClip;
    private Vector2 _facingDir;
    private float _timeToEndAnimation = 0f;
    public int netAnim = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        CurrentState = Idle;
    }

    public void NetUpdate()
    {
        _timeToEndAnimation = Mathf.Max(_timeToEndAnimation - Time.deltaTime, 0);
        if(CurrentState.CanExitWhilePlaying || _timeToEndAnimation <= 0)
        {
            // Switch between Walk and Idle
            if (_axisInput != Vector2.zero && _rb.velocity.magnitude > WalkVelocityThreshold)
            {
                CurrentState = Walk;
                netAnim = 1;
            }
            else
            {
                CurrentState = Idle;
                netAnim = 0;
            }

            ChangeClip();
        }

    }

    public void PropagateNetworkAnim(int val, float x, float y) {
        _facingDir = new Vector2(x, y);
        if(val == 0) {
            CurrentState = Idle;
        }
        if(val == 1) {
            CurrentState = Walk;
        }
        ChangeClip();
    }

    public void PropagateNetworkDirection(float x, float y) {
        _facingDir = new Vector2(x, y);
        ChangeClip();
    }

    public float GetXIn() {
        return _axisInput.x;
    }

    public float GetYIn() {
        return _axisInput.y;
    }

    public int GetNetworkAnim() {
        return netAnim;
    }

    // Need to propagate the state in a more primitive variable, and on update, other hosts will change state and call changeclip()

    private void ChangeClip()
    {
        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(_currentState, _facingDir);

        if (_currentClip == null || _currentClip != expectedClip)
        {
            _animator.Play(expectedClip.name);
            _currentClip = expectedClip;
        }
    }

    public void NetFixedUpdate()
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
        CurrentState = Act;
    }
}
