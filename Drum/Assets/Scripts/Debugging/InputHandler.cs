using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public InputAction pedalAction;

    private void OnEnable()
    {
        pedalAction.Enable();
       
        pedalAction.performed += OnPedalPressed; // Subscribe OnPedalPressed to pedal Input so when the pedal is pressed call OnPedalPressed
    }

    private void OnDisable()
    {
        pedalAction.performed -= OnPedalPressed; // Unsubscribe OnPedalPressed to keep it clean
        pedalAction.Disable();
    }

    private void OnPedalPressed(InputAction.CallbackContext context)
    {
       
       
    }
}
