using UnityEngine;

public class ObjectShatter : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Het aantal stukken waarin het object wordt gesneden.")]
    public int pieceAmount = 10;

    [Tooltip("Prefab van het stuk dat gebruikt wordt.")]
    public GameObject piecePrefab;

    [Tooltip("De kracht waarmee de stukken wegvliegen.")]
    public float explosionForce = 5f;

    [Tooltip("De radius van de explosie.")]
    public float explosionRadius = 2f;

    [Tooltip("De grootte van de stukken.")]
    public Vector3 pieceSize = Vector3.one;

    [Tooltip("Het materiaal dat wordt toegepast op de stukken.")]
    public Material pieceMaterial;

    void Start()
    {
        ShatterObject();
    }

    void ShatterObject()
    {
        if (piecePrefab == null)
        {
            Debug.LogError("Geen stuk prefab toegewezen!");
            return;
        }

        // Verwijder het originele object
        Destroy(gameObject);

        // Maak de stukken aan
        for (int i = 0; i < pieceAmount; i++)
        {
            GameObject piece = Instantiate(piecePrefab, transform.position, Random.rotation);

            // Pas de grootte aan
            piece.transform.localScale = pieceSize;

            // Pas het materiaal aan als de Renderer beschikbaar is
            if (piece.TryGetComponent(out Renderer renderer) && pieceMaterial != null)
            {
                renderer.material = pieceMaterial;
            }

            if (piece.TryGetComponent(out Rigidbody rb))
            {
                // Voeg een explosieve kracht toe aan elk stuk
                Vector3 explosionDirection = (piece.transform.position - transform.position).normalized;
                rb.AddForce(explosionDirection * explosionForce, ForceMode.Impulse);

                // Voeg random variatie toe
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}