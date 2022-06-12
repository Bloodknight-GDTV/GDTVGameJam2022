using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_PlayerMovement : MonoBehaviour
{

    [Header("Layer Mask")]
    [Tooltip("Which Layers can be walked on?")]
    public LayerMask walkableMask;

    [Header("Movement")]
    public float speed = 12f;
    public float jumpHeight = 2f;

    [Header("Auto Run")]
    public bool useAutoRun = true;
    public KeyCode autoRunKey = KeyCode.R;

    [Header("Sprint")]
    public bool useSprint = true;
    public KeyCode sprintKey = KeyCode.LeftShift;

    CharacterController characterController;
    Transform groundCheck;
    Vector3 velocity;
    float speedMultiplier = 1.0f;
    float gravity;
    bool isGrounded;
    bool isRunning;

    void Start()
    {
        Physics.gravity = Vector3.down * 20;
        characterController = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
        gravity = Physics.gravity.y;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKey(sprintKey))
        {
            speedMultiplier = 2.0f;
        }
        else
        {
            speedMultiplier = 1.0f;
        }

        if (useAutoRun && Input.GetKeyDown(autoRunKey))
        {
            isRunning = !isRunning;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, walkableMask);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (useAutoRun && isRunning)
        {
            vertical = 1f;
        }

        Vector3 movementDir = transform.right * horizontal + transform.forward * vertical;

        characterController.Move(movementDir * speed * speedMultiplier * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

}
