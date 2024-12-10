using UnityEngine;

public class ShaderPresetManager : MonoBehaviour
{
    public DrumStickEditor shaderParameterAdjuster;
    public Material targetMaterial;  

    // Lijst van presets die we kunnen beheren in de Inspector
    public PresetsList[] presets;

    // Float om de huidige presetindex te beheren (kan worden geanimeerd)
    [Range(0, 3)]
    public float presetIndexFloat;

    private int currentPresetIndex = 0; // Interne presetindex om bij te houden welke preset actief is

    void Start()
    {
        if (presets.Length > 0)
        {
            ApplyPreset(0);
        }
    }


    void Update()
    {
        // Controleer of presetIndexFloat voldoende is gewijzigd om een nieuwe preset toe te passen
        int newPresetIndex = Mathf.RoundToInt(presetIndexFloat);
        if (newPresetIndex != currentPresetIndex && newPresetIndex >= 0 && newPresetIndex < presets.Length)
        {
            ApplyPreset(newPresetIndex);
        }
    }

    public void ApplyPreset(int presetIndex)
    {
        if (presetIndex >= 0 && presetIndex < presets.Length)
        {
            currentPresetIndex = presetIndex;
            presetIndexFloat = presetIndex; // Synchroniseer de floatwaarde

            PresetsList preset = presets[presetIndex];

            // Stel de waarden van de shader in op basis van de preset
            shaderParameterAdjuster.baseColor = preset.baseColor;
            shaderParameterAdjuster.trialColor = preset.trialColor;
            shaderParameterAdjuster.emissionIntensity = preset.emissionIntensity;
            shaderParameterAdjuster.fresnelEffect = preset.fresnelEffect;

            // Werk de shaderparameters bij in de ShaderParameterAdjuster
            shaderParameterAdjuster.UpdateShaderParameters();

            // Als de preset een specifiek materiaal heeft, pas dit dan toe
            if (preset.presetMaterial != null)
            {
                targetMaterial = preset.presetMaterial;
            }

            // Stel de instellingen van het materiaal in op basis van de preset
            ApplyMaterialChanges(preset);
        }
        else
        {
            Debug.LogWarning("Preset index is out of bounds.");
        }
    }

    public void SetPresetIndex(int presetIndex)
    {
        ApplyPreset(presetIndex);
    }


    private void ApplyMaterialChanges(PresetsList preset)
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_Color", preset.baseColor);  // Stel de _Color in voor het materiaal
            targetMaterial.SetFloat("_FresnelEffect", preset.fresnelEffect);

            targetMaterial.SetColor("_EmissionColor", preset.trialColor * preset.emissionIntensity);
            targetMaterial.SetColor("_BaseColorEmission", preset.baseColor * preset.emissionIntensity);
            targetMaterial.SetFloat("_EmissionValue", preset.emissionIntensity);

            targetMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogWarning("No target material assigned.");
        }
    }

    /// <summary>
    /// Wijzig de presetindex met een offset.
    /// </summary>
    public void IncrementPresetIndex(int offset)
    {
        presetIndexFloat = Mathf.Clamp(presetIndexFloat + offset, 0, presets.Length - 1);
    }
}
