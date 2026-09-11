using System.Collections;
using UnityEngine;

public class RespiracionJugador : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource audioNormal;
    [SerializeField] private AudioSource audioAgitada;

    [Header("Tiempos")]
    [SerializeField] private float retardoInicioAgitada = 1f;
    [SerializeField] private float tiempoCalmaDespuesDeCorrer = 3f;

    private bool corriendoRef;
    private Coroutine corrutinaActiva;
    private bool respiracionAgitadaActiva;

    void Start()
    {
        audioNormal.loop = true;
        audioAgitada.loop = true;

        audioNormal.Play();
    }

    public void ActualizarEstado(bool corriendo)
    {
        bool cambioAcorrer = corriendo && !corriendoRef;
        bool cambioAdejarDeCorrer = !corriendo && corriendoRef;

        corriendoRef = corriendo;

        if (cambioAcorrer)
        {
            if (corrutinaActiva != null) StopCoroutine(corrutinaActiva);
            corrutinaActiva = StartCoroutine(IniciarRespiracionAgitada());
        }

        if (cambioAdejarDeCorrer && respiracionAgitadaActiva)
        {
            if (corrutinaActiva != null) StopCoroutine(corrutinaActiva);
            corrutinaActiva = StartCoroutine(VolverANormalDespuesDeCalma());
        }
    }

    private IEnumerator IniciarRespiracionAgitada()
    {
        yield return new WaitForSeconds(retardoInicioAgitada);

        audioNormal.Stop();
        audioAgitada.Play();
        respiracionAgitadaActiva = true;
    }

    private IEnumerator VolverANormalDespuesDeCalma()
    {
        yield return new WaitForSeconds(tiempoCalmaDespuesDeCorrer);

        audioAgitada.Stop();
        audioNormal.Play();
        respiracionAgitadaActiva = false;
    }
}