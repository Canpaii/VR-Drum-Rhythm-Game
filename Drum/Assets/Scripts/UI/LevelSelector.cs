using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelector : MonoBehaviour
{
    [SerializeField] private SongData[] songData;
    private int currentLevelIndex;
    
    [Header("UI Elements")]
    [SerializeField] private Image albumCover;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text songName;
    [SerializeField] private TMP_Text difficultyName;

    private void Start()
    {
        BeatmapManager.Instance.songData = songData[0];
    }

    public void ChangeUI() // Changes the UI elements 
    {
        albumCover.sprite = songData[currentLevelIndex].songIcon;
        songName.text = songData[currentLevelIndex].songName;
        difficultyName.text = songData[currentLevelIndex].difficulty;

        highScoreText.text = ScoreManager.Instance.GetHighScore(songData[currentLevelIndex].songName).ToString();
    }

    public void NextSong() // select the next song in the array
    {
        currentLevelIndex++;
        
        // It "loops" to the beginning if u reach the last song and press next
        if (currentLevelIndex >= songData.Length )
        {
            currentLevelIndex = 0;
        }
        
        BeatmapManager.Instance.songData = songData[currentLevelIndex];
        ChangeUI();
    }

    public void PreviousSong() // select the previous song in the array
    {
        currentLevelIndex--;
        
        // It "loops" to the end if u reach the last song and press next
        if (currentLevelIndex < 0)
        {
            currentLevelIndex = songData.Length - 1;
        }
        
        BeatmapManager.Instance.songData = songData[currentLevelIndex];
        ChangeUI();
    }
    
    public void StartSong()
    {
        BeatmapManager.Instance.StartSong();
        StateManager.Instance.SetState(DrumState.InGame);
    }

    public void OpenSettings()
    {
        StateManager.Instance.SetState(DrumState.Options);
    }

    public void HelpIcon()
    {
        
    }
    public void ExitSongSelect()
    {
        StateManager.Instance.SetState(DrumState.MainMenu);
    }
}
