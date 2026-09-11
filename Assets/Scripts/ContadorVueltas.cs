using UnityEngine;

public class ContadorVueltas : MonoBehaviour
{
    public static ContadorVueltas Instancia;

    public int VueltaActual { get; private set; } = 0;

    void Awake()
    {
        Instancia = this;
    }

    public void SumarVuelta()
    {
        VueltaActual++;
        Debug.Log("Vuelta actual: " + VueltaActual);
    }
}


