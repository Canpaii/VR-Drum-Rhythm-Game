using UnityEngine;

[System.Serializable]
public class PresetsList
{
    public string name2;
    public Color baseColor2;
    public Color trialColor2;
    public float emissionIntensity2;
    public float fresnelEffect2;
    public Material presetMaterial2;
    public Material trialMaterial2;
    public GameObject[] objectsToEnable2;
    public GameObject[] objectsToDisable2;
    public Material skyboxMaterial2;
}

public class SongArtManager : MonoBehaviour
{
    [Header("Shader Parameters")]
    private Material targetMaterial2;

    public PresetsList[] presets2;

    public float presetIndexFloat2;

    private int currentPresetIndex2 = 0;
    internal static object Instance;

    void Start()
    {
        if (presets2.Length > 0)
        {
            ApplyPreset2(0);
        }
    }

    void Update()
    {
        int newPresetIndex2 = Mathf.RoundToInt(presetIndexFloat2);
        if (newPresetIndex2 != currentPresetIndex2 && newPresetIndex2 >= 0 && newPresetIndex2 < presets2.Length)
        {
            ApplyPreset2(newPresetIndex2);
        }
    }

    public void ApplyPreset2(int presetIndex2)
    {
        if (presetIndex2 >= 0 && presetIndex2 < presets2.Length)
        {
            currentPresetIndex2 = presetIndex2;
            presetIndexFloat2 = presetIndex2;

            PresetsList preset2 = presets2[presetIndex2];

            ApplyShaderParameters2(preset2);

            if (preset2.presetMaterial2 != null)
            {
                targetMaterial2 = preset2.presetMaterial2;
            }

            ApplyMaterialChanges2(preset2);

            ApplyTrailMaterialChanges2(preset2);

            UpdateObjectStates2(preset2);


            if (preset2.skyboxMaterial2 != null)
            {
                RenderSettings.skybox = preset2.skyboxMaterial2;
                DynamicGI.UpdateEnvironment(); // Werk globale belichting bij
            }
        }
    }

    private void ApplyShaderParameters2(PresetsList preset2)
    {
        if (targetMaterial2 != null)
        {
            targetMaterial2.SetColor("_Color", preset2.baseColor2);
            targetMaterial2.SetColor("_EmissionColor", preset2.baseColor2 * preset2.emissionIntensity2);
            targetMaterial2.SetFloat("_FresnelEffect", preset2.fresnelEffect2);
        }
    }

    private void ApplyMaterialChanges2(PresetsList preset2)
    {
        if (targetMaterial2 != null)
        {
            targetMaterial2.SetColor("_Color", preset2.baseColor2);  // Stel de _Color in voor het materiaal
            targetMaterial2.SetFloat("_FresnelEffect", preset2.fresnelEffect2);

            targetMaterial2.SetColor("_EmissionColor", preset2.baseColor2 * preset2.emissionIntensity2);
            targetMaterial2.SetFloat("_EmissionValue", preset2.emissionIntensity2);

            targetMaterial2.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogWarning("No target material assigned.");
        }
    }

    private void ApplyTrailMaterialChanges2(PresetsList preset2)
    {
        if (preset2.trialMaterial2 != null)
        {
            preset2.trialMaterial2.SetColor("_Color", preset2.trialColor2); // Stel de kleur van de trail in
            preset2.trialMaterial2.SetColor("_EmissionColor", preset2.trialColor2 * preset2.emissionIntensity2);
            preset2.trialMaterial2.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogWarning("No trail material assigned in preset: " + preset2.name2);
        }
    }

    private void UpdateObjectStates2(PresetsList preset2)
    {
        // Schakel objecten in
        foreach (GameObject obj2 in preset2.objectsToEnable2)
        {
            if (obj2 != null)
            {
                obj2.SetActive(true);
            }
        }

        // Schakel objecten uit
        foreach (GameObject obj2 in preset2.objectsToDisable2)
        {
            if (obj2 != null)
            {
                obj2.SetActive(false);
            }
        }
    }

    public void SetPresetIndex2(int presetIndex2)
    {
        ApplyPreset2(presetIndex2);
    }

    public void IncrementPresetIndex2(int offset2)
    {
        presetIndexFloat2 = Mathf.Clamp(presetIndexFloat2 + offset2, 0, presets2.Length - 1);
    }
}