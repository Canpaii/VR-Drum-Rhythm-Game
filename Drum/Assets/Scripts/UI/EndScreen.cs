using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{   
    public static EndScreen Instance;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text songDurationText;
    [SerializeField] private TMP_Text songDifficultyText;
    
    [SerializeField] private Image albumCover;
    public void Awake()
    {
        Instance = this;
    }
    public void ChangeUI()
    {
        int scoreInt = (int)ScoreManager.Instance.score ; // Make the score an int so you can display it on screen.
        currentScoreText.text = "Score: " + scoreInt.ToString();
        
        
        albumCover.sprite = BeatmapManager.Instance.songData.songIcon;
        songDurationText.text = "Song Duration: " + BeatmapManager.Instance.songData.songDuration.ToString();
        songDifficultyText.text = "Difficulty: " + BeatmapManager.Instance.songData.difficulty.ToString();
        highScoreText.text = "HighScore: " + ScoreManager.Instance.GetHighScore(BeatmapManager.Instance.songData.songName);
    }
    public void RestartSong()
    {
        BeatmapManager.Instance.StartSong();
        StateManager.Instance.SetState(DrumState.InGame);
        ScoreManager.Instance.ResetScore();
    }

    public void GoToLevelSelector()
    {
       StateManager.Instance.SetState(DrumState.LevelSelect);
       ScoreManager.Instance.ResetScore();
    }
}
