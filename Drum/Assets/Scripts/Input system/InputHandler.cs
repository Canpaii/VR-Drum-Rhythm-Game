using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
   // [SerializeField] private InputActionAsset pedalActionAsset;
    [SerializeField] private InputActionReference pedalActionReference;
    private InputAction _pedalAction;
    [SerializeField] private Drum kick;

    //[SerializeField] private string actionMapName;
    //[SerializeField] private string actionName;
    private void Awake()
    {
       //_pedalAction = pedalActionAsset.FindActionMap(actionMapName).FindAction(actionName);
       _pedalAction = pedalActionReference;
    }

    private void OnEnable()
    {
        _pedalAction.Enable();   
        
        _pedalAction.performed += OnPedalPressed; // Subscribe OnPedalPressed to pedal Input so when the pedal is pressed call OnPedalPressed
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

    public void StartRebinding()
    {
        _pedalAction.Disable();
        // _pedalAction.PerformInteractiveRebinding().OnComplete(operation =>
            
    }
}
