using UnityEngine;

public class PresetsList2
{
    public string presetName;  // Naam van de preset
    public Color baseColor;    // Basiskleur
    public Color trialColor;   // Emissie kleur
    [Range(1, 45)]
    public float emissionIntensity = 1.0f;  // Emissie intensiteit slider
    [Range(0, 1)]
    public float fresnelEffect = 0.5f;     // Fresnel effect slider

    // Optioneel, materiaal dat bij deze preset hoort
    public Material presetMaterial;
}
