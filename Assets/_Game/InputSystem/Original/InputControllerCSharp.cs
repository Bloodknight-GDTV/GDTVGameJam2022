using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControllerCSharp : MonoBehaviour
{
    // **********************************
    // This Class is used for Invoke C# Events version of the input controller
    // This is unused and is here for an example only

    // BKS NOTE: 
    // if you have a nested structure with a replacable mesh then
    // GetComponentInChildren<>(); Is needed here over GetComponent<>(); 
    // This also discovers the self object as a child... 
    // dont ask, its voodoo and it works

    // BKS NOTE:
    // The button action has 3 contexts, started, performed, and cancelled
    // ALL if these contexts are executed which means every instruction is 
    // run 3 times, once per context. you need to select which context runs 
    // which code!


    private Rigidbody PlayerRigidBody;
    private PlayerInput playerInput;

    private void Awake() {
        
        PlayerRigidBody = GetComponentInChildren<Rigidbody>(); 
        
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
    {
        //Debug.Log($"{context}");       // #INFO uncomment this line to see all contexts and details
        if (context.performed){
            PlayerRigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse); 
        }
        
    }

}