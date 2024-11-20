using UnityEngine;

public class EnviornmentPreset : MonoBehaviour
{
    [System.Serializable]
    public class MaterialPreset
    {
        public Material environmentMaterial;  // Materiaal voor objecten met tag "Environment"
        public Material environment2Material; // Materiaal voor objecten met tag "Environment2"
        public GameObject[] specificObjects;  // Objecten die een specifiek materiaal krijgen
        public Material[] specificMaterials;  // Materialen die worden toegewezen aan specificObjects
    }

    public MaterialPreset[] presets; // Lijst van presets
    [Range(0, 3)] public float presetIndexFloat; // Float voor animeren of handmatig wisselen
    private int currentPresetIndex = 0; // Huidige presetindex

    void Start()
    {
        if (presets.Length > 0)
        {
            ApplyPreset(0);
        }
    }

    void Update()
    {
        // Controleer of presetIndexFloat voldoende is gewijzigd
        int newPresetIndex = Mathf.RoundToInt(presetIndexFloat);
        if (newPresetIndex != currentPresetIndex && newPresetIndex >= 0 && newPresetIndex < presets.Length)
        {
            ApplyPreset(newPresetIndex);
        }

        // Optionele toetsen om de preset te wijzigen
        if (Input.GetKeyDown(KeyCode.R))
        {
            IncrementPresetIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            IncrementPresetIndex(-1);
        }
    }

    public void SetPresetIndex(int presetIndex)
    {
        ApplyPreset(presetIndex);
    }


    public void ApplyPreset(int presetIndex)
    {
        if (presetIndex >= 0 && presetIndex < presets.Length)
        {
            currentPresetIndex = presetIndex;
            presetIndexFloat = presetIndex; // Synchroniseer de floatwaarde

            MaterialPreset preset = presets[presetIndex];

            // Verander materialen van objecten met de tag "Environment"
            ChangeTaggedObjectsMaterial("Environment", preset.environmentMaterial);

            // Verander materialen van objecten met de tag "Environment2"
            ChangeTaggedObjectsMaterial("Environment2", preset.environment2Material);

            // Verander materialen van specifieke objecten
            ChangeSpecificObjectsMaterial(preset);
        }
        else
        {
            Debug.LogWarning("Preset index is out of bounds.");
        }
    }

    private void ChangeTaggedObjectsMaterial(string tag, Material material)
    {
        if (material == null) return;

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in taggedObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = material;
            }
        }
    }

    private void ChangeSpecificObjectsMaterial(MaterialPreset preset)
    {
        if (preset.specificObjects.Length != preset.specificMaterials.Length)
        {
            Debug.LogWarning("Mismatch tussen objecten en materialen in de preset.");
            return;
        }

        for (int i = 0; i < preset.specificObjects.Length; i++)
        {
            GameObject obj = preset.specificObjects[i];
            Material material = preset.specificMaterials[i];

            if (obj != null && material != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = material;
                }
            }
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
