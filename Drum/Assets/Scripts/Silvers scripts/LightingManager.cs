using UnityEngine;

[System.Serializable]
public class MotionPreset
{
    public string name;             // Naam van de preset
    public GameObject targetObject; // Het object dat beweegt/roteert
    public Vector3 startPosition;   // Startpositie van het object
    public Vector3 endPosition;     // Eindpositie van het object
    public Vector3 rotationSpeed;   // Rotatiesnelheid (Euler-hoeken per seconde)
    public float moveDuration;      // Tijd (in seconden) voor de beweging
    public float activationTime;    // Tijd (in seconden) waarna het preset wordt geactiveerd
}

[System.Serializable]
public class MotionPresetGroup
{
    public string groupName;            // Naam van de presetgroep
    public MotionPreset[] motionPresets; // Lijst van presets binnen deze groep
}

public class LightingManager : MonoBehaviour
{
    [Header("Motion Preset Groups")]
    public MotionPresetGroup[] motionGroups; // Array van presetgroepen

    [Header("Controller Variables")]
    public float presetSelector = 0f; // Float om de huidige presetgroep te selecteren
    public float currentTime = 0f;    // Huidige tijd binnen de preset
    public bool isMotionActive = false; // Bool om de beweging te activeren of te pauzeren

    private int currentMotionGroupIndex = -1; // Intern bijhouden welke presetgroep actief is

    [Header("References")]
    public ParticleManage particleManager; // Referentie naar het ParticleManage script

    void Update()
    {
        currentTime = BeatmapManager.Instance.globalTime;
        if (particleManager != null)
        {
            presetSelector = particleManager.presetSelector;
        }

        int newMotionGroupIndex = Mathf.Clamp(Mathf.RoundToInt(presetSelector), 0, motionGroups.Length - 1);
        if (newMotionGroupIndex != currentMotionGroupIndex)
        {
            ActivateNewMotionGroup(newMotionGroupIndex);
        }

        // Update tijd en voer beweging/rotatie uit als de preset actief is
        if (isMotionActive && currentMotionGroupIndex >= 0)
        {
            currentTime = BeatmapManager.Instance.globalTime;

            MotionPresetGroup currentGroup = motionGroups[currentMotionGroupIndex];
            foreach (MotionPreset preset in currentGroup.motionPresets)
            {
                if (preset.targetObject != null)
                {
                    // Beweging
                    if (currentTime >= preset.activationTime && currentTime <= preset.activationTime + preset.moveDuration)
                    {
                        float t = (currentTime - preset.activationTime) / preset.moveDuration;
                        preset.targetObject.transform.position = Vector3.Lerp(preset.startPosition, preset.endPosition, t);
                    }

                    // Rotatie
                    preset.targetObject.transform.Rotate(preset.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void ActivateNewMotionGroup(int newIndex)
    {
        currentMotionGroupIndex = newIndex;

        // Reset de objecten naar hun startposities
        MotionPresetGroup currentGroup = motionGroups[currentMotionGroupIndex];
        foreach (MotionPreset preset in currentGroup.motionPresets)
        {
            if (preset.targetObject != null)
            {
                preset.targetObject.transform.position = preset.startPosition;
            }
        }

        Debug.Log($"Activated motion group: {motionGroups[currentMotionGroupIndex].groupName}");
    }

    public void ResetMotion()
    {
        isMotionActive = false; // Stop de beweging
        currentTime = 0f; // Reset de huidige tijd

        if (currentMotionGroupIndex >= 0)
        {
            MotionPresetGroup currentGroup = motionGroups[currentMotionGroupIndex];
            foreach (MotionPreset preset in currentGroup.motionPresets)
            {
                if (preset.targetObject != null)
                {
                    preset.targetObject.transform.position = preset.startPosition;
                }
            }
        }
    }
}
