using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedParticleManager : MonoBehaviour
{
    [System.Serializable]
    public class ParticleSpawn
    {
        public GameObject particlePrefab; // Prefab van het particle effect
        public Vector3 spawnPosition; // Positie waar de particle spawnt
        public float spawnTime; // Tijd waarop de particle moet spawnen
        public float destructTime = 5.0f; // Tijd voordat het particle wordt vernietigd
    }

    [System.Serializable]
    public class ParticleManager
    {
        public string presetName; // Naam van de preset (optioneel)
        public List<ParticleSpawn> particleSpawns; // Lijst van alle particles voor deze preset
        public AudioClip audioClip; // Audioclip gekoppeld aan deze preset
        public float audioStartTime;
    }

    public List<ParticleManager> particlePresets; // Alle presets
    public int currentPresetIndex = 0; // Actieve preset
    public AudioSource audioSource; // AudioSource om audio af te spelen

    private List<GameObject> activeParticles = new List<GameObject>();
    private Coroutine particleCoroutine;

    private void Start()
    {
        // Zorg ervoor dat de AudioSource bestaat
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start de spawns van de eerste preset
        StartPreset(currentPresetIndex);
    }

    public void SwitchToPreset(int presetIndex)
    {
        if (presetIndex < 0 || presetIndex >= particlePresets.Count)
        {
            Debug.LogError("Preset index is out of range!");
            return;
        }

        // Stop huidige spawns
        if (particleCoroutine != null)
        {
            StopCoroutine(particleCoroutine);
        }

        // Stop huidige audio
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Clear actieve particles
        ClearActiveParticles();

        // Update de preset index en start de nieuwe preset
        currentPresetIndex = presetIndex;
        StartPreset(currentPresetIndex);
    }

    private void StartPreset(int presetIndex)
    {
        ParticleManager preset = particlePresets[presetIndex];

        // Start de audiotrack als deze beschikbaar is
        if (preset.audioClip != null)
        {
            audioSource.clip = preset.audioClip;

            // Bereken het startpunt afhankelijk van de audioStartTime
            float audioStartPoint = preset.audioStartTime;
            if (audioStartPoint < 0 || audioStartPoint > audioSource.clip.length)
            {
                Debug.LogWarning($"Audio start time ({audioStartPoint}s) is out of range for clip '{preset.audioClip.name}'. Defaulting to 0.");
                audioStartPoint = 0f;
            }

            audioSource.time = audioStartPoint;
            audioSource.Play();
        }

        // Start de particle spawning
        particleCoroutine = StartCoroutine(SpawnParticles(preset));
    }

    private IEnumerator SpawnParticles(ParticleManager preset)
    {
        foreach (ParticleSpawn spawn in preset.particleSpawns)
        {
            yield return new WaitForSeconds(spawn.spawnTime);

            // Spawn het particle
            GameObject spawnedParticle = Instantiate(spawn.particlePrefab, spawn.spawnPosition, Quaternion.identity);
            activeParticles.Add(spawnedParticle);

            // Start de destruct-timer
            StartCoroutine(DestructParticle(spawnedParticle, spawn.destructTime));
        }
    }

    private IEnumerator DestructParticle(GameObject particle, float delay)
    {
        // Wacht totdat de destruct-tijd voorbij is
        yield return new WaitForSeconds(delay);

        if (particle != null)
        {
            Destroy(particle);
            activeParticles.Remove(particle);
        }
    }

    private void ClearActiveParticles()
    {
        foreach (var particle in activeParticles)
        {
            if (particle != null)
            {
                Destroy(particle);
            }
        }
        activeParticles.Clear();
    }
}
