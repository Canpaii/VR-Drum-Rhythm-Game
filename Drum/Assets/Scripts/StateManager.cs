using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance; // Make singleton 
    public DrumState currentDrumState {get; private set;} // Variable that holds the state 
    [SerializeField] private GameObject[] drumPortraits;
    [SerializeField] private GameObject[] uiPanels;
    private void Awake() // Sets Instance
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        SetState(DrumState.MainMenu);
    }

    public void SetState(DrumState newState) // Call this function to change state
    {
        currentDrumState = newState;
        CheckStateChanges();
    }

    private void CheckStateChanges() // Changes UI panels and portraits based on state
    {
        // Deactivate all portraits
        foreach (var portrait in drumPortraits)
        {
            portrait.SetActive(false);
        }
        
        //Deactivate all UI panels
        foreach (var panel in uiPanels)
        {
            panel.SetActive(false);
        }
        
        switch (currentDrumState)
        {
            case DrumState.MainMenu:
                MainMenu();
                break;
            case DrumState.Options:
                Options();
                break;
            case DrumState.LevelSelect:
                LevelSelect();
                break;
            case DrumState.InGame: 
                // Deactivate all portraits and panels                
                break;
            case DrumState.EndOfSong:
                EndOfSong();    
                break;
        }
    }

    private void MainMenu()
    {
        drumPortraits?[0].SetActive(true);
        uiPanels?[0].SetActive(true);
    }

    private void Options()
    {
        drumPortraits?[1].SetActive(true);
        uiPanels?[1].SetActive(true);
    }

    private void LevelSelect()
    {
        drumPortraits?[2].SetActive(true);
        uiPanels?[2].SetActive(true);
    }

    private void EndOfSong()
    {
        drumPortraits?[3].SetActive(true);
        uiPanels?[3].SetActive(true);
    }
}
