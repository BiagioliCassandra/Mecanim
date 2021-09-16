using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region variables
    public float distanceToAttack = 5f;
    public int pvEnemyMax = 100;
    public ProgressBarCircle pvEnemyBar;
    private float _remainingDistance;
    private bool _spotted;
    private bool _poursuit = false;
    private bool _attack = false;
    private Transform _target;
    private Vector3 _originalPosition;
    private NavMeshAgent _agent;
    private Animator _animator;
    private int _pvEnemy;
    private int _dammageEnemy = 10;
    private int _dammagePlayer = 5;
    #endregion

    #region build in methods
    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();

        _pvEnemy = pvEnemyMax;
        pvEnemyBar.BarValue = _pvEnemy;

    }

    // Update is called once per frame
    void Update()
    {
        _remainingDistance = _agent.remainingDistance;
        if (_poursuit)
        {
            PlayerPoursuit();
        }
        UpdateAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            SpotPlayer();
        }

        if (other.gameObject.layer == 11)
        {
            EnemyTakeDammage(_dammageEnemy);
        }
    }

    private void EnemyTakeDammage(int _dammage)
    {
        _pvEnemy -= _dammage;

        pvEnemyBar.BarValue = _pvEnemy;

        if (_pvEnemy <= 0)
        {
            _target.GetComponent<PlayerController>().PlayerVictory();
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            StopPoursuit();
        }
    }
    #endregion

    #region custom methods
    void StartAttack()
    {
        _target.GetComponent<PlayerController>().TakePlayerDammage(_dammagePlayer);

    }
    void EndAttack()
    {
        Debug.Log("End attack");
    }

    void SpotPlayer()
    {
        _spotted = true;
    }
    void PlayerPoursuit()
    {
        _agent.SetDestination(_target.position);
        if(!_attack)
        {
            if(_remainingDistance < distanceToAttack)
            {
                _attack = true;
            }
        }
    }
    public void StartPoursuit()
    {
        _poursuit = true;
    }
    void StopPoursuit()
    {
        _poursuit = false;
        GoBackToBase();
    }

    void GoBackToBase()
    {
        _agent.SetDestination(_originalPosition);
    }

    void UpdateAnimation()
    {
        if(_spotted)
        {
            _spotted = false;
            _animator.SetTrigger("Spotted");
        }
        _animator.SetBool("InPoursuit", _poursuit);

        if(_remainingDistance < 0.1f && !_poursuit)
        {
            _animator.SetBool("BackToBase", true);
        }
        else
        {
            _animator.SetBool("BackToBase", false);
        }

        if(_attack)
        {
            _attack = false;
            _animator.SetTrigger("Attack");
        }
    }
    #endregion
}
