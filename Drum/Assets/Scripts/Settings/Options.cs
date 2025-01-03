using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using Unity.VisualScripting;


public class Options : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer; // Main audiomixer of the game
    
    public int _masterVolume;
    public int _musicVolume;
    public int _sfxVolume;
    
    [Header("Player reset variables")]
    [SerializeField] private Transform resetTransform; // Position you want the player to be in 
    [SerializeField] private Camera playerHead; // The "Head" of the player
    [SerializeField] private GameObject player; // PLayer gameObject of the player
    
    [Header("UI")]
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;
    [SerializeField] private TMP_Text index; // this was used for testing purposes will remove later
    private int _currentSettingIndex;
    private void Start()
    {
        UpdateVolumeTexts();
        ResetPlayerTransform();
    }

    #region setting select

    
    public void NextSetting()
    {
        // Cycle through the different settings, 0 = master, 1 = music, 2 = sfx & 3 = reset player transform 
        print("next setting");
        _currentSettingIndex = (_currentSettingIndex + 1) % 4; 
        // Should add UI indicator for which option ur changing
        index.text = _currentSettingIndex.ToString();
    }

    public void ChangeValue(int value)
    {
        print("Change setting: " + value);
        switch (_currentSettingIndex)
        {
            case 0:
                SetMasterVolume(value); 
                break;
            case 1:
                SetMusicVolume(value); 
                break;
            case 2:
                SetSfxVolume(value); 
                break;
            case 3:
                ResetPlayerTransform();
                break;
        }
    }

    
    #endregion
    
    
    #region Set Volume
    
    
    private void SetMasterVolume(int value)
    {
        
        print("setting master: " + value);
        _masterVolume += value;
        _masterVolume = Mathf.Clamp(_masterVolume, -80, 20);
  
        masterVolumeText.text = $"Master: {_masterVolume} dB";
        audioMixer.SetFloat("MasterVolume", _masterVolume);
    }
  
    private void SetMusicVolume(int value)
    {
        print("setting music: " + value);
        _musicVolume += value;
        _musicVolume = Mathf.Clamp(_musicVolume, -80, 20);
  
        musicVolumeText.text = $"Music: {_musicVolume} dB";
        audioMixer.SetFloat("MusicVolume", _musicVolume);
    }
  
    private void SetSfxVolume(int value)
    {
        print("setting sfx: " + value);
        _sfxVolume += value;
        _sfxVolume = Mathf.Clamp(_sfxVolume, -80, 20);
  
        sfxVolumeText.text = $"SFX: {_sfxVolume} dB";
        audioMixer.SetFloat("SFXVolume", _sfxVolume);
    }
  
    #endregion


    private void ResetPlayerTransform()
    {
        print("Reset player transform");
        var rotationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.eulerAngles.y;
        player.transform.Rotate(0, rotationAngleY, 0);
        
        var headOffset = playerHead.transform.position - player.transform.position;  // calculates the offset of VR headset relative to the player
        var targetPosition = resetTransform.position - headOffset; // Calculates new target position
        
        player.transform.position = targetPosition; // Set position to target position 
        
    }
    
    
    #region UI Update
    private void UpdateVolumeTexts()
    {
        masterVolumeText.text = $"Master: {_masterVolume} dB";
        musicVolumeText.text = $"Music: {_musicVolume} dB";
        sfxVolumeText.text = $"SFX: {_sfxVolume} dB";
    }
  
    public void ExitSettings()
    {
        StateManager.Instance.SetState(DrumState.LevelSelect);
    } 
    #endregion
 }
