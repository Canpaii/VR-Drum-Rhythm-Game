using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Drum : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent levelSelectFunction; // Function of this drum when selecting a level
    [SerializeField] private UnityEvent optionsFunction; // Function of this drum when changing options
    [SerializeField] private UnityEvent endScreenFunction; // Function of this drum when song ended;
    
    [Header("Error Margins")] 
    [SerializeField] private float perfectMargin; 
    [SerializeField] private float normalHitMargin;
    [SerializeField] private float missMargin;
    
    [Header("References")]
    [SerializeField] private Path path; // The path component of the path attached to this drum
    [SerializeField] private AudioSource audio;
    [SerializeField] private  GameObject particle; // Hit particle of this drum
    [SerializeField] private Transform particlePOS; // Position of the particle
    
    
    #region OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        // Play audio and visual effects
        audio.Play();
        GameObject particleObject = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(particleObject, 1);
        
        switch (StateManager.Instance.currentDrumState)
        {
            case DrumState.MainMenu:
                MainMenuDrumBehviour();
                break;
            case DrumState.Options:
                OptionsDrumBehviour();
                break;
            case DrumState.LevelSelect:
                LevelSelectDrumBehviour();
                break;
            case DrumState.InGame:
                InGameDrumBehviour();
                break;
            case DrumState.EndOfSong:
                EndOfSongDrumBehviour();
                break;
        }
    }

    #endregion
    
    #region StateBehaviors
    
    
    #region MainMenuDrumBehviour
    
    private void MainMenuDrumBehviour()
    {
        StateManager.Instance.SetState(DrumState.LevelSelect);
    }
    
    #endregion
    
    
    #region OptionsDrumBehviour
    private void OptionsDrumBehviour()
    {
        optionsFunction?.Invoke();
    }
    #endregion
    
    
    #region LevelSelectDrumBehviour
    private void LevelSelectDrumBehviour()
    {
        if (levelSelectFunction != null)
        {
            levelSelectFunction?.Invoke();
        }
    }
    #endregion
    
    
    #region InGameDrumBehviour
    
    private void InGameDrumBehviour()
    {
        if (BeatmapManager.Instance.songPlaying)
        {
            CheckForHits();
        }
    }
    
    #endregion
    
    
    #region EndOfSongDrumBehviour
    private void EndOfSongDrumBehviour()
    {
        endScreenFunction?.Invoke();
    }
    #endregion
    

    #endregion
    
    #region Drum Behaviour
    public void CheckForHits()
    {
        // Ensure there are notes left to process
        if (0 >= path.notes.Count) return;

        // Get the current note
        GameObject noteObject = path.notes[0];
        if (noteObject == null)
        {
            print("No note found");
        }
        if (noteObject == null) return;

        Note note = noteObject.GetComponent<Note>();
        double musicTimer = BeatmapManager.GetAudioSourceTime() - BeatmapManager.Instance.inputDelayInMilliseconds / 1000.0;
        double timeDifference = musicTimer - note.timeStamp; // get the time difference when it got hit and when it should've been hit 

        // Check hit timing
        if (timeDifference >= -perfectMargin && timeDifference <= perfectMargin)
        {
            PerfectHit(noteObject);
        }
        else if (timeDifference > perfectMargin && timeDifference <= normalHitMargin)
        {
            LateHit(noteObject);
        }
        else if (timeDifference < -perfectMargin && timeDifference >= -normalHitMargin)
        {
            EarlyHit(noteObject);
        }
        else
        {
            Miss(noteObject);
        }
    }
    private void PerfectHit(GameObject note)
    {
        ScoreManager.Instance.PerfectHit();
        RegisterHit(note,"Perfect Hit");
    }

    private void LateHit(GameObject note)
    {
        ScoreManager.Instance.LateHit();
        RegisterHit(note, "Late Hit");
    }

    private void EarlyHit(GameObject note)
    {
        ScoreManager.Instance.EarlyHit();
        RegisterHit(note, "Early Hit");
    }

    private void Miss(GameObject note)
    {
        ScoreManager.Instance.Miss();
        RegisterHit(note, "Miss");
        
    }
    private void RegisterHit(GameObject note, string hitType)
    {
        Debug.Log(hitType);
        Debug.Log($"Hit registered: {hitType}, Note: {note.name}");

        // Destroy the note and update the path
        note.GetComponent<Note>().hit = true;
        Destroy(note);
        path.notes.RemoveAt(0);
    }
    #endregion
   
}
