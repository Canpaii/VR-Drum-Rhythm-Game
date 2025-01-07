using UnityEngine;

public class HitFeedbackManager : MonoBehaviour
{
    // Referenties naar de GameObjects voor feedback
    public GameObject perfectHitObject;
    public GameObject earlyHitObject;
    public GameObject lateHitObject;
    public GameObject missedNoteObject;

    // De locatie waar het GameObject moet verschijnen
    public Transform feedbackPosition;

    // Referentie naar de ScoreManager
    public ScoreManager scoreManager;

    // Huidige waarden om veranderingen te detecteren
    private int previousPerfectHits;
    private int previousEarlyHits;
    private int previousLateHits;
    private int previousMissedNotes;

    // Huidig actieve feedbackobject
    private GameObject currentFeedbackObject;

    private void Start()
    {
        // Sla de initiële waarden op
        previousPerfectHits = scoreManager.perfectHits;
        previousEarlyHits = scoreManager.earlyHits;
        previousLateHits = scoreManager.lateHits;
        previousMissedNotes = scoreManager.missedNotes;
    }

    private void Update()
    {
        if (scoreManager.perfectHits > previousPerfectHits)
        {
            ShowFeedback(perfectHitObject);
            previousPerfectHits = scoreManager.perfectHits;
        }
        else if (scoreManager.earlyHits > previousEarlyHits)
        {
            ShowFeedback(earlyHitObject);
            previousEarlyHits = scoreManager.earlyHits;
        }
        else if (scoreManager.lateHits > previousLateHits)
        {
            ShowFeedback(lateHitObject);
            previousLateHits = scoreManager.lateHits;
        }
        else if (scoreManager.missedNotes > previousMissedNotes)
        {
            ShowFeedback(missedNoteObject);
            previousMissedNotes = scoreManager.missedNotes;
        }
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
}
