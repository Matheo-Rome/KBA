using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TreeEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float swingSpeed;


    [SerializeField] private float groundDrag;
    
    [Header("Ground Check")]
    [SerializeField] private float playerHeigt;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Slope Handling")] 
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    
    
    public bool grounded;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchYScale;
    private float startYScale;
    
    [Header("Keybinds")]
    //[SerializeField] private KeyCode jumpKey = KeyCode.Space;
    //[SerializeField] private KeyCode sprintKey = KeyCode.RightAlt;
    //[SerializeField] private KeyCode crouchKey = KeyCode.Mouse2;
    
    [Header("Orientation")] [SerializeField] private Transform orientation;

    [Header("Camera")] [SerializeField] private PlayerCam pc;
    [SerializeField] private float grappleFOV = 95f;

    [Header("Grappling")]
    [SerializeField] private Grappling gp;
    [SerializeField] private Swing sw;
    
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    [SerializeField] private MovementState state;
    public enum MovementState
    {
        WALKING,
        SWINGING,
        SPRINTING,
        CROUCHING,
        AIR,
    }
    
    public bool freeze;
    public bool activeGrapple;
    public bool swinging;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeigt * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded && !activeGrapple)
        {
            rb.drag = groundDrag;
            if (sw.fuel < sw.maxfuel)
                sw.fuel+=6;
        }
        else
        {
            if (sw.joint == null && sw.fuel < sw.maxfuel)
                sw.fuel += 4;
            rb.drag = 0;
        }
        sw.IncrementFuel();
        
        
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime) ;
        }

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump
        if (Input.GetButtonDown("Jump") && readyToJump && (grounded || gp.IsGrappling))
        {
            var old = jumpForce;
            if (gp.IsGrappling)
            {
                ResetRestriction();
                // jumpForce *= 2;
            }

            Jump();
            jumpForce = old;
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        //Crouch
        if (Input.GetButton("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        //Stop Crouch
        if (!Input.GetButton("Crouch") && !Physics.BoxCast(transform.position, transform.localScale / 2, Vector3.up, Quaternion.identity, playerHeigt + 0.2f))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        if (Input.GetButton("Crouch"))
        {
            state = MovementState.CROUCHING;
            moveSpeed = crouchSpeed;
        }
        if (grounded && Input.GetButton("Sprint"))
        {
            state = MovementState.SPRINTING;
            moveSpeed = sprintSpeed;
        }
        else if (swinging)
        {
            state = MovementState.SWINGING;
            moveSpeed = swingSpeed;
        }
        else if (grounded)
        {
            state = MovementState.WALKING;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.AIR;
        }
    }

    private void MovePlayer()
    {
        rb.useGravity = !OnSlope();
        
        if (activeGrapple || swinging)
            return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //On slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // On ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
        // In air
        else  if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (activeGrapple)
            return;
        //limite speed on slope
        if (OnSlope() && !exitingSlope && !swinging)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition,trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        
        Invoke(nameof(ResetRestriction), 3f);
    }

    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
        pc.DoFov(grappleFOV);
    }

    public void ResetRestriction()
    {
        activeGrapple = false;
        gp.StopGrapple();
        pc.DoFov(85f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
            enableMovementOnNextTouch = false;
        ResetRestriction();
        gp.StopGrapple();
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeigt * 0.5f + 0.3f) && !swinging)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacement = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);
        
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) +
                                               Mathf.Sqrt(2 * (displacement - trajectoryHeight) / gravity));
        return velocityXZ + velocityY;
    }  
    
}