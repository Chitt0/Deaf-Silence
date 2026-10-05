using UnityEngine;

[System.Serializable]
public class EventoClave
{
    public string nombre;
    public GameObject objeto;
}

public class GestorEventosClave : MonoBehaviour
{
    [SerializeField] private EventoClave[] eventos;

    private int[] vueltaAsignada;
    private int ultimaVueltaProcesada = -1;

    void Start()
    {
        vueltaAsignada = new int[eventos.Length];
        for (int i = 0; i < vueltaAsignada.Length; i++)
        {
            vueltaAsignada[i] = i + 1;
        }

        for (int i = vueltaAsignada.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = vueltaAsignada[i];
            vueltaAsignada[i] = vueltaAsignada[j];
            vueltaAsignada[j] = temp;
        }

        Debug.Log("Orden sorteado: " + string.Join(", ", vueltaAsignada));

        foreach (EventoClave evento in eventos)
        {
            if (evento.objeto != null)
            {
                evento.objeto.SetActive(false);
            }
        }
    }

    void Update()
    {
        int vuelta = ContadorVueltas.Instancia.VueltaActual;

        if (vuelta == ultimaVueltaProcesada) return;
        ultimaVueltaProcesada = vuelta;

        for (int i = 0; i < eventos.Length; i++)
        {
            if (eventos[i].objeto == null) continue;
            eventos[i].objeto.SetActive(vueltaAsignada[i] == vuelta);
        }
    }
}