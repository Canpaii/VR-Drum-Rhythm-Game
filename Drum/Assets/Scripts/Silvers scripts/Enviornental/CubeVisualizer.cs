using UnityEngine;

public class Cubevisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public float heightMultiplier = 10f;
    public float scaleSpeed = 10f;

    void Update()
    {
        // Spectrumdata ophalen
        float[] spectrumData = new float[256];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Gebruik de dominante frequentie om de schaal te bepalen
        float intensity = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            intensity += spectrumData[i];
        }

        // Gemiddelde intensiteit berekenen
        intensity /= spectrumData.Length;

        // Bereken de nieuwe schaal op basis van de intensiteit
        float targetScale = 1 + (intensity * heightMultiplier);
        float currentScale = transform.localScale.y;

        // Smooth de overgang naar de nieuwe schaal
        float newScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleSpeed);

        // Toepassen op de prefab
        transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
    }
}
