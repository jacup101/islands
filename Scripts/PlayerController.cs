using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
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
    [SerializeField] private PlayerState _currentState;
    private AnimationClip _currentClip;
    private Vector2 _facingDir;
    private float _timeToEndAnimation = 0f;
    public int netAnim = 0;
    
    public int netDirection = 0;
    public bool willAct = false;

    private Harvesting _targetHarvesting;
    private BuildBridge _buildBridge;
    private Stamina _staminaController;
    [SerializeField] CombatHitbox ch;
    [SerializeField] GameObject healthSlider;
    [SerializeField] public int max_health = 100;
    [SerializeField] public int health = 100;
    [SerializeField] public Button b1;
    [SerializeField] public Button b2;

    [SerializeField] public Button b3;

    [SerializeField] public Button b4;
    [SerializeField] public GameObject invMenu;
    [SerializeField] public bool is_client = false;
    [SerializeField] public bool can_move = false;
    [SerializeField] public AudioSource use_item;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _targetHarvesting = GetComponentInChildren<Harvesting>();
        _buildBridge = GetComponentInChildren<BuildBridge>();
        _staminaController = GetComponent<Stamina>();
        CurrentState = Idle;
    }

    public void NetUpdate()
    {
        if (!can_move) {
            return;
        }
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
            if(willAct) {

                willAct = false;
                if(_targetHarvesting.EquippedTool == null) {
                    return;
                }
                
                if (_staminaController.canAct() && CurrentState != Act)
                {
                    use_item.Play();
                    _staminaController.isActing = true; 
                    if(_targetHarvesting.EquippedTool.name == "Tool_Bridge_Hammer")
                    {
                        _buildBridge._facingDir = _facingDir;
                        _buildBridge.buildMode = true;
                    }
                    if(_targetHarvesting.EquippedTool.name.Contains("Tool_Weapon"))
                    {
                        ch.DoDamage();
                    }
                    CurrentState = Act;
                    netAnim = 2;
                }
            }
            
            ChangeClip();
        }
    }

    public string PropagateNetworkAnim(int val, float x, float y) {
        _facingDir = new Vector2(x, y);
        if(val == 0) {
            CurrentState = Idle;
        }
        if(val == 1) {
            CurrentState = Walk;
        }
        if(val == 2) {
            CurrentState = Act;
        }
        return ChangeClip();
    }

    public string PropagateNetworkDirection(float x, float y) {
        _facingDir = new Vector2(x, y);
        return ChangeClip();
    }

    public float GetXIn() {
        return _axisInput.x;
    }

    public float GetYIn() {
        return _axisInput.y;
    }

    public int GetNetworkDir() {
        return netDirection;
    }

    public int GetNetworkAnim() {
        return netAnim;
    }

    private string ChangeClip()
    {
        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(_currentState, _facingDir);

        if (_currentClip == null || _currentClip != expectedClip)
        {
            _animator.Play(expectedClip.name);
            _currentClip = expectedClip;
        }
        netDirection = StripDirection(expectedClip.name);
        ch.SetDirection(netDirection);

        return expectedClip.name;
    }

    public void NetFixedUpdate()
    {
        if (!can_move) {
            return;
        }
        if(_currentState.CanMove){
            Vector2 moveForce = _axisInput * MoveForce * Time.fixedDeltaTime;
            _rb.AddForce(moveForce);
        }
        
    }

    private void OnMove(InputValue value)
    {
        if (!can_move) {
            return;
        }
        _axisInput = value.Get<Vector2>();

        if(_axisInput != Vector2.zero)
        {
            _facingDir = _axisInput;
        }
    }

    private void OnAct(InputValue value)
    {   
        if (!can_move) {
            return;
        }
        // CurrentState = Act;
        willAct = true;
    }

    private int StripDirection(string input) {
        if (input.Contains("left")) {
            return 2;
        }
        if (input.Contains("right")) {
            return 3;
        }
        if (input.Contains("up")) {
            return 1;
        }
        if (input.Contains("down")) {
            return 0;
        }
        // By default, return down
        return 0;
    }

    public int Damage(int damage) {
        health -= damage;
        healthSlider.GetComponent<Slider>().value = health;
        return health;
    }

    public void SetHealth(GameObject health_slider) {
        this.healthSlider = health_slider;
    }

    public void SetButtons(Button one, Button two, Button three, Button four) {
        this.b1 = one;
        this.b2 = two;
        this.b3 = three;
        this.b4 = four;
    }

    public void OnHotbar1() {
        if (!can_move) {
            return;
        }
        if (this.b1 != null) {
            this.b1.onClick.Invoke();
        }
    }
    public void OnHotbar2() {
        if (!can_move) {
            return;
        }
        if (this.b2 != null) {
            this.b2.onClick.Invoke();
        }
    }
    public void OnHotbar3() {
        if (!can_move) {
            return;
        }
        if (this.b3 != null) {
            this.b3.onClick.Invoke();
        }
    }
    public void OnHotbar4() {
        if (!can_move) {
            return;
        }
        if (this.b4 != null) {
            this.b4.onClick.Invoke();
        }
    }

    public void SetInventoryMenu(GameObject invMen) {
        this.invMenu = invMen;
    }
    public void OnInventoryToggle() {
        if (!can_move) {
            return;
        }
        if (is_client) {
            invMenu.SetActive(!invMenu.activeSelf);
        }
    }
}
