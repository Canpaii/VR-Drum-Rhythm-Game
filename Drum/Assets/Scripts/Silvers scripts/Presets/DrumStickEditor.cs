using UnityEngine;

public class DrumStickEditor : MonoBehaviour
{
    public Material targetMaterial;

    public Color baseColor = Color.white;
    public Color trialColor = Color.white;  // Dit is je HDR-kleur voor emissie
    [Range(1, 45)]
    public float emissionIntensity = 1.0f;  // Dit is de emissie-intensiteit slider
    public float fresnelEffect = 0.5f;

    void Start()
    {
        if (targetMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                targetMaterial = renderer.material;
            }
        }

        UpdateShaderParameters();
    }

    void Update()
    {
        if (targetMaterial != null)
        {
            // Stel de basiskleur in op de _Color van het materiaal
            targetMaterial.SetColor("_Color", baseColor);

            // Pas de Fresnel effect waarde toe
            targetMaterial.SetFloat("_FresnelEffect", fresnelEffect);

            // Pas de emissiekleur aan door trialColor te vermenigvuldigen met de emissie-intensiteit
            targetMaterial.SetColor("_EmissionColor", trialColor * emissionIntensity);

            // Zet de emissie-intensiteit in de shader (dit heeft effect op de emissie-sterkte)
            targetMaterial.SetFloat("_EmissionValue", emissionIntensity);

            // Zorg ervoor dat de emissie ingeschakeld wordt
            targetMaterial.EnableKeyword("_EMISSION");
        }
    }

    public void UpdateShaderParameters()
    {
        if (targetMaterial != null)
        {
            // Update de shaderparameters
            targetMaterial.SetColor("_Color", baseColor);
            targetMaterial.SetFloat("_FresnelEffect", fresnelEffect);
            targetMaterial.SetColor("_EmissionColor", trialColor * emissionIntensity);
            targetMaterial.SetFloat("_EmissionValue", emissionIntensity);
            targetMaterial.EnableKeyword("_EMISSION");
        }
    }
}
