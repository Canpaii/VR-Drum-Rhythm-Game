using UnityEngine;

[System.Serializable]
public class EffectTrigger
{
    public float triggerScore; // De score waarop het effect moet worden geactiveerd
    public AudioClip soundEffect;
    public ParticleSystem[] particleEffects; // Meerdere particle-effecten
    public Transform spawnPosition; // Specifieke spawnpositie voor particle-effecten
}

public class ScoreSlider : MonoBehaviour
{
    [SerializeField] private RectTransform targetImage;
    [SerializeField] private float fillDuration = 3f;
    [SerializeField] private float slowDownPercentage = 0.1f;
    [SerializeField] private EffectTrigger[] effectTriggers;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip chargeSoundEffect; // Charge sound effect
    [SerializeField] private bool startFillTest = false;

    private const float MinWidth = 0.1f;
    private const float MaxWidth = 4.83f;

    private float targetScore;
    private float currentWidth;
    private bool isFilling = false;
    private bool isComplete = false;
    private bool isChargeSoundPlaying = false; // To track if the charge sound is playing

    private void Update()
    {
        if (startFillTest)
        {
            startFillTest = false;
            BeginFill();
        }

        if (!isFilling || targetImage == null) return;

        // Calculate target width based on score
        float targetWidth = Mathf.Lerp(MinWidth, MaxWidth, Mathf.Clamp01(targetScore / 1000000f));

        // Adjust speed for the slowdown
        float timeFactor = fillDuration * (currentWidth < targetWidth * (1 - slowDownPercentage) ? 1f : 2f);

        // Update current width towards target width
        currentWidth = Mathf.MoveTowards(currentWidth, targetWidth, (MaxWidth / timeFactor) * Time.deltaTime);

        // Update the target image size
        targetImage.sizeDelta = new Vector2(currentWidth, targetImage.sizeDelta.y);

        // If filling is complete, stop the charge sound and trigger effects
        if (Mathf.Approximately(currentWidth, targetWidth))
        {
            isFilling = false;
            if (!isComplete)
            {
                isComplete = true;
                PlayEffectTriggers();
                StopChargeSound(); // Stop the charge sound when filling is complete
            }
        }
        else
        {
            // Start playing the charge sound if it's not playing yet
            if (!isChargeSoundPlaying)
            {
                PlayChargeSound();
            }
        }
    }

    public void BeginFill()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null) return;

        targetScore = scoreManager.score;
        currentWidth = targetImage.sizeDelta.x;
        isFilling = true;
        isComplete = false;
    }

    private void PlayChargeSound()
    {
        if (chargeSoundEffect != null && audioSource != null)
        {
            audioSource.loop = true; // Set loop to true so it plays continuously during the fill
            audioSource.clip = chargeSoundEffect;
            audioSource.Play();
            isChargeSoundPlaying = true;
        }
    }

    private void StopChargeSound()
    {
        if (audioSource.isPlaying && chargeSoundEffect != null)
        {
            audioSource.loop = false; // Stop looping when filling is complete
            audioSource.Stop();
            isChargeSoundPlaying = false;
        }
    }

    private void PlayEffectTriggers()
    {
        if (effectTriggers == null) return;

        foreach (var trigger in effectTriggers)
        {
            if (targetScore >= trigger.triggerScore)
            {
                if (trigger.soundEffect != null && audioSource != null)
                {
                    audioSource.PlayOneShot(trigger.soundEffect);
                }

                if (trigger.particleEffects != null)
                {
                    foreach (var particleEffect in trigger.particleEffects)
                    {
                        if (particleEffect != null)
                        {
                            // Gebruik de specifieke spawnpositie of standaard naar de particle's originele positie
                            Vector3 spawnPos = trigger.spawnPosition != null
                                ? trigger.spawnPosition.position
                                : particleEffect.transform.position;

                            Quaternion spawnRot = trigger.spawnPosition != null
                                ? trigger.spawnPosition.rotation
                                : particleEffect.transform.rotation;

                            var instance = Instantiate(particleEffect, spawnPos, spawnRot);
                            instance.gameObject.SetActive(true);

                            // Opruimen als de particle zichzelf niet vernietigt
                            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constant);
                        }
                    }
                }
            }
        }
    }
}
