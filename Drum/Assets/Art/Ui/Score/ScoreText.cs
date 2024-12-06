using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TMP_Text textMeshPro;
    private Mesh textMesh;
    private Vector3[] vertices;

    public float amplitude = 5f;
    public float frequency = 2f;
    public float speed = 1f;  

    void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        textMeshPro.ForceMeshUpdate();
        textMesh = textMeshPro.mesh;
        vertices = textMesh.vertices;

        Vector3[] originalVertices = textMesh.vertices;

        while (true)
        {
            textMeshPro.ForceMeshUpdate();
            vertices = textMesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 offset = Wobble(Time.time * speed + i);
                vertices[i] = originalVertices[i] + offset;
            }

            textMesh.vertices = vertices;
            textMeshPro.canvasRenderer.SetMesh(textMesh);

            yield return null;
        }
    }

    private Vector3 Wobble(float time)
    {
        return new Vector3(
            Mathf.Sin(time * frequency) * amplitude,
            Mathf.Cos(time * frequency) * amplitude,
            0
        );
    }
}
