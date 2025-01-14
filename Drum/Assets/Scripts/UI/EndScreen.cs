using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public static EndScreen Instance;

    [Header("TMP Text")]
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text songDifficultyText;
    [SerializeField] private TMP_Text lateHitText;
    [SerializeField] private TMP_Text earlyHitText;
    [SerializeField] private TMP_Text missHitText;
    [SerializeField] private TMP_Text perfectHitText;
    
    [Header("UI Image")]
    [SerializeField] private Image albumCover;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeUI()
    {
        // Make the score an int so you can display it on screen.
        int scoreInt = (int)ScoreManager.Instance.score;
        currentScoreText.text = "Score: " + scoreInt.ToString();

        lateHitText.text = ScoreManager.Instance.lateHits.ToString();
        earlyHitText.text = ScoreManager.Instance.earlyHits.ToString();
        perfectHitText.text = ScoreManager.Instance.perfectHits.ToString();
        missHitText.text = ScoreManager.Instance.missedNotes.ToString();

        albumCover.sprite = BeatmapManager.Instance.songData.songIcon;
        songDifficultyText.text = "Difficulty: " + BeatmapManager.Instance.songData.difficulty.ToString();
        highScoreText.text = "High Score: " + ScoreManager.Instance.GetHighScore(BeatmapManager.Instance.songData.songName);
        
        ScoreSlider.Instance.BeginFill();
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
