using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ParticlePreset
{
    public string name;               // Naam van de preset
    public float lifeTime;            // Lengte in seconden
    public GameObject particlePrefab; // Het prefab dat wordt gespawned
    public float activationTime;      // Tijd (in seconden) waarna het particle geactiveerd wordt
    public Vector3 position;          // De locatie waar het particle verschijnt
    public Vector3 rotation;          // De rotatie (in Euler-hoeken) van het particle
    public AudioClip audioClip;       // Audio die afgespeeld moet worden
}

[System.Serializable]
public class ParticlePresetGroup
{
    public string groupName;               // Naam van de presetgroep
    public ParticlePreset[] particlePresets; // Lijst van presets binnen deze groep
    public int songArtIndex;               // SongArtManager index voor de groep
}

public class ParticleManage : MonoBehaviour
{
    [Header("Particle Preset Groups")]
    public ParticlePresetGroup[] presetGroups; // Array van presetgroepen

    [Header("Controller Variables")]
    public float presetSelector = 0f; // Float om de huidige presetgroep te selecteren
    public float currentTime = 0f;    // Huidige tijd binnen de preset
    public bool isPresetActive = false; // Bool om de preset te activeren of te pauzeren

    private int currentPresetGroupIndex = -1; // Intern bijhouden welke presetgroep actief is
    private HashSet<ParticlePreset> spawnedParticles = new HashSet<ParticlePreset>(); // Bijhouden welke particles al gespawned zijn

    private AudioSource audioSource; // AudioSource voor het afspelen van audio

    [Header("References")]
    public SongArtManager songArtManager; // Referentie naar SongArtManager

    void Start()
    {
        // Voeg een AudioSource-component toe als deze niet aanwezig is
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Controleer of een nieuwe presetgroep geselecteerd is
        int newPresetGroupIndex = Mathf.Clamp(Mathf.RoundToInt(presetSelector), 0, presetGroups.Length - 1);
        if (newPresetGroupIndex != currentPresetGroupIndex)
        {
            ActivateNewPresetGroup(newPresetGroupIndex);
        }

        // Update tijd en activeer particles als de preset actief is
        if (isPresetActive && currentPresetGroupIndex >= 0)
        {
            currentTime = BeatmapManager.Instance.globalTime;

            ParticlePresetGroup currentGroup = presetGroups[currentPresetGroupIndex];
            foreach (ParticlePreset preset in currentGroup.particlePresets)
            {
                if (!spawnedParticles.Contains(preset) && currentTime >= preset.activationTime)
                {
                    // Spawn het particle één keer
                    SpawnParticle(preset);
                }
            }
        }
    }

    private void ActivateNewPresetGroup(int newIndex)
    {
        currentPresetGroupIndex = newIndex;
        currentTime = 0f; // Reset de tijd voor de nieuwe groep
        spawnedParticles.Clear(); // Reset de lijst van gespawnede particles

        ParticlePresetGroup newGroup = presetGroups[currentPresetGroupIndex];

        // Synchroniseer met SongArtManager
        if (songArtManager != null)
        {
            songArtManager.ApplyPreset2(newGroup.songArtIndex);
            songArtManager.presetIndexFloat2 = presetSelector;
        }

        Debug.Log($"Activated preset group: {newGroup.groupName} with SongArt index: {newGroup.songArtIndex}");
    }

    private void SpawnParticle(ParticlePreset preset)
    {
        // Bereken de rotatie als een Quaternion
        Quaternion rotation = Quaternion.Euler(preset.rotation);

        // Spawn een instantie van het prefab op de aangegeven positie en rotatie
        GameObject spawnedParticle = Instantiate(preset.particlePrefab, preset.position, rotation);

        // Speel de gekoppelde audio af
        if (preset.audioClip != null)
        {
            audioSource.PlayOneShot(preset.audioClip);
        }

        // Zorg dat het particle vanzelf verdwijnt na de opgegeven duur
        Destroy(spawnedParticle, preset.lifeTime);

        // Voeg het particle toe aan de lijst van gespawnede particles
        spawnedParticles.Add(preset);

        Debug.Log($"Spawned particle: {preset.name} at position {preset.position} with rotation {preset.rotation}");
    }

    public void ResetParticles()
    {
        isPresetActive = false; // Stop de preset
        currentTime = 0f; // Reset de huidige tijd
        spawnedParticles.Clear(); // Reset de lijst van gespawnede particles

        Debug.Log("Particles reset.");
    }
}
