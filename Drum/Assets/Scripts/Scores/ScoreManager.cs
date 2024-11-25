using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
   
   public static ScoreManager Instance; // make script a singleton so it can be called without reference 
   
   [Header("Core Variables")]
   public float score; // The players score 
   public int noteCount; // The DrumNote Count that is in the current song

   [Header("Stats of notes hit")] 
   public int perfectHits;
   public int earlyHits;
   public int lateHits;
   public int missedNotes;
   
   [Header("References")]
   public ComboTracker comboTracker;
   
   private int maxScore = 1000000; // the maximum amount of score you can receive in one map 
   private float noteWorth;
   
   private int maxCombo;
   
   void Awake()
   {
      Instance = this;
   }
   
   public void NoteScoreCalculations() //Calculates how many points each note is worth
   {
      noteWorth = (maxScore / noteCount);
   }

   public void PerfectHit() // call this when note registration is Perfect hit
   {
      score += (noteWorth + 1);
      perfectHits++;
      
      comboTracker.AddToCombo();
   }

   public void EarlyHit() // call this when note registration is Early 
   {
      score += (noteWorth * 0.50f);
      earlyHits++;
      
      comboTracker.AddToCombo();
   }
   public void LateHit() // call this when note registration is Late
   {
      score += (noteWorth * 0.50f);
      lateHits++;
      
      comboTracker.AddToCombo();
   }

   public void Miss() // call this when the note registration is miss
   {
      score += 0;
      missedNotes++;
      
      comboTracker.ResetCombo();
   }

   public void SetHighScore(int score)
   {
      PlayerPrefs.SetInt("HighScore", score);
   }
}
