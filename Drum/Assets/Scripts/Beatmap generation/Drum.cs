using System;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour
{
    [Header("Error Margins")] 
    public float perfectMargin; 
    public float normalHitMargin;
    public float missMargin;
    
    [Header("References")]
    public Path path;
    public AudioSource audio;
    public ParticleSystem[] particleEffects;

    private void OnTriggerEnter(Collider other)
    {
        
        CheckForHits();
        /*// Play audio and particle effects
        audio.Play();
        foreach (var particle in particleEffects)
        {
            particle.Play();
        }

        // Calculate the current music time, adjusted for input delay
        double musicTimer = BeatmapManager.GetAudioSourceTime() - BeatmapManager.Instance.inputDelayInMilliseconds / 1000.0;

        // Check if there are notes remaining
        if (currentNoteIndex >= path.notes.Count) return;

        // Get the current note
        var note = path.notes[currentNoteIndex];
        if (note == null) return;

        // Calculate the time difference between the note and the music timer
        double timeDifference = musicTimer - note.GetComponent<Note>().timeStamp;

        // Check if the note is within hit margins
        if (Math.Abs(timeDifference) <= perfectMargin)
        {
            PerfectHit();
        }
        else if (timeDifference > perfectMargin && timeDifference <= normalHitMargin)
        {
            LateHit();
        }
        else if (timeDifference < -perfectMargin && timeDifference >= -normalHitMargin)
        {
            EarlyHit();
        }
        else
        {
            Miss();
        }*/
    }

    private void CheckForHits()
    {
        // Play audio and visual effects
        audio.Play();
        foreach (var particle in particleEffects)
        {
            particle.Play();
        }

        // Ensure there are notes left to process
        if (0 >= path.notes.Count) return;

        // Get the current note
        GameObject noteObject = path.notes[0];
        if (noteObject == null) return;

        Note note = noteObject.GetComponent<Note>();
        double musicTimer = BeatmapManager.GetAudioSourceTime() - BeatmapManager.Instance.inputDelayInMilliseconds / 1000.0;
        double timeDifference = musicTimer - note.timeStamp;

        // Check hit timing
        if (Math.Abs(timeDifference) <= perfectMargin)
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
        else if(Math.Abs(timeDifference) <= normalHitMargin)
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

        // Destroy the note and update the path
        note.GetComponent<Note>().hit = true;
        Destroy(note);
        path.notes.RemoveAt(0);
    }
}
