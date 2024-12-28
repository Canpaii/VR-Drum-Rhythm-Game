using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{

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
