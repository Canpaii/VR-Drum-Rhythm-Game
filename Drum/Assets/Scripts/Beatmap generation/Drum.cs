using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Drum : MonoBehaviour
{
    [Header("Error Margins")] 
    public float perfectMargin; 
    public float errorMargin; 
    public float missMargin;
    
    [Header("References")]
    public Path path;
    public AudioSource audio;
    
    private int currentNoteIndex = 0;

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        audio.Play();

        double musicTimer = BeatmapManager.GetAudioSourceTime();

        while (currentNoteIndex < path.notes.Count)
        {
            var note = path.notes[currentNoteIndex];
            double timeDifference = musicTimer - note.time;

            if (timeDifference >= -perfectMargin && timeDifference <= perfectMargin)
            {
                // Perfect hit 
                ScoreManager.Instance.perfectHits++;
                currentNoteIndex++;
                
                print("perfect Hit");
            }
            else if (timeDifference > perfectMargin && timeDifference <= errorMargin)
            {
                // Late hit
                ScoreManager.Instance.lateHits++;
                currentNoteIndex++;
                
                print("perfect Hit");
            }
            else if (timeDifference < -perfectMargin && timeDifference >= -errorMargin)
            {
                // Early hit
                ScoreManager.Instance.earlyHits++;
                currentNoteIndex++;
                
                print("perfect Hit");
            }
            else if (timeDifference > errorMargin && timeDifference <= missMargin)
            {
                // Miss timing
                ScoreManager.Instance.missedNotes++;
                currentNoteIndex++;
            }
            else if (timeDifference > missMargin)
            {
                // Note missed
                currentNoteIndex++;
            }
        }
    }
}
