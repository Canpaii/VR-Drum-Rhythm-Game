using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
   
   public static ScoreManager Instance; // make script a singleton so it can be called without reference 
   
   [Header("Core Variables")]
   public float score; // The players score 
   public int noteCount; // The DrumNote Count that is in the current song

   [Header("Stats of notes hit")] // stats that you can see after the game has finished
   public int perfectHits;
   public int earlyHits;
   public int lateHits;
   public int missedNotes;
   
   [Header("References")]
   public ComboTracker comboTracker;
   
   [Header("UI References")]
   public TMP_Text highScoreText;
   public TMP_Text currentScore;
   
   private int maxScore = 1000000; // the maximum amount of score you can receive in one map (1 million)
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

      UpdateScore();
      comboTracker.AddToCombo();
   }

   public void EarlyHit() // call this when note registration is Early 
   {
      score += (noteWorth * 0.50f);
      earlyHits++;

      UpdateScore();
      comboTracker.AddToCombo();
   }
   public void LateHit() // call this when note registration is Late
   {
      score += (noteWorth * 0.50f);
      lateHits++;

      UpdateScore();
      comboTracker.AddToCombo();
   }

   public void Miss() // call this when the note registration is miss
   {
      score += 0;
      missedNotes++;
      
      comboTracker.ResetCombo();
   }

   public void UpdateScore() // updates the score that is visible to the player ingame
   {
      currentScore.text = score.ToString();
   }
   public void SetHighScore(float score, string songName) // Set HighScore 
   {
      int scoreInt = (int)score;
      PlayerPrefs.SetInt($"{songName}: HighScore", scoreInt);
   }

   public int GetHighScore(string songName) // get highscore for specific song 
   {
      if (PlayerPrefs.HasKey($"{songName}: HighScore"))
      {
         int highScore = PlayerPrefs.GetInt($"{songName}: HighScore");
         return highScore;
      }
     
      return 0;
   }
}
