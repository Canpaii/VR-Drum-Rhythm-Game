using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    private int combo;
    private int maxCombo;
    private int comboMilestone;

    public ParticleSystem[] particles;
    
    public void AddToCombo()
    {
        combo++;
        
        if (combo > maxCombo) // keep track of the highest combo 
        {
            maxCombo = combo;
        }
        ComboMileStones();
        
    
    }
    
    public void ResetCombo()
    {
        combo = 0;
    }

    private void ComboMileStones() // whenever a certain combo is met, play particles
    {
        if (combo % comboMilestone == 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }
    }
}
