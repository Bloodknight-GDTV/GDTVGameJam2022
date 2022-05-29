using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// **********************************
// This Class is used for Invoke Unity Events version of the input controller
// This uses the generated class from the input asset.

// BKS NOTE: 
// if you have a nested structure with a replacable mesh then
// GetComponentInChildren<>(); Is needed  over GetComponent<>(); 
// This also discovers the self object as a child... 
// dont ask, its voodoo and it works

// BKS NOTE
// The button action has 3 contexts, started, performed, and cancelled
// ALL if these contexts are executed which means every instruction is 
// run 3 times, once per context. you need to select which context runs 
// which code!

public class InputController : MonoBehaviour, PlayerInputActions.IPlayerActions
{

    private Animator animator;
    private PlayerInput playerInput;

    // SG Add
    public Vector2 inputVector;
    float inputVertical;
    float inputHorizontal;

    // THis is the input system created controller class
    PlayerInputActions playerInputActions;
    float jumpPower = 5.0f;
    float movementSpeed = 5f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        //PlayerRigidBody = GetComponentInChildren<Rigidbody>(); 
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        //playerInputActions.Enable();          // This enables all actionmaps
        playerInputActions.Player.Enable();     // This enables the named actionmap

        // List of subscribed functions/actions
        playerInputActions.Player.Jump.performed += OnJump;
        //playerInputActions.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>;
        //playerInputActions.Player.Movement.performed += Movement_performed;

    }

    private void Update()
    {
        // // This code segment works dont f**k with it, this is baseline
        // Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        // transform.Translate(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed * Time.deltaTime);


        SetInputValues();
        transform.Translate(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed * Time.deltaTime);

        float movement = movementSpeed * inputVector.y;


        Debug.Log($"inputVector.y = {inputVector.y}");
        //Debug.Log($"movement = {movement}");

        if (movement < 0.05)
        {
            Debug.Log($"Idle");
            animator.SetFloat("ForwardSpeed", 0.0f);
        }
        if (movement > 0.45 && movement < 0.95)
        {
            Debug.Log($"walking");
            animator.SetFloat("ForwardSpeed", 0.5f);
        }
        if (movement > 0.95)
        {
            Debug.Log($"running");
            animator.SetFloat("ForwardSpeed", 1.0f);
        }

        //animator.SetFloat("ForwardSpeed", movement);
    }

    private void SetInputValues()
    {
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        inputHorizontal = inputVector.x;
        inputVertical = inputVector.y;
    }

    private void UpdateAnimator()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        transform.Translate(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed * Time.deltaTime);
        GetComponent<Animator>().SetFloat("ForwardSpeed", movementSpeed);
    }

    // public void Jump(InputAction.CallbackContext context)
    // {
    //     Debug.Log($"Jump.{context}");       // #INFO uncomment this line to see all contexts and details
    //     if (context.performed)
    //     {
    //         transform.Translate(Vector3.up * jumpPower);
    //     }
    // }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"OnJump.{context}");       // #INFO uncomment this line to see all contexts and details
        if (context.performed)
        {
            transform.Translate(Vector3.up * jumpPower);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        Debug.Log($"OnCrouch.{context}");       // #INFO uncomment this line to see all contexts and details
    }

    public void OnMana(InputAction.CallbackContext context)
    {
        Debug.Log($"OnMana.{context}");       // #INFO uncomment this line to see all contexts and details
    }

    public void OnHealth(InputAction.CallbackContext context)
    {
        Debug.Log($"OnHealth.{context}");       // #INFO uncomment this line to see all contexts and details
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Debug.Log($"OnMovement.{context}");       // #INFO uncomment this line to see all contexts and details
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        Debug.Log($"OnRun.{context}");       // #INFO uncomment this line to see all contexts and details
    }

    // private void Movement_performed(InputAction.CallbackContext context)
    // {
    //     //Debug.Log($"Movement_performed.{context}");       // #INFO uncomment this line to see all contexts and details


    //     //UpdateAnimator();

    // }

}
