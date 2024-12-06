using System.Collections;
using UnityEngine;
using TMPro;

public class PerfectHit : MonoBehaviour
{
    private TMP_Text textMeshPro;
    private Mesh textMesh;
    private Vector3[] vertices;

    public float minScale = 0.5f;   // Kleinste schaal
    public float maxScale = 1.5f;  // Grootste schaal
    public float speed = 1f;       // Snelheid van de animatie

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

            // Bereken een schaalfactor op basis van een sinus voor een vloeiend in-/uit-zoom effect
            float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * speed) + 1f) / 2f);

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 direction = originalVertices[i] - textMesh.bounds.center;
                vertices[i] = textMesh.bounds.center + direction * scale;
            }

            textMesh.vertices = vertices;
            textMeshPro.canvasRenderer.SetMesh(textMesh);

            yield return null;
        }
    }
}
