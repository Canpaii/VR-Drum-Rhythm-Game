using UnityEngine;

public class PresetManager : MonoBehaviour
{
    public ShaderPresetManager shaderPresetManager;
    public EnviornmentPreset materialPresetChanger;

    [Range(0, 3)]
    public float presetIndexFloat; // Float die de presetindex beheert (voor animeren)

    private int currentPresetIndex = 0;

    void Start()
    {
        // Synchroniseer de startpreset
        ApplyPreset(0);
    }

    void Update()
    {
        // Controleer of presetIndexFloat is gewijzigd
        int newPresetIndex = Mathf.RoundToInt(presetIndexFloat);
        if (newPresetIndex != currentPresetIndex)
        {
            ApplyPreset(newPresetIndex);
        }

    }

    public void ApplyPreset(int presetIndex)
    {
        if (shaderPresetManager != null)
        {
            shaderPresetManager.SetPresetIndex(presetIndex);
        }

        if (materialPresetChanger != null)
        {
            materialPresetChanger.SetPresetIndex(presetIndex);
        }

        currentPresetIndex = presetIndex;
        presetIndexFloat = presetIndex; // Synchroniseer de floatwaarde
    }

    /// <summary>
    /// Wijzig de presetindex met een offset.
    /// </summary>
    public void IncrementPresetIndex(int offset)
    {
        presetIndexFloat = Mathf.Clamp(presetIndexFloat + offset, 0, 3);
        ApplyPreset(Mathf.RoundToInt(presetIndexFloat));
    }
}
