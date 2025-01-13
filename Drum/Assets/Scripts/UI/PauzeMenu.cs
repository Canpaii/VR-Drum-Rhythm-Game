using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzeMenu : MonoBehaviour
{

    public void PauseGame()
    {
        Time.timeScale = 0;
        
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
