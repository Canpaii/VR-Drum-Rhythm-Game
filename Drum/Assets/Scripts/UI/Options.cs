using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;


public class Options : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    public int _masterVolume;
    public int _musicVolume;
    public int _sfxVolume;

    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;
    public void Start() // The players still need their ears for the rest of the game...
    {
        // SetMasterVolume(-30);
        //SetMusicVolume(-30);
        //SetSfxVolume(-30);
    }
    
    #region Set Volume

    public void SetMasterVolume(int value) // Value is determined in the Unity Event 
    {
        _masterVolume += value;
        
        _masterVolume = Mathf.Clamp(_masterVolume, -80, 20); //It starts at -80 DB and 20 DB is the highest volume on the audio mixer
        
        masterVolumeText.text = _masterVolume.ToString() + "/ 100";
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_masterVolume) * 20);
    }
    public void SetMusicVolume(int value)
    {
        _musicVolume += value;
        
        _musicVolume = Mathf.Clamp(_musicVolume, 0, 80);
        
        musicVolumeText.text = _musicVolume.ToString() + "/ 100";
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(_musicVolume) * 20);
    }
    public void SetSfxVolume(int value)
    {
        _sfxVolume += value;
        
        _sfxVolume = Mathf.Clamp(_sfxVolume, 0, 80);
        
        sfxVolumeText.text = _sfxVolume.ToString() + "/ 100";
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(_sfxVolume) * 20);
    }
    #endregion

    public void NextSetting()
    {
        
    }
    public void ExitSettings()
    {
        StateManager.Instance.SetState(DrumState.LevelSelect);
    }
}
