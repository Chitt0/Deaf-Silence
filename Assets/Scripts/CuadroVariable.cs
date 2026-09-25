using UnityEngine;

public class CuadroVariable : MonoBehaviour
{
    [SerializeField] private Transform[] posicionesPorVuelta;

    private int ultimaVueltaProcesada = 0;

    void Start()
    {
        ultimaVueltaProcesada = ContadorVueltas.Instancia.VueltaActual;
    }

    void Update()
    {
        if (posicionesPorVuelta.Length == 0) return;

        int vuelta = ContadorVueltas.Instancia.VueltaActual;

        if (vuelta != ultimaVueltaProcesada)
        {
            ultimaVueltaProcesada = vuelta;
            int indiceAleatorio = Random.Range(0, posicionesPorVuelta.Length);
            transform.position = posicionesPorVuelta[indiceAleatorio].position;
        }
    }
}