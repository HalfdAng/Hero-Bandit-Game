using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputActions _input;
    public bool CharacterActive = true;

    public AudioSource jumpController;
    public AudioClip jumpClip;
    public AudioSource slideController;
    public AudioClip slideClip;

    [Header("Death")]
    public bool isDead;

    [Header("Movement")]
    public float MoveSpeed;
    public float AirMoveSpeed;
    public float Gravity;

    public float GroundDrag;
    public float AirDrag;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask WhatIsGround;
    public float groundCheckRadius = 0.4f;
    public Vector3 groundCheckDisplacement = new Vector3(0, 0.2f, 0);

    private bool _isGrounded;

    [Header("Head Checker")]
    public RaycastScript topCollider;

    [Header("Jumping")]
    public float jumpHeight = 10f;
    public float slideJumpMultiplier;
    public float jumpCooldown = 1f;
    private bool _jumpInitiated = false;
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

    private CapsuleCollider _capsuleCollider;

    private bool _isSliding = false;
    private bool _slideInitiated = false;

    private Vector3 _lastPosition;
    [HideInInspector] public Vector3 displacement;

    public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;

    private Rigidbody _rigidbody;

    [Header("Animation")]
    public GameObject playerObject;
    private Animator _animator;

    private void Start()
    {
        _input = GetComponent<InputActions>();

        _capsuleCollider = GetComponent<CapsuleCollider>();
        _animator = playerObject.GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _rigidbody.linearVelocity.magnitude);
        if (!CharacterActive) return;

        _timeSinceLastJump += Time.deltaTime;
        _timeSinceSlideInitiation += Time.deltaTime;
        _timeSinceSlide += Time.deltaTime;

        // Is jumping?
        if (_isGrounded)
        {
            _rigidbody.linearDamping = GroundDrag;
            _animator.SetBool("Air", false);
        }
        else
        {
            _rigidbody.linearDamping = AirDrag;
            _animator.SetBool("Air", true);
        }

        // Is sliding?
        if (_isSliding)
        {
            _animator.SetBool("IsSliding", true);
        }
        else
        {
            _animator.SetBool("IsSliding", false);
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
        if (!CharacterActive) return;
        Move();
        Jump();
        Slide();

        // Adds more gravity
        _rigidbody.AddForce(new Vector3(0, -Gravity, 0));

        SetIsGrounded(Physics.CheckSphere(transform.position + groundCheckDisplacement, groundCheckRadius, WhatIsGround));

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
                _rigidbody.linearVelocity += Vector3.up * Mathf.Sqrt(2 * Gravity * jumpHeight);
                jumpController.PlayOneShot(jumpClip);
            }
            else
            {
                if (_rigidbody.linearVelocity.magnitude > 1)
                {
                    _rigidbody.linearVelocity += Vector3.up * Mathf.Sqrt(2 * Gravity * jumpHeight) * slideJumpMultiplier;
                    jumpController.PlayOneShot(jumpClip);
                }
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
                if (!_input.Slide && !topCollider.IsColliding) StopSliding();
            }
            
            else _rigidbody.linearVelocity = newVelocity;
        }
    }

    void StartSliding()
    {
        slideController.PlayOneShot(slideClip);
        _slideInitiated = false;
        _timeSinceSlideInitiation = 0;
        SetIsSliding(true);
        // Deprecated: transform.localScale = new Vector3(1f, 0.5f, 1f);
        _capsuleCollider.height = 1f;
        _capsuleCollider.center = new Vector3(0, 0.5f, 0);

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
        // Deprecated: transform.localScale = Vector3.one;
        _capsuleCollider.height = 2f;
        _capsuleCollider.center = new Vector3(0, 1, 0);
    }

    void SetIsSliding(bool state)
    {
        _isSliding = state;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + groundCheckDisplacement, groundCheckRadius);
    }
}
