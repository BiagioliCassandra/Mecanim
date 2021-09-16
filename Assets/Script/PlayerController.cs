using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Character Properties")]
    public ProgressBarCircle healthPlayerBar;
    public float moveSpeed = 3;
    public float coeffGravity = 0.3f;
    public float gravity = 9.81f;
    public float jumpForce = 10;
    public float turnSmoothTime = 1;
    public int pvMaxPlayer = 100;

    [Header("Weapon Properties")]
    public GameObject meleeWeapon;
    public GameObject gunWeapon;

    public bool attackState;
    public Text timer;

    private float _turnSmoothVelocity;
    private float _horizontal;
    private float _vertical;
    private float _verticalSpeed;
    private float _timer = 120;
    private int _currentPvPlayer;
    private bool _playerMove = true;

    private Vector3 _movement;
    private Vector3 _direction = Vector3.zero;
    private Camera _cam;

    private Animator _animator;
    private CharacterController _cc;
    private PlayerInputs _inputs;
    #endregion

    #region Built in Methods
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _animator = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
        _inputs = PlayerInputs.Instance;

        _currentPvPlayer = pvMaxPlayer;
        healthPlayerBar.BarValue = _currentPvPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        if(!attackState)
        {
            if(_playerMove)
            {
                Locomotion();
                VerticalMovement();
            }
        }
        UpdateAnimations();

        Timer();
    }
    #endregion

    #region Custom Methods
    void Locomotion(){
        //CHANGEMENT
        if(!_inputs) return;

        _horizontal = _inputs.Movement.x;
        _vertical = _inputs.Movement.y;

        _direction.Set(_horizontal, 0, _vertical);

        if(_direction.normalized.magnitude >= 0.1f){
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            _direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        _movement = _direction.normalized * (moveSpeed * Time.deltaTime);
    }

    void VerticalMovement(){
        if(_cc.isGrounded){
            _verticalSpeed = -gravity * coeffGravity;
            if(_inputs.Jump){
                _verticalSpeed = jumpForce;
            }
        }else{
            if(Mathf.Approximately(_verticalSpeed, 0)){
                _verticalSpeed = 0f;
            }
            
            _verticalSpeed -= gravity * Time.deltaTime;
        }
        
        _movement += _verticalSpeed * Vector3.up * Time.deltaTime;

        _cc.Move(_movement);
    }

    public void TakePlayerDammage(int dammagePlayer)
    {
        _playerMove = false;
        _currentPvPlayer -= dammagePlayer;
        healthPlayerBar.BarValue = _currentPvPlayer;

        if (_currentPvPlayer <= 0)
        {
            PlayerDeath();
        }
        _playerMove = true;

    }

    public void Timer()
    {
        _timer -= Time.deltaTime;

        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (_timer <= 0)
        {
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        SceneManager.LoadScene("MenuLooser");
    }

    public void PlayerVictory()
    {
        SceneManager.LoadScene("MenuWinner");
    }

    public void MeleeAttackStart(){
        if(meleeWeapon)
            meleeWeapon.SetActive(true);
        _playerMove = false;
    }

    public void GunAttackStart()
    {
        if (gunWeapon)
            gunWeapon.SetActive(true);
        _playerMove = false;
    }

    public void MeleeAttackEnd(){
        if(meleeWeapon)
            meleeWeapon.SetActive(false);
        _playerMove = true;
    }

    public void GunAttackEnd()
    {
        if (gunWeapon)
            gunWeapon.SetActive(false);
        _playerMove = true;
    }

    void UpdateAnimations(){
        if(!_animator) return;

        _animator.SetBool("Grounded", _cc.isGrounded);

        if(!_cc.isGrounded){
            _animator.SetFloat("VerticalSpeed", _verticalSpeed);
        }else{
            _animator.SetFloat("ComboTime", Mathf.Repeat(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1));

            if(_inputs.Attack){
                _animator.SetTrigger("Attack");
            }else{
                _animator.ResetTrigger("Attack");
            }

            if (_inputs.AttackGun)
            {
                _animator.SetTrigger("AttackGun");
            }
            else
            {
                _animator.ResetTrigger("AttackGun");
            }
        }
        _animator.SetBool("Walk", true);
        _animator.SetFloat("Velocity", _direction.magnitude);
    }
    #endregion
}
