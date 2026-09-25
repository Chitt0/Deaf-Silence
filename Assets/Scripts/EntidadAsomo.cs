using System.Collections;
using UnityEngine;

public class EntidadAsomo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform camaraJugador;
    [SerializeField] private Transform posicionOculta;

    [Header("Tiempos")]
    [SerializeField] private float delayReaccionAlVerla = 0.15f;
    [SerializeField] private float duracionDeslizamiento = 0.15f;
    [SerializeField] private float tiempoOcultaAntesDeDesaparecer = 0.3f;

    private bool yaFueVista = false;

    void Update()
    {
        if (yaFueVista) return;

        if (EsVistaPorCamara())
        {
            yaFueVista = true;
            StartCoroutine(SecuenciaDeOcultamiento());
        }
    }

    private IEnumerator SecuenciaDeOcultamiento()
    {
        yield return new WaitForSeconds(delayReaccionAlVerla);

        Vector3 posicionInicial = transform.position;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionDeslizamiento)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracionDeslizamiento;
            transform.position = Vector3.Lerp(posicionInicial, posicionOculta.position, t);
            yield return null;
        }

        transform.position = posicionOculta.position;

        yield return new WaitForSeconds(tiempoOcultaAntesDeDesaparecer);

        gameObject.SetActive(false);
    }

    private bool EsVistaPorCamara()
    {
        Vector3 puntoDeVista = Camera.main.WorldToViewportPoint(transform.position);

        bool dentroDelCampoDeVision = puntoDeVista.z > 0
            && puntoDeVista.x > 0 && puntoDeVista.x < 1
            && puntoDeVista.y > 0 && puntoDeVista.y < 1;

        if (!dentroDelCampoDeVision)
        {
            Debug.Log("Fuera del campo de visión. Viewport: " + puntoDeVista);
            return false;
        }

        Vector3 direccion = transform.position - camaraJugador.position;
        RaycastHit hit;

        if (Physics.Raycast(camaraJugador.position, direccion.normalized, out hit, direccion.magnitude))
        {
            Debug.Log("Raycast golpeó: " + hit.transform.name);
            return hit.transform == transform;
        }

        Debug.Log("Raycast no golpeó nada");
        return false;
    }
}