using UnityEngine;

public class TriggerPasilloEvento : MonoBehaviour
{
    [SerializeField] private EntidadJSPasillo entidad;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (entidad != null)
        {
            entidad.IniciarCarrera();
        }

        GetComponent<Collider>().enabled = false;
    }
}