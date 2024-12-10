using UnityEngine;

public class BasHit : MonoBehaviour
{
    // De rotatie-instellingen
    public float rotationDegrees = 90f; // Hoeveel graden het object roteert
    public float duration = 2f;         // Hoe lang de animatie duurt (heen + terug, in seconden)
    public bool shouldAnimate = false;

    private bool isAnimating = false;   // Controleert of de animatie actief is
    private float animationTime = 0f;  // Houdt de voortgang van de animatie bij
    private Quaternion startRotation;  // De beginrotatie
    private Quaternion peakRotation;   // De maximale rotatie

    private void Update()
    {
        // Controleer of de bool wordt aangezet
        if (shouldAnimate && !isAnimating)
        {
            StartAnimation();
            shouldAnimate = false; // Zet de bool direct terug naar false
        }

        if (isAnimating)
        {
            PerformAnimation();
        }
    }

    // Functie om de animatie te starten
    private void StartAnimation()
    {
        isAnimating = true;

        // Stel de begin- en piekrotatie in
        startRotation = transform.rotation;
        peakRotation = startRotation * Quaternion.Euler(rotationDegrees, 0f, 0f);
        animationTime = 0f;
    }

    // Functie die de animatie uitvoert
    private void PerformAnimation()
    {
        // Bereken de voortgang (heen en terug binnen de totale duur)
        animationTime += Time.deltaTime;
        float halfDuration = duration / 2f;
        float progress = animationTime / halfDuration;

        if (animationTime <= halfDuration) // Eerste helft (heen)
        {
            transform.rotation = Quaternion.Lerp(startRotation, peakRotation, progress);
        }
        else if (animationTime <= duration) // Tweede helft (terug)
        {
            transform.rotation = Quaternion.Lerp(peakRotation, startRotation, progress - 1f);
        }
        else // Animatie voltooid
        {
            transform.rotation = startRotation;
            isAnimating = false;
        }
    }
}
