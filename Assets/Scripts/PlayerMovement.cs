using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Movement")]
    
    public float groundDrag;

    public Transform orientation;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;
    public float customGrav = 30f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        rb.useGravity = false;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        
        MyInput();
        SpeedControl();

        if(grounded)
        {
            rb.drag = groundDrag;
            
            // print("isGrounded");
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ApplyCustomGravity();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        Vector3 flattenedForward = new Vector3(orientation.forward.x, 0, orientation.forward.z).normalized;
        Vector3 flattenedRight = new Vector3(orientation.right.x, 0, orientation.right.z).normalized;

        moveDirection = flattenedForward * verticalInput + flattenedRight * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * GlobalReferences.Instance.moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * GlobalReferences.Instance.moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > GlobalReferences.Instance.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * GlobalReferences.Instance.moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void ApplyCustomGravity()
    {
        if (!grounded)
        {
            rb.AddForce(Vector3.down * customGrav, ForceMode.Acceleration);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
