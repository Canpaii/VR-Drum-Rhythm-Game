using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{   
    public static EndScreen Instance;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;

    public void Awake()
    {
        Instance = this;
    }
    public void ChangeUI()
    {
        highScoreText.text = "HighScore: " + ScoreManager.Instance.GetHighScore(BeatmapManager.Instance.songData.songName);
        currentScoreText.text = "Score: " + ScoreManager.Instance.currentScore.ToString();
    }
    public void RestartSong()
    {
        BeatmapManager.Instance.StartSong();
        StateManager.Instance.SetState(DrumState.InGame);
    }

    public void GoToLevelSelector()
    {
       StateManager.Instance.SetState(DrumState.LevelSelect);
    }
}
