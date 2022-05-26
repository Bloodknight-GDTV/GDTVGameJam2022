/**
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // **********************************
    // This Class is used for Invoke Unity Events version of the input controller

    // BKS NOTE: 
    // if you have a nested structure with a replacable mesh then
    // GetComponentInChildren<>(); Is needed here over GetComponent<>(); 
    // This also discovers the self object as a child... dont ask, its voodoo and it works
    
    // BKS NOTE
    // The button action has 3 contexts, started, performed, and cancelled
    // ALL if these contexts are executed which means every instruction is 
    // run 3 times, once per context. you need to select which context runs 
    // which code!

    private Rigidbody PlayerRigidBody;
    private PlayerInput playerInput;

    private void Awake() 
    {
        PlayerRigidBody = GetComponentInChildren<Rigidbody>(); 
    }

    
    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jump.{context}");       // #INFO uncomment this line to see all contexts and details
        if(context.performed){
            PlayerRigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse); 
        }        
    }


}
**/