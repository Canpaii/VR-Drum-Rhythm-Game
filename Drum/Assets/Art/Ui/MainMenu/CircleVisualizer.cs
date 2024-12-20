using UnityEngine;

public class CircleVisualizer : MonoBehaviour
{
    [Header("Visualizer Settings")]
    public GameObject cubePrefab; // Prefab voor de cubes
    public int numberOfCubes = 64; // Aantal cubes in de cirkel
    public float radius = 5f; // Straal van de cirkel
    public float scaleMultiplier = 10f; // Schaalfactor voor de muziek
    public float smoothSpeed = 0.5f; // Hoe snel de schaal aanpast

    [Header("Audio Settings")]
    public AudioSource audioSource; // Audio bron
    public FFTWindow fftWindow = FFTWindow.Blackman; // Spectrumvenster
    public int spectrumSize = 512; // Aantal samples (512 of 1024 aanbevolen)

    private GameObject[] cubes; // Opslag voor de gegenereerde cubes
    private float[] spectrumData;

    void Start()
    {
        // Initialiseer de cubes en spectrumdata
        cubes = new GameObject[numberOfCubes];
        spectrumData = new float[spectrumSize];

        // Plaats de cubes in een perfecte cirkel
        for (int i = 0; i < numberOfCubes; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfCubes; // Gelijke verdeling rond de cirkel
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            cubes[i] = Instantiate(cubePrefab, position, Quaternion.identity, transform);
            cubes[i].transform.LookAt(transform.position); // Laat de cube naar het midden wijzen
        }
    }

    void Update()
    {
        // Haal spectrum data op
        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);

        for (int i = 0; i < numberOfCubes; i++)
        {
            // Verdeeld over het spectrum, met wrap-around voor naadloze aansluiting
            float normalizedIndex = (float)i / numberOfCubes * spectrumSize;
            int lowerIndex = Mathf.FloorToInt(normalizedIndex) % spectrumSize;
            int upperIndex = (lowerIndex + 1) % spectrumSize;

            // Interpoleer tussen frequenties voor vloeiendere overgangen
            float interpolation = normalizedIndex - lowerIndex;
            float frequencyValue = Mathf.Lerp(spectrumData[lowerIndex], spectrumData[upperIndex], interpolation);

            // Bereken de gewenste schaal
            float targetScale = Mathf.Clamp(frequencyValue * scaleMultiplier, 0.1f, 10f);

            // Smooth de schaal
            Vector3 currentScale = cubes[i].transform.localScale;
            Vector3 newScale = Vector3.Lerp(currentScale, new Vector3(1, targetScale, 1), smoothSpeed * Time.deltaTime);
            cubes[i].transform.localScale = newScale;
        }
    }
}
