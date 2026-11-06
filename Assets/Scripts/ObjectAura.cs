using UnityEngine;

public class ObjectAura : MonoBehaviour
{
    [Header("Brillo (Emission)")]
    [SerializeField] private Color auraColor = new Color(1f, 0.85f, 0.5f);
    [SerializeField] private float minIntensity = 0.1f;
    [SerializeField] private float maxIntensity = 0.9f;
    [SerializeField] private float pulseSpeed = 1.5f;

    [Header("Luz opcional")]
    [SerializeField] private Light auraLight;
    [SerializeField] private float minLightIntensity = 0.1f;
    [SerializeField] private float maxLightIntensity = 0.5f;

    private Material[] materials;
    private bool isActive = true;

    private void Awake()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        var list = new System.Collections.Generic.List<Material>();

        foreach (MeshRenderer r in renderers)
        {
            foreach (Material m in r.materials) // crea instancias propias
            {
                m.EnableKeyword("_EMISSION");
                list.Add(m);
            }
        }

        materials = list.ToArray();

        if (auraLight != null)
            auraLight.color = auraColor;
    }

    private void Update()
    {
        if (!isActive) return;

        // Valor que sube y baja suavemente entre 0 y 1
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        Color emission = auraColor * intensity;

        foreach (Material m in materials)
            m.SetColor("_EmissionColor", emission);

        if (auraLight != null)
            auraLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, t);
    }

    // Llamar cuando el jugador recoja el objeto
    public void StopAura()
    {
        isActive = false;

        foreach (Material m in materials)
            m.SetColor("_EmissionColor", Color.black);

        if (auraLight != null)
            auraLight.enabled = false;
    }
}