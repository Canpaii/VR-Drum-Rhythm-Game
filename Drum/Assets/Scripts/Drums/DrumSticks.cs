using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
public class DrumSticks : MonoBehaviour
{ 
  [Header("Haptic Settings")]
  [SerializeField] HapticImpulsePlayer hip; // reference to the XR haptic feedback component 
  [SerializeField] private float hapticAmplitude; // strenght of the haptic
  [SerializeField] private float hapticDuration; // duration of the haptic 
  
  [Header("Audio")]
  AudioSource audio; // play a sound when drum sticks hit eachother 

  [Header("Drum roll")] 
  public int rollAmplitude;

  private bool drumRolling;
  float _rollFrequency;
  float _time;
  public void CalculateDrumRollFrequency(int bpm)
  {
    _rollFrequency = bpm / 60; // beats per second 
  }

  public void PlayDrumRoll()
  {
    drumRolling = true;
    
    _time += Time.deltaTime;

    // Calculate sine wave value
    float angle = Mathf.Sin(_time * _rollFrequency * 2 * Mathf.PI) * rollAmplitude; // Adjust 30f for amplitude

    // Apply rotation
    transform.localRotation = Quaternion.Euler(angle, 0, 0);
  }
  private void OnTriggerEnter(Collider other)
  {
    hip.SendHapticImpulse(hapticAmplitude, hapticDuration);
    audio.Play();
  }
}
