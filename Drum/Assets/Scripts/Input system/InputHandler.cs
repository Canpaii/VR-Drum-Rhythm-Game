using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference pedalActionReference; // Reference to the pedal action, action is set in the inspector
    private InputAction _pedalAction; // Variable that stores the action and can "perform" the action
    [SerializeField] private Drum kick; // Reference to the drum component on the kick
    
    private void Awake() 
    {
       _pedalAction = pedalActionReference; // assigns the reference to the InputAction variable, so action can be performed
    }

    private void OnEnable()
    {
        _pedalAction.Enable();   
        
        _pedalAction.performed += OnPedalPressed; // Subscribe OnPedalPressed to pedal Input so when the foot pedal is pressed call OnPedalPressed
    }

    private void OnDisable()
    {
        _pedalAction.performed -= OnPedalPressed; // Unsubscribe OnPedalPressed to keep it clean
        _pedalAction.Disable();
    }

    private void OnPedalPressed(InputAction.CallbackContext context)
    {
        print("Pedal Hit");
        if (kick != null)
        {
            kick.CheckForHits();
        }
        else
        {
            Debug.Log("Kick not assigned");
        }
    }
}
