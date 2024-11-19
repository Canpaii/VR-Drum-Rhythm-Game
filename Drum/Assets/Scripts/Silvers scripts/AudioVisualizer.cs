using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject cubePrefab;
    public int numberOfCubes = 64;
    public float heightMultiplier = 10f;
    public float spacing = 1.5f;
    public float backwardOffset = 2.0f;
    private GameObject[] cubes;

    void Start()
    {
        cubes = new GameObject[numberOfCubes];
        int half = numberOfCubes / 2;

        Vector3 startPosition = transform.position;

        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            float positionOffset = (i - half) * spacing;

            cube.transform.position = new Vector3(startPosition.x + positionOffset, startPosition.y, startPosition.z - backwardOffset);
            cube.transform.parent = this.transform;
            cubes[i] = cube;
        }
    }

    void Update()
    {
        float[] spectrumData = new float[numberOfCubes];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        int half = numberOfCubes / 2;

        for (int i = 0; i < half; i++)
        {
            Vector3 leftScale = cubes[half - i - 1].transform.localScale;
            Vector3 rightScale = cubes[half + i].transform.localScale;

            float height = Mathf.Lerp(leftScale.y, spectrumData[i] * heightMultiplier, Time.deltaTime * 30);

            leftScale.y = height;
            rightScale.y = height;

            cubes[half - i - 1].transform.localScale = leftScale;
            cubes[half + i].transform.localScale = rightScale;
        }
    }
}
