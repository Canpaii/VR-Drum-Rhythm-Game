using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelSelector : MonoBehaviour
{
    public SongData songData;
    
    [SerializeField] private Image albumCover;
    [SerializeField] private string songName;
    [SerializeField] private string artistName;
    [SerializeField] private string difficultyName;
    
    [SerializeField] private TMP_Text highScoreText;
    public void ChangeUI()
    {
        albumCover.sprite = songData.songIcon;
        songName = songData.songName;
        artistName = songData.songAuthor;
        difficultyName = songData.difficulty;

        highScoreText.text = ScoreManager.Instance.GetHighScore(songName).ToString();
    }
}
