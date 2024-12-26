using UnityEngine;

public class Cubevisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public float heightMultiplier = 10f;
    public float scaleSpeed = 10f;

    void Update()
    {
        float[] spectrumData = new float[256];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float intensity = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            intensity += spectrumData[i];
        }

        intensity /= spectrumData.Length;
        float targetScale = 1 + (intensity * heightMultiplier);
        float currentScale = transform.localScale.y;
        float newScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleSpeed);
        transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
    }
}
