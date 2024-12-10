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
  [SerializeField] private float amplitude; // strenght of the haptic
  [SerializeField] private float duration; // duration of the haptic 
  [Header("Audio")]
  AudioSource audio; // play a sound when drum sticks hit eachother 
  private void OnTriggerEnter(Collider other)
  {
    hip.SendHapticImpulse(amplitude, duration);
    audio.Play();
  }
}
