using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Voeg deze import toe voor TMP_Text

public class ComboTracker : MonoBehaviour
{
    public int combo;
    private int maxCombo;
    public int comboMilestone;

    public ParticleSystem[] particles;

    // Voeg een TMP_Text component toe voor het weergeven van de combo op het scherm.
    public TMP_Text comboText;

    void Start()
    {
        // Zorg ervoor dat de combo tekst correct wordt weergegeven bij de start
        UpdateComboText();
    }

    public void AddToCombo()
    {
        combo++;

        if (combo > maxCombo) // Houd de hoogste combo bij
        {
            maxCombo = combo;
        }

        ComboMileStones();

        // Werk de tekst bij elke keer als de combo verandert
        UpdateComboText();
    }

    public void ResetCombo()
    {
        combo = 0;
        UpdateComboText();
    }

    private void ComboMileStones() // Wanneer een bepaalde combo wordt gehaald, speel dan de particles af
    {
        if (combo % comboMilestone == 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }
    }

    // Functie om de combo tekst bij te werken op het scherm
    private void UpdateComboText()
    {
        if (comboText != null)
        {
            comboText.text = "Combo: " + combo.ToString();
        }
    }
}
