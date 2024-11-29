using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Drum : MonoBehaviour
{
    [Header("Error Margins")] 
    public float perfectMargin; 
    public float normalHitMargin;
    
    [Header("References")]
    public Path path;
    public AudioSource audio;
    public ParticleSystem[] particle;
    
    public int currentNoteIndex = 0; // this needs to be called by the "Note" component/script 
    
    private void OnTriggerEnter(Collider other)
    {
        audio.Play();
        for (int i = 0; i < particle.Length; i++)
        {
            particle[i].Play();
        }

        double musicTimer = BeatmapManager.GetAudioSourceTime() - BeatmapManager.Instance.inputDelayInMilliseconds/1000;

        while (currentNoteIndex < path.notes.Count)
        {
            var note = path.notes[currentNoteIndex];
            double timeDifference = musicTimer - note.GetComponent<Note>().timeStamp;

            if (timeDifference >= -perfectMargin && timeDifference <= perfectMargin) // check if timing is withing perfect hit margin
            {
                // Perfect hit
                PerfectHit();
            }
            else if (timeDifference >= perfectMargin && timeDifference <= normalHitMargin) // moet dit nog ff goed uitleggen
            {
                // Late hit
               LateHit();
            }
            else if (timeDifference <= -perfectMargin && timeDifference >= -normalHitMargin) 
            {
                // Early hit
                EarlyHit();
            }
            else if (timeDifference >= normalHitMargin || timeDifference <= -normalHitMargin) // timing outside the hit margin
            {
                // Miss timing
                Miss();
            }
        }
    }

    private void PerfectHit()
    {
        // Perfect hit 
        ScoreManager.Instance.PerfectHit();
        Destroy(path.notes[currentNoteIndex]);
                
        currentNoteIndex++;
        print("perfect Hit");
    }

    private void LateHit()
    {
        ScoreManager.Instance.LateHit();
        Destroy(path.notes[currentNoteIndex]);
                
        currentNoteIndex++;
                
        print("Late Hit");
    }

    private void EarlyHit()
    {
        ScoreManager.Instance.EarlyHit();
        Destroy(path.notes[currentNoteIndex]);
                
        currentNoteIndex++;
                
        print("Early Hit");
    }

    private void Miss() 
    {
        ScoreManager.Instance.Miss();
        Destroy(path.notes[currentNoteIndex]);
                
        currentNoteIndex++;
        
        print ("Miss");
    }
}
