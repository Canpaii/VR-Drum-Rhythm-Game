using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable]
public struct InputActionMapping
{
    public InputActionReference inputActionReference;
    public UnityEvent onActionPerformed;
}

public class InputManager : MonoBehaviour
{
    [SerializeField] private List<InputActionMapping> inputActionMappings = new List<InputActionMapping>();
    private List<InputAction> enabledActions = new List<InputAction>();

    private void Awake()
    {
        foreach (var mapping in inputActionMappings)
        {
            if (mapping.inputActionReference == null)
            {
                Debug.LogWarning("Input Action Reference is not assigned for one of the mappings.");
                continue;
            }

            InputAction action = mapping.inputActionReference.action;
            if (action == null)
            {
                Debug.LogWarning("Input Action is null for one of the mappings.");
                continue;
            }

            action.performed += ctx => OnActionPerformed(mapping);
            enabledActions.Add(action);
        }
    }

    private void OnEnable()
    {
        foreach (var action in enabledActions)
        {
            action.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var action in enabledActions)
        {
            action.Disable();
        }
    }

    private void OnActionPerformed(InputActionMapping mapping)
    {
        mapping.onActionPerformed.Invoke();
    }
}