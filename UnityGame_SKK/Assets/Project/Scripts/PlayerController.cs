using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.MainActions input;
    CharacterController controller;
    Animator animator;
    AudioSource audioSource;

    [Header("Controller")]
    public float moveSpeed = 5;
    public float gravity = -9.8f;
    public float jumpHeight = 1.2f;
    public float originalHeight;
    public float crouchHeight = 1.0f;

    Vector3 _PlayerVelocity;

    bool isGrounded;

    [Header("Sprint")]
    public float sprintSpeed = 8f;
    public float sprintDelay = 0.1f;
    private bool isSprinting = false;

    [Header("crouch")]
    public float crouchSpeed = 2f;
    public float slideSpeed = 10f;
    public float slideDuration = 1.0f;
    private bool isCrouching = false;
    private bool isSliding = false;

    [Header("Camera")]
    public Camera cam;
    public float sensitivity;
    float xRotation = 0f;

    [Header("Camera Bobbing")]
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;

    private float bobTimer = 0f;
    private Vector3 camOriginalLocalPos;
    private Vector3 camCrouchLocalPos;

    [Header("Crouch Camera")]
    public float crouchCamHeightOffset = -0.5f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        playerInput = new PlayerInput();
        input = playerInput.Main;
        AssignInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camOriginalLocalPos = cam.transform.localPosition;
        camCrouchLocalPos = camOriginalLocalPos + new Vector3(0, -0.5f, 0);

        originalHeight = controller.height;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        Vector2 moveInput = input.Movement.ReadValue<Vector2>();
        bool isMovingForward = moveInput.y > 0.1f;
        bool sprintHeld = input.Sprint.IsPressed();
        bool crouchHeld = input.Crouch.IsPressed();
        isSprinting = sprintHeld && isMovingForward && !isCrouching && !isSliding;

        if (isSprinting && crouchHeld && isGrounded && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if (!isSliding)
        {
            isCrouching = crouchHeld;
            controller.height = isCrouching ? crouchHeight : originalHeight;
            controller.center = new Vector3(0, controller.height / 2, 0);
        }
        
        // Repeat Inputs
        if (input.Attack.IsPressed())
        { Attack(); }

        SetAnimations();
        CameraBob();
    }

    IEnumerator Slide()
    {
        isSliding = true;
        float time = 0f;
        Vector2 moveInput = input.Movement.ReadValue<Vector2>();
        Vector3 moveDrection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 direction = transform.TransformDirection(moveDrection.normalized);

        while (time < slideDuration)
        {
            float t = time / slideDuration;
            float currentSpeed = Mathf.Lerp(slideSpeed, 0, t);
            controller.Move(direction * currentSpeed * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }

        isSliding = false;
        isSprinting = false;
        isCrouching = true;
    }

    void FixedUpdate() 
    { MoveInput(input.Movement.ReadValue<Vector2>()); }

    void LateUpdate() 
    { LookInput(input.Look.ReadValue<Vector2>()); }

    void MoveInput(Vector2 input)
    {
        if (isSliding) return;
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : moveSpeed);

        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
        _PlayerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && _PlayerVelocity.y < 0)
            _PlayerVelocity.y = -2f;
        controller.Move(_PlayerVelocity * Time.deltaTime);
    }

    void LookInput(Vector3 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime * sensitivity);
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * sensitivity));
    }

    void OnEnable() 
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }

    void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
            _PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    }
    void AssignInputs()
    {
        input.Jump.performed += ctx => Jump();
        input.Attack.started += ctx => Attack();
    }

    // ---------- //
    // ANIMATIONS //
    // ---------- //

    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    string currentAnimationState;

    public void ChangeAnimationState(string newState) 
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    void SetAnimations()
    {
        // If player is not attacking
        if(!attacking)
        {
            if(_PlayerVelocity.x == 0 &&_PlayerVelocity.z == 0)
            { ChangeAnimationState(IDLE); }
            else
            { ChangeAnimationState(WALK); }
        }
    }

    void CameraBob()
    {
        Vector2 move = input.Movement.ReadValue<Vector2>();
        
        Vector3 baseCamPos = (isCrouching || isSliding) ? camCrouchLocalPos : camOriginalLocalPos;

        if (move.magnitude > 0.1f && isGrounded)
        {
            float speedMultiplier = isSprinting ? 2.0f : 1.0f;
            float verticalAmount = isSprinting ? bobAmount * 2.0f : bobAmount * 1.0f;
            float horizontalAmount = isSprinting ? bobAmount * 1.8f : bobAmount * 0.5f;

            bobTimer += Time.deltaTime * bobSpeed * speedMultiplier;

            float bobOffsetY = Mathf.Sin(bobTimer) * verticalAmount;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * horizontalAmount;

            Vector3 targetPos = baseCamPos + new Vector3(bobOffsetX, bobOffsetY, 0);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,targetPos,Time.deltaTime * bobSpeed);
        }
        else
        {
            bobTimer = 0;

            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,baseCamPos,Time.deltaTime * bobSpeed);
        }
    }

    // ------------------- //
    // ATTACKING BEHAVIOUR //
    // ------------------- //

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public void Attack()
    {
        if(!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);

        if(attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        { 
            HitTarget(hit.point);

            if(hit.transform.TryGetComponent<Actor>(out Actor T))
            { T.TakeDamage(attackDamage); }
        } 
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}