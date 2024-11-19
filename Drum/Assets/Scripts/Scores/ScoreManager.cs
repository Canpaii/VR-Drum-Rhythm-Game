using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   
   static ScoreManager instance; // make script a singleton so it can be called without reference 
 
   static float score; // The players score 
   static int noteCount; // The DrumNote Count that is in the current song

   [Header("List of notes hit")] 
   static int perfectHits;
   static int earlyHits;
   static int lateHits;
   static int missedNotes;
   
   private int maxScore = 1000000; // the maximum amount of score you can receive in one map 
   private float noteWorth;
   void Awake()
   {
      instance = this;
   }
   
   public void NoteScoreCalculations() //Calculates how many points each note is worth
   {
      noteWorth = (maxScore / noteCount);
   }

   public void PerfectHit() // call this when note registration is Perfect hit
   {
      score += (noteWorth + 1);
      perfectHits++;
   }

   public void EarlyHit() // call this when note registration is Early 
   {
      score += (noteWorth * 0.50f);
      earlyHits++;
   }
   public void LateHit() // call this when note registration is Late
   {
      score += (noteWorth * 0.50f);
      lateHits++;
   }

   public void Miss() // call this when the note registration is miss
   {
      score += 0;
      missedNotes++;
   }  
}
