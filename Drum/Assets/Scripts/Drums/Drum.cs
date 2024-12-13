using System;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour
{
    [Header("Error Margins")] 
    [SerializeField] private float perfectMargin; 
    [SerializeField] private float normalHitMargin;
    [SerializeField] private float missMargin;
    
    [Header("References")]
    [SerializeField] private Path path;
    [SerializeField] private AudioSource audio;
    [SerializeField] private  GameObject particle;
    [SerializeField] private Transform particlePOS;

    private void OnTriggerEnter(Collider other)
    {
        CheckForHits();
    }

    public void CheckForHits()
    {
        // Play audio and visual effects
        audio.Play();
        GameObject particleObject = Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(particleObject, 1);
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
