using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    #region Variables
    private static PlayerInputs _instance;
    private Vector2 _movement = Vector2.zero;
    private bool _jump = false;
    private bool _attack = false;
    private bool _attackGun = false;
    #endregion

    #region Proprietes
    public static PlayerInputs Instance => _instance;
    public Vector2 Movement => _movement;
    public bool Jump{
        get{
            return _jump;
        }
    }
    public bool Attack{
        get{
            return _attack;
        }
    }

    public bool AttackGun
    {
        get
        {
            return _attackGun;
        }
    }
    #endregion

    #region Built in Methods
    void Awake(){
        if(_instance != null){
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        _movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _jump = Input.GetButton("Jump");
        _attack = Input.GetButton("Fire1");
        _attackGun = Input.GetButton("Fire2");
    }
    #endregion
}
