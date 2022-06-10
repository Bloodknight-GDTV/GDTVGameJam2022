using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************************
Gamepad buttons

A - KeyCode.Joystick1Button0
B - KeyCode.Joystick1Button1
X - KeyCode.Joystick1Button2
Y - KeyCode.Joystick1Button3

LB - KeyCode.Joystick1Button4
RB - KeyCode.Joystick1Button5

View - KeyCode.Joystick1Button6
Menu - KeyCode.Joystick1Button7

LS - KeyCode.Joystick1Button8
RS - KeyCode.Joystick1Button9

Gamepad Axis  **ALL** axis options need to be added as a new item in input controller list
Right Stick X - 4th Axis
Right Stick Y - 5th Axis

DPad U/D - 7th Axis
DPad L/R - 6th Axis

LT - 9th Axis
RT - 10th Axis

***************************************/



public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Speeds")]
    //[SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float runSpeed = 24f;
    private Vector3 moveDirection;


    [Header("verticality")]
    [SerializeField] private float JumpPower;
    [SerializeField] private float gravity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;

    private CharacterController controller;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            OpenPauseMenu();
        }


        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            StartCoroutine(Attack());
        }
    }

    private void OpenPauseMenu()
    {
        //throw new NotImplementedException();
        Debug.Log("Pause menu button");
    }

    private IEnumerator Attack()
    {
        int layer = animator.GetLayerIndex("AttackLayer");
        animator.SetLayerWeight(layer, 1);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);
        animator.SetLayerWeight(layer, 0);
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0, 0, moveZ);

        // set forward direction to players forward direction not global directiom
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {

            if (velocity.y < 0)
            {
                velocity.y = gravity;// / 2f;
            }

            if (moveDirection != Vector3.zero && (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Joystick1Button2)))
            {
                Run();
            }
            else if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            //moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button3))
            {
                Jump();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0.0f);
    }

    private void Walk()
    {
        moveDirection *= walkSpeed;
        animator.SetFloat("Speed", 0.5f);
    }

    private void Run()
    {
        moveDirection *= runSpeed;
        animator.SetFloat("Speed", 1.0f);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(JumpPower * -2 * gravity);
    }


}
