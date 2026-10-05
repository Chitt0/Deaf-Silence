using System.Collections;
using UnityEngine;

public class EntidadJSPasillo : MonoBehaviour
{
    [Header("Posiciones")]
    [SerializeField] private Transform puntoSpawn;
    [SerializeField] private Transform puntoDestino;

    [Header("Configuración")]
    [SerializeField] private float velocidadCarga = 25f;
    [SerializeField] private AudioSource audioSusto;

    public void IniciarCarrera()
    {
        gameObject.SetActive(true);

        if (puntoSpawn != null && puntoDestino != null)
        {
            transform.position = puntoSpawn.position;
            
            Vector3 direccion = (puntoDestino.position - puntoSpawn.position).normalized;
            if (direccion != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direccion);
            }
        }

        if (audioSusto != null)
        {
            audioSusto.Play();
        }

        StartCoroutine(MoverHaciaDestino());
    }

    private IEnumerator MoverHaciaDestino()
    {
        if (puntoSpawn == null || puntoDestino == null)
        {
            Debug.LogError("¡Faltan asignar las referencias de Spawn o Destino en el Inspector!");
            yield break;
        }

        Vector3 origen = puntoSpawn.position;
        Vector3 destino = puntoDestino.position;

        float distanciaTotal = Vector3.Distance(origen, destino);
        float duracion = distanciaTotal / velocidadCarga;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            transform.position = Vector3.Lerp(origen, destino, tiempoTranscurrido / duracion);
            yield return null;
        }

        transform.position = destino;
        gameObject.SetActive(false);
    }
}