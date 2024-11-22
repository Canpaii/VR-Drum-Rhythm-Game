using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public int[] noteNumbers; // determines what notes can access this lane
    public List<DrumHits> notes = new List<DrumHits>(); // notes for this path
    
    public void AddNote(DrumHits drumHit)
    {
        notes.Add(drumHit);
    }

    public bool CheckNoteNumber(int noteNumber) // check what drum part plays this note, each number is a different drum part
    {
        return System.Array.Exists(noteNumbers, n => n == noteNumber);
        
        // 35/36 = Bass/kick, 38/40 = Snare, 41/43 = Floor Tom, 42 = Hi-Hat closed,
        // 46 = Hi-Hat open, 47/48 = Mid tom, 49 = Crash cymbal, 50 = high tom,  
    }
    
}
