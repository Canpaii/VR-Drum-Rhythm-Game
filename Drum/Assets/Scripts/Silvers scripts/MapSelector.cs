using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapSelector : MonoBehaviour
{
    [System.Serializable]
    public class SongData
    {
        public string songName;
        public string difficulty;
        public string songAuthor;
        public Sprite songImage; // Image to display for the map/song.
        public AudioClip songSample; // Audio sample of the song to play
    }
    public static MapSelector instance;
    
    [Header("Dependencies")]
    public LevelSelector levelSelector; // Reference to the LevelSelector script.
    public AudioSource audioSource;     // AudioSource to play the song sample.

    [Header("Settings")]
    public List<SongData> songDataArray = new List<SongData>(); // Array of SongData to display.
    public GameObject mapPanelPrefab;    // Prefab for the map panel.
    public Transform panelParent;        // Parent object for the panels.
    public float radiusX = 300f;         // Radius of the circular layout on the X-axis.
    public float radiusZ = 300f;         // Radius of the circular layout on the Z-axis.
    public float scrollSpeed = 5f;       // Speed of the scroll animation.

    [Header("Inspector Controls")]
    public bool swipeLeft = false;       // Trigger to swipe left.
    public bool swipeRight = false;      // Trigger to swipe right.

    private List<GameObject> panels = new List<GameObject>();
    public int currentIndex = 0;
    private float targetAngle = 0f;
    private float currentAngle = 0f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CopySongDataFromLevelSelector();
        CreateMapPanels();
        UpdatePanelPositions();
    }

    void Update()
    {
        // Update the currentIndex to match the LevelSelector's currentLevelIndex
        if (currentIndex != levelSelector.currentLevelIndex)
        {
            currentIndex = levelSelector.currentLevelIndex;

            // Update the target angle based on the current index, ensuring we stay within bounds
            targetAngle = -360f / panels.Count * currentIndex;

            // To avoid sudden jumps when reaching the end or beginning
            if (currentIndex == 0 && levelSelector.currentLevelIndex == panels.Count - 1)
            {
                currentIndex = panels.Count - 1; // Ensures smooth transition from last to first
            }
            else if (currentIndex == panels.Count - 1 && levelSelector.currentLevelIndex == 0)
            {
                currentIndex = 0; // Ensures smooth transition from first to last
            }
        }

        // Smoothly animate towards the target angle.
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * scrollSpeed);
        UpdatePanelPositions();

        if (swipeLeft)
        {
            SwipeLeft();
            swipeLeft = false;
        }

        if (swipeRight)
        {
            SwipeRight();
            swipeRight = false;
        }
    }

    private void CopySongDataFromLevelSelector()
    {
        foreach (var song in levelSelector.songData)
        {
            songDataArray.Add(new SongData
            {
                songName = song.songName,
                songImage = song.songIcon,
                difficulty = song.difficulty,
                songAuthor = song.songAuthor,
                songSample = song.SongAudioClip // Ensure the audio sample is assigned
            });
        }
    }

    private void CreateMapPanels()
    {
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }
        panels.Clear();

        foreach (var songData in songDataArray)
        {
            GameObject panel = Instantiate(mapPanelPrefab, panelParent);

            // Set song name.
            var nameText = panel.transform.Find("SongName")?.GetComponent<TMP_Text>();
            if (nameText != null)
            {
                nameText.text = songData.songName;
            }

            // Set difficulty.
            var difficultyText = panel.transform.Find("Difficulty")?.GetComponent<TMP_Text>();
            if (difficultyText != null)
            {
                difficultyText.text = "Difficulty: " + songData.difficulty;
            }

            // Set song author.
            var authorText = panel.transform.Find("SongAuthor")?.GetComponent<TMP_Text>();
            if (authorText != null)
            {
                authorText.text = "By: " + songData.songAuthor;
            }

            // Set song image.
            var imageComponent = panel.transform.Find("SongImage")?.GetComponent<UnityEngine.UI.Image>();
            if (imageComponent != null && songData.songImage != null)
            {
                imageComponent.sprite = songData.songImage;
            }
            
            var highScoreText = panel.transform.Find("Highscore")?.GetComponent<TMP_Text>();
            if (highScoreText != null)
            {
                highScoreText.text = "Highscore: " + ScoreManager.Instance.GetHighScore(songData.songName);
            }

            panels.Add(panel);
        }
    }

    private void UpdatePanelPositions()
    {
        int totalPanels = panels.Count;
        float angleStep = 360f / totalPanels;

        for (int i = 0; i < panels.Count; i++)
        {
            float angle = (i * angleStep + currentAngle) * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(angle) * radiusX, 0, Mathf.Cos(angle) * radiusZ);
            panels[i].transform.localPosition = position;

            // Scale panels based on their proximity to the center.
            float scale = 1f - Mathf.Abs(Mathf.DeltaAngle(i * angleStep, -currentAngle) / 180f);
            panels[i].transform.localScale = Vector3.one * Mathf.Clamp(scale, 0.5f, 1f);
        }
    }

    public void SwipeRight()
    {
        currentIndex = (currentIndex - 1 + panels.Count) % panels.Count;
        targetAngle += 360f / panels.Count;
    }

    public void SwipeLeft()
    {
        currentIndex = (currentIndex + 1) % panels.Count;
        targetAngle -= 360f / panels.Count;
    }
    
    public void ResetHighScoreUI() // I dont feel like rewriting this whole script so this is the solution for resetting the highscore for now
    {
        foreach (var panel in panels)
        {
            var highScoreText = panel.transform.Find("Highscore")?.GetComponent<TMP_Text>();
            if (highScoreText != null)
            {
                
                var nameText = panel.transform.Find("SongName")?.GetComponent<TMP_Text>();
                if (nameText != null)
                {
                    string songName = nameText.text;
                    
                    highScoreText.text = "Highscore: " +  ScoreManager.Instance.GetHighScore(songName);;
                }
            }
        }
    }
}
