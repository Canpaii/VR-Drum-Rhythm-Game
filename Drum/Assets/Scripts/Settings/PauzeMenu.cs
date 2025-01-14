using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzeMenu : MonoBehaviour
{
    public void PauseGame()
    {
        if (StateManager.Instance.currentDrumState == DrumState.InGame)
        {
            Time.timeScale = 0;
            StateManager.Instance.SetState(DrumState.Pause);
        }
        else
        {
            Debug.Log("Cannot pause game");
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        StateManager.Instance.SetState(DrumState.InGame);
    }

    public void ResetSong()
    {
        
    }

    public void ReturnToLevelSelection()
    {
        Time.timeScale = 1;
        StateManager.Instance.SetState(DrumState.LevelSelect);
    }
    
}
