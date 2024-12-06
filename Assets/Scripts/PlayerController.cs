using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputActions _input;

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
    public float slideJumpMultiplier;
    public float jumpCooldown = 1f;
    private bool _jumpInitiated = false;
    private float _lastSpeedBeforeTakeoff;
    private float _timeSinceLastJump = 999f;

    [Header("Sliding")]
    public float SlideCooldown;
    public float slideMinTime;
    public float slideCancelEarlySpeedThreshold;
    public float addedSlideSpeed = 3f; // Adding flat speed + also add a percentage of horizontal speed up to 66% of base speed
    public float slideSpeedDampening = 0.99f; // The speed will be multiplied by this every frame (to stop in ~3 seconds)
    public float slideSteeringPower = 0.5f; // How much you can steer around while sliding
    private float _timeSinceSlideInitiation;
    private float _timeSinceSlide = 999f;

    private bool _isSliding = false;
    private bool _slideInitiated = false;

    [Header("Wall Running")]
    public bool fallWhileWallRunning; // Slowly fall character while wall running
    public float keepWallRunningSpeedThreshold = 3f; // If speed drops below this, stop wall running
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
        _input = GetComponent<InputActions>();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        _timeSinceLastJump += Time.deltaTime;
        _timeSinceSlideInitiation += Time.deltaTime;
        _timeSinceSlide += Time.deltaTime;

        if (_isGrounded)
        {
            _rigidbody.linearDamping = GroundDrag;
        }
        else
        {
            _rigidbody.linearDamping = AirDrag;
        }

        if (_input.Jump && _timeSinceLastJump > jumpCooldown)
        {
            _jumpInitiated = true;
            _timeSinceLastJump = 0;
        }

        if (_input.Slide && !_isSliding)
        {
            _slideInitiated = true;
        }

    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Slide();

        SetIsGrounded(bottomCollider.IsColliding);

        // Calculate displacement
        displacement = (transform.position - _lastPosition) / Time.fixedDeltaTime; // Script source did "x 50" instead of "/ Time.fixedDeltaTime"
        _lastPosition = transform.position;
    }

    private void Move()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // calculate movement direction


        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (_isGrounded)
        {
            // Sliding gives you less movement capabilities
            if (!_isSliding)
            {
                _rigidbody.AddForce(_moveDirection.normalized * MoveSpeed * 10);
            }
            else
            {
                _rigidbody.AddForce(_moveDirection.normalized * MoveSpeed * 10 * slideSteeringPower);
            }
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

            // Bigger Jump while sliding
            if (!_isSliding)
            {
                _rigidbody.linearVelocity += Vector3.up * jumpForce;
            }
            else
            {
                _rigidbody.linearVelocity += Vector3.up * jumpForce * slideJumpMultiplier;
                // Add bonus speed
                StopSliding();
            }
        }
    }


    private void SetIsGrounded(bool state)
    {
        _isGrounded = state;
    }

    #region Sliding

    void Slide()
    {
        if (_slideInitiated)
        {
            if (!_isGrounded || _timeSinceSlide < SlideCooldown) return; // Don't cancel the state to slide as soon as you land

            StartSliding();
        }

        // While sliding, slowly lose momentum until it ends
        if (_isSliding)
        {
            // Dampen the speed
            Vector3 newVelocity = _rigidbody.linearVelocity * slideSpeedDampening;

            // If the speed is too high, sliding gets a little cooldown, but if not ignores the cooldown, letting the Player freely dance.
            if (_rigidbody.linearVelocity.magnitude < slideCancelEarlySpeedThreshold || _timeSinceSlideInitiation > slideMinTime)
            {
                // If Player stops holding shift the slide gets cancelled
                if (!_input.Slide) StopSliding();
            }
            
            else _rigidbody.linearVelocity = newVelocity;
        }
    }

    void StartSliding()
    {
        _slideInitiated = false;
        _timeSinceSlideInitiation = 0;
        SetIsSliding(true);
        transform.localScale = new Vector3(1f, 0.5f, 1f);

        // Add bonus speed
        float currSpeedModifier = Mathf.Clamp(_rigidbody.linearVelocity.magnitude / 40, 0, 1); // Maximum speed at 20
        float boost = addedSlideSpeed + Mathf.Lerp(0, addedSlideSpeed * 2f, currSpeedModifier); // Boost the speed (up to 40% more speed with Y speed)

        // Get direction, get speed, amplify speed, apply
        Vector3 direction = _rigidbody.linearVelocity.normalized;

        //? Using displacement.magnitude as the "base" speed, so if player runs into wall etc. momentum resets
        _rigidbody.linearVelocity = direction * (displacement.magnitude + boost);
    }

    void StopSliding()
    {
        _timeSinceSlide = 0;

        SetIsSliding(false);
        transform.localScale = Vector3.one;
    }

    void SetIsSliding(bool state)
    {
        _isSliding = state;
    }

    #endregion

    #region Wall Running

    void WallRun()
    {
        if (_jumpInitiated && !_isGrounded) // Must initiate jump, be off the ground, ...
        {
            if (_isWallRunning) // If already wall running, jump off
            {
                int directionCount = 1; // Can be up to 3 directions, magnitude can be 1, 1.25, 1.5 depending

                // Jump off the wall
                Vector3 jumpDirection = Vector3.up;

                // If holding forward, add a force forward
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    jumpDirection += transform.forward;
                    directionCount++;
                }

                // If holding the horizontal direction AWAY from the wall, add that horizontal direction as well
                if (Input.GetAxisRaw("Horizontal") < 0 && _onRightWall)
                {
                    jumpDirection += rightCollider.outHit.normal;
                    directionCount++;
                }
                else if (Input.GetAxisRaw("Horizontal") > 0 && !_onRightWall)
                {
                    jumpDirection += leftCollider.outHit.normal;
                    directionCount++;
                }

                // Normalize (otherwise you can artifically buff up speed)
                float magnitude = 1 + (directionCount - 1) * 0.25f;
                jumpDirection = jumpDirection.normalized * magnitude;

                _rigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

                StopWallRunning();
            }
            else // Check to see if you can START wall running
            {
                // ... and have a wall in contact
                if (leftCollider.IsColliding) StartWallRunning(false);
                else if (rightCollider.IsColliding) StartWallRunning(true);
            }

            _jumpInitiated = false;
        }

        if (_isWallRunning)
        {
            //? Can't remove this (what happens when flat wall ends?)
            if (_isGrounded)
            {
                StopWallRunning();
                return;
            }
            if (!leftCollider.IsColliding && !rightCollider.IsColliding)
            {
                StopWallRunning();
                return;
            }

            //*  - THE DIRECITON -

            // Which wall? where is the collider?
            _onRightWall = rightCollider.IsColliding; // temp
            var col = _onRightWall ? rightCollider : leftCollider;
            Vector3 wallNormal = col.outHit.normal;

            // Direction to travel along
            Vector3 wallForward = Vector3.Cross(
                wallNormal,
                transform.up
            );

            // Ensure the forward direction aligns with the player's orientation (aka changing direction while running along the wall)
            if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            {
                wallForward = -wallForward;
            }

            // Current negative Y velocity
            float ySpeed = _rigidbody.linearVelocity.y;

            // Apply the velocity
            _rigidbody.linearVelocity = wallForward * _wallRunStartingSpeed;

            // Threshold?
            if (_rigidbody.linearVelocity.magnitude < keepWallRunningSpeedThreshold)
            {
                StopWallRunning();
                return;
            }

            // However negative is the Y speed, half it and add it to the Y speed
            if (fallWhileWallRunning && ySpeed < 0) _rigidbody.linearVelocity += new Vector3(0, ySpeed * 0.75f, 0);

            // Add force TOWARDS the wall
            _rigidbody.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }

    void StartWallRunning(bool rightWall)
    {
        SetIsWallRunning(true);

        if (!fallWhileWallRunning) _rigidbody.useGravity = false;

        _wallRunStartingSpeed = _rigidbody.linearVelocity.magnitude;

        Debug.Log($"WALL RUN [START] (spd: {_wallRunStartingSpeed})");
    }

    void StopWallRunning()
    {
        SetIsWallRunning(false);

        if (!fallWhileWallRunning) _rigidbody.useGravity = true;

        Debug.Log("WALL RUN [STOP]");
    }

    void SetIsWallRunning(bool state)
    {
        _isWallRunning = state;
    }

    #endregion
}
