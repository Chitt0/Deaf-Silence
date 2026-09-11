using System.Collections;
using UnityEngine;

public class ControladorFade : MonoBehaviour
{
    public static ControladorFade Instancia;

    [SerializeField] private CanvasGroup grupoFade;
    [SerializeField] private float duracionFade = 0.5f;

    void Awake()
    {
        Instancia = this;
    }

    public IEnumerator FundirANegro()
    {
        yield return Fundir(0f, 1f);
    }

    public IEnumerator FundirDesdeNegro()
    {
        yield return Fundir(1f, 0f);
    }

    private IEnumerator Fundir(float alphaInicial, float alphaFinal)
    {
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            grupoFade.alpha = Mathf.Lerp(alphaInicial, alphaFinal, tiempo / duracionFade);
            yield return null;
        }
        grupoFade.alpha = alphaFinal;
    }
}