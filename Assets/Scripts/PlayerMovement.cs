using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float AirMoveSpeed;

    public float GroundDrag;
    public float AirDrag;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatIsGround;

    private bool _grounded;

    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        _grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround);

        if (_grounded)
        {
            _rigidbody.linearDamping = GroundDrag;
        }
        else
        {
            _rigidbody.linearDamping = AirDrag;
        }
    
            
     }

    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // calculate movement direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
        
        if (_grounded)
        {
            _rigidbody.AddForce(_moveDirection.normalized * MoveSpeed * 10);
        }
        else
        {
            _rigidbody.AddForce(_moveDirection.normalized * AirMoveSpeed * 10);
        }
    }
}
