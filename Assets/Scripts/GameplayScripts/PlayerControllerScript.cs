using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;
    [SerializeField] Transform cameraPosition;

    [Header("Movement")]
    public float movementSpeed = 6f;
    public float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Ground Detection")]
    public LayerMask groundMasks;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode slideKey = KeyCode.C;

    [Header("Drag")]
    [SerializeField] float grounDrag = 6f;
    [SerializeField] float airDrag = 2f;

    [SerializeField] private LevelScript levelScript;

    float horizontalMovement;
    float verticalMovement;

    bool isGrounded;

    bool isSliding;
    bool isSlidingProcess;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up) 
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (levelScript.isEnd || levelScript.isPaused) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMasks);

        Slide();

        if (isSlidingProcess)
        {
            rb.drag *= .55f;
        }

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (!Input.GetKey(slideKey))
        {
            isSlidingProcess = false;
        }

        isSliding = Input.GetKey(slideKey);
        print(isSliding);
        print(isSlidingProcess);

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void Slide()
    {
        if (isSliding)
        {
            cameraPosition.position = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);
            jumpForce = 30f;
            if (!isSlidingProcess)
            {
                isSlidingProcess = true;
                rb.mass = 5f;
                rb.drag = 3f;

                if (isGrounded)
                {
                    rb.AddForce(moveDirection.normalized * movementSpeed * 5f, ForceMode.Impulse);
                }

                //if (isGrounded && !OnSlope())
                //{
                //    rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier, ForceMode
                //        .VelocityChange);
                //}
                //else if (isGrounded && OnSlope())
                //{
                //    rb.AddForce(slopeMoveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.VelocityChange);
                //}
                //else if (!isGrounded)
                //{
                //    rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier * 
                //        airMultiplier, ForceMode.VelocityChange);
                //}
            }
            return;
        }
        cameraPosition.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.8f, this.transform.position.z);
        rb.mass = 1f;
        jumpForce = 10f;
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, sprintSpeed, acceleration * Time.deltaTime);
            return;
        }
        movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, acceleration * Time.deltaTime);
    }

    void ControlDrag()
    {
        if (isSlidingProcess) return;

        if (isGrounded)
        {
            rb.drag = grounDrag;
            return;
        }
        rb.drag = airDrag;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (levelScript.isEnd || levelScript.isPaused) return;
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isSliding) return;

        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier, ForceMode
                .Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier * 
                airMultiplier, ForceMode.Acceleration);
        }
    }
}
