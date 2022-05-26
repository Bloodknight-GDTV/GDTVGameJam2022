using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
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

    private Rigidbody PlayerRigidBody;// = GetComponentInChildren<Rigidbody>();
    private PlayerInput playerInput;
    PlayerInputActions playerInputActions;

    private void Awake() 
    {
        PlayerRigidBody = GetComponentInChildren<Rigidbody>(); 
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();        
        //playerInputActions.Enable();          // This enables all actionmaps
        playerInputActions.Player.Enable();     // This enables the named actionmap

        // List of subscribed functions/actions
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Movement.performed += Movement_performed;

    }

    private void FixedUpdate() {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();
        float speed = 1f;
        PlayerRigidBody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed,  ForceMode.Force);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jump.{context}");       // #INFO uncomment this line to see all contexts and details
        if(context.performed){
            PlayerRigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse); 
        }        
    }

    private void Movement_performed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        Vector2 inputVector = context.ReadValue<Vector2>();
        float speed = 5f;
        PlayerRigidBody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed,  ForceMode.Force); 
    }




}
