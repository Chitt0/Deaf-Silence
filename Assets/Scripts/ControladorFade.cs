using System.Collections;
using UnityEngine;

public class ControladorFade : MonoBehaviour
{
    public static ControladorFade Instancia;

    [SerializeField] private CanvasGroup grupoFade;
    [SerializeField] private float duracionFade = 0.5f;
    [SerializeField] private float duracionFadeInicial = 1f;

    void Awake()
    {
        Instancia = this;
        grupoFade.alpha = 1f;
    }

    void Start()
    {
        StartCoroutine(Fundir(1f, 0f, duracionFadeInicial));
    }

    public IEnumerator FundirANegro()
    {
        yield return Fundir(0f, 1f, duracionFade);
    }

    public IEnumerator FundirDesdeNegro()
    {
        yield return Fundir(1f, 0f, duracionFade);
    }

    private IEnumerator Fundir(float alphaInicial, float alphaFinal, float duracion)
    {
        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            grupoFade.alpha = Mathf.Lerp(alphaInicial, alphaFinal, tiempo / duracion);
            yield return null;
        }
        grupoFade.alpha = alphaFinal;
    }
}