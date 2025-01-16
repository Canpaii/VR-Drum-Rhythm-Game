using System;
using UnityEngine;

public class HitFeedbackManager : MonoBehaviour
{
    public static HitFeedbackManager instance;
    // Referenties naar de GameObjects voor feedback
    public GameObject perfectHitObject;
    public GameObject earlyHitObject;
    public GameObject lateHitObject;
    public GameObject missedNoteObject;

    // De locatie waar het GameObject moet verschijnen
    public Transform feedbackPosition;

    // Referentie naar de ScoreManager
    public ScoreManager scoreManager;

    // Particle-system prefab voor perfect hit feedback
    public ParticleSystem perfectHitParticle1;
    public ParticleSystem perfectHitParticle2;

    // Huidige waarden om veranderingen te detecteren
    private int previousPerfectHits;
    private int previousEarlyHits;
    private int previousLateHits;
    private int previousMissedNotes;

    // Huidig actieve feedbackobject
    private GameObject currentFeedbackObject;

    private void Awake()
    {
         instance = this;
    }

    private void Start()
    {
        // Sla de initi�le waarden op
        previousPerfectHits = scoreManager.perfectHits;
        previousEarlyHits = scoreManager.earlyHits;
        previousLateHits = scoreManager.lateHits;
        previousMissedNotes = scoreManager.missedNotes;
    }
    public void ShowPerfect()
    {
        ShowFeedback(perfectHitObject);
        SpawnPerfectHitParticles(); // Spawn particles bij een perfecte hit
        previousPerfectHits = scoreManager.perfectHits;
    }

    public void ShowEarly()
    {
        ShowFeedback(earlyHitObject);
        previousEarlyHits = scoreManager.earlyHits;
    }

    public void ShowLate()
    {
        ShowFeedback(lateHitObject);
        previousLateHits = scoreManager.lateHits;
    }

    public void ShowMissed()
    {
        ShowFeedback(missedNoteObject);
        previousMissedNotes = scoreManager.missedNotes;
    }

    private void ShowFeedback(GameObject feedbackObject)
    {
        if (currentFeedbackObject == feedbackObject)
        {
            return;
        }

        // Verberg het huidige feedbackobject als er een actief is
        if (currentFeedbackObject != null)
        {
            currentFeedbackObject.SetActive(false);
        }

        // Toon het nieuwe feedbackobject
        feedbackObject.SetActive(true);
        feedbackObject.transform.position = feedbackPosition.position;

        // Stel het huidige feedbackobject in
        currentFeedbackObject = feedbackObject;
    }

    private void SpawnPerfectHitParticles()
    {
        // Eerste particle komt iets naar links van de feedback positie
        Vector3 particle1Position = feedbackPosition.position + new Vector3(-0.5f, 0, 0);
        if (perfectHitParticle1 != null)
        {
            // Instantiate de particle binnen dezelfde parent als feedbackPosition
            ParticleSystem particle1 = Instantiate(perfectHitParticle1, particle1Position, Quaternion.identity, feedbackPosition.parent);
            Destroy(particle1.gameObject, particle1.main.duration + particle1.main.startLifetime.constantMax);
        }

        // Tweede particle komt iets naar rechts van de feedback positie
        Vector3 particle2Position = feedbackPosition.position + new Vector3(0.5f, 0, 0);
        if (perfectHitParticle2 != null)
        {
            // Instantiate de particle binnen dezelfde parent als feedbackPosition
            ParticleSystem particle2 = Instantiate(perfectHitParticle2, particle2Position, Quaternion.identity, feedbackPosition.parent);
            Destroy(particle2.gameObject, particle2.main.duration + particle2.main.startLifetime.constantMax);
        }
    }
}
