using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool _confirm = false;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject mainMenuPanel;
    public void StartGame() 
    {
        confirmPanel.SetActive(false); //Cancel confirmation panel 
        mainMenuPanel.SetActive(true);
        
        if (!_confirm)
        {
            StateManager.Instance.SetState(DrumState.LevelSelect);
        }
        
        _confirm = false;
    }
    
    public void QuitGame() //Needs to be called twice to quit game
    {
        confirmPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        if (_confirm)
        {
            Application.Quit();
        }

        _confirm = true;
    }
}
