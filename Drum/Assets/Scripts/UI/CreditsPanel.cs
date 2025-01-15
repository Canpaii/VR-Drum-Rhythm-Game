using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
    // Quality programming
    [SerializeField] private GameObject creditsPanel;
    private bool panelEnabled;
    public void EnablePanel()
    {
        if (!panelEnabled)
        {
            creditsPanel.SetActive(true);
            panelEnabled = true;
        }
        else
        {
            creditsPanel.SetActive(false);
            panelEnabled = false;
        }
    }

    public void DisablePanel()
    {
        creditsPanel.SetActive(false);
        panelEnabled = false;
    }
}
