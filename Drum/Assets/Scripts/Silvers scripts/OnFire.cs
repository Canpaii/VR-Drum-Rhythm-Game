using System.Collections;
using UnityEngine;

public class OnFire : MonoBehaviour
{
    [Header("Combo Settings")]
    public ComboTracker comboTracker; // Referentie naar het ComboTracker-script
    public int comboThreshold = 5; // Aantal hits nodig voor opladen

    [Header("Slider Settings")]
    [Range(0, 1)] private float maxValue = 1f;
    [Range(0, 1)] private float minValue = 0f;
    public float chargeSpeed = 0.5f; // Hoe snel de sliders opladen

    private float vignetteValue;
    private float sparkleValue;

    private bool isCharging;

    void Start()
    {
        vignetteValue = minValue;
        sparkleValue = minValue;
        isCharging = false;
    }

    void Update()
    {
        // Controleer of de huidige combo de drempel bereikt
        if (comboTracker.combo >= comboThreshold && !isCharging)
        {
            StartCoroutine(ChargeSliders());
        }
    }

    private IEnumerator ChargeSliders()
    {
        isCharging = true;

        while (vignetteValue < maxValue || sparkleValue < maxValue)
        {
            vignetteValue = Mathf.MoveTowards(vignetteValue, maxValue, chargeSpeed * Time.deltaTime);
            sparkleValue = Mathf.MoveTowards(sparkleValue, maxValue, chargeSpeed * Time.deltaTime);

            Shader.SetGlobalFloat("_VignetteActivation", vignetteValue);
            Shader.SetGlobalFloat("_SparkleActivate", sparkleValue);

            yield return null; // Wacht tot de volgende frame
        }

        // Reset combo zodat het opnieuw opgebouwd moet worden
        comboTracker.ResetCombo();
        isCharging = false;
    }
}
