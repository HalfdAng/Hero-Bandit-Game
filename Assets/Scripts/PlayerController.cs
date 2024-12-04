using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float AirMoveSpeed;

    public float GroundDrag;
    public float AirDrag;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatIsGround;

    private bool _isGrounded;

    [Header("Ground & Wall Checkers")]
    public RaycastScript bottomCollider;
    public RaycastScript rightCollider;
    public RaycastScript leftCollider;

    [Header("Jumping")]
    public float jumpForce = 10f;
    private bool _jumpInitiated = false;
    private float _lastSpeedBeforeTakeoff;

    [Header("Sliding")]
    public float slideSpeedThreshold = 6f; // Minimum speed needed to begin sliding
    public float addedSlideSpeed = 3f; // Adding flat speed + also add a percentage of horizontal speed up to 66% of base speed
    public float slideSpeedDampening = 0.99f; // The speed will be multiplied by this every frame (to stop in ~3 seconds)
    public float keepSlidingSpeedThreshold = 3f; // As long as speed is above this, keep sliding
    public float slideSteeringPower = 0.5f; // How much you can steer around while sliding

    private bool _isSliding = false;
    private bool _slideInitiated = false;

    [Header("Wall Running")]
    public bool fallWhileWallRunning; // Slowly fall character while wall running
    public float keepWallRunningSpeedThreshold = 3f; // If speed drops below this, stop wall running
    public Transform playerCameraZRotator; // To be able to rotate the camera on the Z axis without affecting other rotations
    private float _wallRunStartingSpeed; // The speed that you begin wall running with, will be maintained while you keep wall running

    private bool _isWallRunning = false;
    private bool _onRightWall = false;

    private Vector3 _lastPosition;
    [HideInInspector] public Vector3 displacement;

    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _jumpInitiated = true;
        if (Input.GetKeyDown(KeyCode.LeftShift)) _slideInitiated = true;

        if (_isGrounded)
        {
            _rigidbody.linearDamping = GroundDrag;
        }
        else
        {
            _rigidbody.linearDamping = AirDrag;
        }

        if (Input.GetKeyDown(KeyCode.Space)) _jumpInitiated = true;

    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        SetIsGrounded(bottomCollider.IsColliding);
    }

    private void Move()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // calculate movement direction
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (_isGrounded)
        {
            _rigidbody.AddForce(_moveDirection.normalized * MoveSpeed * 10);
        }
        else
        {
            _rigidbody.AddForce(_moveDirection.normalized * AirMoveSpeed * 10);
        }
    }

    private void Jump()
    {
        if (_jumpInitiated)
        {
            _jumpInitiated = false;

            if (!_isGrounded) return;

            _rigidbody.linearVelocity += Vector3.up * jumpForce;
        }
    }


    private void SetIsGrounded(bool state)
    {
        _isGrounded = state;
        if (!_isGrounded && _isSliding) StopSliding();
    }

    #region Sliding

    void Slide()
    {
        if (_slideInitiated)
        {
            if (!_isGrounded) return; // Don't cancel the state to slide as soon as you land

            // -- INITIATE --
            _slideInitiated = false;

            // TODO If going backwards, or not moving, dont slide (not moving handled by speed threshold)

            // If already sliding... return;
            if (_isSliding) return;

            // Can only slide if the "horizontal" speed (X & Z) is above a threshold
            float horizontalSpeed = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z).magnitude;
            if (horizontalSpeed < slideSpeedThreshold) return;

            StartSliding();
        }

        // While sliding, slowly lose momentum until it ends
        if (_isSliding)
        {
            // Dampen the speed
            Vector3 newVelocity = _rigidbody.linearVelocity * slideSpeedDampening;

            // If the speed is still above the threshold, keep sliding
            if (newVelocity.magnitude > keepSlidingSpeedThreshold) _rigidbody.linearVelocity = newVelocity;
            else StopSliding();
        }
    }

    void StartSliding()
    {
        _slideInitiated = false;
        SetIsSliding(true);

        // Add bonus speed
        float currSpeedModifier = Mathf.Clamp(_rigidbody.linearVelocity.magnitude / 40, 0, 1); // Maximum speed at 20
        float boost = addedSlideSpeed + Mathf.Lerp(0, addedSlideSpeed * 2f, currSpeedModifier); // Boost the speed (up to 40% more speed with Y speed)

        // Get direction, get speed, amplify speed, apply
        Vector3 direction = _rigidbody.linearVelocity.normalized;

        //? Using displacement.magnitude as the "base" speed, so if player runs into wall etc. momentum resets
        _rigidbody.linearVelocity = direction * (displacement.magnitude + boost);

        // Debug.Log($"SLIDE [START] (boost: {boost}, y speed: {Mathf.Abs(rb.velocity.y)} ");
    }

    void StopSliding()
    {
        SetIsSliding(false);

        // Debug.Log("SLIDE [END]");
    }

    void SetIsSliding(bool state)
    {
        _isSliding = state;
    }

    #endregion
}
