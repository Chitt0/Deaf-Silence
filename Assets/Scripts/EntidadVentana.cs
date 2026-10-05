using System.Collections;
using UnityEngine;

public class EntidadVentana : MonoBehaviour
{
    [Header("Posiciones")]
    [SerializeField] private Transform posicionFuera;
    [SerializeField] private Transform posicionAdentro;

    [Header("Tiempos")]
    [SerializeField] private float duracionEmbestida = 0.15f;
    [SerializeField] private float tiempoVisibleAdentro = 0.4f;
    [SerializeField] private float duracionRetirada = 0.25f;

    void OnEnable()
    {
        // Coloca a la entidad en la posición de origen al activarse
        if (posicionFuera != null)
        {
            transform.position = posicionFuera.position;
        }
    }

    public void Activar()
    {
        // 1. Activa el GameObject primero para permitir que la corrutina pueda ejecutarse
        gameObject.SetActive(true);

        // 2. Asegura la posición de origen antes de arrancar la secuencia
        if (posicionFuera != null)
        {
            transform.position = posicionFuera.position;
        }

        // 3. Inicia la secuencia de movimiento
        StartCoroutine(SecuenciaDeSusto());
    }

    private IEnumerator SecuenciaDeSusto()
    {
        // Validación de seguridad para evitar errores NullReferenceException
        if (posicionFuera == null || posicionAdentro == null)
        {
            Debug.LogError("¡Faltan asignar las referencias de Transform (posicionFuera / posicionAdentro) en el Inspector!");
            yield break;
        }

        // Embestida hacia la ventana/adentro
        yield return MoverEntre(posicionFuera.position, posicionAdentro.position, duracionEmbestida);

        // Tiempo en el que se queda visible
        yield return new WaitForSeconds(tiempoVisibleAdentro);

        // Retirada rápida hacia afuera
        yield return MoverEntre(posicionAdentro.position, posicionFuera.position, duracionRetirada);

        // Desactiva el GameObject al terminar el evento
        gameObject.SetActive(false);
    }

    private IEnumerator MoverEntre(Vector3 origen, Vector3 destino, float duracion)
    {
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            transform.position = Vector3.Lerp(origen, destino, tiempoTranscurrido / duracion);
            yield return null;
        }
        transform.position = destino;
    }
}