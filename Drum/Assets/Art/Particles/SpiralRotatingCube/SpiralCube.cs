using System.Collections.Generic;
using UnityEngine;

public class MusicDrivenRotationWithPrefabsAndDelay : MonoBehaviour
{
    [Header("Music Rotation Settings")]
    public float rotationMultiplier = 10f; // Hoe sterk de muziek de rotatie beïnvloedt.
    public float dropOffSpeed = 5f;        // Hoe snel de rotatie afneemt als de muziek zwijgt.

    [Header("Prefab Spawn Settings")]
    public GameObject prefab;             // Het prefab dat gespawnd wordt.
    public int numberOfPrefabs = 5;       // Hoeveel extra objecten gespawnt worden.
    public Vector3 spawnOffset = new Vector3(2f, 0f, 0f); // Offset van de spawnpositie.
    public float rotationOffset = 20f;   // Rotatie-offset op de Z-as.
    public float movementDelay = 0.1f;   // Vertraging tussen de beweging van elk object.

    [Header("Audio Settings")]
    public AudioSource audioSource;       // Audiobron met muziek.
    public FFTWindow fftWindow = FFTWindow.Rectangular;
    public int spectrumIndex = 0;         // Index van de spectrumfrequentie die je wilt gebruiken.

    private float[] spectrumData = new float[64];
    private float currentRotationSpeed = 0f; // Huidige rotatiesnelheid.
    private List<GameObject> spawnedPrefabs = new List<GameObject>(); // Opslag voor de gespawnde objecten.

    void Start()
    {
        // Spawn de extra objecten met een offset en rotatie.
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector3 spawnPosition = transform.position + (i + 1) * spawnOffset;
            Quaternion spawnRotation = Quaternion.Euler(0, 0, i * rotationOffset);
            GameObject obj = Instantiate(prefab, spawnPosition, spawnRotation, transform);
            spawnedPrefabs.Add(obj);
        }
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            // Haal spectrumgegevens op.
            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);

            // Bepaal de luidheid van de muziek bij een specifieke frequentie.
            float musicLoudness = spectrumData[spectrumIndex] * rotationMultiplier;

            // Update de huidige rotatiesnelheid met een soepel verval.
            if (musicLoudness > currentRotationSpeed)
            {
                currentRotationSpeed = musicLoudness;
            }
            else
            {
                currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, musicLoudness, Time.deltaTime * dropOffSpeed);
            }
        }
        else
        {
            // Langzaam afremmen als er geen muziek speelt.
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime * dropOffSpeed);
        }

        // Draai het hoofdobject.
        transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

        // Laat de gespawnde objecten met vertraging meebewegen.
        for (int i = 0; i < spawnedPrefabs.Count; i++)
        {
            GameObject obj = spawnedPrefabs[i];
            float delayFactor = 1f - (i * movementDelay);
            float delayedSpeed = currentRotationSpeed * delayFactor;

            // Zorg dat rotatie alleen positief blijft.
            if (delayedSpeed < 0) delayedSpeed = 0;

            obj.transform.Rotate(0, 0, delayedSpeed * Time.deltaTime);
        }
    }
}
