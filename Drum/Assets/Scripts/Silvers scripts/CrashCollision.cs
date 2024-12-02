using System.Collections;
using UnityEngine;

public class CrashRotation : MonoBehaviour
{
    private Quaternion originalRotation; // De originele rotatie
    private Vector3 originalPosition;    // De originele positie
    public float rotationAngle = 30f;   // Hoeveel graden de crash roteert bij een hit
    public float returnSpeed = 5f;      // Hoe snel de crash terugkeert naar de originele rotatie/positie
    public float returnDelay = 2f;      // Hoe lang wachten voordat het object terugkeert
    private bool isReturning = false;  // Of de crash terug aan het roteren is
    private float lastCollisionTime;    // Tijdstip van de laatste botsing

    void Start()
    {
        // Sla de originele rotatie en positie op
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
        lastCollisionTime = Time.time;
    }

    void Update()
    {
        // Controleer of het object moet terugkeren na de wachttijd
        if (!isReturning && Time.time - lastCollisionTime >= returnDelay)
        {
            StartCoroutine(ReturnToOriginalState());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Controleer of de botsing van de drumstick komt
        if (collision.gameObject.CompareTag("Drumstick") && !isReturning)
        {
            // Update de laatste botsingstijd
            lastCollisionTime = Time.time;

            // Draai de crash tijdelijk
            RotateCrash();
        }
    }

    private void RotateCrash()
    {
        // Roteer de crash naar voren (bijv. om de X-as)
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x + rotationAngle,
            transform.localRotation.eulerAngles.y,
            transform.localRotation.eulerAngles.z
        );
    }

    private IEnumerator ReturnToOriginalState()
    {
        isReturning = true;

        // Laat de crash langzaam terugkeren naar de originele rotatie en positie
        while (Quaternion.Angle(transform.localRotation, originalRotation) > 0.01f ||
               Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, Time.deltaTime * returnSpeed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * returnSpeed);
            yield return null;
        }

        // Zet rotatie en positie exact terug naar de originele staat
        transform.localRotation = originalRotation;
        transform.localPosition = originalPosition;
        isReturning = false;
    }
}
